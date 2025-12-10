using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoContasDoPrefeitoAnexoConfiguration : IEntityTypeConfiguration<FiscalizacaoContasDoPrefeitoAnexo>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoContasDoPrefeitoAnexo> builder)
        {
            builder.ToTable("fiscalizacao_contas_do_prefeito_anexos");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .HasColumnName("id_fiscalizacao_contas_do_prefeito_anexo");

            builder.Property(a => a.IdFiscalizacaoContasDoPrefeito)
                   .HasColumnName("id_fiscalizacao_contas_do_prefeito")
                   .IsRequired();

            builder.Property(a => a.Link)
                   .HasColumnName("link")
                   .IsRequired();

            builder.Property(a => a.TipoArquivo)
                   .HasColumnName("tipo_arquivo")
                   .HasMaxLength(10);

            builder.Property(a => a.NomeExibicao)
                   .HasColumnName("nome_exibicao")
                   .HasMaxLength(120);

            builder.Property(a => a.Ordem)
                   .HasColumnName("ordem")
                   .IsRequired();

            builder.HasOne(a => a.ContasDoPrefeito)
                   .WithMany(p => p.Anexos)
                   .HasForeignKey(a => a.IdFiscalizacaoContasDoPrefeito)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
