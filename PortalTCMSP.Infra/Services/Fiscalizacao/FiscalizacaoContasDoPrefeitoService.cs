using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Domain.Mappings.Fiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Domain.Services.Fiscalizacao;
using PortalTCMSP.Infra.Helpers;

namespace PortalTCMSP.Infra.Services.Fiscalizacao
{
    public class FiscalizacaoContasDoPrefeitoService(IFiscalizacaoContasDoPrefeitoRepository repo) : IFiscalizacaoContasDoPrefeitoService
    {
        private readonly IFiscalizacaoContasDoPrefeitoRepository _repo = repo;

        public async Task<FiscalizacaoContasDoPrefeitoResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithAnexosByIdAsync(id))?.ToResponse();

        public async Task<long> CreateAsync(FiscalizacaoContasDoPrefeitoCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Ano == request.Ano &&
                                          x.Pauta == request.Pauta))
                throw new InvalidOperationException("Já existe um registro com este Ano/Pauta na mesma seção.");

            var entity = request.FromCreate(DateTime.UtcNow);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, FiscalizacaoContasDoPrefeitoUpdateRequest request)
        {
            var e = await _repo.GetWithAnexosByIdAsync(id);
            if (e is null) return false;

            if (await _repo.AnyAsync(x => x.Id != id &&
                                          x.Ano == request.Ano &&
                                          x.Pauta == request.Pauta))
                throw new InvalidOperationException("Já existe um registro com este Ano/Pauta na mesma seção.");

            e.ApplyUpdate(request, DateTime.UtcNow);

            var novos = request.Anexos?.Select(a => new FiscalizacaoContasDoPrefeitoAnexo
            {
                IdFiscalizacaoContasDoPrefeito = e.Id,
                Link = a.Link?.Trim() ?? string.Empty,
                TipoArquivo = a.TipoArquivo?.Trim(),
                NomeExibicao = a.NomeExibicao?.Trim(),
                Ordem = a.Ordem
            }) ?? [];

            await _repo.ReplaceAnexosAsync(e.Id, novos);

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

        public Task<ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>?> GetListAsync(
            FiscalizacaoContasDoPrefeitoSearchRequest request)
        {
            var query = _repo.Search(request);
            var total = query.Count();

            if (total == 0)
                return Task.FromResult<ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>?>(null);

            var pageItems = PaginationHelper<FiscalizacaoContasDoPrefeito>.Set(request.Page, request.Count, query);
            var resp = FiscalizacaoContasDoPrefeitoMapper.BuildPaged(request.Page, request.Count, total, pageItems);
            return Task.FromResult<ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>?>(resp);
        }
    }
}
