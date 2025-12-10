using PortalTCMSP.Domain.Entities.FiscalizacaoEntity;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Domain.Services.Fiscalizacao;
using PortalTCMSP.Domain.Mappings.Fiscalizacao;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;

namespace PortalTCMSP.Infra.Services.Fiscalizacao
{
    public class FiscalizacaoSecretariaControleExternoSeervice(IFiscalizacaoSecretariaControleExternoRepository repo) : IFiscalizacaoSecretariaControleExternoSeervice
    {
        private readonly IFiscalizacaoSecretariaControleExternoRepository _repo = repo;

        public async Task<IEnumerable<FiscalizacaoSecretariaResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<FiscalizacaoSecretariaResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<FiscalizacaoSecretariaResponse?> GetBySlugAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAsync(slug))?.ToResponse();

        public async Task<long> CreateAsync(FiscalizacaoSecretariaCreateRequest request)
        {
            if (await _repo.AnyAsync(x => x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um conteúdo com este slug.");

            var entity = request.FromCreate(DateTime.UtcNow);

            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(long id, FiscalizacaoSecretariaUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            if (e.Slug != request.Slug && await _repo.AnyAsync(x => x.Id != id && x.Slug == request.Slug))
                throw new InvalidOperationException("Já existe um conteúdo com este slug.");

            e.ApplyUpdate(request, DateTime.UtcNow);

            // replace listas
            var novosTitulos = request.Titulos?.Select(t => new FiscalizacaoSecretariaSecaoConteudoTitulo
            {
                IdSecaoConteudo = e.Id,
                Ordem = t.Ordem,
                Titulo = t.Titulo?.Trim() ?? string.Empty,
                ImagemUrl = t.ImagemUrl?.Trim() ?? string.Empty,
                Descricao = t.Descricao?.Trim()
            }) ?? [];

            var novosCarrossel = request.Carrossel?.Select(c => new FiscalizacaoSecretariaSecaoConteudoCarrosselItem
            {
                IdSecaoConteudo = e.Id,
                Ordem = c.Ordem,
                Titulo = c.Titulo?.Trim() ?? string.Empty,
                Descricao = c.Descricao?.Trim(),
                ImagemUrl = c.ImagemUrl?.Trim() ?? string.Empty
            }) ?? [];


            await _repo.ReplaceTitulosAsync(e.Id, novosTitulos);
            await _repo.ReplaceCarrosselAsync(e.Id, novosCarrossel);

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
