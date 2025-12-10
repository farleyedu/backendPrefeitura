using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;

namespace PortalTCMSP.Infra.Configurations.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioDescritivoItemDetalheConfiguration : IEntityTypeConfiguration<CartaServicosUsuarioDescritivoItemDetalhe>
    {
        public void Configure(EntityTypeBuilder<CartaServicosUsuarioDescritivoItemDetalhe> builder)
        {
            builder.ToTable("servicos_carta_servicos_usuario_descritivo_item_detalhe");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_carta_servicos_usuario_descritivo_item_detalhe");

            builder.Property(x => x.IdCartaServicosUsuarioItemDetalhe)
                .HasColumnName("id_carta_servicos_usuario_item_detalhe")
                .IsRequired();

            builder.Property(x => x.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(x => x.Descritivo)
                .HasColumnName("descritivo")
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasOne(x => x.ItemDetalhe)
                .WithMany(x => x.DescritivoItemDetalhe)
                .HasForeignKey(x => x.IdCartaServicosUsuarioItemDetalhe)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
