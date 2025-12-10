using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.BannerEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.BannerConfiguration
{
    [ExcludeFromCodeCoverage]
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("banners");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("id_banner");

            builder.Property(b => b.Nome)
                   .HasColumnName("nome")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.Imagem)
                   .HasColumnName("imagem")
                   .IsRequired();

            builder.Property(b => b.Ativo)
                   .HasColumnName("ativo")
                   .IsRequired();

            builder.Property(b => b.DataCriacao).HasColumnName("data_criacao");
            builder.Property(b => b.DataAtualizacao).HasColumnName("data_atualizacao");

            builder.HasIndex(b => b.Ativo)
                   .IsUnique()
                   .HasFilter("[ativo] = 1");
        }
    }
}
