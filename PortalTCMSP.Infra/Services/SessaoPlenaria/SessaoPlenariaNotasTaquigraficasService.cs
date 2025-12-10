using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;
using PortalTCMSP.Infra.Helpers;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaNotasTaquigraficasService : ISessaoPlenariaNotasTaquigraficasService
    {
        private readonly ISessaoPlenariaNotasTaquigraficasRepository _repo;

        public SessaoPlenariaNotasTaquigraficasService(ISessaoPlenariaNotasTaquigraficasRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SessaoPlenariaNotasTaquigraficasResponse>> GetAllAsync()
            => (await _repo.AllWithAnexosAsync()).ToResponse();

        public async Task<SessaoPlenariaNotasTaquigraficasResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithAnexosByIdAsync(id))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaNotasTaquigraficasCreateRequest request)
        {
            var entity = request.FromCreate(DateTime.UtcNow);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaNotasTaquigraficasUpdateRequest request)
        {
            var e = await _repo.GetWithAnexosByIdAsync(id); // tracked + Anexos

            e.ApplyPartialUpdate(request, DateTime.UtcNow);

            if (request.Anexos is not null)
            {
                if (request.Anexos.Any())
                {
                    var novos = request.Anexos.Select(a => new SessaoPlenariaNotasTaquigraficasAnexos
                    {
                        Link = a.Link?.Trim() ?? string.Empty,
                        TipoArquivo = a.TipoArquivo?.Trim(),
                        NomeExibicao = a.NomeExibicao?.Trim(),
                        Ordem = a.Ordem
                    });
                    await _repo.ReplaceAnexosAsync(e.Id, novos);
                }
                else
                {
                    await _repo.ReplaceAnexosAsync(e.Id, Enumerable.Empty<SessaoPlenariaNotasTaquigraficasAnexos>());
                }
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

        public async Task<ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse>?> GetAllNotas(SessaoPlenariaNotasTaquigraficasSearchRequest request)
        {
            var notas = await _repo.Search(request) ?? new List<SessaoPlenariaNotasTaquigraficas>();

            var page = request.Page <= 0 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            var paginadas = PaginationHelper<SessaoPlenariaNotasTaquigraficas>.Set(page, count, notas);
            return SessaoPlenariaNotasTaquigraficasMapper.Build(page, count, notas.Count(), paginadas);
        }
    }
}
