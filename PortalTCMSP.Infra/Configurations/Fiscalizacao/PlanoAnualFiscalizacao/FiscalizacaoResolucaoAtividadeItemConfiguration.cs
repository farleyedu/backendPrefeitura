using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAtividadeItemConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoAtividadeItem>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoAtividadeItem> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_atividade_item");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.AtividadeId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_atividade_item");

            builder.Property(x => x.Descricao)
                   .HasColumnName("descricao");

            builder.Property(x => x.Quantidade)
                   .HasColumnName("quantidade");

            builder.Property(x => x.DUSFs)
                   .HasColumnName("dusfs");
        }
    }
}
