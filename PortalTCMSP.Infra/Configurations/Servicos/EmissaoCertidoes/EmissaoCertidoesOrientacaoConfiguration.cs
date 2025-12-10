using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesOrientacaoConfiguration : IEntityTypeConfiguration<EmissaoCertidoesOrientacao>
    {
        public void Configure(EntityTypeBuilder<EmissaoCertidoesOrientacao> builder)
        {
            builder.ToTable("servicos_emissao_certidoes_orientacao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_emissao_certidoes_orientacao");

            builder.Property(x => x.IdEmissaoCertidoesSecaoOrientacao)
                .HasColumnName("id_emissao_certidoes_secao_orientacao");

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.HasOne(x => x.SecaoOrientacao)
                .WithMany(x => x.Orientacoes)
                .HasForeignKey(x => x.IdEmissaoCertidoesSecaoOrientacao)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
