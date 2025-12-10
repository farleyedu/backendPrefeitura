using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PortalTCMSP.Domain.Enum
{
    [ExcludeFromCodeCoverage]
    public static class PautaTipoExtensions
    {
        public static string GetDisplayName(this PautaTipo value)
        {
            var member = typeof(PautaTipo).GetMember(value.ToString()).First();
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }

        public static PautaTipo ParseFromName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Tipo inválido.");

            var norm = input.Trim();

            foreach (var v in System.Enum.GetValues(typeof(PautaTipo)).Cast<PautaTipo>())
            {
                if (string.Equals(v.GetDisplayName(), norm, StringComparison.OrdinalIgnoreCase))
                    return v;
            }

            if (System.Enum.TryParse<PautaTipo>(norm, true, out var parsed))
                return parsed;

            throw new ArgumentException($"Tipo desconhecido: {input}");
        }
    }
}
