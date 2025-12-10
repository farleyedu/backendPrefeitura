using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.CategoriaConfiguration
{
    [ExcludeFromCodeCoverage]
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categorias");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id");

            builder.Property(c => c.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Slug)
                .HasColumnName("slug")
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(c => c.Slug)
                .IsUnique()
                .HasDatabaseName("ix_categoria_slug");
        }
    }
}
