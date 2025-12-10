using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatoriosFiscalizacaoConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacao>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacao> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_relatorio_fiscalizacao");

            builder.Property(x => x.Slug)
                    .HasColumnName("slug")
                    .IsRequired();

            builder.Property(x => x.Titulo)
                   .HasColumnName("titulo")
                   .IsRequired();

            builder.Property(x => x.Descricao)
                   .HasColumnName("descricao");

            builder.Property(x => x.Ativo)
                   .HasColumnName("ativo")
                   .IsRequired();

            builder.Property(x => x.DataCriacao)
                   .HasColumnName("data_criacao")
                   .IsRequired();

            builder.Property(x => x.DataAtualizacao)
                   .HasColumnName("data_atualizacao");

            builder.HasMany(x => x.Carrocel)
                   .WithOne(c => c.EntityConteudo)
                   .HasForeignKey(c => c.IdConteudo)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_fisc_conteudo__carrossel");

            builder.HasIndex(x => x.Slug).IsUnique();
        }
    }
}
