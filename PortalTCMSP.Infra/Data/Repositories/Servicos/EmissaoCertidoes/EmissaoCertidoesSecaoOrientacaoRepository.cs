using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesSecaoOrientacaoRepository(PortalTCMSPContext context)
        : BaseRepository<EmissaoCertidoesSecaoOrientacao>(context), IEmissaoCertidoesSecaoOrientacaoRepository
    {
        public async Task CreateScoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novos)
        {
            var entity = await context.EmissaoCertidoes
                .Include(p => p.SecaoOrientacoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"EmissaoCertidoes {id} não encontrado.");
            entity.SecaoOrientacoes ??= [];
            foreach (var nova in novos)
                entity.SecaoOrientacoes.Add(nova);
            context.EmissaoCertidoes.Update(entity);
        }

        public async Task UpdateSecoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novos)
        {
            var entity = await context.EmissaoCertidoes
                .Include(p => p.SecaoOrientacoes)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new InvalidOperationException($"EmissaoCertidoes {id} não encontrado.");
            entity.SecaoOrientacoes ??= [];
            context.EmissaoCertidoes.Update(entity);
        }
    }
}
