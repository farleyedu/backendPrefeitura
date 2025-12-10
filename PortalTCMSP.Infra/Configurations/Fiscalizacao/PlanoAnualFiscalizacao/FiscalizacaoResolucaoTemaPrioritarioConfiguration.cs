using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoTemaPrioritarioConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoTemaPrioritario>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoTemaPrioritario> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_tema");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.AnexoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_tema");

            builder.Property(x => x.Ordem)
                   .HasColumnName("ordem");

            builder.Property(x => x.Tema)
                   .HasColumnName("tema");

            builder.Property(x => x.Descricao)
                   .HasColumnName("descricao");
        }
    }
}
