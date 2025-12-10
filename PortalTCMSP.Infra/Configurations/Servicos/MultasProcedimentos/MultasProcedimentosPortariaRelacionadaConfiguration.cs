using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class MultasProcedimentosPortariaRelacionadaConfiguration : IEntityTypeConfiguration<MultasProcedimentosPortariaRelacionada>
{
    public void Configure(EntityTypeBuilder<MultasProcedimentosPortariaRelacionada> builder)
    {
        builder.ToTable("servicos_multas_procedimentos_portaria_relacionada");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id_multas_procedimentos_portaria_relacionada");

        builder.Property(c => c.IdMultasProcedimentos)
            .HasColumnName("id_multas_procedimentos")
            .IsRequired();

        builder.Property(c => c.Ordem)
            .HasColumnName("ordem")
            .IsRequired();

        builder.Property(c => c.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Url)
            .HasColumnName("url")
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(c => c.MultasProcedimentos)
            .WithMany(p => p.PortariasRelacionadas)
            .HasForeignKey(c => c.IdMultasProcedimentos)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_multas_procedimentos_portaria_relacionada__multas_procedimentos");
    }
}
