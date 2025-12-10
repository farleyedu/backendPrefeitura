using PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Mappings.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Services.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Infra.Services.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisService(
        IPrazosProcessuaisRepository repo,
        IPrazosProcessuaisItemRepository repoProcessuaisItem,
        IPrazosProcessuaisItemAnexoRepository repoProcessuaisItemAnexo
    ) : IPrazosProcessuaisService
    {
        private readonly IPrazosProcessuaisRepository _repo = repo;
        private readonly IPrazosProcessuaisItemRepository _repoProcessuaisItem = repoProcessuaisItem;
        private readonly IPrazosProcessuaisItemAnexoRepository _repoProcessuaisItemAnexo = repoProcessuaisItemAnexo;

        public async Task<IEnumerable<PrazosProcessuaisResponse>> GetAllAsync()
        {
            var all = await _repo.AllWithChildrenAsync();
            return all.ToResponse();
        }
        public async Task<PrazosProcessuaisResponse?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetWithChildrenByIdAsync(id);
            return entity?.ToResponse();
        }
        public async Task<PrazosProcessuaisResponse> CreateAsync(PrazosProcessuaisCreateRequest request)
        {
            var existeSlugAtivo = await _repo.GetBySlugAtivoAsync(request.Slug);
            if (existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var now = DateTime.UtcNow;
            var entity = request.FromCreate(now);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();

            return entity.ToResponse();
        }

        public async Task<bool> CreateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexoRequest> novos)
        {
            var entidades = novos.Select(d => d.ToEntity(id));

            await _repoProcessuaisItemAnexo.CreateAnexoAsync(id, entidades);
            return await _repo.CommitAsync();
        }

        public async Task<bool> CreatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItemRequest> novos)
        {
            var entidades = novos.Select(d => d.ToEntity(id));

            await _repoProcessuaisItem.CreatePrazosProcessuaisItemAsync(id, entidades);
            return await _repo.CommitAsync();
        }

        public async Task<bool> UpdateAsync(long id, PrazosProcessuaisCreateRequest request)
        {
            var entity = await _repo.GetWithChildrenByIdAsync(id);
            if (entity is null) return false;

            var updateRequest = new PrazosProcessuaisUpdateRequest
            {
                TituloPagina = request.TituloPagina,
                Slug = request.Slug,
                Ativo = request.Ativo//,
                //PrazosProcessuaisItens = request.PrazosProcessuaisItens
            };

            entity.ApplyUpdate(updateRequest, DateTime.UtcNow);

            await _repo.UpdateAsync(entity);
            return await _repo.CommitAsync();
        }

        public async Task<bool> UpdateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexoUpdate> novos)
        {
            var entidades = novos.Select(s => new PrazosProcessuaisItemAnexo
            {
                Id = s.Id,
                IdPrazosProcessuaisItem = id,
                Ordem = s.Ordem,
                Ativo = s.Ativo,
                NomeArquivo = s.NomeArquivo?.Trim() ?? string.Empty,
                Url = s.Url?.Trim() ?? string.Empty,
                Tipo = s.Tipo?.Trim() ?? string.Empty
            });

            await _repoProcessuaisItemAnexo.UpdateAnexoAsync(id, entidades);
            return await _repo.CommitAsync();
        }

        public async Task<bool> UpdatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItemUpdate> novos)
        {
            var entidade = await _repo.GetWithChildrenByIdAsync(id)
                ?? throw new InvalidOperationException("PrazosProcessuais não encontrado.");

            var antigos = entidade.PrazosProcessuaisItens;

            var lista = new List<PrazosProcessuaisItem>();

            foreach (var novo in novos)
            {
                var existente = antigos.FirstOrDefault(a => a.Id == novo.Id);
                if (existente is not null)
                {
                    existente.Id = novo.Id;
                    existente.Ordem = novo.Ordem;
                    existente.Nome = novo.Nome.Trim();
                    existente.Ativo = novo.Ativo;
                    existente.DataPublicacao = novo.DataPublicacao;
                    existente.TempoDecorrido = novo.TempoDecorrido;
                    //Anexos = s.Anexos?.Select(a => new PrazosProcessuaisItemAnexo
                    //{
                    //    Ordem = a.Ordem,
                    //    Ativo = a.Ativo,
                    //    NomeArquivo = a.NomeArquivo?.Trim() ?? string.Empty,
                    //    Url = a.Url?.Trim() ?? string.Empty,
                    //    Tipo = a.Tipo?.Trim() ?? string.Empty
                    //}).ToList() ?? []

                    lista.Add(existente);
                }
            }

            await _repoProcessuaisItem.UpdatePrazosProcessuaisItemAsync(id, lista);
            return await _repo.CommitAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }
        public async Task<PrazosProcessuaisResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<bool> DisableAsync(long id)
        {
            return await _repo.DisableAsync(id);
        }
    }
}
