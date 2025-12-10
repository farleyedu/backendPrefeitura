using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes>(context), IOficioseIntimacoesRepository
    {
        public Task<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetWithChildrenByIdAsync(long id)
         => Set.Include(x => x.Secoes)
               .ThenInclude(s => s.SecaoItem)
               .FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes>> AllWithChildrenAsync()
            => Set.Include(x => x.Secoes)
                  .ThenInclude(s => s.SecaoItem)
                  .ToListAsync();

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.OficioseIntimacoes.FindAsync(id);
            if (entity is null)
                return false;

            entity.Ativo = false;
            context.OficioseIntimacoes.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetBySlugAtivoAsync(string slug)
        {
            return await context.OficioseIntimacoes
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Slug == slug && o.Ativo);
        }

        public async Task<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetWithChildrenBySlugAtivoAsync(string slug)
        {
            return await context.OficioseIntimacoes
                .Include(o => o.Secoes)
                .ThenInclude(s => s.SecaoItem)
                .Where(o => o.Slug == slug && o.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task ReplaceSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas)
        {
            var set = Context.Set<OficioseIntimacoesSecao>();
            var antigas = await set.Where(t => t.IdOficioseIntimacoes == id).ToListAsync();
            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novas)
                n.IdOficioseIntimacoes = id;

            await set.AddRangeAsync(novas);
        }

        public async Task ReplaceSecaoItensAsync(long idSecao, IEnumerable<OficioseIntimacoesSecaoItem> novos)
        {
            var set = Context.Set<OficioseIntimacoesSecaoItem>();
            var antigos = await set.Where(t => t.IdOficioseIntimacoesSecao == idSecao).ToListAsync();
            if (antigos.Count > 0)
                set.RemoveRange(antigos);

            foreach (var n in novos)
                n.IdOficioseIntimacoesSecao = idSecao;

            await set.AddRangeAsync(novos);
        }
    }
}
