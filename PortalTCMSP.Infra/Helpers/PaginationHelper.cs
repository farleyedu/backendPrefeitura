using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Infra.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class PaginationHelper<TEntity> where TEntity : class
    {
        public static IEnumerable<TEntity> Set(int page, int count, IEnumerable<TEntity> listData)
        {
            return listData.Skip((page - 1) * count)
                    .Take(count);
        }
    }
}
