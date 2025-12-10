using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Base
{
    [ExcludeFromCodeCoverage]
    public class ResultadoPaginadoResponse<TEntity> where TEntity : class
    {
        public int Page { get; }
        public int Total { get; }
        public int Count { get; }
        public IEnumerable<TEntity> List { get; }

        public ResultadoPaginadoResponse(int pagina, int tamanhoPaginacao, int resultadoTotal, IEnumerable<TEntity> lista)
        {
            Page = pagina;
            Total = resultadoTotal;
            Count = tamanhoPaginacao;
            List = lista;
        }
    }
}