using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesSecaoOrientacaoConfiguration : IEntityTypeConfiguration<EmissaoCertidoesSecaoOrientacao>
    {
        public void Configure(EntityTypeBuilder<EmissaoCertidoesSecaoOrientacao> builder)
        {
            builder.ToTable("servicos_emissao_certidoes_secao_orientacao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_emissao_certidoes_secao_orientacao");

            builder.Property(x => x.IdEmissaoCertidoes)
                .HasColumnName("id_emissao_certidoes")
                .IsRequired();

            builder.Property(x => x.TipoSecao)
                .HasColumnName("tipo_secao")
                .IsRequired();

            builder.Property(x => x.TituloPagina)
                .HasColumnName("titulo_pagina")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .IsRequired();

            builder.HasOne(x => x.EmissaoCertidoes)
                .WithMany(x => x.SecaoOrientacoes)
                .HasForeignKey(x => x.IdEmissaoCertidoes)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
