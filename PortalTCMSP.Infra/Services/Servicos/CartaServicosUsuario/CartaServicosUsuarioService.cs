using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Mappings.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Mappings.SessaoPlenaria;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario.Domain.Interfaces.Servicos.CartaServicosUsuarioInterface;
using PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Services.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioService(ICartaServicosUsuarioRepository repo,
            ICartaServicosUsuarioServicoRepository repoServicos,
            ICartaServicosUsuarioServicoItemRepository repoServicosItens,
            ICartaServicosUsuarioServicoItemDetalheRepository repoItemDetalhe,
            ICartaServicosUsuarioDescritivoItemDetalheRepository repoDescritivo) : ICartaServicosUsuarioService
    {
        private readonly ICartaServicosUsuarioRepository _repo = repo;
        private readonly ICartaServicosUsuarioServicoRepository _repoServicos = repoServicos;
        private readonly ICartaServicosUsuarioServicoItemRepository _repoServicosItens = repoServicosItens;
        private readonly ICartaServicosUsuarioServicoItemDetalheRepository _repoItemDetalhe = repoItemDetalhe;
        private readonly ICartaServicosUsuarioDescritivoItemDetalheRepository _repoDescritivo = repoDescritivo;

        public async Task<CartaServicosUsuarioResponse?> GetSearchAsync(
    CartaServicosUsuarioDescritivoItemDetalheSearchRequest request)
        {
            var carta = await _repo.Search(request);

            if (carta is null)
                return null;

            return carta.ToResponse();
        }

        public async Task<IEnumerable<CartaServicosUsuarioResponse>> GetAllAsync()
        {
            var all = await _repo.AllWithChildrenAsync();
            return all.ToResponse();
        }

        public async Task<CartaServicosUsuarioResponse?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetWithChildrenByIdAsync(id);
            return entity?.ToResponse();
        }
        public async Task<CartaServicosUsuarioResponse> CreateAsync(CartaServicosUsuarioRequest request)
        {
            var existeSlugAtivo = await _repo.GetBySlugAtivoAsync(request.Slug);
            if (existeSlugAtivo is not null)
                await _repo.DisableAsync(existeSlugAtivo.Id);

            var now = DateTime.UtcNow;
            var entity = request.FromCreate(now);
            await _repo.InsertAsync(entity);
            await _repo.CommitAsync();

            return entity.ToResponse();
        }

        public async Task<CartaServicosUsuarioResponse?> CreateServicosAsync(
    long idCartaServicosUsuario,
    IEnumerable<CartaServicosUsuarioServicoRequest> novos)
        {
            var entidades = novos.Select(s => s.ToEntity(idCartaServicosUsuario));

            await _repoServicos.CreateServicosAsync(idCartaServicosUsuario, entidades);
            await _repo.CommitAsync();

            // id da rota = id da carta
            return await LoadCartaByIdAsync(idCartaServicosUsuario);
        }


        public async Task<CartaServicosUsuarioResponse?> CreateServicosItensAsync(
    long idCartaServicosUsuarioServico,
    IEnumerable<CartaServicosUsuarioServicoItemRequest> novos)
        {
            var entidades = novos.Select(i => i.ToEntity(idCartaServicosUsuarioServico));

            await _repoServicosItens.CreateServicosItensAsync(idCartaServicosUsuarioServico, entidades);
            await _repo.CommitAsync();

            // id da rota = id do serviço
            return await LoadCartaByServicoIdAsync(idCartaServicosUsuarioServico);
        }
        public async Task<CartaServicosUsuarioResponse?> CreateServicosItensDetalhesAsync(
    long idCartaServicosUsuarioServicoItem,
    IEnumerable<CartaServicosUsuarioItemDetalheRequest> novos)
        {
            var entidades = novos.Select(d => d.ToEntity(idCartaServicosUsuarioServicoItem));

            await _repoItemDetalhe.CreateServicosItensDetalhesAsync(idCartaServicosUsuarioServicoItem, entidades);
            await _repo.CommitAsync();

            // id da rota = id do item de serviço
            return await LoadCartaByServicoItemIdAsync(idCartaServicosUsuarioServicoItem);
        }

        public async Task<CartaServicosUsuarioResponse?> CreateDescritivoItemDetalheAsync(
    long idCartaServicosUsuarioItemDetalhe,
    IEnumerable<CartaServicosUsuarioDescritivoItemDetalheRequest> novos)
        {
            var entidades = novos.Select(d => d.ToEntity(idCartaServicosUsuarioItemDetalhe));

            await _repoDescritivo.CreateDescritivoItemDetalheAsync(idCartaServicosUsuarioItemDetalhe, entidades);
            await _repo.CommitAsync();

            // id da rota = id do item detalhe
            return await LoadCartaByItemDetalheIdAsync(idCartaServicosUsuarioItemDetalhe);
        }
        public async Task<bool> UpdateAsync(long id, CartaServicosUsuarioRequest request)
        {
            var entity = await _repo.GetWithChildrenByIdAsync(id);
            if (entity is null) return false;

            entity.ApplyUpdate(request, DateTime.UtcNow);

            //var novos = request.Servicos?.Select(p => new CartaServicosUsuarioServico
            //{
            //    IdCartaServicosUsuario = entity.Id,
            //    Ordem = p.Ordem,
            //    Ativo = p.Ativo,
            //    Titulo = p.Titulo?.Trim() ?? string.Empty,
            //    ServicosItens = p.ServicosItens?.Select(i => new CartaServicosUsuarioServicoItem
            //    {
            //        Ordem = i.Ordem,
            //        Ativo = i.Ativo,
            //        Titulo = i.Titulo?.Trim() ?? string.Empty,
            //        Acao = i.Acao?.Trim() ?? string.Empty,
            //        LinkItem = i.LinkItem?.Trim() ?? string.Empty,
            //        ItemDetalhe = (i.ItemDetalhe?.Select(d => new CartaServicosUsuarioItemDetalhe
            //        {
            //            Ordem = d.Ordem,
            //            Ativo = d.Ativo,
            //            TituloDetalhe = d.TituloDetalhe?.Trim() ?? string.Empty,
            //            DescritivoItemDetalhe = (d.DescritivoItemDetalhe?.Select(x => new CartaServicosUsuarioDescritivoItemDetalhe
            //            {
            //                Ordem = x.Ordem,
            //                Descritivo = x.Descritivo?.Trim() ?? string.Empty
            //            }).ToList() ?? [])
            //        }).ToList() ?? [])
            //    }).ToList() ?? []
            //}) ?? [];

            //await _repo.ReplaceServicosAsync(entity.Id, novos);

            await _repo.UpdateAsync(entity);
            return await _repo.CommitAsync();
        }

        public async Task<CartaServicosUsuarioResponse?> UpdateServicosAsync(
    long idCartaServicosUsuario,
    IEnumerable<CartaServicosUsuarioServicoUpdate> novos)
        {
            var entidade = await _repo.GetWithChildrenByIdAsync(idCartaServicosUsuario)
                ?? throw new InvalidOperationException("CartaServicosUsuario não encontrada.");

            var antigos = entidade.Servicos;
            var lista = new List<CartaServicosUsuarioServico>();

            foreach (var novo in novos)
            {
                var existente = antigos.FirstOrDefault(a => a.Id == novo.Id);
                if (existente is not null)
                {
                    existente.Ordem = novo.Ordem;
                    existente.Titulo = novo.Titulo?.Trim() ?? string.Empty;
                    existente.Ativo = novo.Ativo;

                    lista.Add(existente);
                }
            }

            await _repoServicos.UpdateServicosAsync(idCartaServicosUsuario, lista);
            await _repo.CommitAsync();

            return await LoadCartaByIdAsync(idCartaServicosUsuario);
        }

        public async Task<CartaServicosUsuarioResponse?> UpdateServicosItensAsync(
     long idCartaServicosUsuarioServico,
     IEnumerable<CartaServicosUsuarioServicoItemUpdate> novos)
        {
            var entidades = novos.Select(s => new CartaServicosUsuarioServicoItem
            {
                Id = s.Id, // precisa existir no DTO de update
                IdCartaServicosUsuarioServico = idCartaServicosUsuarioServico,
                Ordem = s.Ordem,
                Ativo = s.Ativo,
                Titulo = s.Titulo?.Trim() ?? string.Empty,
                LinkItem = s.LinkItem?.Trim() ?? string.Empty,
                Acao = s.Acao?.Trim() ?? string.Empty
            });

            await _repoServicosItens.UpdateServicosItensAsync(idCartaServicosUsuarioServico, entidades);
            await _repo.CommitAsync();

            return await LoadCartaByServicoIdAsync(idCartaServicosUsuarioServico);
        }

        public async Task<CartaServicosUsuarioResponse?> UpdateServicosItensDetalhesAsync(
    long idCartaServicosUsuarioServicoItem,
    IEnumerable<CartaServicosUsuarioItemDetalheUpdate> novos)
        {
            var entidades = novos.Select(s => new CartaServicosUsuarioItemDetalhe
            {
                Id = s.Id, // idem, precisa existir no DTO
                IdCartaServicosUsuarioServicoItem = idCartaServicosUsuarioServicoItem,
                Ordem = s.Ordem,
                Ativo = s.Ativo,
                TituloDetalhe = s.TituloDetalhe?.Trim() ?? string.Empty
            });

            await _repoItemDetalhe.UpdateServicosItensDetalhesAsync(idCartaServicosUsuarioServicoItem, entidades);
            await _repo.CommitAsync();

            return await LoadCartaByServicoItemIdAsync(idCartaServicosUsuarioServicoItem);
        }

        public async Task<CartaServicosUsuarioResponse?> UpdateDescritivoItemDetalheAsync(
    long idCartaServicosUsuarioItemDetalhe,
    IEnumerable<CartaServicosUsuarioDescritivoItemDetalheUpdate> novos)
        {
            var entidades = novos.Select(s => new CartaServicosUsuarioDescritivoItemDetalhe
            {
                Id = s.Id, // idem
                IdCartaServicosUsuarioItemDetalhe = idCartaServicosUsuarioItemDetalhe,
                Ordem = s.Ordem,
                Descritivo = s.Descritivo?.Trim() ?? string.Empty
            });

            await _repoDescritivo.UpdateDescritivoItemDetalheAsync(idCartaServicosUsuarioItemDetalhe, entidades);
            await _repo.CommitAsync();

            return await LoadCartaByItemDetalheIdAsync(idCartaServicosUsuarioItemDetalhe);
        }


        public async Task<bool> DeleteAsync(long id)
        {
            var e = await _repo.FindByIdAsync(id);
            if (e is null) return false;

            await _repo.DeleteAsync(e);
            return await _repo.CommitAsync();
        }

        public async Task<CartaServicosUsuarioResponse?> GetWithChildrenBySlugAtivoAsync(string slug)
            => (await _repo.GetWithChildrenBySlugAtivoAsync(slug))?.ToResponse();
        public async Task<bool> DisableAsync(long id)
        {
            return await _repo.DisableAsync(id);
        }

        private async Task<CartaServicosUsuarioResponse?> LoadCartaByIdAsync(long cartaId)
        {
            var entity = await _repo.GetWithChildrenByIdAsync(cartaId);
            return entity?.ToResponse();
        }

        private async Task<CartaServicosUsuarioResponse?> LoadCartaByServicoIdAsync(long servicoId)
        {
            var entity = await _repo.GetByServicoIdWithChildrenAsync(servicoId);
            return entity?.ToResponse();
        }

        private async Task<CartaServicosUsuarioResponse?> LoadCartaByServicoItemIdAsync(long servicoItemId)
        {
            var entity = await _repo.GetByServicoItemIdWithChildrenAsync(servicoItemId);
            return entity?.ToResponse();
        }

        private async Task<CartaServicosUsuarioResponse?> LoadCartaByItemDetalheIdAsync(long itemDetalheId)
        {
            var entity = await _repo.GetByItemDetalheIdWithChildrenAsync(itemDetalheId);
            return entity?.ToResponse();
        }

        private async Task<CartaServicosUsuarioResponse?> LoadCartaByDescritivoIdAsync(long descritivoId)
        {
            var entity = await _repo.GetByDescritivoIdWithChildrenAsync(descritivoId);
            return entity?.ToResponse();
        }
    }
}

