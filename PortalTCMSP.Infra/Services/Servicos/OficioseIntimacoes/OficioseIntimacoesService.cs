using PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Services.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Mappings.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Services.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesService : IOficioseIntimacoesService
    {
        private readonly IOficioseIntimacoesRepository _repo;
        private readonly IOficioseIntimacoesSecaoRepository _repoSecoes;
        private readonly IOficioseIntimacoesSecaoItemRepository _repoSecaoItens;

        public OficioseIntimacoesService(
            IOficioseIntimacoesRepository repo,
            IOficioseIntimacoesSecaoRepository repoSecoes,
            IOficioseIntimacoesSecaoItemRepository repoSecaoItens)
        {
            _repo = repo;
            _repoSecoes = repoSecoes;
            _repoSecaoItens = repoSecaoItens;
        }

        public async Task<IEnumerable<OficioseIntimacoesResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<OficioseIntimacoesResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<bool> DisableAsync(long id)
            => await _repo.DisableAsync(id);

        public async Task<long> CreateAsync(OficioseIntimacoesCreateRequest request)
        {
            var existeSlugAtivo = await GetBySlugAtivoAsync(request.Slug);
            if (existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> CreateSecoesAsync(long id, List<OficioseIntimacoesSecaoRequest> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"OficioseIntimacoes with Id {id} not found.");

            var entidades = novos.Select(s => new OficioseIntimacoesSecao
            {
                IdOficioseIntimacoes = id,
                Ordem = s.Ordem,
                Nome = s.Nome.Trim(),
                SecaoItem = s.SecaoItem?.Select(i => new OficioseIntimacoesSecaoItem
                {
                    Ordem = i.Ordem,
                    Descricao = i.Descricao.Trim()
                }).ToList() ?? new List<OficioseIntimacoesSecaoItem>()
            });

            await _repoSecoes.CreateSecoesAsync(id, entidades);
            return await _repoSecoes.CommitAsync();
        }

        public async Task<bool> CreateSecaoItensAsync(long idSecao, List<OficioseIntimacoesSecaoItemRequest> novos)
        {
            var secao = await _repoSecoes.FindByIdAsync(idSecao) ?? throw new InvalidOperationException($"Secao with Id {idSecao} not found.");

            var entidades = novos.Select(i => new OficioseIntimacoesSecaoItem
            {
                IdOficioseIntimacoesSecao = idSecao,
                Ordem = i.Ordem,
                Descricao = i.Descricao.Trim()
            });

            await _repoSecaoItens.CreateSecaoItensAsync(idSecao, entidades);
            return await _repoSecaoItens.CommitAsync();
        }

        public async Task<OficioseIntimacoesResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();

        private async Task<OficioseIntimacoesResponse?> GetBySlugAtivoAsync(string slug)
            => (await _repo.GetBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<bool> UpdateAsync(long id, OficioseIntimacoesUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;

            e.ApplyUpdate(request, DateTime.UtcNow);

            var novasSecoes = request.Secoes?.Select(s => new OficioseIntimacoesSecao
            {
                IdOficioseIntimacoes = e.Id,
                Ordem = s.Ordem,
                Nome = s.Nome.Trim(),
                SecaoItem = s.SecaoItem?.Select(i => new OficioseIntimacoesSecaoItem
                {
                    Ordem = i.Ordem,
                    Descricao = i.Descricao.Trim()
                }).ToList() ?? new List<OficioseIntimacoesSecaoItem>()
            }) ?? new List<OficioseIntimacoesSecao>();

            await _repo.ReplaceSecoesAsync(e.Id, novasSecoes);

            await _repo.UpdateAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<bool> UpdateSecoesAsync(long id, List<OficioseIntimacoesSecaoUpdate> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"OficioseIntimacoes with Id {id} not found.");

            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem seções para serem atualizadas.");
            var secoesAntigas = entidadesAntigas.Secoes;

            foreach (var nova in novos)
            {
                var ef = await _repoSecoes.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Seção com Id {nova.Id} não encontrada.");

                foreach (var antiga in secoesAntigas)
                {
                    if (nova.Id == antiga.Id)
                    {
                        antiga.Ordem = nova.Ordem;
                        antiga.Nome = nova.Nome.Trim();
                    }
                }
            }

            await _repoSecoes.UpdateSecoesAsync(id, secoesAntigas);
            return await _repoSecoes.CommitAsync();
        }

        public async Task<bool> UpdateSecaoItensAsync(long idSecao, List<OficioseIntimacoesSecaoItemUpdate> novos)
        {
            var secao = await _repoSecoes.FindByIdAsync(idSecao) ?? throw new InvalidOperationException($"Seção com Id {idSecao} não encontrada.");

            var agregate = await _repo.GetWithChildrenByIdAsync(secao.IdOficioseIntimacoes) ?? throw new InvalidOperationException("Seção pai não encontrado.");
            var itensAntigos = agregate.Secoes.First(s => s.Id == idSecao).SecaoItem;

            foreach (var nova in novos)
            {
                var ef = await _repoSecaoItens.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Item com Id {nova.Id} não encontrado.");

                foreach (var antigo in itensAntigos)
                {
                    if (nova.Id == antigo.Id)
                    {
                        antigo.Ordem = nova.Ordem;
                        antigo.Descricao = nova.Descricao.Trim();
                    }
                }
            }

            await _repoSecaoItens.UpdateSecaoItensAsync(idSecao, itensAntigos);
            return await _repoSecaoItens.CommitAsync();
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
