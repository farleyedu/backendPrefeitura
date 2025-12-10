using PortalTCMSP.Domain.DTOs.Requests.Categoria;
using PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse;
using PortalTCMSP.Domain.Mappings.Home;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Infra.Services.Home
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaResponse>> ListarAsync()
        {
            var categorias = await _categoriaRepository.ListarTodasAsync();
            return categorias.Select(CategoriaMapper.ToResponse);
        }

        public async Task<CategoriaResponse?> ObterPorIdAsync(int id)
        {
            var categoria = await _categoriaRepository.FindByIdAsync(id);
            if (categoria == null)
                return null;
            return categoria == null ? null : CategoriaMapper.ToResponse(categoria);
        }

        public async Task<int> CriarAsync(CategoriaCreateRequest request)
        {
            var categoria = CategoriaMapper.ToEntity(request);

            await _categoriaRepository.InsertAsync(categoria);
            await _categoriaRepository.CommitAsync();

            return (int)categoria.Id;
        }

        public async Task<bool> AtualizarAsync(CategoriaUpdateRequest request)
        {
            var categoria = await _categoriaRepository.FindByIdAsync(request.Id);
            if (categoria == null) return false;

            CategoriaMapper.ApplyUpdate(categoria, request);

            await _categoriaRepository.UpdateAsync(categoria);
            await _categoriaRepository.CommitAsync();

            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var categoria = await _categoriaRepository.FindByIdAsync(id);
            if (categoria == null) return false;

            await _categoriaRepository.DeleteAsync(categoria);
            await _categoriaRepository.CommitAsync();

            return true;
        }
    }
}
