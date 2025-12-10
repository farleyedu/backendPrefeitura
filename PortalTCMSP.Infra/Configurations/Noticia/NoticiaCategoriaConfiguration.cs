using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.NoticiaEntity;

namespace PortalTCMSP.Infra.Configurations.Noticia
{
    public class NoticiaCategoriaConfiguration : IEntityTypeConfiguration<NoticiaCategoria>
    {
        public void Configure(EntityTypeBuilder<NoticiaCategoria> b)
        {
            b.ToTable("noticia_categorias");

            b.HasKey(x => new { x.NoticiaId, x.CategoriaId });

            b.Property(x => x.NoticiaId);
            b.Property(x => x.CategoriaId);

            b.HasOne(x => x.Noticia)
                .WithMany(n => n.NoticiaCategorias)
                .HasForeignKey(x => x.NoticiaId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
