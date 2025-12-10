using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoCarrosselConfiguration : IEntityTypeConfiguration<FiscalizacaoRelatorioFiscalizacaoCarrossel>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoRelatorioFiscalizacaoCarrossel> builder)
        {
            builder.ToTable("fiscalizacao_relatorio_fiscalizacao_carrossel");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_relatorio_fiscalizacao_carrossel");

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo");

            builder.HasMany(c => c.ConteudoCarrocel)
                   .WithOne(cc => cc.CarrosselItem)
                   .HasForeignKey(cc => cc.IdCarrosselItem)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_fisc_relatorio_fiscalizacao_carrossel");

            builder.HasIndex(c => new { c.IdConteudo, c.Ordem }).HasDatabaseName("ux_fisc_relatorio_fiscalizacao_carrossel_ordem").IsUnique();
        }
    }
}
