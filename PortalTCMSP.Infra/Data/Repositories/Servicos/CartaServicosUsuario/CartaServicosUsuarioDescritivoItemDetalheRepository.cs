using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioDescritivoItemDetalheRepository(PortalTCMSPContext context)
    : BaseRepository<CartaServicosUsuarioDescritivoItemDetalhe>(context), ICartaServicosUsuarioDescritivoItemDetalheRepository
    {
        public async Task CreateDescritivoItemDetalheAsync(long IdCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos)
        {

            var entity = await context.CartaServicosUsuarioItemDetalhe
                .Include(c => c.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioItemDetalhe)
                ?? throw new InvalidOperationException($"CartaServicosUsuarioItemDetalhe with Id {IdCartaServicosUsuarioItemDetalhe} not found.");

            foreach (var novo in novos)
                entity.DescritivoItemDetalhe.Add(novo);

            context.CartaServicosUsuarioItemDetalhe.Update(entity);
        }

        public async Task UpdateDescritivoItemDetalheAsync(long IdCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos)
        {
            var entity = await context.CartaServicosUsuarioItemDetalhe
                .Include(c => c.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(c => c.Id == IdCartaServicosUsuarioItemDetalhe)
                ?? throw new InvalidOperationException($"CartaServicosUsuarioItemDetalhe with Id {IdCartaServicosUsuarioItemDetalhe} not found.");

            foreach (var novo in novos)
            {
                var existente = entity.DescritivoItemDetalhe.FirstOrDefault(i => i.IdCartaServicosUsuarioItemDetalhe == novo.IdCartaServicosUsuarioItemDetalhe);
                if (existente != null)
                {
                    entity.DescritivoItemDetalhe.Remove(existente);
                    entity.DescritivoItemDetalhe.Add(novo);
                }
            }

            context.CartaServicosUsuarioItemDetalhe.Update(entity);
        }
    }
}
