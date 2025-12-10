using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaNotasTaquigraficasAnexosConfiguration : IEntityTypeConfiguration<SessaoPlenariaNotasTaquigraficasAnexos>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaNotasTaquigraficasAnexos> builder)
        {
            builder.ToTable("sessoes_plenarias_notas_taquigraficas_anexos");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id_sessao_plenaria_notas_taquigraficas_anexo");

            builder.Property(a => a.IdSessaoPlenariaNotasTaquigraficas)
                   .HasColumnName("id_sessao_plenaria_notas_taquigraficas")
                   .IsRequired();

            builder.Property(a => a.Link)
                   .HasColumnName("link")
                   .IsRequired();

            builder.Property(a => a.TipoArquivo)
                   .HasColumnName("tipo_arquivo")
                   .HasMaxLength(10);

            builder.Property(a => a.NomeExibicao)
                   .HasColumnName("nome_exibicao")
                   .HasMaxLength(120);

            builder.Property(a => a.Ordem)
                   .HasColumnName("ordem")
                   .IsRequired();

            builder.HasOne(a => a.NotasTaquigraficas)
                   .WithMany(n => n.Anexos)
                   .HasForeignKey(a => a.IdSessaoPlenariaNotasTaquigraficas)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
