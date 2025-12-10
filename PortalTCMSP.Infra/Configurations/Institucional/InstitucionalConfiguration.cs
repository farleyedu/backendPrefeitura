using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortalTCMSP.Infra.Configurations.Institucional
{
    public class InstitucionalConfiguration : IEntityTypeConfiguration<Domain.Entities.InstitucionalEntity.Institucional>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.InstitucionalEntity.Institucional> builder)
        {
            builder.ToTable("institucionais");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("id_institucional");

            builder.Property(i => i.Titulo).HasColumnName("titulo").IsRequired().HasMaxLength(255);
            builder.Property(i => i.Slug).HasColumnName("slug").IsRequired().HasMaxLength(200);
            builder.Property(i => i.Subtitulo).HasColumnName("subtitulo").HasMaxLength(255);
            builder.Property(i => i.Descricao).HasColumnName("descricao").HasColumnType("text");
            builder.Property(i => i.Resumo).HasColumnName("resumo").HasColumnType("text");
            builder.Property(i => i.AutorNome).HasColumnName("autor_nome").HasMaxLength(255);
            builder.Property(i => i.Creditos).HasColumnName("creditos").HasMaxLength(255);
            builder.Property(i => i.SeoTitulo).HasColumnName("seo_titulo").HasMaxLength(255);
            builder.Property(i => i.SeoDescricao).HasColumnName("seo_descricao").HasMaxLength(500);
            builder.Property(i => i.ImagemUrlPrincipal).HasColumnName("imagem_url_principal").HasMaxLength(2048);

            builder.Property(i => i.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(i => i.DataAtualizacao).HasColumnName("data_atualizacao");
            builder.Property(i => i.DataPublicacao).HasColumnName("data_publicacao");

            builder.Property(i => i.Ativo).HasColumnName("ativo").IsRequired().HasMaxLength(1);

            builder.HasIndex(i => i.Slug)
                   .IsUnique()
                   .HasDatabaseName("ux_institucional_slug");
        }
    }
}
