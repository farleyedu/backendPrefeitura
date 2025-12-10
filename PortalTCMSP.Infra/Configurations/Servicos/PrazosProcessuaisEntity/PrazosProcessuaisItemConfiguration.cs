using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.PrazosProcessuaisEntity
{
    [ExcludeFromCodeCoverage]
    public class PrazosProcessuaisItemConfiguration : IEntityTypeConfiguration<PrazosProcessuaisItem>
    {
        public void Configure(EntityTypeBuilder<PrazosProcessuaisItem> builder)
        {
            builder.ToTable("servicos_prazos_processuais_item");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_prazos_processuais_item");

            builder.Property(x => x.IdPrazosProcessuais)
                .HasColumnName("id_prazos_processuais")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.DataPublicacao)
                .HasColumnName("data_publicacao")
                .IsRequired();

            builder.Property(x => x.TempoDecorrido)
                .HasColumnName("tempo_decorrido")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(x => x.PrazosProcessuais)
                   .WithMany(p => p.PrazosProcessuaisItens)
                   .HasForeignKey(x => x.IdPrazosProcessuais)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_prazos_processuais_item__prazos_processuais");
        }
    }
}
