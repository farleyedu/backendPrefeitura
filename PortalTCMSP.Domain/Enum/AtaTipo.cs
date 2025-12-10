using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Enum
{
    public enum AtaTipo
    {
        [Display(Name = "Ordinária")] Ordinaria = 1,
        [Display(Name = "Extraordinária")] Extraordinaria = 2,
        [Display(Name = "Especial")] Especial = 3,
        [Display(Name = "Primeira Câmara")] PrimeiraCamara = 4,
        [Display(Name = "Segunda Câmara")] SegundaCamara = 5,
        [Display(Name = "Sessão Ordinária Não Presencial")] OrdinariaNaoPresencial = 6,
        [Display(Name = "Sessão Extraordinária Não Presencial")] ExtraordinariaNaoPresencial = 7
    }
}
