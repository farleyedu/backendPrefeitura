using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoPlanoAnualFiscalizacaoResolucaoConfiguration : IEntityTypeConfiguration<FiscalizacaoPlanoAnualFiscalizacaoResolucao>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoPlanoAnualFiscalizacaoResolucao> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao");

            builder.HasKey(x => x.Id);
            builder
                .Property(x => x.Id)
                .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao");

            builder.Property(x => x.Slug)
                   .HasColumnName("slug")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(x => x.Numero)
                   .HasColumnName("numero");

            builder.Property(x => x.Ano)
                   .HasColumnName("ano");

            builder.Property(x => x.Ativo)
                   .HasColumnName("ativo")
                   .IsRequired();

            builder.Property(x => x.DataPublicacao)
                   .HasColumnName("data_publicacao");

            builder.Property(x => x.Titulo)
                   .HasColumnName("titulo")
                   .IsRequired();

            builder.Property(x => x.SubTitulo)
                   .HasColumnName("sub_titulo");

            builder.HasOne(x => x.Ementa)
                   .WithOne(e => e.Resolucao)
                   .HasForeignKey<FiscalizacaoResolucaoEmenta>(e => e.ResolucaoId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_resolucao_ementa_resolucao");

            builder.HasMany(x => x.Dispositivos)
                   .WithOne(d => d.Resolucao)
                   .HasForeignKey(d => d.ResolucaoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Anexos)
                   .WithOne(a => a.Resolucao)
                   .HasForeignKey(a => a.ResolucaoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Atas)
                   .WithOne(a => a.Resolucao)
                   .HasForeignKey(a => a.ResolucaoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
