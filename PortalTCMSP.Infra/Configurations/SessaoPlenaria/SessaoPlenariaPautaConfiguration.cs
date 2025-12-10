using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OpenApi.Extensions;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Configurations.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaPautaConfiguration : IEntityTypeConfiguration<SessaoPlenariaPauta>
    {
        public void Configure(EntityTypeBuilder<SessaoPlenariaPauta> builder)
        {
            builder.ToTable("sessoes_plenarias_pauta");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id_sessao_plenaria_pauta");

            builder.Property(s => s.IdSessaoPlenaria).HasColumnName("id_sessao_plenaria");
            builder.Property(s => s.Numero).HasColumnName("numero").HasMaxLength(50);
            builder.Property(s => s.Tipo)
               .HasColumnName("tipo")
               .HasMaxLength(40)
               .HasConversion(
                   v => v.HasValue ? v.Value.GetDisplayName() : null,  
                   v => string.IsNullOrWhiteSpace(v) ? null  
                        : PautaTipoExtensions.ParseFromName(v!)
               );
            builder.Property(s => s.DataDaSesao).HasColumnName("data_sessao");
            builder.Property(s => s.DataPublicacao).HasColumnName("data_publicacao");
            builder.Property(s => s.DataCriacao).HasColumnName("data_criacao").IsRequired();
            builder.Property(s => s.DataAtualizacao).HasColumnName("data_atualizacao");

            builder.HasOne(s => s.SessaoPlenaria)
                   .WithMany(p => p.Pautas)
                   .HasForeignKey(s => s.IdSessaoPlenaria)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
