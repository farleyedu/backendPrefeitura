using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoEmentaLinkConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoEmentaLink>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoEmentaLink> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_ementa_link");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_ementa_link");

            builder.Property(x => x.Link)
                   .HasColumnName("link");

            builder.Property(x => x.EmentaId)
                   .HasColumnName("id_ementa");
        }
    }
}
