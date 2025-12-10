using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoItemDetalheRepository(PortalTCMSPContext context)
        : BaseRepository<CartaServicosUsuarioItemDetalhe>(context), ICartaServicosUsuarioServicoItemDetalheRepository
    {
        public async Task CreateServicosItensDetalhesAsync(long IdCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalhe> novos)
        {
            var entity = await context.CartaServicosUsuarioServicoItem
                .Include(c => c.ItemDetalhe)
                .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioServicoItem)
                ?? throw new InvalidOperationException($"CartaServicosUsuarioServicoItem with Id {IdCartaServicosUsuarioServicoItem} not found.");

            foreach (var novo in novos)
                entity.ItemDetalhe.Add(novo);

            context.CartaServicosUsuarioServicoItem.Update(entity);
        }

        public async Task UpdateServicosItensDetalhesAsync(long IdCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalhe> novos)
        {
            var entity = await context.CartaServicosUsuarioServicoItem
                .Include(c => c.ItemDetalhe)
                .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioServicoItem)
                ?? throw new InvalidOperationException($"CartaServicosUsuarioServicoItem with Id {IdCartaServicosUsuarioServicoItem} not found.");

            foreach (var novo in novos)
            {
                var existente = entity.ItemDetalhe.FirstOrDefault(i => i.IdCartaServicosUsuarioServicoItem == novo.IdCartaServicosUsuarioServicoItem);
                if (existente != null)
                {
                    entity.ItemDetalhe.Remove(existente);
                    entity.ItemDetalhe.Add(novo);
                }
            }

            context.CartaServicosUsuarioServicoItem.Update(entity);
        }
    }
}
