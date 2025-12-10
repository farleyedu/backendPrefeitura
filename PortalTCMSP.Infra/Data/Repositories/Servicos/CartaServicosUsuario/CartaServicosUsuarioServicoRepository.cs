using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoRepository(PortalTCMSPContext context)
        : BaseRepository<CartaServicosUsuarioServico>(context), ICartaServicosUsuarioServicoRepository
    {
        public async Task CreateServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos)
        {
            var entity = await context.CartaServicosUsuario
                .Include(c => c.Servicos)
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new InvalidOperationException($"CartaServicosUsuario with Id {id} not found.");

            foreach (var novo in novos)
                entity.Servicos.Add(novo);

            context.CartaServicosUsuario.Update(entity);
        }

        public async Task UpdateServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos)
        {
            var entity = await context.CartaServicosUsuario
                .Include(c => c.Servicos)
                .FirstOrDefaultAsync(c => c.Id == id) 
                ?? throw new InvalidOperationException($"CartaServicosUsuario with Id {id} not found.");

  
            context.CartaServicosUsuario.Update(entity);
        }
    }
}
