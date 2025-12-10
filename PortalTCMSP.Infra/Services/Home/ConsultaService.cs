using PortalTCMSP.Domain.DTOs.Requests.Consulta;
using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Consulta;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Domain.Services.Home;
using System.Net;
using System.Text.RegularExpressions;

namespace PortalTCMSP.Infra.Services.Home
{
    public class ConsultaService : IConsultaService
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly INoticiaOldRepository _noticiaOldRepository;

        public ConsultaService(
            INoticiaRepository noticiaRepository,
            INoticiaOldRepository noticiaOldRepository)
        {
            _noticiaRepository = noticiaRepository;
            _noticiaOldRepository = noticiaOldRepository;
        }

        public ResultadoPaginadoResponse<ConsultaNoticiaItemResponse> BuscarNoticiasConsolidado(ConsultaNoticiasRequest request)
        {
            // Normalizamos paginação
            var page = request.Page < 1 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            // Monta requests específicos reaproveitando seus filtros existentes
            var reqNova = new NoticiaListarRequest
            {
                Query = request.Query,
                Categoria = request.Categoria,
                ApenasAtivas = request.ApenasAtivas,
                Page = 1,   // vamos paginar só no consolidado
                Count = int.MaxValue
            };

            var reqOld = new NoticiaOldListarRequest
            {
                Query = request.Query,      // se seu NoticiaOldListarRequest ainda não tem Query/Categoria, mantenha nulo
                Categoria = request.Categoria,
                ApenasAtivas = request.ApenasAtivas,
                Page = 1,
                Count = int.MaxValue
            };

            // Repositórios já retornam IEnumerable filtrado e ordenado (Nova: Destaque desc, Data desc; Old: idem)
            var novas = _noticiaRepository.Search(reqNova).ToList();
            var antigas = _noticiaOldRepository.Search(reqOld).ToList();

            // Map para o DTO unificado
            var novasMapped = novas.Select(n => new ConsultaNoticiaItemResponse
            {
                Id = n.Id,
                Slug = n.Slug,
                Titulo = n.Titulo,
                Resumo = n.Resumo,
                PublicadoQuando = n.DataPublicacao,
                Destaque = n.Destaque,
                ImageUrl = n.ImagemUrl,
                IsOld = false,
                Categorias = n.NoticiaCategorias
                    .Select(nc => new CategoriaItem { Id = nc.Categoria.Id, Nome = nc.Categoria.Nome, Slug = nc.Categoria.Slug })
                    .ToList()
            }).ToList();

            var antigasMapped = antigas.Select(o => new ConsultaNoticiaItemResponse
            {
                Id = o.Id,
                Slug = o.Slug,
                Titulo = HtmlToPlain(o.Titulo),        
                Resumo = HtmlToPlain(o.Resumo),
                PublicadoQuando = o.DataPublicacao,
                Destaque = o.Destaque,
                ImageUrl = string.IsNullOrWhiteSpace(o.ImagemUrl) ? o.Og_Image_Url : o.ImagemUrl,
                IsOld = true,
                // NoticiaOld não tem join de categoria; se houver CategoriaId único, você pode mapear de outra fonte futuramente
                Categorias = new List<CategoriaItem>()
            }).ToList();

            // Requisito: priorizar sempre as novas; se esgotar as novas em uma página, completar com antigas.
            // Basta concatenar: novas (já ordenadas) + antigas (já ordenadas)
            var merged = new List<ConsultaNoticiaItemResponse>(novasMapped.Count + antigasMapped.Count);
            merged.AddRange(novasMapped);
            merged.AddRange(antigasMapped);

            var total = merged.Count;
            var pageItems = merged
                .Skip((page - 1) * count)
                .Take(count)
                .ToList();

            return new ResultadoPaginadoResponse<ConsultaNoticiaItemResponse>(page, count, total, pageItems);
        }

        private static string? HtmlToPlain(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;

            var withBreaks = Regex.Replace(html, @"<\s*br\s*/?\s*>", "\n", RegexOptions.IgnoreCase);

            var noTags = Regex.Replace(withBreaks, "<.*?>", string.Empty, RegexOptions.Singleline);

            var decoded = WebUtility.HtmlDecode(noTags);

            var collapsedSpaces = Regex.Replace(decoded, @"[ \t]+", " ");
            var normalizedLines = Regex.Replace(collapsedSpaces, @"\s*\n\s*", "\n").Trim();

            return string.IsNullOrWhiteSpace(normalizedLines) ? null : normalizedLines;
        }
    }
}
