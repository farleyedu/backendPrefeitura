using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Institucional
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoDescricaoConfiguration : IEntityTypeConfiguration<InstitucionalBlocoDescricao>
    {
        public void Configure(EntityTypeBuilder<InstitucionalBlocoDescricao> builder)
        {
            builder.ToTable("institucional_bloco_descricoes");

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).HasColumnName("id_bloco_descricao");

            builder.Property(d => d.Ordem).HasColumnName("ordem");
            builder.Property(d => d.Texto).HasColumnName("texto").IsRequired().HasColumnType("text");

            builder.Property(d => d.IdBloco).HasColumnName("id_bloco");

            builder.HasIndex(d => new { d.IdBloco, d.Ordem }).HasDatabaseName("ix_bloco_descricoes_bloco_ordem");
        }
    }
}
