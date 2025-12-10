using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoContasDoPrefeitoConfiguration : IEntityTypeConfiguration<FiscalizacaoContasDoPrefeito>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoContasDoPrefeito> builder)
        {
            builder.ToTable("fiscalizacao_contas_do_prefeito");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_contas_do_prefeito");

            builder.Property(x => x.Pauta)
                   .HasColumnName("pauta");

            builder.Property(x => x.Ano)
                   .HasColumnName("ano")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(x => x.DataSessao)
                   .HasColumnName("data_sessao");

            builder.Property(x => x.DataPublicacao)
                   .HasColumnName("data_publicacao");

            builder.Property(x => x.DataCriacao)
                   .HasColumnName("data_criacao")
                   .IsRequired();

            builder.Property(x => x.DataAtualizacao)
                   .HasColumnName("data_atualizacao");

            builder.HasMany(x => x.Anexos)
                   .WithOne(a => a.ContasDoPrefeito)
                   .HasForeignKey(a => a.IdFiscalizacaoContasDoPrefeito)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
