using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity;

namespace PortalTCMSP.Infra.Configurations.Institucional
{
    public class InstitucionalBlocoSubtextoConfiguration : IEntityTypeConfiguration<InstitucionalBlocoSubtexto>
    {
        public void Configure(EntityTypeBuilder<InstitucionalBlocoSubtexto> builder)
        {
            builder.ToTable("institucional_bloco_subtexto");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Ordem).HasColumnName("ordem").IsRequired();
            builder.Property(x => x.Texto).HasColumnName("texto").IsRequired();

            builder.Property(x => x.IdDescricao).HasColumnName("id_descricao").IsRequired();

            builder.HasOne(x => x.Descricao)
                   .WithMany(d => d.Subtextos)
                   .HasForeignKey(x => x.IdDescricao)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
