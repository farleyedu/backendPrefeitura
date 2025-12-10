using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAtividadeConfiguration : IEntityTypeConfiguration<FiscalizacaoResolucaoAtividade>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoResolucaoAtividade> builder)
        {
            builder.ToTable("fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_atividade");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.AnexoId)
                   .HasColumnName("id_fiscalizacao_plano_anual_fiscalizacao_resolucao_anexo_atividade");

            builder.Property(x => x.Tipo)
                   .HasColumnName("tipo");

            builder.HasMany(a => a.AtividadeItem)
                   .WithOne(i => i.Atividade)
                   .HasForeignKey(i => i.AtividadeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
