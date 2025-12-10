using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public class CartorioConfiguration : IEntityTypeConfiguration<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio> builder)
        {
            builder.ToTable("servicos_cartorio");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_cartorio");

            builder.Property(x => x.TituloPagina)
                .HasColumnName("titulo_pagina")
                .HasMaxLength(255)
                .IsRequired();

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
