using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;

namespace PortalTCMSP.Infra.Configurations.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoItemConfiguration : IEntityTypeConfiguration<CartaServicosUsuarioServicoItem>
    {
        public void Configure(EntityTypeBuilder<CartaServicosUsuarioServicoItem> builder)
        {
            builder.ToTable("servicos_carta_servicos_usuario_servico_item");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_carta_servicos_usuario_servico_item");

            builder.Property(x => x.IdCartaServicosUsuarioServico)
                .HasColumnName("id_carta_servicos_usuario_servico")
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

            builder.Property(x => x.Acao)
                .HasColumnName("acao")
                .HasMaxLength(255);

            builder.Property(x => x.LinkItem)
                .HasColumnName("link_item")
                .HasMaxLength(500);

            builder.HasOne(x => x.Servico)
                .WithMany(x => x.ServicosItens)
                .HasForeignKey(x => x.IdCartaServicosUsuarioServico)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
