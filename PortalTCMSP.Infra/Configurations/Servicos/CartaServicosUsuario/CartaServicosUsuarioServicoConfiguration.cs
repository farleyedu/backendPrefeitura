using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;

namespace PortalTCMSP.Infra.Configurations.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoConfiguration : IEntityTypeConfiguration<CartaServicosUsuarioServico>
    {
        public void Configure(EntityTypeBuilder<CartaServicosUsuarioServico> builder)
        {
            builder.ToTable("servicos_carta_servicos_usuario_servico");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id_carta_servicos_usuario_servico");

            builder.Property(x => x.IdCartaServicosUsuario)
                .HasColumnName("id_carta_servicos_usuario")
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

            builder.HasOne(x => x.CartaServicosUsuario)
                .WithMany(x => x.Servicos)
                .HasForeignKey(x => x.IdCartaServicosUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
