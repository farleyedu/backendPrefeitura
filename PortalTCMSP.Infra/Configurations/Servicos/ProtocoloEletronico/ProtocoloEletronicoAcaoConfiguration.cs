using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public class ProtocoloEletronicoAcaoConfiguration : IEntityTypeConfiguration<ProtocoloEletronicoAcao>
    {
        public void Configure(EntityTypeBuilder<ProtocoloEletronicoAcao> builder)
        {
            builder.ToTable("servicos_protocolo_eletronico_acao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id_protocolo_eletronico_acao");

            builder.Property(x => x.IdProtocoloEletronico)
                .HasColumnName("id_protocolo_eletronico")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.DataPublicacao)
                .HasColumnName("data_publicacao")
                .IsRequired();

            builder.Property(x => x.TipoAcao)
                .HasColumnName("tipo_acao")
                .IsRequired();

            builder.Property(x => x.UrlAcao)
                .HasColumnName("url_acao")
                .HasMaxLength(500)
                .IsRequired();

            builder.HasOne(a => a.ProtocoloEletronico)
                   .WithMany(p => p.Acoes)
                   .HasForeignKey(a => a.IdProtocoloEletronico)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_protocolo_eletronico_acao__protocolo_eletronico");
        }
    }
}
