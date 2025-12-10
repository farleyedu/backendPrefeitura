//using Microsoft.EntityFrameworkCore;
//using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
//using PortalTCMSP.Infra.Data.Context;
//using PortalTCMSP.Infra.Data.Repositories.Noticia;
//using PortalTCMSP.Unit.Tests.Repository.FixFeature;
//using System.Diagnostics.CodeAnalysis;

//namespace PortalTCMSP.Unit.Tests.Repository
//{
//    [ExcludeFromCodeCoverage]
//    public class NoticiaRepositoryTest
//    {
//        private PortalTCMSPContext GetInMemoryContext(string? dbName = null)
//        {
//            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
//                .UseInMemoryDatabase(dbName ?? "NoticiaRepoDB_" + Guid.NewGuid())
//                .EnableSensitiveDataLogging()
//                .Options;
//            return new PortalTCMSPContext(options);
//        }

//        [Fact]
//        public async Task ObterPorSlugAsync_DeveRetornarNoticia_QuandoExiste()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var noticia = fx.GetNoticia(1, "slug-um");
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.Add(noticia.Categoria);
//            ctx.Noticia.Add(noticia);
//            await ctx.SaveChangesAsync();

//            var repo = new NoticiaRepository(ctx);
//            var result = await repo.ObterPorSlugAsync("slug-um");

//            Assert.NotNull(result);
//            Assert.Equal(noticia.Slug, result!.Slug);
//            Assert.Equal(noticia.Titulo, result.Titulo);
//            Assert.NotNull(result.Categoria);
//            Assert.Equal(noticia.Categoria.Nome, result.Categoria.Nome);
//            Assert.Equal(noticia.Blocos.Count, result.Blocos.Count);
//        }

//        [Fact]
//        public async Task ObterPorSlugAsync_DeveNormalizarESerCaseInsensitive()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var noticia = fx.GetNoticia(2, "slug-misto");
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.Add(noticia.Categoria);
//            ctx.Noticia.Add(noticia);
//            await ctx.SaveChangesAsync();

//            var repo = new NoticiaRepository(ctx);
//            var result = await repo.ObterPorSlugAsync("Slug-Misto");

//            Assert.NotNull(result);
//            Assert.Equal("slug-misto", result!.Slug);
//        }

//        [Fact]
//        public async Task ObterPorSlugAsync_DeveRetornarNull_QuandoNaoExiste()
//        {
//            using var ctx = GetInMemoryContext();
//            var repo = new NoticiaRepository(ctx);

//            var result = await repo.ObterPorSlugAsync("nao-existe");

//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task ObterPorIdComBlocosAsync_DeveRetornarComBlocos()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var noticia = fx.GetNoticia(5, "slug-id", blocos: 3);
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.Add(noticia.Categoria);
//            ctx.Noticia.Add(noticia);
//            await ctx.SaveChangesAsync();

//            var repo = new NoticiaRepository(ctx);
//            var result = await repo.ObterPorIdComBlocosAsync((int)noticia.Id);

//            Assert.NotNull(result);
//            Assert.Equal(3, result!.Blocos.Count);
//        }

//        [Fact]
//        public async Task GerarSlugUnicoAsync_DeveGerarComSufixoQuandoDuplicado()
//        {
//            using var ctx = GetInMemoryContext();
//            var fx = new NoticiaRepositoryFixture();
//            var cat = fx.GetCategoria();
//            var existente = fx.GetNoticia(1, "base-slug", cat);
//            ctx.Categoria.Add(cat);
//            ctx.Noticia.Add(existente);
//            await ctx.SaveChangesAsync();

//            var repo = new NoticiaRepository(ctx);

//            var novoSlug = await repo.GerarSlugUnicoAsync("Base Slug");
//            Assert.Equal("base-slug-2", novoSlug);

//            var segundo = fx.GetNoticia(2, novoSlug, cat);
//            ctx.Noticia.Add(segundo);
//            await ctx.SaveChangesAsync();

//            var terceiroSlug = await repo.GerarSlugUnicoAsync("base-slug");
//            Assert.Equal("base-slug-3", terceiroSlug);
//        }

//        [Fact]
//        public void Search_DeveFiltrarPorCategoriaNome()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var (n1, n2, n3) = fx.GetNoticiasParaBusca();
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.AddRange(n1.Categoria, n3.Categoria);
//            ctx.Noticia.AddRange(n1, n2, n3);
//            ctx.SaveChanges();

//            var repo = new NoticiaRepository(ctx);
//            var request = new NoticiaListarRequest { Categoria = "Tecnologia" };

//            var result = repo.Search(request).ToList();

//            Assert.Equal(2, result.Count);
//            Assert.All(result, r => Assert.Equal("Tecnologia", r.Categoria.Nome));
//        }

//        [Fact]
//        public void Search_DeveFiltrarPorCategoriaSlug()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var (n1, n2, n3) = fx.GetNoticiasParaBusca();
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.AddRange(n1.Categoria, n3.Categoria);
//            ctx.Noticia.AddRange(n1, n2, n3);
//            ctx.SaveChanges();

//            var repo = new NoticiaRepository(ctx);
//            var request = new NoticiaListarRequest { Categoria = n3.Categoria.Slug.ToUpperInvariant() };

//            var result = repo.Search(request).ToList();

//            Assert.Single(result);
//            Assert.Equal(n3.CategoriaId, result[0].CategoriaId);
//        }

//        [Fact]
//        public void Search_DeveFiltrarApenasAtivas()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var (n1, n2, n3) = fx.GetNoticiasParaBusca();
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.AddRange(n1.Categoria, n3.Categoria);
//            ctx.Noticia.AddRange(n1, n2, n3);
//            ctx.SaveChanges();

//            var repo = new NoticiaRepository(ctx);
//            var request = new NoticiaListarRequest { ApenasAtivas = true };

//            var result = repo.Search(request).ToList();

//            Assert.DoesNotContain(result, r => r.Id == n2.Id);
//            Assert.Equal(new[] { n1.Id, n3.Id }.OrderBy(i => i), result.Select(r => r.Id).OrderBy(i => i));
//        }

//        [Fact]
//        public void Search_DeveFiltrarPorQueryEmTituloResumoOuAutor()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var (n1, n2, n3) = fx.GetNoticiasParaBusca();
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.AddRange(n1.Categoria, n3.Categoria);
//            ctx.Noticia.AddRange(n1, n2, n3);
//            ctx.SaveChanges();

//            var repo = new NoticiaRepository(ctx);

//            var requestTitulo = new NoticiaListarRequest { Query = "cloud" };
//            var rTitulo = repo.Search(requestTitulo).ToList();
//            Assert.Single(rTitulo);
//            Assert.Equal(n1.Id, rTitulo[0].Id);

//            var requestResumo = new NoticiaListarRequest { Query = "orchestration" };
//            var rResumo = repo.Search(requestResumo).ToList();
//            Assert.Single(rResumo);
//            Assert.Equal(n2.Id, rResumo[0].Id);

//            var requestAutor = new NoticiaListarRequest { Query = "carlos" };
//            var rAutor = repo.Search(requestAutor).ToList();
//            Assert.Single(rAutor);
//            Assert.Equal(n3.Id, rAutor[0].Id);
//        }

//        [Fact]
//        public void Search_DeveOrdenarPorDestaqueDepoisDataPublicacaoDesc()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var (n1, n2, n3) = fx.GetNoticiasParaBusca();
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.AddRange(n1.Categoria, n3.Categoria);
//            ctx.Noticia.AddRange(n1, n2, n3);
//            ctx.SaveChanges();

//            var repo = new NoticiaRepository(ctx);
//            var result = repo.Search(new NoticiaListarRequest { }).ToList();

//            Assert.Equal(n1.Id, result[0].Id);
//            var tail = result.Skip(1).Select(r => r.Id).ToArray();
//            Assert.Equal(new long[] { n2.Id, n3.Id }, tail);
//        }

//        [Fact]
//        public async Task UpdateAsync_DevePersistirAlteracoes()
//        {
//            var fx = new NoticiaRepositoryFixture();
//            var noticia = fx.GetNoticia(50, "update-slug", destaque: false, ativo: true, titulo: "Original");
//            using var ctx = GetInMemoryContext();
//            ctx.Categoria.Add(noticia.Categoria);
//            ctx.Noticia.Add(noticia);
//            await ctx.SaveChangesAsync();

//            noticia.Titulo = "Alterado";
//            noticia.Destaque = true;

//            var repo = new NoticiaRepository(ctx);
//            await ((PortalTCMSP.Domain.Repositories.Home.IBaseRepository<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>)repo).UpdateAsync(noticia);

//            var reloaded = await ctx.Noticia.FirstAsync(n => n.Id == noticia.Id);
//            Assert.Equal("Alterado", reloaded.Titulo);
//            Assert.True(reloaded.Destaque);
//        }
//    }
//}
