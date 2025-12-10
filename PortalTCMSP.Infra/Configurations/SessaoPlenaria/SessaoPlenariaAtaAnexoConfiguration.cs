using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaAtaAnexoConfiguration : IEntityTypeConfiguration<SessaoPlenariaAtaAnexo>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaAtaAnexo> builder)
        {
            builder.ToTable("sessoes_plenarias_ata_anexo");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id_sessao_plenaria_ata_anexo");

            builder.Property(a => a.IdSessaoPlenariaAta).HasColumnName("id_sessao_plenaria_ata").IsRequired();
            builder.Property(a => a.Link).HasColumnName("link").IsRequired();
            builder.Property(a => a.TipoArquivo).HasColumnName("tipo_arquivo").HasMaxLength(10);
            builder.Property(a => a.NomeExibicao).HasColumnName("nome_exibicao").HasMaxLength(120);
            builder.Property(a => a.Ordem).HasColumnName("ordem").IsRequired();

            builder.HasOne(a => a.Ata)
                   .WithMany(s => s.Anexos)
                   .HasForeignKey(a => a.IdSessaoPlenariaAta)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
