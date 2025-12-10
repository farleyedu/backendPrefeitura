using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoDocumentoAnexoConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_documento_anexo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdConteudoLink)
                    .HasColumnName("id_documento_anexo")
                    .IsRequired();

            builder.Property(x => x.Link)
                .HasColumnName("link")
                .IsRequired();

            builder.Property(x => x.TipoArquivo)
                .HasColumnName("tipo_arquivo")
                .HasMaxLength(10);

            builder.Property(x => x.NomeExibicao)
                .HasColumnName("nome_exibicao")
                .HasMaxLength(120);
        }
    }
}
