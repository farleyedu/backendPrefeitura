using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PortalTCMSP.Domain.Enum
{
    [ExcludeFromCodeCoverage]
    public static class NotasTipoExtensions
    {
        public static string GetDisplayName(this NotasTipo value)
        {
            var member = typeof(NotasTipo).GetMember(value.ToString()).First();
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }
    }
}
