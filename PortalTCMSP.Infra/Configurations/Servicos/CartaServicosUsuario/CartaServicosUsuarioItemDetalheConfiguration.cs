using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;

namespace PortalTCMSP.Infra.Configurations.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioItemDetalheConfiguration : IEntityTypeConfiguration<CartaServicosUsuarioItemDetalhe>
    {
        public void Configure(EntityTypeBuilder<CartaServicosUsuarioItemDetalhe> builder)
        {
            builder.ToTable("servicos_carta_servicos_usuario_item_detalhe");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_carta_servicos_usuario_item_detalhe");

            builder.Property(x => x.IdCartaServicosUsuarioServicoItem)
                .HasColumnName("id_carta_servicos_usuario_servico_item")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.TituloDetalhe)
                .HasColumnName("titulo_detalhe")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(x => x.ServicoItem)
                .WithMany(x => x.ItemDetalhe)
                .HasForeignKey(x => x.IdCartaServicosUsuarioServicoItem)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
