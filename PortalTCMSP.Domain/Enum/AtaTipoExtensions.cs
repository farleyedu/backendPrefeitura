using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PortalTCMSP.Domain.Enum
{
    [ExcludeFromCodeCoverage]
    public static class AtaTipoExtensions
    {
        public static string GetDisplayName(this AtaTipo value)
        {
            var member = typeof(AtaTipo).GetMember(value.ToString()).First();
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }
    }
}
