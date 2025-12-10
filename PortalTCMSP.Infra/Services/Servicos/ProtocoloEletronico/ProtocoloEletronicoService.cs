using PortalTCMSP.Domain.DTOs.Requests.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Mappings.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Services.Servicos.ProtocoloEletronico;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Services.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public class ProtocoloEletronicoService : IProtocoloEletronicoService
    {
        private readonly IProtocoloEletronicoRepository _repo;
        private readonly IProtocoloEletronicoAcaoRepository _repoAcoes;

        public ProtocoloEletronicoService(
            IProtocoloEletronicoRepository repo,
            IProtocoloEletronicoAcaoRepository repoAcoes)
        {
            _repo = repo;
            _repoAcoes = repoAcoes;
        }

        public async Task<IEnumerable<ProtocoloEletronicoResponse>> GetAllAsync()
                => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<ProtocoloEletronicoResponse?> GetByIdAsync(long id)
                => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<long> CreateAsync(ProtocoloEletronicoCreateRequest request)
        {
            var existeSlugAtivo = await GetBySlugAtivoAsync(request.Slug);
            if (existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        private async Task<ProtocoloEletronicoResponse?> GetBySlugAtivoAsync(string slug)
            => (await _repo.GetBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<bool> UpdateAsync(long id, ProtocoloEletronicoUpdateRequest request)
        {
            var now = DateTime.UtcNow;
            var entity = await _repo.GetWithChildrenByIdAsync(id);
            if (entity is null) return false;

            entity.ApplyUpdate(request, now);

            if (request.Acoes is not null)
            {
                await _repo.ReplaceAcoesAsync(id, request.Acoes.Select(a => new ProtocoloEletronicoAcao
                {
                    IdProtocoloEletronico = id,
                    Ativo = a.Ativo,
                    Ordem = a.Ordem,
                    Titulo = a.Titulo.Trim(),
                    DataPublicacao = a.DataPublicacao,
                    TipoAcao = a.TipoAcao,
                    UrlAcao = a.UrlAcao.Trim()
                }));
            }

            await _repo.UpdateAsync(entity);
            return await _repo.CommitAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<bool> CreateAcoesAsync(long id, List<ProtocoloEletronicoAcaoRequest> novas)
        {
            await _repoAcoes.CreateAcoesAsync(id, novas.Select(a => new ProtocoloEletronicoAcao
            {
                IdProtocoloEletronico = id,
                Ativo = a.Ativo,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                DataPublicacao = a.DataPublicacao,
                TipoAcao = a.TipoAcao,
                UrlAcao = a.UrlAcao.Trim()
            }));

            return await _repoAcoes.CommitAsync();
        }

        public async Task<bool> UpdateAcoesAsync(long id, List<ProtocoloEletronicoAcaoUpdate> novas)
        {
            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem protocolos para serem atualizados.");
            var acoesAntigas = entidadesAntigas.Acoes;

            foreach (var nova in novas)
            {
                var ef = await _repoAcoes.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Acao com Id {nova.Id} não encontrado.");

                foreach (var antiga in acoesAntigas)
                {
                    if (nova.Id == antiga.Id)
                    {
                        antiga.Ordem = nova.Ordem;
                        antiga.TipoAcao = nova.TipoAcao;
                        antiga.Titulo = nova.Titulo.Trim();
                        antiga.UrlAcao = nova.UrlAcao.Trim();
                        antiga.DataPublicacao = nova.DataPublicacao;
                        antiga.Ativo = nova.Ativo;
                    }
                }
            }

            await _repoAcoes.UpdateAcoesAsync(id, acoesAntigas);
            return await _repoAcoes.CommitAsync();
            
        }

        public async Task<ProtocoloEletronicoResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();
    }

}
