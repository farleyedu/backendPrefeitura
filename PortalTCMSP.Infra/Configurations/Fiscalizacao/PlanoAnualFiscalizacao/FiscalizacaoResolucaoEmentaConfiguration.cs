using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoEmentaConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoEmenta>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoEmenta> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_ementa");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_ementa");

            builder.Property(x => x.Descritivo)
                   .HasColumnName("descritivo");

            builder.HasMany(e => e.LinksArtigos)
                   .WithOne(l => l.Ementa)
                   .HasForeignKey(l => l.EmentaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
