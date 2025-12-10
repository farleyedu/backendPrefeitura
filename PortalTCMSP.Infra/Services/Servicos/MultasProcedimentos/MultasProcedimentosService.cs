using PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Mappings.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Services.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Mappings.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Services.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosService(
        IMultasProcedimentosRepository repo, 
        IMultasProcedimentosProcedimentoRepository repoProcedimentos,
        IMultasProcedimentosPortariaRelacionadaRepository repoPortariaRelacionada) : IMultasProcedimentosService
    {
        private readonly IMultasProcedimentosRepository _repo = repo;
        private readonly IMultasProcedimentosProcedimentoRepository _repoProcedimentos = repoProcedimentos;
        private readonly IMultasProcedimentosPortariaRelacionadaRepository _repoPortariaRelacionada = repoPortariaRelacionada;

        public async Task<IEnumerable<MultasProcedimentosResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<MultasProcedimentosResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<bool> DisableAsync(long id)
            => await _repo.DisableAsync(id);

        public async Task<long> CreateAsync(MultasProcedimentosCreateRequest request)
        {
            var existeSlugAtivo = await GetBySlugAtivoAsync(request.Slug);
            if(existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> CreateProcedimentosAsync(long id, List<MultasProcedimentosProcedimentoRequest> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");

            var entidades = novos.Select(p => new MultasProcedimentosProcedimento
            {
                IdMultasProcedimentos = id,
                Ordem = p.Ordem,
                Texto = p.Texto.Trim(),
                UrlImagem = p.UrlImagem?.Trim()
            });

            await _repoProcedimentos.CreateProcedimentosAsync(id, entidades);
            return await _repoProcedimentos.CommitAsync();
        }

        public async Task<bool> CreatePortariaRelacionadasAsync(long id, List<MultasProcedimentosPortariaRelacionadaRequest> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");

            var entidades = novos.Select(p => new MultasProcedimentosPortariaRelacionada
            {
                IdMultasProcedimentos = id,
                Ordem = p.Ordem,
                Titulo = p.Titulo.Trim(),
                Url = p.Url.Trim()
            });

            await _repoPortariaRelacionada.CreatePortariaRelacionadaAsync(id, entidades);
            return await _repoPortariaRelacionada.CommitAsync();
        }

        public async Task<MultasProcedimentosResponse?> GetBySlugAtivoAsync(string slug)
            => (await _repo.GetBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<MultasProcedimentosResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<bool> UpdateAsync(long id, MultasProcedimentosUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            e.ApplyUpdate(request, DateTime.UtcNow);

            var novosProc = request.Procedimentos?.Select(p => new MultasProcedimentosProcedimento
            {
                IdMultasProcedimentos = e.Id,
                Ordem = p.Ordem,
                Texto = p.Texto.Trim(),
                UrlImagem = p.UrlImagem?.Trim()
            }) ?? [];

            var novasPort = request.PortariasRelacionadas?.Select(p => new MultasProcedimentosPortariaRelacionada
            {
                IdMultasProcedimentos = e.Id,
                Ordem = p.Ordem,
                Titulo = p.Titulo.Trim(),
                Url = p.Url.Trim()
            }) ?? [];

            await _repo.ReplaceProcedimentosAsync(e.Id, novosProc);
            await _repo.ReplacePortariasAsync(e.Id, novasPort);

            await _repo.UpdateAsync(e);
            return await _repo.CommitAsync();
        }
        
        public async Task<bool> UpdateProcedimentosAsync(long id, List<MultasProcedimentosProcedimentoUpdate> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");

            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem procedimentos para serem atualizados.");
            var procedimentosAntigos = entidadesAntigas.Procedimentos;

            foreach (var nova in novos)
            {
                var ef = await _repoProcedimentos.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Procedimento com Id {nova.Id} não encontrado.");

                foreach (var antiga in procedimentosAntigos)
                {
                    if(nova.Id == antiga.Id)
                    {
                        antiga.Ordem = nova.Ordem;
                        antiga.Texto = nova.Texto.Trim();
                        antiga.UrlImagem = nova.UrlImagem?.Trim();
                    }
                }
            }

            await _repoProcedimentos.UpdateProcedimentosAsync(id, procedimentosAntigos);
            return await _repoProcedimentos.CommitAsync();
        }

        public async Task<bool> UpdatePortariaRelacionadasAsync(long id, List<MultasProcedimentosPortariaRelacionadaUpdate> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");

            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem procedimentos para serem atualizados.");
            var portariasRelacionadasAntigas = entidadesAntigas.PortariasRelacionadas;

            foreach (var nova in novos)
            {
                var ef = await _repoPortariaRelacionada.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Portaria com Id {nova.Id} não encontrado.");

                foreach (var antiga in portariasRelacionadasAntigas)
                {
                    if (nova.Id == antiga.Id)
                    {
                        antiga.Ordem = nova.Ordem;
                        antiga.Titulo = nova.Titulo.Trim();
                        antiga.Url = nova.Url.Trim();
                    }
                }
            }

            await _repoPortariaRelacionada.UpdatePortariaRelacionadaAsync(id, portariasRelacionadasAntigas);
            return await _repoPortariaRelacionada.CommitAsync();
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
