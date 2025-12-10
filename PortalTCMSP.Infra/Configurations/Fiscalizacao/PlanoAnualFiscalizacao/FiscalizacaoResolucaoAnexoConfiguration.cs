using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAnexoConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoAnexo>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoAnexo> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.ResolucaoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo");

            builder.Property(x => x.Numero)
                   .HasColumnName("numero");

            builder.Property(x => x.Titulo)
                   .HasColumnName("titulo");

            builder.Property(x => x.Descritivo)
                   .HasColumnName("descritivo");

            builder.HasMany(a => a.TemasPrioritarios)
                   .WithOne(t => t.Anexo)
                   .HasForeignKey(t => t.AnexoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Atividades)
                   .WithOne(at => at.Anexo)
                   .HasForeignKey(at => at.AnexoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Distribuicao)
                   .WithOne(di => di.Anexo)
                   .HasForeignKey(di => di.AnexoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
