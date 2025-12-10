using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoCarrocelConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_conteudo_carrossel");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_relatorio_fiscalizacao_conteudo_carrossel");

            builder.Property(x => x.Ativo)
                   .HasColumnName("ativo");

            builder.Property(x => x.Ordem)
                   .HasColumnName("ordem");

            builder.Property(x => x.Descricao)
                   .HasMaxLength(255);

            builder.Property(x => x.Link)
                    .HasMaxLength(255);

            builder.HasIndex(c => new { c.IdCarrosselItem, c.Ordem }).HasDatabaseName("ux_fisc_carrossel_conteudo_ordem").IsUnique();
        }
    }
}
