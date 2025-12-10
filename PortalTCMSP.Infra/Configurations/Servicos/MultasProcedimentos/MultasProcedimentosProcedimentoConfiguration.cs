using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public class MultasProcedimentosProcedimentoConfiguration : IEntityTypeConfiguration<MultasProcedimentosProcedimento>
    {
        public void Configure(EntityTypeBuilder<MultasProcedimentosProcedimento> builder)
        {
            builder.ToTable("servicos_multas_procedimentos_procedimento");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .HasColumnName("id_multas_procedimentos_procedimento");

            builder.Property(t => t.IdMultasProcedimentos)
                .HasColumnName("id_multas_procedimentos")
                .IsRequired();

            builder.Property(t => t.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(t => t.Texto)
                .HasColumnName("texto")
                .IsRequired();

            builder.Property(t => t.UrlImagem)
                .HasColumnName("url_imagem");

            builder.HasOne(t => t.MultasProcedimentos)
                .WithMany(c => c.Procedimentos)
                .HasForeignKey(t => t.IdMultasProcedimentos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_multas_procedimentos_procedimento__multas_procedimentos");
        }
    }

}
