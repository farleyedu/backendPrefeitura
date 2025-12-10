using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Institucional
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoConfiguration : IEntityTypeConfiguration<InstitucionalBloco>
    {
        public void Configure(EntityTypeBuilder<InstitucionalBloco> builder)
        {
            builder.ToTable("institucional_blocos");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("id_bloco");

            builder.Property(b => b.IdInstitucional).HasColumnName("id_institucional"); 
            builder.HasOne(b => b.Institucional)                                      
                   .WithMany(i => i.Blocos)
                   .HasForeignKey(b => b.IdInstitucional)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.Ordem).HasColumnName("ordem");
            builder.Property(b => b.Html).HasColumnName("html").HasColumnType("text");
            builder.Property(b => b.Titulo).HasColumnName("titulo");
            builder.Property(b => b.Subtitulo).HasColumnName("subtitulo");
            builder.Property(b => b.Ativo).HasColumnName("ativo").IsRequired().HasMaxLength(1);

            builder.HasMany(b => b.Descricoes)
                   .WithOne(d => d.Bloco)
                   .HasForeignKey(d => d.IdBloco)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Anexos)
                   .WithOne(a => a.Bloco)
                   .HasForeignKey(a => a.IdBloco)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(b => new { b.IdInstitucional, b.Ativo, b.Ordem })
                   .HasDatabaseName("ix_blocos_institucional_icativo_ordem");
        }
    }
}
