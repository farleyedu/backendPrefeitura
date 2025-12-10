using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesDescritivoConfiguration : IEntityTypeConfiguration<EmissaoCertidoesDescritivo>
    {
        public void Configure(EntityTypeBuilder<EmissaoCertidoesDescritivo> builder)
        {
            builder.ToTable("servicos_emissao_certidoes_descritivo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_emissao_certidoes_descritivo");

            builder.Property(x => x.IdEmissaoCertidoesOrientacao)
                .HasColumnName("id_emissao_certidoes_orientacao")
                .IsRequired();

            builder.Property(x => x.Texto)
                .HasColumnName("texto")
                .IsRequired();

            builder.HasOne(x => x.Orientacao)
                .WithMany(x => x.Descritivos)
                .HasForeignKey(x => x.IdEmissaoCertidoesOrientacao)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
