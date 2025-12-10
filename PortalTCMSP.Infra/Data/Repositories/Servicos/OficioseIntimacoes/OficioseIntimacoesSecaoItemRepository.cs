using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesSecaoItemRepository(PortalTCMSPContext context)
        : BaseRepository<OficioseIntimacoesSecaoItem>(context), IOficioseIntimacoesSecaoItemRepository
    {
        public async Task CreateSecaoItensAsync(long idSecao, IEnumerable<OficioseIntimacoesSecaoItem> novos)
        {
            var entity = await context.OficioseIntimacoesSecao
                .Include(s => s.SecaoItem)
                .FirstOrDefaultAsync(s => s.Id == idSecao) ?? throw new InvalidOperationException($"OficioseIntimacoesSecao with Id {idSecao} not found.");

            entity.SecaoItem ??= [];

            foreach (var novo in novos)
                entity.SecaoItem.Add(novo);

            context.OficioseIntimacoesSecao.Update(entity);
        }

        public async Task UpdateSecaoItensAsync(long idSecao, IEnumerable<OficioseIntimacoesSecaoItem> novos)
        {
            var entity = await context.OficioseIntimacoesSecao
                .Include(s => s.SecaoItem)
                .FirstOrDefaultAsync(s => s.Id == idSecao) ?? throw new InvalidOperationException($"OficioseIntimacoesSecao with Id {idSecao} not found.");

            entity.SecaoItem ??= [];

            context.OficioseIntimacoesSecao.Update(entity);
        }
    }
}
