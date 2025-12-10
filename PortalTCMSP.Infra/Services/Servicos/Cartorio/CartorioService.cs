using PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.Cartorio;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.Cartorio;
using PortalTCMSP.Domain.Mappings.Servicos.Cartorio;
using PortalTCMSP.Domain.Services.Servicos.Cartorio;

namespace PortalTCMSP.Infra.Services.Servicos.Cartorio
{
    public class CartorioService(
        ICartorioRepository repo,
        ICartorioAtendimentoRepository repoAtendimento
    ) : ICartorioService
    {
        private readonly ICartorioRepository _repo = repo;
        private readonly ICartorioAtendimentoRepository _repoAtendimento = repoAtendimento;

        public async Task<IEnumerable<CartorioResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<CartorioResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<bool> DisableAsync(long id)
            => await _repo.DisableAsync(id);

        public async Task<long> CreateAsync(CartorioCreateRequest request)
        {
            var existeSlugAtivo = await _repo.GetBySlugAtivoAsync(request.Slug);
            if (existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> CreateAtendimentosAsync(long id, List<CartorioAtendimentoRequest> novos)
        {
            var entidades = novos.Select(a => new CartorioAtendimento
            {
                IdCartorio = id,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                Descricao = a.Descricao.Trim()
            });

            await _repoAtendimento.CreateAtendimentosAsync(id, entidades);
            return await _repoAtendimento.CommitAsync();
        }

        public async Task<bool> UpdateAsync(long id, CartorioUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            e.ApplyUpdate(request, DateTime.UtcNow);

            var novos = request.Atendimentos?.Select(a => new CartorioAtendimento
            {
                IdCartorio = e.Id,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                Descricao = a.Descricao.Trim()
            }) ?? [];

            await _repo.ReplaceAtendimentosAsync(e.Id, novos);
            await _repo.UpdateAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<bool> UpdateAtendimentosAsync(long id, List<CartorioAtendimentoUpdate> novos)
        {
            var entidade = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Cartório não encontrado.");
            var antigos = entidade.Atendimentos;

            foreach (var novo in novos)
            {
                var existente = antigos.FirstOrDefault(a => a.Id == novo.Id);
                if (existente is not null)
                {
                    existente.Ordem = novo.Ordem;
                    existente.Titulo = novo.Titulo.Trim();
                    existente.Descricao = novo.Descricao.Trim();
                }
            }

            await _repoAtendimento.UpdateAtendimentosAsync(id, antigos);
            return await _repoAtendimento.CommitAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<CartorioResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();
    }
}
