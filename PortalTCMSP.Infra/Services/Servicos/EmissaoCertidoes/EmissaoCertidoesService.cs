using PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Mappings.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Services.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Infra.Services.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesService(
       IEmissaoCertidoesRepository repo,
       IEmissaoCertidoesAcaoRepository repoAcoes,
       IEmissaoCertidoesSecaoOrientacaoRepository repoSecao) : IEmissaoCertidoesService
    {
        private readonly IEmissaoCertidoesRepository _repo = repo;
        private readonly IEmissaoCertidoesAcaoRepository _repoAcoes = repoAcoes;
        private readonly IEmissaoCertidoesSecaoOrientacaoRepository _repoSecao = repoSecao;

        public async Task<IEnumerable<EmissaoCertidoesResponse>> GetAllAsync()
            => (await _repo.AllWithChildrenAsync()).ToResponse();

        public async Task<EmissaoCertidoesResponse?> GetByIdAsync(long id)
            => (await _repo.GetWithChildrenByIdAsync(id))?.ToResponse();

        public async Task<bool> DisableAsync(long id)
            => await _repo.DisableAsync(id);

        public async Task<long> CreateAsync(EmissaoCertidoesCreateRequest request)
        {
            var existe = await _repo.GetBySlugAtivoAsync(SlugHelper.Slugify(request.Slug));
            if (existe is not null) await _repo.DisableAsync(existe.Id);

            var entity = request.FromCreate(DateTime.UtcNow);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> CreateAcoesAsync(long id, List<EmissaoCertidoesAcaoRequest> novas)
        {
            await _repoAcoes.CreateAcoesAsync(id, novas.Select(a => new EmissaoCertidoesAcao
            {
                IdEmissaoCertidoes = id,
                Ativo = a.Ativo,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                DataPublicacao = a.DataPublicacao,
                TipoAcao = a.TipoAcao,
                UrlAcao = a.UrlAcao.Trim()
            }));

            return await _repoAcoes.CommitAsync();
        }

        public async Task<bool> CreateSecoesAsync(long id, List<EmissaoCertidoesSecaoOrientacaoRequest> novos)
        {
            var e = await _repo.FindByIdAsync(id) ?? throw new InvalidOperationException($"EmissaoCertidoes with Id {id} not found.");
            var entidades = novos.Select(s => new EmissaoCertidoesSecaoOrientacao
            {
                IdEmissaoCertidoes = id,
                TipoSecao = s.TipoSecao,
                TituloPagina = s.TituloPagina.Trim(),
                Descricao = s.Descricao.Trim(),
                Orientacoes = s.Orientacoes?.Select(o => new EmissaoCertidoesOrientacao
                {
                    Ordem = o.Ordem,
                    Descritivos = o.Descritivos?.Select(d => new EmissaoCertidoesDescritivo { Texto = d.Trim() }).ToList() ?? []
                }).ToList() ?? []
            });

            await _repoSecao.CreateScoesAsync(id, entidades);
            return await _repoSecao.CommitAsync();
        }

        public async Task<EmissaoCertidoesResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();

        private async Task<EmissaoCertidoesResponse?> GetBySlugAtivoAsync(string slug)
             => (await _repo.GetBySlugAtivoAsync(slug))?.ToResponse();

        public async Task<bool> UpdateAsync(long id, EmissaoCertidoesUpdateRequest request)
        {
            var e = await _repo.GetWithChildrenByIdAsync(id);
            if (e is null) return false;
            e.ApplyUpdate(request, DateTime.UtcNow);

            var novasAcoes = request.Acoes?.Select(a => new EmissaoCertidoesAcao
            {
                IdEmissaoCertidoes = e.Id,
                Ativo = a.Ativo,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                DataPublicacao = a.DataPublicacao,
                TipoAcao = a.TipoAcao,
                UrlAcao = a.UrlAcao.Trim()
            }) ?? [];

            var novasSecoes = request.SecaoOrientacoes?.Select(s => new EmissaoCertidoesSecaoOrientacao
            {
                IdEmissaoCertidoes = e.Id,
                TipoSecao = s.TipoSecao,
                TituloPagina = s.TituloPagina.Trim(),
                Descricao = s.Descricao.Trim(),
                Orientacoes = s.Orientacoes?.Select(o => new EmissaoCertidoesOrientacao
                {
                    Ordem = o.Ordem,
                    Descritivos = o.Descritivos?.Select(d => new EmissaoCertidoesDescritivo { Texto = d.Trim() }).ToList() ?? new List<EmissaoCertidoesDescritivo>()
                }).ToList() ?? []
            }) ?? [];

            await _repo.ReplaceAcoesAsync(e.Id, novasAcoes);
            await _repo.ReplaceSecoesAsync(e.Id, novasSecoes);

            await _repo.UpdateAsync(e);
            return await _repo.CommitAsync();
        }
        public async Task<bool> UpdateAcoesAsync(long id, List<EmissaoCertidoesAcaoUpdate> novas)
        {
            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem emissaoCertidoes para serem atualizados.");
            var acoesAntigas = entidadesAntigas.Acoes;

            foreach (var nova in novas)
            {
                var ef = await _repoAcoes.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"Acao com Id {nova.Id} não encontrado.");

                foreach (var antiga in acoesAntigas)
                {
                    if (nova.Id == antiga.Id)
                    {
                        antiga.Ordem = nova.Ordem;
                        antiga.TipoAcao = nova.TipoAcao;
                        antiga.Titulo = nova.Titulo.Trim();
                        antiga.UrlAcao = nova.UrlAcao.Trim();
                        antiga.DataPublicacao = nova.DataPublicacao;
                        antiga.Ativo = nova.Ativo;
                    }
                }
            }

            await _repoAcoes.UpdateAcoesAsync(id, acoesAntigas);
            return await _repoAcoes.CommitAsync();
        }
        public async Task<bool> UpdateSecoesAsync(long id, List<EmissaoCertidoesSecaoOrientacaoUpdate> novas)
        {
            var entidadesAntigas = await _repo.GetWithChildrenByIdAsync(id) ?? throw new InvalidOperationException("Não existem emissaoCertidoes para serem atualizados.");
            var secaoOrientacoesAntigas = entidadesAntigas.SecaoOrientacoes;

            foreach (var nova in novas)
            {
                var ef = await _repoSecao.FindByIdAsync(nova.Id) ?? throw new InvalidOperationException($"SecaoOrientacao. com Id {nova.Id} não encontrada.");

                foreach (var antiga in secaoOrientacoesAntigas)
                {
                    if (nova.Id == antiga.Id)
                    {
                        antiga.TipoSecao = nova.TipoSecao;
                        antiga.TituloPagina = nova.TituloPagina.Trim();
                        antiga.Descricao = nova.Descricao.Trim();

                        if (nova.Orientacoes != null)
                        {
                            antiga.Orientacoes.Clear();

                            foreach (var orientacao in nova.Orientacoes)
                            {
                                var novaOrientacao = new EmissaoCertidoesOrientacao
                                {
                                    Ordem = orientacao.Ordem,
                                    Descritivos = orientacao.Descritivos?.Select(d => new EmissaoCertidoesDescritivo { Texto = d.Trim() }).ToList() ?? new List<EmissaoCertidoesDescritivo>()
                                };
                                antiga.Orientacoes.Add(novaOrientacao);
                            }
                        }
                    }
                }
            }

            await _repoSecao.UpdateSecoesAsync(id, secaoOrientacoesAntigas);
            return await _repoSecao.CommitAsync();
        }
        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }
    }
}
