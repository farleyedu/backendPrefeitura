using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaService : ISessaoPlenariaService
    {
        private readonly ISessaoPlenariaRepository _repo;

        public SessaoPlenariaService(ISessaoPlenariaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SessaoPlenariaResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<SessaoPlenariaResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<SessaoPlenariaResponse?> GetBySlugAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe uma sessão com este slug.");

            var entity = request.FromCreate(DateTime.UtcNow);

            if (entity.Ativo == "S")
                await _repo.DeactivateAllAsync();

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            if (!string.IsNullOrWhiteSpace(request.Slug) &&
                !string.Equals(e.Slug, request.Slug, StringComparison.OrdinalIgnoreCase) &&
                await _repo.AnyAsync(x => x.Id != id && x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe uma sessão com este slug.");

            var wasActive = (e.Ativo == "S");

            e.ApplyPartialUpdate(request, DateTime.UtcNow);

            if (!wasActive && e.Ativo == "S")
                await _repo.DeactivateAllExceptAsync(e.Id);

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

        public async Task<IEnumerable<SessaoPlenariaResponse>> GetAtivosAsync()
            => (await _repo.GetAtivosAsync()).ToResponse();

        public async Task<SessaoPlenariaResponse?> GetAtivaAsync()
           => (await _repo.GetAtivaWithChildrenAsync())?.ToResponse();

    }
}
