using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoesSecaoItemConfiguration : IEntityTypeConfiguration<OficioseIntimacoesSecaoItem>
    {
        public void Configure(EntityTypeBuilder<OficioseIntimacoesSecaoItem> builder)
        {
            builder.ToTable("servicos_oficiose_intimacoes_secao_item");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_oficiose_intimacoes_secao_item");

            builder.Property(x => x.IdOficioseIntimacoesSecao)
                .HasColumnName("id_oficiose_intimacoes_secao")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasOne(x => x.OficioseIntimacoesSecao)
                .WithMany(p => p.SecaoItem)
                .HasForeignKey(x => x.IdOficioseIntimacoesSecao)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_oficiose_intimacoes_secao_item__oficiose_intimacoes_secao");
        }
    }
}
