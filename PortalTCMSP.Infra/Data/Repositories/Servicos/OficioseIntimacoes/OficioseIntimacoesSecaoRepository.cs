using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesSecaoRepository(PortalTCMSPContext context)
        : BaseRepository<OficioseIntimacoesSecao>(context), IOficioseIntimacoesSecaoRepository
    {
        public async Task CreateSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas)
        {
            var entity = await context.OficioseIntimacoes
                .Include(o => o.Secoes)
                .FirstOrDefaultAsync(o => o.Id == id) ?? throw new InvalidOperationException($"OficioseIntimacoes with Id {id} not found.");

            entity.Secoes ??= [];

            foreach (var nova in novas)
                entity.Secoes.Add(nova);

            context.OficioseIntimacoes.Update(entity);
        }

        public async Task UpdateSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas)
        {
            var entity = await context.OficioseIntimacoes
                .Include(o => o.Secoes)
                .FirstOrDefaultAsync(o => o.Id == id) ?? throw new InvalidOperationException($"OficioseIntimacoes with Id {id} not found.");

            entity.Secoes ??= [];

            context.OficioseIntimacoes.Update(entity);
        }
    }
}
