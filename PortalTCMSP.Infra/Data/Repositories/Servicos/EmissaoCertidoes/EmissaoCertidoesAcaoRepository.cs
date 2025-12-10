using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesAcaoRepository(PortalTCMSPContext context)
        : BaseRepository<EmissaoCertidoesAcao>(context), IEmissaoCertidoesAcaoRepository
    {
        public async Task CreateAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos)
        {
            var entity = await context.EmissaoCertidoes
                .Include(p => p.Acoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"EmissaoCertidoes {id} não encontrado.");
            entity.Acoes ??= [];
            foreach (var nova in novos)
                entity.Acoes.Add(nova);
            context.EmissaoCertidoes.Update(entity);
        }

        public async Task UpdateAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos)
        {
            var entity = await context.EmissaoCertidoes
                .Include(p => p.Acoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"EmissaoCertidoes {id} não encontrado.");
            entity.Acoes ??= [];
            context.EmissaoCertidoes.Update(entity);
        }
    }
}
