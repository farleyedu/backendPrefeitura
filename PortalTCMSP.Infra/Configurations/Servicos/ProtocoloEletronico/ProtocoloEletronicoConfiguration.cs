using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public class ProtocoloEletronicoConfiguration : IEntityTypeConfiguration<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico> builder)
        {
            builder.ToTable("servicos_protocolo_eletronico");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id_protocolo_eletronico");

            builder.Property(x => x.TituloPagina)
                .HasColumnName("titulo_pagina")
                .HasMaxLength(255).IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.DataCriacao)
                .HasColumnName("data_criacao")
                .IsRequired();

            builder.Property(x => x.DataAtualizacao)
                .HasColumnName("data_atualizacao");
        }
    }
}
