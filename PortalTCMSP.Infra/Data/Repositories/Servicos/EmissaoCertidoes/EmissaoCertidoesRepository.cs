using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;
using System.Collections.Generic;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>(context), IEmissaoCertidoesRepository
    {
        public Task<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetWithChildrenByIdAsync(long id)
            => Set.Include(x => x.Acoes).Include(x => x.SecaoOrientacoes).ThenInclude(s => s.Orientacoes).ThenInclude(o => o.Descritivos)
                   .FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>> AllWithChildrenAsync()
            => Set.Include(x => x.Acoes).Include(x => x.SecaoOrientacoes).ThenInclude(s => s.Orientacoes).ThenInclude(o => o.Descritivos)
                   .ToListAsync();

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.EmissaoCertidoes.FindAsync(id);
            if (entity is null) return false;
            entity.Ativo = false;
            context.EmissaoCertidoes.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetBySlugAtivoAsync(string slug)
        {
            return await context.EmissaoCertidoes
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Slug == slug && o.Ativo);
        }

        public async Task<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetWithChildrenBySlugAtivoAsync(string slug)
            => await context.EmissaoCertidoes
                .Include(p => p.Acoes)
                .Include(p => p.SecaoOrientacoes)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.Ativo);

        public async Task ReplaceAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos)
        {
            var set = Context.Set<EmissaoCertidoesAcao>();
            var antigos = await set.Where(x => x.IdEmissaoCertidoes == id).ToListAsync();
            if (antigos.Any()) set.RemoveRange(antigos);
            foreach (var n in novos) n.IdEmissaoCertidoes = id;
            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceSecoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novas)
        {
            var secSet = Context.Set<EmissaoCertidoesSecaoOrientacao>();
            var oriSet = Context.Set<EmissaoCertidoesOrientacao>();
            var desSet = Context.Set<EmissaoCertidoesDescritivo>();

            var antigas = await secSet.Where(s => s.IdEmissaoCertidoes == id).ToListAsync();
            if (antigas.Any())
            {
                var orientacaoIds = antigas.SelectMany(a => Context.Set<EmissaoCertidoesOrientacao>().Where(o => o.IdEmissaoCertidoesSecaoOrientacao == a.Id).Select(o => o.Id)).ToList();
                if (orientacaoIds.Any())
                {
                    var desAnt = await desSet.Where(d => orientacaoIds.Contains(d.IdEmissaoCertidoesOrientacao)).ToListAsync();
                    if (desAnt.Any()) desSet.RemoveRange(desAnt);
                    var oriAnt = await oriSet.Where(o => orientacaoIds.Contains(o.Id)).ToListAsync();
                    if (oriAnt.Any()) oriSet.RemoveRange(oriAnt);
                }

                secSet.RemoveRange(antigas);
            }

            foreach (var n in novas) n.IdEmissaoCertidoes = id;
            await secSet.AddRangeAsync(novas);
        }
    }
}
