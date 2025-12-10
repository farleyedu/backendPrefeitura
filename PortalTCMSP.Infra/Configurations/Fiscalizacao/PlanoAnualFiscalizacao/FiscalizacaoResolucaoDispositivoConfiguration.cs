using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoDispositivoConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoDispositivo>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoDispositivo> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_dispositivos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_dispositivos");

            builder.Property(x => x.ResolucaoId).HasColumnName("id_resolucao");
            builder.Property(x => x.Ordem).HasColumnName("ordem");
            builder.Property(x => x.Artigo).HasColumnName("artigo");

            builder.HasMany(d => d.Paragrafo)
                   .WithOne(p => p.Dispositivo)
                   .HasForeignKey(p => p.DispositivoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Incisos)
                   .WithOne(i => i.Dispositivo)
                   .HasForeignKey(i => i.DispositivoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
