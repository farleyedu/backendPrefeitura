using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;
using PortalTCMSP.Infra.Helpers;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaAtaService(ISessaoPlenariaAtasRepository repo) : ISessaoPlenariaAtaService
    {
        private readonly ISessaoPlenariaAtasRepository _repo = repo;

        public async Task<IEnumerable<SessaoPlenariaAtaResponse>> GetAllAsync()
            => (await _repo.AllWithAnexosAsync()).ToResponse();

        public async Task<SessaoPlenariaAtaResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithAnexosByIdAsync(id))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaAtaCreateRequest request)
        {
            var entity = request.FromCreate(DateTime.UtcNow);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaAtaUpdateRequest request)
        {
            var e = await _repo.GetWithAnexosByIdAsync(id);
            if (e is null) return false;

            if (request.IdSessaoPlenaria.HasValue && request.IdSessaoPlenaria.Value > 0)
            {
                var existeSessao = await _repo.ExistsSessaoPlenariaAsync(request.IdSessaoPlenaria.Value);
                if (!existeSessao)
                    throw new InvalidOperationException("IdSessaoPlenaria não encontrado.");
            }

            e.ApplyPartialUpdate(request, DateTime.UtcNow);

            if (request.Anexos is not null)
            {
                var novos = request.Anexos.Select(a => new SessaoPlenariaAtaAnexo
                {
                    Link = a.Link?.Trim() ?? string.Empty,
                    TipoArquivo = a.TipoArquivo?.Trim(),
                    NomeExibicao = a.NomeExibicao?.Trim(),
                    Ordem = a.Ordem
                }).ToList();

                await _repo.ReplaceAnexosAsync(e, novos);
            }

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

        public async Task<ResultadoPaginadoResponse<SessaoPlenariaAtaResponse>?> GetAllAtas(SessaoPlenariaAtaSearchRequest request)
        {
            var atas = await _repo.Search(request) ?? [];

            var page = request.Page <= 0 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            var paginadas = PaginationHelper<SessaoPlenariaAta>.Set(page, count, atas);
            return SessaoPlenariaAtaMapper.Build(page, count, atas.Count(), paginadas);
        }
    }
}
