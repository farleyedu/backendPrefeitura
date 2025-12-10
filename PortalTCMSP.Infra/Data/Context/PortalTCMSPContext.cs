using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.BannerEntity;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity;
using PortalTCMSP.Domain.Entities.InstitucionalEntity;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class PortalTCMSPContext(DbContextOptions<PortalTCMSPContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Banner> Banner { get; set; }
        
        // Noticia
        public DbSet<Noticia> Noticia { get; set; }
        public DbSet<NoticiaOld> NoticiaOld { get; set; }
        public DbSet<NoticiaCategoria> NoticiaCategoria { get; set; }
        public DbSet<NoticiaBloco> NoticiaBloco { get; set; }
        
        // Institucional
        public DbSet<Institucional> Institucional { get; set; }
        public DbSet<InstitucionalBloco> InstitucionalBloco { get; set; }
        public DbSet<InstitucionalBlocoAnexo> InstitucionalBlocoAnexo { get; set; }
        public DbSet<InstitucionalBlocoDescricao> InstitucionalBlocoDescricao { get; set; }
        public DbSet<InstitucionalBlocoSubtexto> InstitucionalBlocoSubtexto { get; set; }

        // SessaoPlenaria
        public DbSet<SessaoPlenaria> SessaoPlenaria { get; set; }
        public DbSet<SessaoPlenariaAta> SessoesPlenariasAtas { get; set; }
        public DbSet<SessaoPlenariaAtaAnexo> SessoesPlenariasAtasAnexos { get; set; }
        public DbSet<SessaoPlenariaJurispudencia> SessaoPlenariaJurispudencia { get; set; }
        public DbSet<SessaoPlenariaNotasTaquigraficas> SessaoPlenariaNotasTaquigraficas { get; set; }
        public DbSet<SessaoPlenariaNotasTaquigraficasAnexos> SessaoPlenariaNotasTaquigraficasAnexos { get; set; }
        public DbSet<SessaoPlenariaPauta> SessaoPlenariaPauta { get; set; }
        public DbSet<SessaoPlenariaPautaAnexo> SessoesPlenariasPautasAnexos { get; set; }
        public DbSet<SessaoPlenariaSustentacaoOral> SessaoPlenariaSustentacaoOral { get; set; }
        public DbSet<SessaoPlenariaSustentacaoOralAnexos> SessaoPlenariaSustentacaoOralAnexos { get; set; }
        
        //Fiscalizacao
        public DbSet<FiscalizacaoSecretariaControleExterno> FiscalizacaoSecretariaControleExterno { get; set; }
        public DbSet<FiscalizacaoSecretariaSecaoConteudoCarrosselItem> FiscalizacaoSecretariaSecaoConteudoCarrosselItem { get; set; }
        public DbSet<FiscalizacaoSecretariaSecaoConteudoTitulo> FiscalizacaoSecretariaSecaoConteudoTitulo { get; set; }
        public DbSet<FiscalizacaoContasDoPrefeito> FiscalizacaoContasDoPrefeito { get; set; }
        public DbSet<FiscalizacaoContasDoPrefeitoAnexo> FiscalizacaoContasDoPrefeitoAnexo { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacao> FiscalizacaoRelatoriosFiscalizacao { get; set; }
        public DbSet<FiscalizacaoPlanoAnualFiscalizacaoResolucao> FiscalizacaoPlanoAnualFiscalizacao { get; set; }

        //Relatorio Fiscalizacao
        public DbSet<FiscalizacaoRelatorioFiscalizacao> FiscalizacaoRelatorioFiscalizacao { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoCarrossel> FiscalizacaoRelatorioFiscalizacaoCarrossel { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel> FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoConteudoLink> FiscalizacaoRelatorioFiscalizacaoConteudoLink { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque> FiscalizacaoRelatorioFiscalizacaoConteudoDestaque { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoTcRelacionado> FiscalizacaoRelatorioFiscalizacaoTcRelacionado { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo> FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoImagemAnexa> FiscalizacaoRelatorioFiscalizacaoImagemAnexa { get; set; }
        public DbSet<FiscalizacaoRelatorioFiscalizacaoDescricao> FiscalizacaoRelatorioFiscalizacaoDescricao { get; set; }

        //Plano Anual Fiscalizacao
        public DbSet<FiscalizacaoPlanoAnualFiscalizacaoResolucao> FiscalizacaoPlanoAnualFiscalizacaoResolucao { get; set; }
        public DbSet<FiscalizacaoResolucaoAnexo> FiscalizacaoResolucaoAnexo { get; set; }
        public DbSet<FiscalizacaoResolucaoAta> FiscalizacaoResolucaoAta { get; set; }
        public DbSet<FiscalizacaoResolucaoAtividade> FiscalizacaoResolucaoAtividade { get; set; }
        public DbSet<FiscalizacaoResolucaoAtividadeItem> FiscalizacaoResolucaoAtividadeItem { get; set; }
        public DbSet<FiscalizacaoResolucaoDispositivo> FiscalizacaoResolucaoDispositivo { get; set; }
        public DbSet<FiscalizacaoResolucaoDistribuicao> FiscalizacaoResolucaoDistribuicao { get; set; }
        public DbSet<FiscalizacaoResolucaoEmenta> FiscalizacaoResolucaoEmenta { get; set; }
        public DbSet<FiscalizacaoResolucaoEmentaLink> FiscalizacaoResolucaoEmentaLink { get; set; }
        public DbSet<FiscalizacaoResolucaoInciso> FiscalizacaoResolucaoInciso { get; set; }
        public DbSet<FiscalizacaoResolucaoParagrafo> FiscalizacaoResolucaoParagrafo { get; set; }
        public DbSet<FiscalizacaoResolucaoTemaPrioritario> FiscalizacaoResolucaoTemaPrioritario { get; set; }

        //Servicos 

        //Multas e Procedimentos
        public DbSet<MultasProcedimentos> MultasProcedimentos { get; set; }
        public DbSet<MultasProcedimentosPortariaRelacionada> MultasProcedimentosPortariaRelacionada { get; set; }
        public DbSet<MultasProcedimentosProcedimento> MultasProcedimentosProcedimento { get; set; }

        //OficioseIntimacoes
        public DbSet<OficioseIntimacoes> OficioseIntimacoes { get; set; }
        public DbSet<OficioseIntimacoesSecao> OficioseIntimacoesSecao { get; set; }
        public DbSet<OficioseIntimacoesSecaoItem> OficioseIntimacoesSecaoItem { get; set; }

        //Cartorio
        public DbSet<Cartorio> Cartorio { get; set; }
        public DbSet<CartorioAtendimento> CartorioAtendimento { get; set; }

        //ProtocoloEletronico
        public DbSet<ProtocoloEletronico> ProtocoloEletronico { get; set; }
        public DbSet<ProtocoloEletronicoAcao> ProtocoloEletronicoAcao { get; set; }

        //CartaServicosUsuario
        public DbSet<CartaServicosUsuario> CartaServicosUsuario { get; set; }
        public DbSet<CartaServicosUsuarioServico> CartaServicosUsuarioServico { get; set; }
        public DbSet<CartaServicosUsuarioServicoItem> CartaServicosUsuarioServicoItem { get; set; }
        public DbSet<CartaServicosUsuarioItemDetalhe> CartaServicosUsuarioItemDetalhe { get; set; }
        public DbSet<CartaServicosUsuarioDescritivoItemDetalhe> CartaServicosUsuarioDescritivoItemDetalhe { get; set; }

        //PrazosProcessuais
        public DbSet<PrazosProcessuais> PrazosProcessuais { get; set; }
        public DbSet<PrazosProcessuaisItem> PrazosProcessuaisItem { get; set; }
        public DbSet<PrazosProcessuaisItemAnexo> PrazosProcessuaisItemAnexo { get; set; }
        //EmissaoCertidoes
        public DbSet<EmissaoCertidoes> EmissaoCertidoes { get; set; }
        public DbSet<EmissaoCertidoesAcao> EmissaoCertidoesAcao { get; set; }
        public DbSet<EmissaoCertidoesSecaoOrientacao> EmissaoCertidoesSecaoOrientacao { get; set; }
        public DbSet<EmissaoCertidoesOrientacao> EmissaoCertidoesOrientacao { get; set; }
        public DbSet<EmissaoCertidoesDescritivo> EmissaoCertidoesDescritivo { get; set; }

    }
}
