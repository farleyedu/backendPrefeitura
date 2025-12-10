using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoSecretariaSecaoConteudoTituloConfiguration : IEntityTypeConfiguration<FiscalizacaoSecretariaSecaoConteudoTitulo>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoSecretariaSecaoConteudoTitulo> builder)
        {
            builder.ToTable("fiscalizacao_secretaria_controle_titulo");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                   .HasColumnName("id_fiscalizacao_secretaria_controle_titulo");

            builder.Property(t => t.IdSecaoConteudo)
                   .HasColumnName("id_fiscalizacao_secretaria_controle") 
                   .IsRequired();

            builder.Property(t => t.Ordem)
                   .HasColumnName("ordem")
                   .IsRequired();

            builder.Property(t => t.Titulo)
                   .HasColumnName("titulo")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(t => t.Descricao)
                   .HasColumnName("descricao");

            builder.Property(t => t.ImagemUrl)
                    .HasColumnName("imagem_url");

            builder.HasOne(t => t.Conteudo)
                   .WithMany(c => c.Titulos)
                   .HasForeignKey(t => t.IdSecaoConteudo)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_fisc_secretaria_controle_titulo__controle");

            builder.HasIndex(t => new { t.IdSecaoConteudo, t.Ordem })
                   .HasDatabaseName("ux_titulo_conteudo_ordem")
                   .IsUnique();
        }
    }
}
