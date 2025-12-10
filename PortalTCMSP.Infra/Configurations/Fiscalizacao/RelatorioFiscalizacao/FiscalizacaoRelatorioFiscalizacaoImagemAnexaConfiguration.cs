using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoImagemAnexaConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoImagemAnexa>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoImagemAnexa> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_imagem_anexa");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdConteudoLink)
                .HasColumnName("id_imagem_anexa")
                .IsRequired();

            builder.Property(x => x.NomeExibicao)
                .HasColumnName("nome_exibicao");

            builder.Property(x => x.ImagemUrl)
                .HasColumnName("imagem_url")
                .HasMaxLength(1024);
        }
    }
}
