namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    public class NoticiaCategoria
    {
        public long NoticiaId { get; set; }
        public Noticia Noticia { get; set; } = default!;
        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = default!;
    }
}
