using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Services.SessaoPlenaria;
using PortalTCMSP.Infra.Helpers;

namespace PortalTCMSP.Infra.Services.SessaoPlenaria
{
    public class SessaoPlenariaPautaService : ISessaoPlenariaPautaService
    {
        private readonly ISessaoPlenariaPautaRepository _repo;

        public SessaoPlenariaPautaService(ISessaoPlenariaPautaRepository repo)
        {
            _repo = repo;
        }

        public async Task<SessaoPlenariaPautaResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithAnexosByIdAsync(id))?.ToResponse();

        public async Task<long> CreateAsync(SessaoPlenariaPautaCreateRequest request)
        {
            var entity = request.FromCreate(DateTime.UtcNow);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, SessaoPlenariaPautaUpdateRequest request)
        {
            var e = await _repo.GetWithAnexosByIdAsync(id);
            if (e is null) return false;

            e.ApplyPartialUpdate(request, DateTime.UtcNow);

            if (request.Anexos is not null)
            {
                if (request.Anexos.Any())
                {
                    var novos = request.Anexos.Select(a => new SessaoPlenariaPautaAnexo
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
                    await _repo.ReplaceAnexosAsync(e.Id, Enumerable.Empty<SessaoPlenariaPautaAnexo>());
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

        public async Task<ResultadoPaginadoResponse<SessaoPlenariaPautaResponse>?> GetAllPautas(SessaoPlenariaPautaSearchRequest request)
        {
            var pautas = await _repo.Search(request) ?? new List<SessaoPlenariaPauta>();

            var pautasOrdenadas = pautas
                .OrderByDescending(p => p.DataPublicacao ?? p.DataCriacao)
                .ThenByDescending(p => p.Id)
                .ToList();

            var paginadas = PaginationHelper<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPauta>
                .Set(request.Page, request.Count, pautasOrdenadas);

            return SessaoPlenariaPautaMapper.Build(
                request.Page,
                request.Count,
                pautasOrdenadas.Count,
                paginadas
            );
        }
    }
}
