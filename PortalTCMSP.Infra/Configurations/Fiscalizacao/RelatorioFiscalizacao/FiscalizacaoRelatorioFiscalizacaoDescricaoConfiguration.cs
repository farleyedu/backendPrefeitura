using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoDescricaoConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoDescricao>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoDescricao> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_descricao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_descricao")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnType("nvarchar(max)");
        }
    }
}
