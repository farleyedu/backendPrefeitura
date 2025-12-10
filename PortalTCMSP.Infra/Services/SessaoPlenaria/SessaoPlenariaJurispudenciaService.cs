using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaJurispudenciaService : ISessaoPlenariaJurispudenciaService
    {
        private readonly ISessaoPlenariaJurispudenciaRepository _repo;

        public SessaoPlenariaJurispudenciaService(ISessaoPlenariaJurispudenciaRepository repo)
        {
            _repo = repo;
        }
        public async Task<SessaoPlenariaJurisprudenciaResponse?> GetAtivoAsync()
        => (await _repo.GetAtivaAsync())?.ToResponse();

        public async Task<IEnumerable<SessaoPlenariaJurisprudenciaResponse>> GetAllAsync()
            => (await _repo.AllAsync()).ToResponse();

        public async Task<SessaoPlenariaJurisprudenciaResponse?> GetByIdAsync(long id)
            => (await _repo.FindByIdAsync(id))?.ToResponse();

        public async Task<SessaoPlenariaJurisprudenciaResponse?> GetBySlugAsync(string slug)
            => (await _repo.FindBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaJurisprudenciaCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um registro com este slug para esta seção.");

            var entity = request.FromCreate(DateTime.UtcNow);

            if (entity.Ativo)
                await _repo.DesativarTodosExcetoAsync(null);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaJurisprudenciaUpdateRequest request)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            if (!string.IsNullOrWhiteSpace(request.Slug) &&
                !string.Equals(e.Slug, request.Slug, StringComparison.OrdinalIgnoreCase) &&
                await _repo.AnyAsync(x => x.Id != id && x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um registro com este slug.");

            var wasActive = e.Ativo;

            e.ApplyPartialUpdate(request, DateTime.UtcNow);

            if (!wasActive && e.Ativo)
                await _repo.DesativarTodosExcetoAsync(e.Id);

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
