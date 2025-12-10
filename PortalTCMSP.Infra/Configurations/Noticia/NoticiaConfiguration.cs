using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace PortalTCMSP.Infra.Configurations.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaConfiguration : IEntityTypeConfiguration<Domain.Entities.NoticiaEntity.Noticia>
    {
        // Helpers sem null-propagation, usados pelo ValueComparer
        private static bool StringArrayEquals(string[]? a, string[]? b)
            => a == b || a != null && b != null && a.Length == b.Length && a.SequenceEqual(b);

        private static int StringArrayHashCode(string[]? v)
        {
            if (v == null) return 0;
            var hash = new HashCode();
            foreach (var s in v)
            {
                // nada de s?.GetHashCode() — use checagem explícita
                hash.Add(s == null ? 0 : s.GetHashCode());
            }
            return hash.ToHashCode();
        }

        private static string[]? StringArraySnapshot(string[]? v)
            => v == null ? null : v.ToArray();

        public void Configure(EntityTypeBuilder<Domain.Entities.NoticiaEntity.Noticia> b)
        {
            b.ToTable("noticias");

            b.HasIndex(x => x.Slug).IsUnique();

            b.Property(x => x.Slug).HasMaxLength(300).IsRequired();
            b.Property(x => x.Titulo).HasMaxLength(500).IsRequired();
            b.Property(x => x.Subtitulo).HasMaxLength(1000);
            b.Property(x => x.Resumo).HasMaxLength(2000);
            b.Property(x => x.ImagemUrl).HasMaxLength(1000);

            // --------- Arrays (SQL Server): JSON em nvarchar(max) + ValueComparer (sem ?.) ---------
            var json = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            var stringArrayComparer = new ValueComparer<string[]?>(
                (a, b) => StringArrayEquals(a, b),
                v => StringArrayHashCode(v),
                v => StringArraySnapshot(v)
            );

            b.Property(x => x.Tags)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, json),
                    v => string.IsNullOrWhiteSpace(v) ? null : JsonSerializer.Deserialize<string[]>(v, json))
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(stringArrayComparer);

            b.Property(x => x.CategoriasExtras)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, json),
                    v => string.IsNullOrWhiteSpace(v) ? null : JsonSerializer.Deserialize<string[]>(v, json))
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(stringArrayComparer);
            // ------------------------------------------------------------------------------

            // Conteúdo longo
            b.Property(x => x.ConteudoNoticia).HasColumnType("nvarchar(max)");

            // Owned types
            b.OwnsOne(x => x.Autoria, nav =>
            {
                nav.Property(p => p.AutorNome).HasColumnName("autor_nome").HasMaxLength(300);
                nav.Property(p => p.Creditos).HasColumnName("autor_creditos").HasMaxLength(500);
            });

            b.Navigation(x => x.Autoria).IsRequired(false);

            b.OwnsOne(x => x.Metadados, nav =>
            {
                nav.Property(p => p.SeoTitle).HasColumnName("seo_title").HasMaxLength(300);
                nav.Property(p => p.SeoDescription).HasColumnName("seo_description").HasMaxLength(500);
                nav.Property(p => p.OgImageUrl).HasColumnName("og_image_url").HasMaxLength(1000);
                nav.Property(p => p.Canonical).HasColumnName("canonical").HasMaxLength(1000);
            });

            b.Navigation(x => x.Metadados).IsRequired(false);

            b.OwnsOne(x => x.Auditoria, nav =>
            {
                nav.Property(p => p.CriadoEm).HasColumnName("criado_em").IsRequired();
                nav.Property(p => p.CriadoPor).HasColumnName("criado_por").HasMaxLength(200);
                nav.Property(p => p.AtualizadoEm).HasColumnName("atualizado_em").IsRequired();
                nav.Property(p => p.AtualizadoPor).HasColumnName("atualizado_por").HasMaxLength(200);
                nav.Property(p => p.Versao).HasColumnName("versao").HasDefaultValue(1);
            });

            b.Navigation(x => x.Auditoria).IsRequired(false);

            // Blocos 1:N
            b.HasMany(x => x.Blocos)
             .WithOne(x => x.Noticia)
             .HasForeignKey(x => x.NoticiaId)
             .OnDelete(DeleteBehavior.Cascade);

            // Categorias N:N (lado Noticia -> join)
            b.HasMany(x => x.NoticiaCategorias)
             .WithOne(nc => nc.Noticia)
             .HasForeignKey(nc => nc.NoticiaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
