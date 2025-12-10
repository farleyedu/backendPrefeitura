//using PortalTCMSP.Domain.Entities.NoticiaEntity;
//using System.Diagnostics.CodeAnalysis;

//namespace PortalTCMSP.Unit.Tests.Repository.FixFeature
//{
//    [ExcludeFromCodeCoverage]
//    public class NoticiaRepositoryFixture
//    {
//        public Categoria GetCategoria(long id = 1, string nome = "Tecnologia")
//            => new Categoria
//            {
//                Id = id,
//                Nome = nome,
//                Slug = nome.ToLower().Replace(' ', '-'),
//                // remova qualquer navegação antiga como "Noticias"
//                // se tiver navegação N:N, pode ser: NoticiaCategorias = new List<NoticiaCategoria>()
//            };

//        public NoticiaBloco GetBloco(int ordem = 1, string tipo = "texto")
//            => new NoticiaBloco
//            {
//                Ordem = ordem,
//                Tipo = tipo,
//                ConfigJson = "{\"layout\":\"padrão\"}",
//                ValorJson = "{\"conteudo\":\"Exemplo\"}"
//            };

//        public Noticia GetNoticia(
//            long id = 1,
//            string slug = "noticia-1",
//            Categoria? categoria = null,
//            bool ativo = true,
//            bool destaque = false,
//            string titulo = "Notícia 1",
//            string resumo = "Resumo padrão",
//            string autor = "Autor Padrão",
//            int blocos = 1,
//            DateTime? data = null)
//        {
//            var cat = categoria ?? GetCategoria();
//            var n = new Noticia
//            {
//                Id = id,
//                Slug = slug,
//                Titulo = titulo,
//                Resumo = resumo,
//                Ativo = ativo,
//                Destaque = destaque,
//                DataPublicacao = data ?? DateTime.UtcNow,
//                ImagemUrl = $"https://cdn.site.com/img/{id}.jpg",
//                Visualizacao = (int)(id * 10),
//                ConteudoNoticia = null,
//                Autoria = new Autoria { AutorNome = autor },
//                Metadados = new Metadados(),
//                Auditoria = new Auditoria()
//            };

//            // vincula categoria via tabela de junção (N:N)
//            n.NoticiaCategorias.Add(new NoticiaCategoria
//            {
//                Noticia = n,
//                NoticiaId = n.Id,
//                Categoria = cat,
//                CategoriaId = cat.Id
//            });

//            for (int i = 1; i <= blocos; i++)
//                n.Blocos.Add(GetBloco(i));

//            return n;
//        }

//        public (Noticia a, Noticia b, Noticia c) GetNoticiasParaBusca()
//        {
//            var catTec = GetCategoria(10, "Tecnologia");
//            var catEsp = GetCategoria(20, "Esportes");

//            var n1 = GetNoticia(1, "tec-1", catTec, ativo: true, destaque: true,
//                titulo: "Cloud Edge", resumo: "Resumo tecnologia cloud", autor: "Alice",
//                data: DateTime.UtcNow.AddHours(-2));
//            var n2 = GetNoticia(2, "tec-2", catTec, ativo: false, destaque: false,
//                titulo: "Kubernetes Evolução", resumo: "Container orchestration", autor: "Bob",
//                data: DateTime.UtcNow.AddHours(-1));
//            var n3 = GetNoticia(3, "esp-1", catEsp, ativo: true, destaque: false,
//                titulo: "Futebol Tático", resumo: "Resumo esporte análise", autor: "Carlos",
//                data: DateTime.UtcNow.AddHours(-3));

//            return (n1, n2, n3);
//        }
//    }
//}
