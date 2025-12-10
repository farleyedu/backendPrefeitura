using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaConfiguration : IEntityTypeConfiguration<Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria> builder)
        {
            builder.ToTable("sessoes_plenarias");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id_sessao_plenaria");

            builder.Property(s => s.Slug).HasColumnName("slug").IsRequired();
            builder.Property(s => s.Titulo).HasColumnName("titulo").IsRequired();
            builder.Property(s => s.Descricao).HasColumnName("descricao").HasColumnType("text");
            builder.Property(s => s.SeoTitulo).HasColumnName("seo_titulo");
            builder.Property(s => s.SeoDescricao).HasColumnName("seo_descricao");
            builder.Property(s => s.UrlVideo).HasColumnName("url_video").HasMaxLength(2048);
            builder.Property(s => s.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(s => s.DataAtualizacao).HasColumnName("data_atualizacao");
            builder.Property(s => s.DataPublicacao).HasColumnName("data_publicacao");
            builder.Property(s => s.Ativo).HasColumnName("ativo").HasMaxLength(1).IsRequired();
        }
    }
}
