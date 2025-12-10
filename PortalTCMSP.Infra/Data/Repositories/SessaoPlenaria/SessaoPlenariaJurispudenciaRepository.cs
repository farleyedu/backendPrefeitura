using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaJurispudenciaRepository(PortalTCMSPContext context) : BaseRepository<SessaoPlenariaJurispudencia>(context), ISessaoPlenariaJurispudenciaRepository
    {
        public Task<SessaoPlenariaJurispudencia?> FindBySlugAsync(string slug)
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<SessaoPlenariaJurispudencia?> GetAtivaAsync()
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Ativo);

        public async Task<int> DesativarTodosExcetoAsync(long? idMantidoAtivo)
        {
            var itens = await Set
                .Where(x => x.Ativo && (!idMantidoAtivo.HasValue || x.Id != idMantidoAtivo.Value))
                .ToListAsync();

            foreach (var i in itens)
            {
                i.Ativo = false;
                i.DataAtualizacao = DateTime.UtcNow;
            }
            return itens.Count;
        }
    }
}
