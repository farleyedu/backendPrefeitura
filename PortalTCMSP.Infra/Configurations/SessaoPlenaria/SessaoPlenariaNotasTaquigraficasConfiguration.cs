using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaNotasTaquigraficasConfiguration : IEntityTypeConfiguration<SessaoPlenariaNotasTaquigraficas>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaNotasTaquigraficas> builder)
        {
            builder.ToTable("sessoes_plenarias_notas_taquigraficas");

            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).HasColumnName("id_sessao_plenaria_notas_taquigraficas");

            builder.Property(n => n.IdSessaoPlenaria).HasColumnName("id_sessao_plenaria");

            builder.Property(n => n.Numero).HasColumnName("numero").IsRequired();
            builder.Property(a => a.Tipo)
                   .HasColumnName("tipo")
                   .HasConversion<string>()  
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(n => n.DataPublicacao).HasColumnName("data_publicacao");
            builder.Property(n => n.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(n => n.DataAtualizacao).HasColumnName("data_atualizacao");

            builder.HasOne(n => n.SessaoPlenaria)
                   .WithMany(s => s.NotasTaquigraficas) 
                   .HasForeignKey(n => n.IdSessaoPlenaria)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(n => n.Anexos)
                   .WithOne(a => a.NotasTaquigraficas)
                   .HasForeignKey(a => a.IdSessaoPlenariaNotasTaquigraficas)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
