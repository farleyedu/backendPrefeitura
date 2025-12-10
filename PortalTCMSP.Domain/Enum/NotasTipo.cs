using System.ComponentModel.DataAnnotations;

namespace PortalTCMSP.Domain.Enum
{
    public enum NotasTipo
    {
        [Display(Name = "Ordinária")] Ordinaria = 1,
        [Display(Name = "Extraordinária")] Extraordinaria = 2,
        [Display(Name = "Especial")] Especial = 3,
        [Display(Name = "Primeira Câmara")] PrimeiraCamara = 4,
        [Display(Name = "Segunda Câmara")] SegundaCamara = 5
    }
}
