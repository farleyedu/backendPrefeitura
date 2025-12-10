using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoDistribuicaoConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoDistribuicao>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoDistribuicao> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_distribuicao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.AnexoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_distribuicao");

            builder.Property(x => x.TipoFiscalizacao)
                   .HasColumnName("tipo_fiscalizacao");

            builder.Property(x => x.TotalPAF)
                   .HasColumnName("total_paf");

            builder.Property(x => x.DezPorCentoTotalPAF)
                   .HasColumnName("DezPorCentoTotalPAF");

            builder.Property(x => x.LimitePorConselheiro)
                   .HasColumnName("limite_por_conselheiro");

            builder.Property(x => x.LimiteConselheiros)
                   .HasColumnName("limite_conselheiros");

            builder.Property(x => x.LimitePlenoeCameras)
                   .HasColumnName("limite_pleno_cameras");

            builder.Property(x => x.LimitePresidente)
                   .HasColumnName("limite_presidente");

            builder.Property(x => x.ListaDePrioridades)
                   .HasColumnName("lista_de_prioridades");
        }
    }
}
