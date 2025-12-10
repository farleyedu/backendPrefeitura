using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Text.Json;

namespace PortalTCMSP.Infra.Configurations.Noticia
{
    public class NoticiaOldConfiguration : IEntityTypeConfiguration<NoticiaOld>
    {
        public void Configure(EntityTypeBuilder<NoticiaOld> b)
        {
            b.ToTable("noticias_old"); 

            b.HasKey(x => x.Id);
            b.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired();

            b.Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasMaxLength(300)
                .IsRequired();

            b.Property(x => x.Titulo)
                .HasColumnName("Titulo")
                .HasMaxLength(500)
                .IsRequired();

            b.Property(x => x.Subtitulo)
                .HasColumnName("Subtitulo")
                .HasMaxLength(1000);

            b.Property(x => x.Resumo)
                .HasColumnName("Resumo");

            var arrayConverter = new ValueConverter<string[]?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrWhiteSpace(v) ? null : JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions?)null)
            );
            var arrayComparer = new ValueComparer<string[]?>(
                (l, r) => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(r, (JsonSerializerOptions?)null),
                v => v == null ? 0 : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),
                v => v == null ? null : v.ToArray()
            );

            b.Property(x => x.Tags)
                .HasColumnName("Tags")
                .HasConversion(arrayConverter)
                .Metadata.SetValueComparer(arrayComparer);

            b.Property(x => x.CategoriasExtras)
                .HasColumnName("CategoriasExtras")
                .HasConversion(arrayConverter)
                .Metadata.SetValueComparer(arrayComparer);

            b.Property(x => x.Autor_Nome)
                .HasColumnName("autor_nome")
                .HasMaxLength(300);

            b.Property(x => x.Autor_Creditos)
                .HasColumnName("autor_creditos")
                .HasMaxLength(500);

            b.Property(x => x.Seo_Title)
                .HasColumnName("seo_title")
                .HasMaxLength(300);

            b.Property(x => x.Seo_Description)
                .HasColumnName("seo_description");

            b.Property(x => x.Og_Image_Url)
                .HasColumnName("og_image_url")
                .HasMaxLength(1000);

            b.Property(x => x.Canonical)
                .HasColumnName("canonical")
                .HasMaxLength(1000);

            b.Property(x => x.DataPublicacao)
                .HasColumnName("DataPublicacao")
                .HasColumnType("datetime2(7)")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            b.Property(x => x.Ativo)
                .HasColumnName("Ativo")
                .HasDefaultValue(true);

            b.Property(x => x.Destaque)
                .HasColumnName("Destaque")
                .HasDefaultValue(false);

            b.Property(x => x.Visualizacao)
                .HasColumnName("Visualizacao");

            b.Property(x => x.ImagemUrl)
                .HasColumnName("ImagemUrl")
                .HasMaxLength(1000);

            b.Property(x => x.ConteudoNoticia)
                .HasColumnName("ConteudoNoticia");

            b.Property(x => x.Criado_Em)
                .HasColumnName("criado_em")
                .HasColumnType("datetime2(7)")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            b.Property(x => x.Criado_Por)
                .HasColumnName("criado_por")
                .HasMaxLength(200);

            b.Property(x => x.Atualizado_Em)
                .HasColumnName("atualizado_em")
                .HasColumnType("datetime2(7)");

            b.Property(x => x.Atualizado_Por)
                .HasColumnName("atualizado_por")
                .HasMaxLength(200);

            b.Property(x => x.Versao)
                .HasColumnName("versao");

            b.Property(x => x.CategoriaId)
                .HasColumnName("CategoriaId");

            b.HasIndex(x => x.Slug).IsUnique();
        }
    }
}
