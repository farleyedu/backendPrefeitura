using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoIncisoConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoInciso>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoInciso> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_inciso");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.DispositivoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_inciso");

            builder.Property(x => x.Ordem)
                   .HasColumnName("ordem");

            builder.Property(x => x.Texto)
                   .HasColumnName("texto");
        }
    }
}
