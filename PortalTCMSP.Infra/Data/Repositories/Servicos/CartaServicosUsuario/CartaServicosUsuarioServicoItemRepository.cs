using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoItemRepository(PortalTCMSPContext context)
        : BaseRepository<CartaServicosUsuarioServicoItem>(context), ICartaServicosUsuarioServicoItemRepository
    {
        public async Task CreateServicosItensAsync(long IdCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItem> novos)
        {
            var entity = await context.CartaServicosUsuarioServico
            .Include(c => c.ServicosItens)
            .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioServico)
            ?? throw new InvalidOperationException($"CartaServicosUsuarioServico with Id {IdCartaServicosUsuarioServico} not found.");

            foreach (var novo in novos)
                entity.ServicosItens.Add(novo);

            context.CartaServicosUsuarioServico.Update(entity);
        }

        public async Task UpdateServicosItensAsync(long IdCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItem> novos)
        {
            var entity = await context.CartaServicosUsuarioServico
                .Include(c => c.ServicosItens)
                .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioServico)
                ?? throw new InvalidOperationException($"CartaServicosUsuarioServico with Id {IdCartaServicosUsuarioServico} not found.");

            foreach (var novo in novos)
            {
                var existente = entity.ServicosItens.FirstOrDefault(i => i.IdCartaServicosUsuarioServico == novo.IdCartaServicosUsuarioServico);
                if (existente != null)
                {
                    entity.ServicosItens.Remove(existente);
                    entity.ServicosItens.Add(novo);
                }
            }

            context.CartaServicosUsuarioServico.Update(entity);
        }
    }
}
