using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesAcaoConfiguration : IEntityTypeConfiguration<EmissaoCertidoesAcao>
    {
        public void Configure(EntityTypeBuilder<EmissaoCertidoesAcao> builder)
        {
            builder.ToTable("servicos_emissao_certidoes_acao");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_emissao_certidoes_acao");

            builder.Property(x => x.IdEmissaoCertidoes)
                .HasColumnName("id_emissao_certidoes")
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

            builder.HasOne(x => x.EmissaoCertidoes)
                .WithMany(x => x.Acoes)
                .HasForeignKey(x => x.IdEmissaoCertidoes)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
