using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoSecretariaSecaoConteudoCarrosselItemConfiguration : IEntityTypeConfiguration<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoSecretariaSecaoConteudoCarrosselItem> builder)
        {
            builder.ToTable("fiscalizacao_secretaria_controle_carrossel");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                   .HasColumnName("id_fiscalizacao_secretaria_controle_carrossel");

            builder.Property(i => i.IdSecaoConteudo)
                   .HasColumnName("id_fiscalizacao_secretaria_controle")
                   .IsRequired();

            builder.Property(i => i.Ordem)
                   .HasColumnName("ordem")
                   .IsRequired();

            builder.Property(i => i.Titulo)
                   .HasColumnName("titulo")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(i => i.Descricao)
                   .HasColumnName("descricao");

            builder.Property(i => i.ImagemUrl)
                   .HasColumnName("imagem_url")
                   .HasMaxLength(1024)
                   .IsRequired();

            builder.HasOne(i => i.Conteudo)
                   .WithMany(c => c.Carrossel)
                   .HasForeignKey(i => i.IdSecaoConteudo)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_fisc_secretaria_controle_carrossel__controle");

            builder.HasIndex(i => new { i.IdSecaoConteudo, i.Ordem })
                   .HasDatabaseName("ux_carrossel_conteudo_ordem")
                   .IsUnique();
        }
    }
}
