using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public class MultasProcedimentosConfiguration : IEntityTypeConfiguration<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>
    {
        public void Configure(EntityTypeBuilder<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos> builder)
        {
            builder.ToTable("servicos_multas_procedimentos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_multas_procedimentos");

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
