//using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
//using PortalTCMSP.Domain.Entities.NoticiaEntity;
//using System.Diagnostics.CodeAnalysis;

//namespace PortalTCMSP.Unit.Tests.Services.FixFeature
//{
//    [ExcludeFromCodeCoverage]
//    public class NoticiaServiceFixture
//    {
//        // Ajuste estes nomes se suas DTOs tiverem mais campos
//        public NoticiaCreateRequest GetCreateRequest(
//            int categoriaId = 1,
//            string slug = "noticia-de-teste",
//            bool ativo = true,
//            bool destaque = false)
//            => new NoticiaCreateRequest
//            {
//                Titulo = "Notícia de Teste",
//                Slug = slug,
//                CategoriaId = categoriaId,
//                ImagemUrl = "img/teste.jpg",
//                Ativo = ativo,
//                Destaque = destaque,
//                // Se a DTO tiver Subtitulo/Resumo/ConteudoNoticia pode setar aqui
//                Blocos = new List<NoticiaBlocoCreateRequest>
//                {
//                    new NoticiaBlocoCreateRequest
//                    {
//                        Ordem = 1,
//                        Tipo = "texto",
//                    }
//                }
//            };

//        public NoticiaUpdateRequest GetUpdateRequest(
//            long id = 1,
//            int categoriaId = 1,
//            string slug = "noticia-atualizada",
//            bool ativo = false,
//            bool destaque = true)
//            => new NoticiaUpdateRequest
//            {
//                Id = id,
//                Titulo = "Notícia Atualizada",
//                Slug = slug,
//                CategoriaId = categoriaId,
//                ImagemUrl = "img/atualizado.jpg",
//                Ativo = ativo,
//                Destaque = destaque,
//                Blocos = new List<NoticiaBlocoCreateRequest>
//                {
//                    new NoticiaBlocoCreateRequest
//                    {
//                        Ordem = 1,
//                        Tipo = "imagem",
//                    }
//                }
//            };

//        public PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia GetNoticiaEntity(long id = 1, int categoriaId = 1, string slug = "noticia-de-teste")
//            => new PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia
//            {
//                Id = id,
//                Titulo = "Notícia de Teste",
//                Slug = slug,
//                CategoriaId = categoriaId,
//                ImagemUrl = "img/teste.jpg",
//                Ativo = true,
//                Destaque = false,
//                ConteudoNoticia = null,
//                Blocos = new List<NoticiaBloco>()
//            };

//        public PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia GetNoticiaEntityWithBlocos(long id = 1, long categoriaId = 1, string slug = "noticia-com-blocos")
//            => new PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia
//            {
//                Id = id,
//                Titulo = "Notícia com Blocos",
//                Slug = slug,
//                CategoriaId = categoriaId,
//                ImagemUrl = "img/teste.jpg",
//                Ativo = true,
//                Destaque = false,
//                Blocos = new List<NoticiaBloco>
//                {
//                    new NoticiaBloco
//                    {
//                        Ordem = 1,
//                        Tipo = "texto",
//                        ConfigJson = "{\"Conteudo\":\"Bloco\"}",
//                        ValorJson = "{\"Valor\":\"Conteudo\"}"
//                    }
//                }
//            };

//        public Categoria GetCategoriaEntity(long id = 1, string nome = "Tecnologia")
//            => new Categoria
//            {
//                Id = id,
//                Nome = nome,
//                Slug = nome.ToLowerInvariant().Replace(' ', '-'),
//                Noticias = new List<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>()
//            };
//    }
//}
