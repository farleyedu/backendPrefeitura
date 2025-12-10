using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos
{
    public class CartaServicosUsuarioRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithNestedChildren()
        {
            using var context = CreateContext(nameof(GetWithChildrenByIdAsync_ReturnsEntityWithNestedChildren));
            var repo = new CartaServicosUsuarioRepository(context);

            var descritivo = new CartaServicosUsuarioDescritivoItemDetalhe { Descritivo = "d", Ordem = 1 };
            var detalhe = new CartaServicosUsuarioItemDetalhe { TituloDetalhe = "det", DescritivoItemDetalhe = new List<CartaServicosUsuarioDescritivoItemDetalhe> { descritivo } };
            var item = new CartaServicosUsuarioServicoItem { Titulo = "item", ItemDetalhe = new List<CartaServicosUsuarioItemDetalhe> { detalhe } };
            var servico = new CartaServicosUsuarioServico { Titulo = "s", ServicosItens = new List<CartaServicosUsuarioServicoItem> { item } };
            var carta = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { TituloPagina = "p", Servicos = new List<CartaServicosUsuarioServico> { servico } };

            context.CartaServicosUsuario.Add(carta);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenByIdAsync(carta.Id);

            Assert.NotNull(result);
            Assert.NotNull(result.Servicos);
            Assert.Single(result.Servicos);
            Assert.NotNull(result.Servicos.First().ServicosItens);
            Assert.Single(result.Servicos.First().ServicosItens);
            Assert.NotNull(result.Servicos.First().ServicosItens.First().ItemDetalhe);
            Assert.Single(result.Servicos.First().ServicosItens.First().ItemDetalhe);
            Assert.NotNull(result.Servicos.First().ServicosItens.First().ItemDetalhe.First().DescritivoItemDetalhe);
            Assert.Single(result.Servicos.First().ServicosItens.First().ItemDetalhe.First().DescritivoItemDetalhe);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllEntitiesWithChildren()
        {
            using var context = CreateContext(nameof(AllWithChildrenAsync_ReturnsAllEntitiesWithChildren));
            var repo = new CartaServicosUsuarioRepository(context);

            var carta1 = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { TituloPagina = "p1", Servicos = new List<CartaServicosUsuarioServico> { new() { Titulo = "s1" } } };
            var carta2 = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { TituloPagina = "p2", Servicos = new List<CartaServicosUsuarioServico> { new() { Titulo = "s2" } } };

            context.CartaServicosUsuario.AddRange(carta1, carta2);
            await context.SaveChangesAsync();

            var result = await repo.AllWithChildrenAsync();

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.NotNull(r.Servicos));
            Assert.Contains(result, r => r.TituloPagina == "p1");
            Assert.Contains(result, r => r.TituloPagina == "p2");
        }

        [Fact]
        public async Task DisableAsync_ExistingId_DisablesAndReturnsTrue()
        {
            using var context = CreateContext(nameof(DisableAsync_ExistingId_DisablesAndReturnsTrue));
            var repo = new CartaServicosUsuarioRepository(context);

            var carta = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { TituloPagina = "p", Ativo = true };
            context.CartaServicosUsuario.Add(carta);
            await context.SaveChangesAsync();

            var result = await repo.DisableAsync(carta.Id);

            Assert.True(result);
            var fromDb = await context.CartaServicosUsuario.FindAsync(carta.Id);
            Assert.False(fromDb!.Ativo);
        }

        [Fact]
        public async Task DisableAsync_NonExistingId_ReturnsFalse()
        {
            using var context = CreateContext(nameof(DisableAsync_NonExistingId_ReturnsFalse));
            var repo = new CartaServicosUsuarioRepository(context);

            var result = await repo.DisableAsync(9999);
            Assert.False(result);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsOnlyActiveBySlug()
        {
            using var context = CreateContext(nameof(GetBySlugAtivoAsync_ReturnsOnlyActiveBySlug));
            var repo = new CartaServicosUsuarioRepository(context);

            var active = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Slug = "slug-1", Ativo = true, TituloPagina = "active" };
            var inactive = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Slug = "slug-1", Ativo = false, TituloPagina = "inactive" };

            context.CartaServicosUsuario.AddRange(active, inactive);
            await context.SaveChangesAsync();

            var result = await repo.GetBySlugAtivoAsync("slug-1");

            Assert.NotNull(result);
            Assert.Equal(active.TituloPagina, result!.TituloPagina);
            Assert.True(result.Ativo);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsEntityWithChildrenWhenActive()
        {
            using var context = CreateContext(nameof(GetWithChildrenBySlugAtivoAsync_ReturnsEntityWithChildrenWhenActive));
            var repo = new CartaServicosUsuarioRepository(context);

            var servico = new CartaServicosUsuarioServico { Titulo = "s" };
            var carta = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Slug = "by-slug", Ativo = true, Servicos = new List<CartaServicosUsuarioServico> { servico } };

            context.CartaServicosUsuario.Add(carta);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenBySlugAtivoAsync("by-slug");

            Assert.NotNull(result);
            Assert.True(result!.Ativo);
            Assert.NotEmpty(result.Servicos);
        }

        [Fact]
        public async Task ReplaceDescritivoItemDetalheAsync_ReplacesExistingEntries()
        {
            using var context = CreateContext(nameof(ReplaceDescritivoItemDetalheAsync_ReplacesExistingEntries));
            var repo = new CartaServicosUsuarioRepository(context);

            var detalhe = new CartaServicosUsuarioItemDetalhe { TituloDetalhe = "old" };
            context.CartaServicosUsuarioItemDetalhe.Add(detalhe);
            await context.SaveChangesAsync();

            var existing = new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = detalhe.Id, Descritivo = "to-remove" };
            context.CartaServicosUsuarioDescritivoItemDetalhe.Add(existing);
            await context.SaveChangesAsync();

            var novos = new List<CartaServicosUsuarioDescritivoItemDetalhe>
            {
                new() { Descritivo = "new1" },
                new() { Descritivo = "new2" }
            };

            await repo.ReplaceDescritivoItemDetalheAsync(detalhe.Id, novos);
            await context.SaveChangesAsync();

            var list = await context.CartaServicosUsuarioDescritivoItemDetalhe.Where(x => x.IdCartaServicosUsuarioItemDetalhe == detalhe.Id).ToListAsync();
            Assert.Equal(2, list.Count);
            Assert.All(list, d => Assert.Equal(detalhe.Id, d.IdCartaServicosUsuarioItemDetalhe));
            Assert.Contains(list, d => d.Descritivo == "new1");
            Assert.Contains(list, d => d.Descritivo == "new2");
        }

        [Fact]
        public async Task ReplaceItemDetalheAsync_ReplacesExistingEntries()
        {
            using var context = CreateContext(nameof(ReplaceItemDetalheAsync_ReplacesExistingEntries));
            var repo = new CartaServicosUsuarioRepository(context);

            var servicoItem = new CartaServicosUsuarioServicoItem { Titulo = "si" };
            context.CartaServicosUsuarioServicoItem.Add(servicoItem);
            await context.SaveChangesAsync();

            var existing = new CartaServicosUsuarioItemDetalhe { IdCartaServicosUsuarioServicoItem = servicoItem.Id, TituloDetalhe = "old" };
            context.CartaServicosUsuarioItemDetalhe.Add(existing);
            await context.SaveChangesAsync();

            var novos = new List<CartaServicosUsuarioItemDetalhe>
            {
                new() { TituloDetalhe = "n1" },
                new() { TituloDetalhe = "n2" }
            };

            await repo.ReplaceItemDetalheAsync(servicoItem.Id, novos);
            await context.SaveChangesAsync();

            var list = await context.CartaServicosUsuarioItemDetalhe.Where(x => x.IdCartaServicosUsuarioServicoItem == servicoItem.Id).ToListAsync();
            Assert.Equal(2, list.Count);
            Assert.All(list, d => Assert.Equal(servicoItem.Id, d.IdCartaServicosUsuarioServicoItem));
            Assert.Contains(list, d => d.TituloDetalhe == "n1");
            Assert.Contains(list, d => d.TituloDetalhe == "n2");
        }

        [Fact]
        public async Task ReplaceServicosAsync_ReplacesExistingEntries()
        {
            using var context = CreateContext(nameof(ReplaceServicosAsync_ReplacesExistingEntries));
            var repo = new CartaServicosUsuarioRepository(context);

            var carta = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { TituloPagina = "c" };
            context.CartaServicosUsuario.Add(carta);
            await context.SaveChangesAsync();

            var existing = new CartaServicosUsuarioServico { IdCartaServicosUsuario = carta.Id, Titulo = "old" };
            context.CartaServicosUsuarioServico.Add(existing);
            await context.SaveChangesAsync();

            var novos = new List<CartaServicosUsuarioServico>
            {
                new() { Titulo = "s1" },
                new() { Titulo = "s2" }
            };

            await repo.ReplaceServicosAsync(carta.Id, novos);
            await context.SaveChangesAsync();

            var list = await context.CartaServicosUsuarioServico.Where(x => x.IdCartaServicosUsuario == carta.Id).ToListAsync();
            Assert.Equal(2, list.Count);
            Assert.All(list, s => Assert.Equal(carta.Id, s.IdCartaServicosUsuario));
            Assert.Contains(list, s => s.Titulo == "s1");
            Assert.Contains(list, s => s.Titulo == "s2");
        }

        [Fact]
        public async Task ReplaceServicosItensAsync_ReplacesExistingEntries()
        {
            using var context = CreateContext(nameof(ReplaceServicosItensAsync_ReplacesExistingEntries));
            var repo = new CartaServicosUsuarioRepository(context);

            var servico = new CartaServicosUsuarioServico { Titulo = "serv" };
            context.CartaServicosUsuarioServico.Add(servico);
            await context.SaveChangesAsync();

            var existing = new CartaServicosUsuarioServicoItem { IdCartaServicosUsuarioServico = servico.Id, Titulo = "old" };
            context.CartaServicosUsuarioServicoItem.Add(existing);
            await context.SaveChangesAsync();

            var novos = new List<CartaServicosUsuarioServicoItem>
            {
                new() { Titulo = "i1" },
                new() { Titulo = "i2" }
            };

            await repo.ReplaceServicosItensAsync(servico.Id, novos);
            await context.SaveChangesAsync();

            var list = await context.CartaServicosUsuarioServicoItem.Where(x => x.IdCartaServicosUsuarioServico == servico.Id).ToListAsync();
            Assert.Equal(2, list.Count);
            Assert.All(list, s => Assert.Equal(servico.Id, s.IdCartaServicosUsuarioServico));
            Assert.Contains(list, s => s.Titulo == "i1");
            Assert.Contains(list, s => s.Titulo == "i2");
        }
    }
}