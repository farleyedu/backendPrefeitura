using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortalTCMSP.Infra.Configurations.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioConfiguration : IEntityTypeConfiguration<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario> builder)
        {
            builder.ToTable("servicos_carta_servicos_usuario");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_carta_servicos_usuario");

            builder.Property(x => x.TituloPagina)
                .HasColumnName("titulo_pagina")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.TituloPesquisa)
                .HasColumnName("titulo_pesquisa")
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
