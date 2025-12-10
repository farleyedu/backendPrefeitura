using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;
namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaJurispudenciaConfiguration : IEntityTypeConfiguration<SessaoPlenariaJurispudencia>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaJurispudencia> builder)
        {
            builder.ToTable("sessoes_plenarias_jurispudencia");

            builder.HasKey(j => j.Id);
            builder.Property(j => j.Id).HasColumnName("id_sessao_plenaria_jurispudencia");

            builder.Property(j => j.Slug).HasColumnName("slug").IsRequired();
            builder.Property(j => j.Titulo).HasColumnName("titulo").IsRequired();
            builder.Property(j => j.Descricao).HasColumnName("descricao").HasColumnType("text");

            builder.Property(j => j.TituloBoletins).HasColumnName("titulo_boletins");
            builder.Property(j => j.DescricaoBoletins).HasColumnName("descricao_boletins").HasColumnType("text");
            builder.Property(j => j.LinkBoletins).HasColumnName("link_boletins");

            builder.Property(j => j.TituloPesquisa).HasColumnName("titulo_pesquisa");
            builder.Property(j => j.DescricaoPesquisa).HasColumnName("descricao_pesquisa").HasColumnType("text");
            builder.Property(j => j.LinkPesquisa).HasColumnName("link_pesquisa");

            builder.Property(j => j.TituloSumulas).HasColumnName("titulo_sumulas");
            builder.Property(j => j.DescricaoSumulas).HasColumnName("descricao_sumulas").HasColumnType("text");
            builder.Property(j => j.LinkSumulas).HasColumnName("link_sumulas");

            builder.Property(j => j.Ativo).HasColumnName("ativo").IsRequired();
            builder.Property(j => j.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(j => j.DataAtualizacao).HasColumnName("data_atualizacao");
        }
    }
}
