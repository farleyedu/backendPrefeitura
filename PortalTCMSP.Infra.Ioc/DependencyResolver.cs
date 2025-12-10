using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Domain.Repositories.Institucional;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario.Domain.Interfaces.Servicos.CartaServicosUsuarioInterface;
using PortalTCMSP.Domain.Repositories.Servicos.Cartorio;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Domain.Services.Fiscalizacao;
using PortalTCMSP.Domain.Services.Home;
using PortalTCMSP.Domain.Services.Institucional;
using PortalTCMSP.Domain.Services.Noticia;
using PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Services.Servicos.Cartorio;
using PortalTCMSP.Domain.Services.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Services.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Services.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Services.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Services.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Services.SessaoPlenaria;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Infra.Data.Repositories.Home;
using PortalTCMSP.Infra.Data.Repositories.Institucional;
using PortalTCMSP.Infra.Data.Repositories.Noticia;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Data.Repositories.Servicos.Cartorio;
using PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Data.Repositories.Servicos.ProtocoloEletronico;
using PortalTCMSP.Infra.Data.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Extensions;
using PortalTCMSP.Infra.Services.Fiscalizacao;
using PortalTCMSP.Infra.Services.Home;
using PortalTCMSP.Infra.Services.Institucional;
using PortalTCMSP.Infra.Services.Noticia;
using PortalTCMSP.Infra.Services.Servicos.CartaServicosUsuario;
using PortalTCMSP.Infra.Services.Servicos.Cartorio;
using PortalTCMSP.Infra.Services.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Services.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Services.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Services.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Services.Servicos.ProtocoloEletronico;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static IServiceCollection ConfigureApplicationContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddServices(configuration);
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServices(services, configuration);
            ConfigureRepositories(services, configuration);

            services.AddDbContext<PortalTCMSPContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            services.AddScoped<INoticiaService, NoticiaService>();
            services.AddScoped<INoticiaOldService, NoticiaOldService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IHtmlService, HtmlService>();
            services.AddScoped<IS3Service, S3Service>();
            services.AddScoped<IConsultaService, ConsultaService>();

            //Institucional
            services.AddScoped<IInstitucionalService, InstitucionalService>();
            services.AddScoped<IColegiadoService, ColegiadoService>();
            services.AddScoped<IHistoriaService, HistoriaService>();
            services.AddScoped<IMissaoVisaoValoresService, MissaoVisaoValoresService>();
            services.AddScoped<IRelatoriasService, RelatoriasService>();
            services.AddScoped<IOrganogramaService, OrganogramaService>();
            services.AddScoped<ICompetenciasService, CompetenciasService>();
            services.AddScoped<IPlanejamentoEstrategicoService, PlanejamentoEstrategicoService>();
            services.AddScoped<ILegislacaoService, LegislacaoService>();
            services.AddScoped<IProtecaoDadosService, ProtecaoDadosService>();
            services.AddScoped<IIso9001Service, Iso9001Service>();

            //Plenaria
            services.AddScoped<ISessaoPlenariaService, SessaoPlenariaService>();
            services.AddScoped<ISessaoPlenariaAtaService, SessaoPlenariaAtaService>();
            services.AddScoped<ISessaoPlenariaPautaService, SessaoPlenariaPautaService>();
            services.AddScoped<ISessaoPlenariaNotasTaquigraficasService, SessaoPlenariaNotasTaquigraficasService>();
            services.AddScoped<ISessaoPlenariaSustentacaoOralService, SessaoPlenariaSustentacaoOralService>();
            services.AddScoped<ISessaoPlenariaJurispudenciaService, SessaoPlenariaJurispudenciaService>();

            // Fiscalizacao
            services.AddScoped<IFiscalizacaoSecretariaControleExternoSeervice, FiscalizacaoSecretariaControleExternoSeervice>();
            services.AddScoped<IFiscalizacaoContasDoPrefeitoService, FiscalizacaoContasDoPrefeitoService>();
            services.AddScoped<IFiscalizacaoRelatoriosFiscalizacaoService, FiscalizacaoRelatoriosFiscalizacaoService>();
            services.AddScoped<IFiscalizacaoPlanoAnualFiscalizacaoService, FiscalizacaoPlanoAnualFiscalizacaoService>();

            //Servicos
            services.AddScoped<IMultasProcedimentosService, MultasProcedimentosService>();
            services.AddScoped<IOficioseIntimacoesService, OficioseIntimacoesService>();
            services.AddScoped<ICartorioService, CartorioService>();
            services.AddScoped<IProtocoloEletronicoService, ProtocoloEletronicoService>();
            services.AddScoped<ICartaServicosUsuarioService, CartaServicosUsuarioService>();
            services.AddScoped<IPrazosProcessuaisService, PrazosProcessuaisService>();
            services.AddScoped<IEmissaoCertidoesService, EmissaoCertidoesService>();
        }

        private static void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddS3Configuration(configuration);
            services.AddScoped<INoticiaRepository, NoticiaRepository>();
            services.AddScoped<INoticiaOldRepository, NoticiaOldRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IInstitucionalRepository, InstitucionalRepository>();

            //Plenaria
            services.AddScoped<ISessaoPlenariaRepository, SessaoPlenariaRepository>();
            services.AddScoped<ISessaoPlenariaAtasRepository, SessaoPlenariaAtasRepository>();
            services.AddScoped<ISessaoPlenariaJurispudenciaRepository, SessaoPlenariaJurispudenciaRepository>();
            services.AddScoped<ISessaoPlenariaNotasTaquigraficasRepository, SessaoPlenariaNotasTaquigraficasRepository>();
            services.AddScoped<ISessaoPlenariaPautaRepository, SessaoPlenariaPautaRepository>();
            services.AddScoped<ISessaoPlenariaSustentacaoOralRepository, SessaoPlenariaSustentacaoOralRepository>();

            //Fiscalizacao
            services.AddScoped<IFiscalizacaoSecretariaControleExternoRepository, FiscalizacaoSecretariaControleExternoRepository>();
            services.AddScoped<IFiscalizacaoContasDoPrefeitoRepository, FiscalizacaoContasDoPrefeitoRepository>();
            services.AddScoped<IFiscalizacaoRelatorioFiscalizacaoRepository, FiscalizacaoRelatoriosFiscalizacaoRepository>();
            services.AddScoped<IFiscalizacaoPlanoAnualFiscalizacaoRepository, FiscalizacaoPlanoAnualFiscalizacaoRepository>();

            //Servicos
            services.AddScoped<IMultasProcedimentosRepository, MultasProcedimentosRepository>();
            services.AddScoped<IMultasProcedimentosProcedimentoRepository, MultasProcedimentosProcedimentoRepository>();
            services.AddScoped<IMultasProcedimentosPortariaRelacionadaRepository, MultasProcedimentosPortariaRelacionadaRepository>();

            services.AddScoped<IOficioseIntimacoesRepository, OficioseIntimacoesRepository>();
            services.AddScoped<IOficioseIntimacoesSecaoItemRepository, OficioseIntimacoesSecaoItemRepository>();
            services.AddScoped<IOficioseIntimacoesSecaoRepository, OficioseIntimacoesSecaoRepository>();

            services.AddScoped<ICartorioRepository, CartorioRepository>();
            services.AddScoped<ICartorioAtendimentoRepository, CartorioAtendimentoRepository>();

            services.AddScoped<IProtocoloEletronicoRepository, ProtocoloEletronicoRepository>();
            services.AddScoped<IProtocoloEletronicoAcaoRepository, ProtocoloEletronicoAcaoRepository>();

            services.AddScoped<ICartaServicosUsuarioRepository, CartaServicosUsuarioRepository>();
            services.AddScoped<ICartaServicosUsuarioServicoRepository, CartaServicosUsuarioServicoRepository>();
            services.AddScoped<ICartaServicosUsuarioServicoItemRepository, CartaServicosUsuarioServicoItemRepository>();
            services.AddScoped<ICartaServicosUsuarioServicoItemDetalheRepository, CartaServicosUsuarioServicoItemDetalheRepository>();
            services.AddScoped<ICartaServicosUsuarioDescritivoItemDetalheRepository, CartaServicosUsuarioDescritivoItemDetalheRepository>();

            services.AddScoped<IPrazosProcessuaisRepository, PrazosProcessuaisRepository>();
            services.AddScoped<IPrazosProcessuaisItemRepository, PrazosProcessuaisItemRepository>();
            services.AddScoped<IPrazosProcessuaisItemAnexoRepository, PrazosProcessuaisItemAnexoRepository>();

            services.AddScoped<IEmissaoCertidoesRepository, EmissaoCertidoesRepository>();
            services.AddScoped<IEmissaoCertidoesAcaoRepository, EmissaoCertidoesAcaoRepository>();
            services.AddScoped<IEmissaoCertidoesSecaoOrientacaoRepository, EmissaoCertidoesSecaoOrientacaoRepository>();
        }
    }
}
