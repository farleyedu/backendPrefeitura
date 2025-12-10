using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoTcRelacionadoConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoTcRelacionado>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoTcRelacionado> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_tcrelacionado");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdConteudoLink)
                .HasColumnName("id_tcrelacionado").IsRequired();

            builder.Property(x => x.Link)
                .HasColumnName("link")
                .IsRequired();

            builder.Property(x => x.Descritivo)
                .HasMaxLength(1000);
        }
    }
}
