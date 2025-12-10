using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.ProtocoloEletronico
{
    public class ProtocoloEletronicoAcaoRepository(PortalTCMSPContext context)
            : BaseRepository<ProtocoloEletronicoAcao>(context), IProtocoloEletronicoAcaoRepository
    {
        public async Task CreateAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas)
        {
            var entity = await context.ProtocoloEletronico
                .Include(p => p.Acoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"ProtocoloEletronico {id} não encontrado.");
            entity.Acoes ??= [];
            foreach (var nova in novas)
                entity.Acoes.Add(nova);
            context.ProtocoloEletronico.Update(entity);
        }

        public async Task UpdateAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas)
        {
            var entity = await context.ProtocoloEletronico
                .Include(p => p.Acoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"ProtocoloEletronico {id} não encontrado.");
            entity.Acoes ??= [];
            context.ProtocoloEletronico.Update(entity);
        }
    }
}
