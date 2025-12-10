using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoesSecaoConfiguration : IEntityTypeConfiguration<OficioseIntimacoesSecao>
    {
        public void Configure(EntityTypeBuilder<OficioseIntimacoesSecao> builder)
        {
            builder.ToTable("servicos_oficiose_intimacoes_secao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_oficiose_intimacoes_secao");

            builder.Property(x => x.IdOficioseIntimacoes)
                .HasColumnName("id_oficiose_intimacoes")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(x => x.OficioseIntimacoes)
                .WithMany(p => p.Secoes)
                .HasForeignKey(x => x.IdOficioseIntimacoes)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_oficiose_intimacoes_secao__oficiose_intimacoes");
        }
    }
}
