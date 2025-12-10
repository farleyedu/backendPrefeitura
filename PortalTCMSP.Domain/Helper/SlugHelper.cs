using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PortalTCMSP.Domain.Helper
{
    [ExcludeFromCodeCoverage]
    public static class SlugHelper
    {
        public static string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string normalized = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            var result = sb.ToString().Normalize(NormalizationForm.FormC);
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
            result = Regex.Replace(result, @"\s+", "-").Trim('-');
            result = Regex.Replace(result, "-{2,}", "-");
            return result;
        }
    }
}
