using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario.Domain.Interfaces.Servicos.CartaServicosUsuarioInterface;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>(context), ICartaServicosUsuarioRepository
    {

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByServicoIdWithChildrenAsync(long servicoId)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Servicos.Any(s => s.Id == servicoId));
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByServicoItemIdWithChildrenAsync(long servicoItemId)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Servicos
                    .Any(s => s.ServicosItens.Any(i => i.Id == servicoItemId)));
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByItemDetalheIdWithChildrenAsync(long itemDetalheId)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Servicos
                    .Any(s => s.ServicosItens
                        .Any(i => i.ItemDetalhe.Any(d => d.Id == itemDetalheId))));
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByDescritivoIdWithChildrenAsync(long descritivoId)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Servicos
                    .Any(s => s.ServicosItens
                        .Any(i => i.ItemDetalhe
                            .Any(d => d.DescritivoItemDetalhe.Any(di => di.Id == descritivoId)))));
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetWithChildrenByIdAsync(long id)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> Search(
    CartaServicosUsuarioDescritivoItemDetalheSearchRequest request)
        {
            var query = context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .Where(x => x.Ativo);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var termo = request.Query.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.TituloPagina!, $"%{termo}%") ||
                    EF.Functions.Like(x.TituloPesquisa!, $"%{termo}%") ||

                    x.Servicos!.Any(s =>
                        EF.Functions.Like(s.Titulo!, $"%{termo}%") ||

                        s.ServicosItens!.Any(i =>
                            EF.Functions.Like(i.Titulo!, $"%{termo}%") ||

                            i.ItemDetalhe!.Any(d =>
                                EF.Functions.Like(d.TituloDetalhe!, $"%{termo}%") ||
                                d.DescritivoItemDetalhe!.Any(di =>
                                    EF.Functions.Like(di.Descritivo!, $"%{termo}%")
                                )
                            )
                        )
                    )
                );
            }

            return await query
                .OrderBy(x => x.Id) 
                .FirstOrDefaultAsync();
        }

        public async Task<List<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>> AllWithChildrenAsync()
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .ToListAsync();
        }

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.CartaServicosUsuario.FindAsync(id);
            if (entity is null) return false;

            entity.Ativo = false;
            context.CartaServicosUsuario.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetBySlugAtivoAsync(string slug)
            => await context.CartaServicosUsuario.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug && c.Ativo);

        public async Task<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetWithChildrenBySlugAtivoAsync(string slug)
        {
            return await context.CartaServicosUsuario
                .Include(x => x.Servicos)
                    .ThenInclude(s => s.ServicosItens)
                        .ThenInclude(i => i.ItemDetalhe)
                            .ThenInclude(d => d.DescritivoItemDetalhe)
                .FirstOrDefaultAsync(x => x.Slug == slug && x.Ativo);
        }

        public async Task ReplaceDescritivoItemDetalheAsync(long id, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos)
        {
            var set = context.Set<CartaServicosUsuarioDescritivoItemDetalhe>();
            var antigas = await set
                .Where(x => x.IdCartaServicosUsuarioItemDetalhe == id)
                .ToListAsync();

            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novos)
                n.IdCartaServicosUsuarioItemDetalhe = id;

            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceItemDetalheAsync(long id, IEnumerable<CartaServicosUsuarioItemDetalhe> novos)
        {
            var set = context.Set<CartaServicosUsuarioItemDetalhe>();
            var antigas = await set
                .Where(x => x.IdCartaServicosUsuarioServicoItem == id)
                .ToListAsync();

            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novos)
                n.IdCartaServicosUsuarioServicoItem = id;

            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos)
        {
            var set = context.Set<CartaServicosUsuarioServico>();
            var antigas = await set
                .Where(x => x.IdCartaServicosUsuario == id)
                .ToListAsync();

            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novos)
                n.IdCartaServicosUsuario = id;

            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceServicosItensAsync(long id, IEnumerable<CartaServicosUsuarioServicoItem> novos)
        {
            var set = context.Set<CartaServicosUsuarioServicoItem>();
            var antigas = await set
                .Where(x => x.IdCartaServicosUsuarioServico == id)
                .ToListAsync();

            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novos)
                n.IdCartaServicosUsuarioServico = id;

            await set.AddRangeAsync(novos);
        }
    }
}
