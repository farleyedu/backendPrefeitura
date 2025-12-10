using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaSustentacaoOralService : ISessaoPlenariaSustentacaoOralService
    {
        private readonly ISessaoPlenariaSustentacaoOralRepository _repo;

        public SessaoPlenariaSustentacaoOralService(ISessaoPlenariaSustentacaoOralRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SessaoPlenariaSustentacaoOralResponse>> GetAllAsync()
            => (await _repo.AllWithAnexosAsync()).ToResponse();

        public async Task<SessaoPlenariaSustentacaoOralResponse?> GetAtivaAsync()
             => (await _repo.GetAtivaWithAnexosAsync())?.ToResponse();

        public async Task<SessaoPlenariaSustentacaoOralResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithAnexosByIdAsync(id))?.ToResponse();

        public async Task<SessaoPlenariaSustentacaoOralResponse?> GetBySlugAsync(string slug)
            => (await _repo.GetWithAnexosBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaSustentacaoOralCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um registro com este slug.");

            var entity = request.FromCreate(DateTime.UtcNow);

            if (entity.Ativo)
                await _repo.DesativarTodosExcetoAsync(null);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();  
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaSustentacaoOralUpdateRequest request)
        {
            var e = await _repo.GetWithAnexosByIdAsync(id); 
            if (e is null) return false;

            if (!string.IsNullOrWhiteSpace(request.Slug) &&
                !string.Equals(e.Slug, request.Slug, StringComparison.OrdinalIgnoreCase) &&
                await _repo.AnyAsync(x => x.Id != id && x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um registro com este slug.");

            var now = DateTime.UtcNow;
            var wasActive = e.Ativo;

            if (!string.IsNullOrWhiteSpace(request.Slug))
                e.Slug = request.Slug.Trim();

            if (request.Titulo is not null)
                e.Titulo = request.Titulo?.Trim();

            if (request.Descricao is not null)
                e.Descricao = request.Descricao;

            if (request.Ativo.HasValue)
                e.Ativo = request.Ativo.Value;

            e.DataAtualizacao = now;

            if (!wasActive && e.Ativo)
                await _repo.DesativarTodosExcetoAsync(e.Id);

            if (request.Anexos is not null && request.Anexos.Any())
            {
                var novos = request.Anexos.Select(a => new SessaoPlenariaSustentacaoOralAnexos
                {
                    Ordem = a.Ordem?.Trim() ?? string.Empty,
                    Titulo = a.Titulo?.Trim() ?? string.Empty,
                    Descricao = a.Descricao?.Trim()
                });

                await _repo.ReplaceAnexosAsync(e.Id, novos);
            }
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
