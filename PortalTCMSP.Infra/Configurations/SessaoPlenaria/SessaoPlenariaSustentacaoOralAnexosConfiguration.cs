using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaSustentacaoOralAnexosConfiguration : IEntityTypeConfiguration<SessaoPlenariaSustentacaoOralAnexos>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaSustentacaoOralAnexos> builder)
        {
            builder.ToTable("sessoes_plenarias_sustentacao_oral_anexos");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id_sessao_plenaria_sustentacao_oral_anexo");

            builder.Property(a => a.IdSessaoPlenariaSustentacaoOral)
                   .HasColumnName("id_sessao_plenaria_sustentacao_oral")
                   .IsRequired();

            builder.Property(a => a.Ordem).HasColumnName("ordem").HasMaxLength(20).IsRequired();
            builder.Property(a => a.Titulo).HasColumnName("titulo").HasMaxLength(255).IsRequired();
            builder.Property(a => a.Descricao).HasColumnName("descricao").HasColumnType("text");
        }
    }
}
