using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace PortalTCMSP.Domain.Mappings.Noticia
{
    public static class NoticiaContentMapper
    {
        public static string? StripToPlainText(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;
            var s = Regex.Replace(html, "<.*?>", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            s = HttpUtility.HtmlDecode(s);
            s = Regex.Replace(s, @"\s+", " ").Trim();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        public static List<TBloco> ParseHtmlToBlocks<TBloco>(string? html)
            where TBloco : class, new()
        {
            var blocks = new List<TBloco>();
            if (string.IsNullOrWhiteSpace(html)) return blocks;

            var ps = Regex.Matches(html, @"<p[^>]*>(.*?)</p>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var paragraphs = ps.Count > 0
                ? ps.Select(m => m.Groups[1].Value).ToList()
                : new List<string> { html };

            int ordem = 0;

            foreach (var pHtml in paragraphs)
            {
                var parts = Regex.Split(pHtml, @"<br\s*/?>", RegexOptions.IgnoreCase);
                foreach (var part in parts)
                {
                    var trimmed = part?.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed)) continue;

                    if (trimmed.Contains("Importante", StringComparison.OrdinalIgnoreCase) &&
                        Regex.IsMatch(trimmed, @"<span[^>]*background[^>]*>", RegexOptions.IgnoreCase))
                    {
                        var content = StripToPlainText(Regex.Replace(trimmed, @"<span[^>]*>Importante!?<\/span>", "", RegexOptions.IgnoreCase));
                        blocks.Add(NewBlock<TBloco>(++ordem, "callout",
                            new { title = "Importante!", content },
                            new { style = "warning" }));
                        continue;
                    }

                    var strongStartMatch = Regex.Match(trimmed, @"^\s*<strong[^>]*>(.*?)</strong>(.*)$",
                        RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    if (strongStartMatch.Success)
                    {
                        var title = StripToPlainText(strongStartMatch.Groups[1].Value);
                        var rest = StripToPlainText(strongStartMatch.Groups[2].Value);
                        if (!string.IsNullOrWhiteSpace(title))
                            blocks.Add(NewBlock<TBloco>(++ordem, "heading", new { text = title, level = 3 }));
                        if (!string.IsNullOrWhiteSpace(rest))
                            blocks.Add(NewBlock<TBloco>(++ordem, "paragraph", new { text = rest }));
                        continue;
                    }

                    var aMatches = Regex.Matches(trimmed, @"<a[^>]*href\s*=\s*[""']([^""']+)[""'][^>]*>(.*?)</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    if (aMatches.Count >= 2 && IsMostlyLinks(trimmed))
                    {
                        var items = aMatches
                            .Select(m => new
                            {
                                href = m.Groups[1].Value.Trim(),
                                text = StripToPlainText(m.Groups[2].Value) ?? m.Groups[1].Value.Trim()
                            })
                            .Where(x => !string.IsNullOrWhiteSpace(x.href))
                            .ToArray();

                        if (items.Length > 0)
                            blocks.Add(NewBlock<TBloco>(++ordem, "links", new { items }));
                        continue;
                    }

                    if (aMatches.Count == 1 && IsMostlySingleLink(trimmed))
                    {
                        var href = aMatches[0].Groups[1].Value.Trim();
                        var text = StripToPlainText(aMatches[0].Groups[2].Value) ?? href;

                        if (IsYouTube(href))
                            blocks.Add(NewBlock<TBloco>(++ordem, "embed_youtube", new { url = href }));
                        else
                            blocks.Add(NewBlock<TBloco>(++ordem, "link", new { text, href }));

                        continue;
                    }

                    var plain = StripToPlainText(trimmed);
                    if (!string.IsNullOrWhiteSpace(plain))
                        blocks.Add(NewBlock<TBloco>(++ordem, "paragraph", new { text = plain }));
                }
            }

            ReorderBlocks(blocks);
            return blocks;
        }

        private static void ReorderBlocks<TBloco>(List<TBloco> blocks) where TBloco : class
        {
            var prop = typeof(TBloco).GetProperty("Ordem");
            if (prop == null) return;
            for (int i = 0; i < blocks.Count; i++)
                prop.SetValue(blocks[i], i + 1);
        }

        private static TBloco NewBlock<TBloco>(int ordem, string tipo, object valor, object? config = null)
            where TBloco : class, new()
        {
            if (typeof(TBloco) == typeof(NoticiaOldMappedResponse.BlocoResponse))
            {
                var b = new NoticiaOldMappedResponse.BlocoResponse
                {
                    Ordem = ordem,
                    Tipo = tipo,
                    Valor = JsonSerializer.SerializeToElement(valor),
                    Config = config != null ? JsonSerializer.SerializeToElement(config) : null
                };
                return (TBloco)(object)b;
            }

            var inst = new TBloco();
            var t = typeof(TBloco);
            t.GetProperty("Ordem")?.SetValue(inst, ordem);
            t.GetProperty("Tipo")?.SetValue(inst, tipo);
            t.GetProperty("Valor")?.SetValue(inst, JsonSerializer.SerializeToElement(valor));
            if (config != null)
                t.GetProperty("Config")?.SetValue(inst, JsonSerializer.SerializeToElement(config));
            return inst;
        }

        private static bool IsYouTube(string url)
            => !string.IsNullOrWhiteSpace(url) &&
               (url.Contains("youtube.com/watch", StringComparison.OrdinalIgnoreCase) ||
                url.Contains("youtu.be/", StringComparison.OrdinalIgnoreCase));

        private static bool IsMostlyLinks(string htmlFrag)
        {
            var plain = StripToPlainText(htmlFrag) ?? "";
            var linksText = string.Join(" ", Regex.Matches(htmlFrag, @"<a[^>]*>(.*?)</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase)
                                                .Select(m => StripToPlainText(m.Groups[1].Value) ?? ""));
            return !string.IsNullOrWhiteSpace(linksText) && linksText.Length > plain.Length * 0.6;
        }

        private static bool IsMostlySingleLink(string htmlFrag)
        {
            var plain = StripToPlainText(htmlFrag) ?? "";
            var innerLink = Regex.Match(htmlFrag, @"<a[^>]*>(.*?)</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var linkText = innerLink.Success ? (StripToPlainText(innerLink.Groups[1].Value) ?? "") : "";
            return plain.Length <= linkText.Length * 1.6 + 50;
        }
    }
}
