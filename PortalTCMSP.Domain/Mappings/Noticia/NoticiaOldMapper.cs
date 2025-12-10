using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Entities.NoticiaEntity;

namespace PortalTCMSP.Domain.Mappings.Noticia
{
    public static class NoticiaOldMapper
    {
        public static NoticiaOldResponse ToResponse(this NoticiaOld n) => new()
        {
            Id = n.Id,
            Slug = n.Slug,
            Titulo = n.Titulo,
            Subtitulo = n.Subtitulo,
            Resumo = n.Resumo,
            Tags = n.Tags,
            CategoriasExtras = n.CategoriasExtras,
            Autor_Nome = n.Autor_Nome,
            Autor_Creditos = n.Autor_Creditos,
            Seo_Title = n.Seo_Title,
            Seo_Description = n.Seo_Description,
            Og_Image_Url = n.Og_Image_Url,
            Canonical = n.Canonical,
            DataPublicacao = n.DataPublicacao,
            Ativo = n.Ativo,
            Destaque = n.Destaque,
            Visualizacao = n.Visualizacao,
            ImagemUrl = n.ImagemUrl,
            ConteudoNoticia = n.ConteudoNoticia,
            Criado_Em = n.Criado_Em,
            Criado_Por = n.Criado_Por,
            Atualizado_Em = n.Atualizado_Em,
            Atualizado_Por = n.Atualizado_Por,
            Versao = n.Versao,
            Antiga = true,
            CategoriaId = n.CategoriaId
        };

        public static ResultadoPaginadoResponse<NoticiaOldResponse> BuildList(
            int page, int count, int total, IEnumerable<NoticiaOld> list)
            => new(page, count, total, list.Select(ToResponse).ToList());

        public static ResultadoPaginadoResponse<NoticiaOldMappedResponse> BuildList(
    int page, int count, int total, IEnumerable<NoticiaOldMappedResponse> items)
    => new(page, count, total, items);

        public static NoticiaOldMappedResponse ToMappedResponse(NoticiaOld e)
        {
            return new NoticiaOldMappedResponse
            {
                Id = e.Id,
                Slug = e.Slug,
                Titulo = e.Titulo,
                Subtitulo = StripToPlainText(e.Subtitulo),
                Resumo = StripToPlainText(e.Resumo),
                AutorNome = e.Autor_Nome,
                Creditos = e.Autor_Creditos,
                SeoTitulo = StripToPlainText(e.Seo_Title),
                SeoDescricao = StripToPlainText(e.Seo_Description),
                OgImageUrl = NullIfEmpty(e.Og_Image_Url),
                Canonical = e.Canonical,
                Ativo = e.Ativo,
                Destaques = e.Destaque,
                Tags = e.Tags,
                CategoriasExtras = e.CategoriasExtras,
                ImageUrl = PreferImage(e.ImagemUrl, e.Og_Image_Url),
                PublicadoQuando = e.DataPublicacao,
                CriadoEm = e.Criado_Em,
                CriadoPor = e.Criado_Por,
                AtualizadoEm = e.Atualizado_Em,
                AtualizadoPor = e.Atualizado_Por,
                Versao = e.Versao,
                Antiga = true,
                Blocos = BuildBlocks(e)
            };
        }

        private static List<NoticiaOldMappedResponse.BlocoResponse> BuildBlocks(NoticiaOld e)
        {
            var blocos = new List<NoticiaOldMappedResponse.BlocoResponse>();

            var resumoBlocks = NoticiaContentMapper.ParseHtmlToBlocks<NoticiaOldMappedResponse.BlocoResponse>(e.Resumo);
            if (resumoBlocks.Count > 0) blocos.AddRange(resumoBlocks);

            var contentBlocks = NoticiaContentMapper.ParseHtmlToBlocks<NoticiaOldMappedResponse.BlocoResponse>(e.ConteudoNoticia);
            blocos.AddRange(contentBlocks);

            for (int i = 0; i < blocos.Count; i++) blocos[i].Ordem = i + 1;
            return blocos;
        }

        private static string StripToPlainText(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;
            return NoticiaContentMapper.StripToPlainText(html);
        }

        private static string? NullIfEmpty(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s!.Trim();

        private static string? PreferImage(string? a, string? b)
            => NullIfEmpty(a) ?? NullIfEmpty(b);
    }

}
