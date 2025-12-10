using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.PrazosProcessuaisEntity
{
    [ExcludeFromCodeCoverage]
    public class PrazosProcessuaisItemAnexoConfiguration : IEntityTypeConfiguration<PrazosProcessuaisItemAnexo>
    {
        public void Configure(EntityTypeBuilder<PrazosProcessuaisItemAnexo> builder)
        {
            builder.ToTable("servicos_prazos_processuais_item_anexo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_prazos_processuais_item_anexo");

            builder.Property(x => x.IdPrazosProcessuaisItem)
                .HasColumnName("id_prazos_processuais_item")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.NomeArquivo)
                .HasColumnName("nome_arquivo")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Tipo)
                .HasColumnName("tipo")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(x => x.PrazosProcessuaisItem)
                   .WithMany(p => p.Anexos)
                   .HasForeignKey(x => x.IdPrazosProcessuaisItem)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_prazos_processuais_item_anexo__prazos_processuais_item");
        }
    }
}
