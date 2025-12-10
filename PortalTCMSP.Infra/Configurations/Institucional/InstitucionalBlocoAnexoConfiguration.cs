using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Institucional
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoAnexoConfiguration : IEntityTypeConfiguration<InstitucionalBlocoAnexo>
    {
        public void Configure(EntityTypeBuilder<InstitucionalBlocoAnexo> builder)
        {
            builder.ToTable("institucional_bloco_anexos");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id_bloco_anexo");

            builder.Property(a => a.Ordem).HasColumnName("ordem");
            builder.Property(a => a.Link).HasColumnName("link").IsRequired().HasMaxLength(2048);

            builder.Property(a => a.IdBloco).HasColumnName("id_bloco");

            builder.HasIndex(a => new { a.IdBloco, a.Ordem }).HasDatabaseName("ix_bloco_anexos_bloco_ordem");
        }
    }
}
