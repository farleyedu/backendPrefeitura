using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAtaConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoAta>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoAta> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_ata");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ResolucaoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_ata");

            builder.Property(x => x.Ordem)
                   .HasColumnName("ordem");

            builder.Property(x => x.TituloAta)
                   .HasColumnName("titulo_ata");

            builder.Property(x => x.TituloAtaAEsquerda)
                   .HasColumnName("titulo_ata_a_esquerda");

            builder.Property(x => x.ConteudoAta)
                   .HasColumnName("conteudo_ata");
        }
    }
}
