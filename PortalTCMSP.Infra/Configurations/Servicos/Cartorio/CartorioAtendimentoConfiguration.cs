using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public class CartorioAtendimentoConfiguration : IEntityTypeConfiguration<CartorioAtendimento>
    {
        public void Configure(EntityTypeBuilder<CartorioAtendimento> builder)
        {
            builder.ToTable("servicos_cartorio_atendimento");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("id_cartorio_atendimento");

            builder.Property(c => c.IdCartorio)
                .HasColumnName("id_cartorio")
                .IsRequired();

            builder.Property(c => c.Ordem)
                .HasColumnName("ordem")
                .IsRequired();

            builder.Property(c => c.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(c => c.Cartorio)
                .WithMany(p => p.Atendimentos)
                .HasForeignKey(c => c.IdCartorio)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_cartorio_atendimento__cartorio");
        }
    }
}
