using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaAtaConfiguration : IEntityTypeConfiguration<SessaoPlenariaAta>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaAta> builder)
        {
            builder.ToTable("sessoes_plenarias_ata");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id_sessao_plenaria_ata");

            builder.Property(s => s.IdSessaoPlenaria).HasColumnName("id_sessao_plenaria");
            builder.Property(s => s.Numero).HasColumnName("numero").HasMaxLength(50);
            builder.Property(a => a.Tipo)
                   .HasColumnName("tipo")
                   .HasConversion<string>() 
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(s => s.DataPublicacao).HasColumnName("data_publicacao");
            builder.Property(s => s.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(s => s.DataAtualizacao).HasColumnName("data_atualizacao");

            builder.HasOne(s => s.SessaoPlenaria)
                   .WithMany(p => p.Atas)
                   .HasForeignKey(s => s.IdSessaoPlenaria)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}