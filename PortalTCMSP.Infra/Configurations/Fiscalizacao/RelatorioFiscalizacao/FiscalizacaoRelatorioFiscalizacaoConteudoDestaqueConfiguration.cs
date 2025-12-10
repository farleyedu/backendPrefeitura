using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_conteudo_destaque");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_conteudo_destaque").IsRequired();

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(255);

            builder.Property(x => x.ImagemUrl)
                .HasColumnName("imagem_url")
                .HasMaxLength(1024);

            builder.HasMany(x => x.Descricoes)
                   .WithOne()
                   .HasForeignKey(x => x.IdConteudoDestaque)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
