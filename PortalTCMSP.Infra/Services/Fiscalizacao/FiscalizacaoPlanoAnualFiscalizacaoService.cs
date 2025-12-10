using NPOI.SS.Formula.Functions;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Domain.Services.Fiscalizacao;
using PortalTCMSP.Domain.Mappings.Fiscalizacao;

namespace PortalTCMSP.Infra.Services.Fiscalizacao
{
    public class FiscalizacaoPlanoAnualFiscalizacaoService : IFiscalizacaoPlanoAnualFiscalizacaoService
    {
        private readonly IFiscalizacaoPlanoAnualFiscalizacaoRepository _repo;
        public FiscalizacaoPlanoAnualFiscalizacaoService(IFiscalizacaoPlanoAnualFiscalizacaoRepository fiscalizacaoPlanoAnualFiscalizacaoRepositor)
        {
            _repo = fiscalizacaoPlanoAnualFiscalizacaoRepositor;
        }

        public async Task<IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>> GetAllAsync()
        => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse?> GetBySlugAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe uma resolução com este slug.");

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            if (e.Slug != request.Slug && await _repo.AnyAsync(x => x.Id != id && x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe uma resolução com este slug.");

            e.ApplyUpdate(request, DateTime.UtcNow);

            var novosDispositivos = request.Dispositivos?.Select(d => new FiscalizacaoResolucaoDispositivo
            {
                ResolucaoId = id,
                Ordem = d.Ordem,
                Artigo = d.Artigo?.Trim() ?? string.Empty,
                Paragrafo = d.Paragrafo?.Select(p => new FiscalizacaoResolucaoParagrafo { Ordem = p.Ordem, Texto = p.Texto?.Trim() ?? string.Empty }).ToList() ?? [],
                Incisos = d.Incisos?.Select(i => new FiscalizacaoResolucaoInciso { Ordem = i.Ordem, Texto = i.Texto?.Trim() ?? string.Empty }).ToList() ?? []
            }) ?? [];

            var novosAnexos = request.Anexos?.Select(a => new FiscalizacaoResolucaoAnexo
            {
                ResolucaoId = id,
                Numero = a.Numero,
                Titulo = a.Titulo?.Trim() ?? string.Empty,
                Descritivo = a.Descritivo?.Trim(),
                TemasPrioritarios = a.TemasPrioritarios?.Select(t => new FiscalizacaoResolucaoTemaPrioritario { Ordem = t.Ordem, Tema = t.Tema?.Trim() ?? string.Empty, Descricao = t.Descricao?.Trim() }).ToList() ?? [],
                Atividades = a.Atividades?.Select(at => new FiscalizacaoResolucaoAtividade
                {
                    Tipo = at.Tipo?.Trim() ?? string.Empty,
                    AtividadeItem = at.AtividadeItem?.Select(ai => new FiscalizacaoResolucaoAtividadeItem { Descricao = ai.Descricao?.Trim() ?? string.Empty, Quantidade = ai.Quantidade, DUSFs = ai.DUSFs }).ToList() ?? []
                }).ToList() ?? [],
                Distribuicao = a.Distribuicao?.Select(d => new FiscalizacaoResolucaoDistribuicao
                {
                    TipoFiscalizacao = d.TipoFiscalizacao,
                    TotalPAF = d.TotalPAF,
                    DezPorCentoTotalPAF = d.DezPorCentoTotalPAF,
                    LimitePorConselheiro = d.LimitePorConselheiro,
                    LimiteConselheiros = d.LimiteConselheiros,
                    LimitePlenoeCameras = d.LimitePlenoeCameras,
                    LimitePresidente = d.LimitePresidente,
                    ListaDePrioridades = d.ListaDePrioridades
                }).ToList() ?? []
            }).ToList() ?? [];

            var novasAtas = request.Atas?.Select(a => new FiscalizacaoResolucaoAta
            {
                ResolucaoId = id,
                Ordem = a.Ordem,
                TituloAta = a.TituloAta?.Trim() ?? string.Empty,
                TituloAtaAEsquerda = a.TituloAtaAEsquerda,
                ConteudoAta = a.ConteudoAta?.Trim() ?? string.Empty
            }).ToList() ?? [];

            FiscalizacaoResolucaoEmenta? novaEmenta = null;
            if (request.Ementa != null)
            {
                novaEmenta = new FiscalizacaoResolucaoEmenta
                {
                    Descritivo = request.Ementa.Descritivo?.Trim() ?? string.Empty,
                    LinksArtigos = request.Ementa.LinksArtigos?.Select(l => new FiscalizacaoResolucaoEmentaLink { Link = l?.Trim() ?? string.Empty }).ToList() ?? []
                };
            }

            await _repo.ReplaceDispositivosAsync(e.Id, novosDispositivos);
            await _repo.ReplaceAnexosAsync(e.Id, novosAnexos);
            await _repo.ReplaceAtasAsync(e.Id, novasAtas);
            await _repo.ReplaceEmentaAsync(e.Id, novaEmenta);

            await _repo.UpdateAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }
    }
}
