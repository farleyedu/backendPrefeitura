using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.ContasDoPrefeito;

namespace PortalTCMSP.Unit.Tests.Repository.Fiscalizacao
{
    public class FiscalizacaoContasDoPrefeitoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase($"test_{Guid.NewGuid():N}")
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithAnexosByIdAsync_ReturnsEntityWithAnexos()
        {
            await using var ctx = CreateContext();
            var entidade = new FiscalizacaoContasDoPrefeito
            {
                Id = 1,
                Ano = "2024",
                Pauta = "Pauta A",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>
                {
                    new() { Id = 10, IdFiscalizacaoContasDoPrefeito = 1 },
                    new() { Id = 11, IdFiscalizacaoContasDoPrefeito = 1 }
                }
            };
            ctx.Add(entidade);
            await ctx.SaveChangesAsync();

            var repo = new FiscalizacaoContasDoPrefeitoRepository(ctx);
            var result = await repo.GetWithAnexosByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.NotNull(result.Anexos);
            Assert.Equal(2, result.Anexos.Count);
            Assert.Contains(result.Anexos, a => a.Id == 10);
            Assert.Contains(result.Anexos, a => a.Id == 11);
        }

        [Fact]
        public async Task AllWithAnexosAsync_ReturnsAllEntitiesIncludingAnexos()
        {
            await using var ctx = CreateContext();
            var e1 = new FiscalizacaoContasDoPrefeito
            {
                Id = 1,
                Ano = "2021",
                Pauta = "One",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo> { new() { Id = 101, IdFiscalizacaoContasDoPrefeito = 1 } }
            };
            var e2 = new FiscalizacaoContasDoPrefeito
            {
                Id = 2,
                Ano = "2022",
                Pauta = "Two",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo> { new() { Id = 201, IdFiscalizacaoContasDoPrefeito = 2 } }
            };
            ctx.AddRange(e1, e2);
            await ctx.SaveChangesAsync();

            var repo = new FiscalizacaoContasDoPrefeitoRepository(ctx);
            var all = await repo.AllWithAnexosAsync();

            Assert.NotNull(all);
            Assert.Equal(2, all.Count);
            Assert.All(all, item => Assert.NotNull(item.Anexos));
            Assert.Contains(all, x => x.Id == 1 && x.Anexos.Any(a => a.Id == 101));
            Assert.Contains(all, x => x.Id == 2 && x.Anexos.Any(a => a.Id == 201));
        }

        [Fact]
        public async Task ReplaceAnexosAsync_ReplacesExistingAnexos()
        {
            await using var ctx = CreateContext();

            var entity = new FiscalizacaoContasDoPrefeito { Id = 5, Ano = "2020", Pauta = "ReplaceTest" };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();

            var existingAnexos = new[]
            {
                new FiscalizacaoContasDoPrefeitoAnexo { Id = 1000, IdFiscalizacaoContasDoPrefeito = 5 },
                new FiscalizacaoContasDoPrefeitoAnexo { Id = 1001, IdFiscalizacaoContasDoPrefeito = 5 }
            };
            ctx.AddRange(existingAnexos);
            await ctx.SaveChangesAsync();

            var repo = new FiscalizacaoContasDoPrefeitoRepository(ctx);

            var novos = new[]
            {
                new FiscalizacaoContasDoPrefeitoAnexo { Id = 2000 },
                new FiscalizacaoContasDoPrefeitoAnexo { Id = 2001 }
            };

            await repo.ReplaceAnexosAsync(5, novos);

            await ctx.SaveChangesAsync();

            var atualmente = await ctx.FiscalizacaoContasDoPrefeitoAnexo
                .Where(a => a.IdFiscalizacaoContasDoPrefeito == 5)
                .OrderBy(a => a.Id)
                .ToListAsync();

            Assert.Equal(2, atualmente.Count);
            Assert.DoesNotContain(atualmente, a => a.Id == 1000 || a.Id == 1001);
            Assert.Contains(atualmente, a => a.Id == 2000 && a.IdFiscalizacaoContasDoPrefeito == 5);
            Assert.Contains(atualmente, a => a.Id == 2001 && a.IdFiscalizacaoContasDoPrefeito == 5);
        }

        [Fact]
        public async Task Search_FiltersAndOrdersCorrectly()
        {
            await using var ctx = CreateContext();

            var now = DateTime.UtcNow;
            var older = now.AddDays(-10);
            var newer = now.AddDays(1);

            var a = new FiscalizacaoContasDoPrefeito
            {
                Id = 1,
                Ano = "2022",
                Pauta = "Budget",
                DataSessao = older,
                DataPublicacao = null,
                DataCriacao = older
            };
            var b = new FiscalizacaoContasDoPrefeito
            {
                Id = 2,
                Ano = "2023",
                Pauta = "Audit",
                DataSessao = now,
                DataPublicacao = newer,
                DataCriacao = now
            };
            var c = new FiscalizacaoContasDoPrefeito
            {
                Id = 3,
                Ano = "2024",
                Pauta = "Extra",
                DataSessao = now.AddDays(2),
                DataPublicacao = now.AddDays(2),
                DataCriacao = now.AddDays(2)
            };

            ctx.AddRange(a, b, c);
            await ctx.SaveChangesAsync();

            var repo = new FiscalizacaoContasDoPrefeitoRepository(ctx);

            var r2 = new FiscalizacaoContasDoPrefeitoSearchRequest { Ano = "2023" };
            var list2 = await repo.Search(r2).ToListAsync();
            Assert.Single(list2);
            Assert.Equal(2, list2[0].Id);

            var r3 = new FiscalizacaoContasDoPrefeitoSearchRequest { Pauta = "Extra" };
            var list3 = await repo.Search(r3).ToListAsync();
            Assert.Single(list3);
            Assert.Equal(3, list3[0].Id);

            var r4 = new FiscalizacaoContasDoPrefeitoSearchRequest { SessaoDe = now.AddDays(-1), SessaoAte = now.AddDays(3) };
            var list4 = await repo.Search(r4).ToListAsync();
            Assert.Equal(2, list4.Count);
            Assert.DoesNotContain(list4, x => x.Id == 1);

            var r5 = new FiscalizacaoContasDoPrefeitoSearchRequest { PublicadaDe = now, PublicadaAte = now.AddDays(3) };
            var list5 = await repo.Search(r5).ToListAsync();
            Assert.Equal(2, list5.Count);
            Assert.Contains(list5, x => x.Id == 2);
            Assert.Contains(list5, x => x.Id == 3);

            var r6 = new FiscalizacaoContasDoPrefeitoSearchRequest { Query = "bud" };
            var list6 = await repo.Search(r6).ToListAsync();
            Assert.Single(list6);
            Assert.Equal(1, list6[0].Id);

            var r7 = new FiscalizacaoContasDoPrefeitoSearchRequest();
            var list7 = await repo.Search(r7).ToListAsync();
            Assert.Equal(new long[] { 3, 2, 1 }, list7.Select(x => x.Id).ToArray());
        }
    }
}