using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.Cartorio;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.Cartorio
{
    public class CartorioAtendimentoRepository(PortalTCMSPContext context)
        : BaseRepository<CartorioAtendimento>(context), ICartorioAtendimentoRepository
    {
        public async Task CreateAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos)
        {
            var entity = await context.Cartorio
                .Include(c => c.Atendimentos)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new InvalidOperationException($"Cartorio with Id {id} not found.");

            foreach (var novo in novos)
                entity.Atendimentos.Add(novo);

            context.Cartorio.Update(entity);
        }

        public async Task UpdateAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos)
        {
            var entity = await context.Cartorio
                .Include(c => c.Atendimentos)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new InvalidOperationException($"Cartorio with Id {id} not found.");

            context.Cartorio.Update(entity);
        }
    }
}
