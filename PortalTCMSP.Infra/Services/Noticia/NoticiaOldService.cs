using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Mappings.Noticia;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Domain.Services.Noticia;
using Microsoft.EntityFrameworkCore;

namespace PortalTCMSP.Infra.Services.Noticia
{
    public class NoticiaOldService : INoticiaOldService
    {
        private readonly INoticiaOldRepository _repository;
        public NoticiaOldService(INoticiaOldRepository repository)
        {
            _repository = repository;
        }

        public ResultadoPaginadoResponse<NoticiaOldResponse> ListarNoticiasOld(NoticiaOldListarRequest request)
        {
            var all = _repository.Search(request).ToList();
            var total = all.Count;
            var page = request.Page < 1 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            var items = all.Skip((page - 1) * count).Take(count).ToList();
            return NoticiaOldMapper.BuildList(page, count, total, items);
        }

        public async Task<ResultadoPaginadoResponse<NoticiaOldMappedResponse>> ListarNoticiasOldMapAsync(NoticiaOldListarRequest request)
        {
            var queryAny = _repository.Search(request);

            var page = request.Page < 1 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            if (queryAny is IQueryable<PortalTCMSP.Domain.Entities.NoticiaEntity.NoticiaOld> efQuery)
            {
                var totalEf = await efQuery.CountAsync();

                var pageItemsEf = await efQuery
                    .OrderByDescending(x => x.DataPublicacao)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToListAsync();

                var mappedEf = pageItemsEf.Select(NoticiaOldMapper.ToMappedResponse).ToList();
                return NoticiaOldMapper.BuildList(page, count, totalEf, mappedEf);
            }

            var list = queryAny.ToList();
            var total = list.Count;

            var pageItems = list
                .OrderByDescending(x => x.DataPublicacao)
                .Skip((page - 1) * count)
                .Take(count)
                .ToList();

            var mapped = pageItems.Select(NoticiaOldMapper.ToMappedResponse).ToList();
            return NoticiaOldMapper.BuildList(page, count, total, mapped);
        }

        public async Task<NoticiaOldResponse?> GetByIdAsync(long id, CancellationToken ct = default)
        {
            var e = await _repository.GetByIdAsync(id, ct);
            return e is null ? null : NoticiaOldMapper.ToResponse(e);
        }

        public async Task<NoticiaOldMappedResponse?> GetByIdMapAsync(long id, CancellationToken ct = default)
        {
            var e = await _repository.GetByIdAsync(id, ct);
            return e is null ? null : NoticiaOldMapper.ToMappedResponse(e);
        }

        public async Task<NoticiaOldResponse?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var e = await _repository.GetBySlugAsync(slug, ct);
            return e is null ? null : NoticiaOldMapper.ToResponse(e);
        }

        public async Task<NoticiaOldMappedResponse?> GetBySlugMapAsync(string slug, CancellationToken ct = default)
        {
            var e = await _repository.GetBySlugAsync(slug, ct);
            return e is null ? null : NoticiaOldMapper.ToMappedResponse(e);
        }
    }
}
