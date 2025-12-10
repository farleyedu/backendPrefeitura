using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoesConfiguration : IEntityTypeConfiguration<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes> builder)
        {
            builder.ToTable("servicos_oficiose_intimacoes");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id_oficiose_intimacoes");

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo")
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
