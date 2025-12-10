using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Domain.Services.Fiscalizacao;
using PortalTCMSP.Domain.Mappings.Fiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;

namespace PortalTCMSP.Infra.Services.Fiscalizacao
{
    public class FiscalizacaoRelatoriosFiscalizacaoService : IFiscalizacaoRelatoriosFiscalizacaoService
    {
        private readonly IFiscalizacaoRelatorioFiscalizacaoRepository _repo;
        public FiscalizacaoRelatoriosFiscalizacaoService(IFiscalizacaoRelatorioFiscalizacaoRepository repo) => _repo = repo;

        public async Task<IEnumerable<FiscalizacaoRelatorioFiscalizacaoResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<FiscalizacaoRelatorioFiscalizacaoResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<FiscalizacaoRelatorioFiscalizacaoResponse?> GetBySlugAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(FiscalizacaoRelatorioFiscalizacaoCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == SlugHelper.Slugify(request.Slug)))
                throw new InvalidOperationException("Já existe um conteúdo com este slug.");

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            var newSlug = SlugHelper.Slugify(request.Slug);
            if (e.Slug != newSlug && await _repo.AnyAsync(x => x.Id != id && x.Slug == newSlug))
                throw new InvalidOperationException("Já existe um conteúdo com este slug.");

            e.ApplyUpdate(request, DateTime.Now);

            var novosCarrossel = request.Carrocel?.Select(c => new FiscalizacaoRelatorioFiscalizacaoCarrossel
            {
                IdConteudo = e.Id,
                Ordem = c.Ordem,
                Ativo = c.Ativo,
                Titulo = c.Titulo?.Trim() ?? string.Empty,
                ConteudoCarrocel = c.ConteudoCarrocel?.Select(cc => new FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel
                {
                    Ordem = cc.Ordem,
                    Ativo = cc.Ativo,
                    Descricao = cc.Descricao?.Trim(),
                    Link = cc.Link?.Trim(),
                }).ToList() ?? new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel>()
            }) ?? new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>();

            await _repo.ReplaceCarrosselAsync(e.Id, novosCarrossel);
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
