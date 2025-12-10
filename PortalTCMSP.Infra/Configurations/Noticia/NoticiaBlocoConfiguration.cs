using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaBlocoConfiguration : IEntityTypeConfiguration<NoticiaBloco>
    {
        public void Configure(EntityTypeBuilder<NoticiaBloco> builder)
        {
            builder.ToTable("noticia_blocos");

            builder.Property(x => x.Tipo).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ConfigJson).HasColumnType("text");
            builder.Property(x => x.ValorJson).HasColumnType("text").IsRequired();

            builder.Property(b => b.CriadoEm)
                .HasColumnName("criadoEm")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
