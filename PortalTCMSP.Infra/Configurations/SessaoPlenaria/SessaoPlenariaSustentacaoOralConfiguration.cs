using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaSustentacaoOralConfiguration : IEntityTypeConfiguration<SessaoPlenariaSustentacaoOral>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaSustentacaoOral> builder)
        {
            builder.ToTable("sessoes_plenarias_sustentacao_oral");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id_sessao_plenaria_sustentacao_oral");

            builder.Property(s => s.Slug).HasColumnName("slug").HasMaxLength(255).IsRequired();
            builder.Property(s => s.Titulo).HasColumnName("titulo");
            builder.Property(s => s.Descricao).HasColumnName("descricao").HasColumnType("text");

            builder.Property(s => s.Ativo).HasColumnName("ativo").IsRequired();
            builder.Property(s => s.DataCriacao).HasColumnName("data_criacao");
            builder.Property(s => s.DataAtualizacao).HasColumnName("data_atualizacao");

            builder.HasMany(s => s.Anexos)
                   .WithOne(a => a.SustentacaoOral)
                   .HasForeignKey(a => a.IdSessaoPlenariaSustentacaoOral)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Ativo)
                   .HasFilter("([ativo] = 1)")
                   .IsUnique()
                   .HasDatabaseName("ux_sustentacao_oral_ativo_unico");
        }
    }
}
