using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoLinkConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoConteudoLink>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoConteudoLink> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_conteudo_link");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_conteudo_link").IsRequired();

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo").HasMaxLength(255);

            builder.Property(x => x.ConselheiroRelator)
                .HasColumnName("conselheiro_relator").HasMaxLength(50);

            builder.Property(x => x.ImagemUrl)
                .HasColumnName("imagem_url")
                .HasMaxLength(1024);

            builder.Property(x => x.Descritivo)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.PeriodoRealizacao)
                .HasMaxLength(200)
                .HasColumnName("periodo_realizacao");

            builder.Property(x => x.PeriodoAbrangencia)
                .HasMaxLength(200)
                .HasColumnName("periodo_abrangencia");

            builder.Property(x => x.TituloDestaque)
                .HasMaxLength(200)
                .HasColumnName("titulo_destaque");

            builder.HasMany(x => x.ConteudoDestaque)
                   .WithOne()
                   .HasForeignKey(x => x.IdConteudoLink)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.TcRelacionados)
                   .WithOne()
                   .HasForeignKey(x => x.IdConteudoLink)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.DocumentosAnexos)
                   .WithOne()
                   .HasForeignKey(x => x.IdConteudoLink)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ImagensAnexas)
                   .WithOne()
                   .HasForeignKey(x => x.IdConteudoLink)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
