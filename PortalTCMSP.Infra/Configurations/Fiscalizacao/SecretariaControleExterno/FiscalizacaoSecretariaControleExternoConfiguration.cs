using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoSecretariaControleExternoConfiguration : IEntityTypeConfiguration<FiscalizacaoSecretariaControleExterno>
    {
        public void Configure(EntityTypeBuilder<FiscalizacaoSecretariaControleExterno> builder)
        {
            builder.ToTable("fiscalizacao_secretaria_controle");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .HasColumnName("id_fiscalizacao_secretaria_controle");

            builder.Property(c => c.Slug)
                   .HasColumnName("slug")
                   .IsRequired();

            builder.Property(c => c.Titulo)
                   .HasColumnName("titulo")
                   .IsRequired();

            builder.Property(c => c.Descricao)
                   .HasColumnName("descricao");

            builder.Property(c => c.Creditos)
                   .HasColumnName("creditos");

            builder.Property(c => c.Ativo)
                   .HasColumnName("ativo")
                   .IsRequired();

            builder.Property(c => c.DataCriacao)
                   .HasColumnName("data_criacao")
                   .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                   .HasColumnName("data_atualizacao");

            builder.HasMany(c => c.Titulos)
                   .WithOne(t => t.Conteudo)
                   .HasForeignKey(t => t.IdSecaoConteudo)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Carrossel)
                   .WithOne(i => i.Conteudo)
                   .HasForeignKey(i => i.IdSecaoConteudo)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.Slug).IsUnique();
        }
    }
}
