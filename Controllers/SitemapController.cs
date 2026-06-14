using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using GreenOasisResort.Models;

namespace GreenOasisResort.Controllers;

public class SitemapController : Controller
{
    private readonly SiteSettings _settings;

    public SitemapController(IOptions<SiteSettings> settings) => _settings = settings.Value;

    [Route("sitemap.xml")]
    [ResponseCache(Duration = 3600)]
    public ContentResult Index()
    {
        var baseUrl = _settings.SiteUrl.TrimEnd('/');
        var lastMod = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var pages = new[]
        {
            new SitemapEntry($"{baseUrl}/", "1.0", "weekly"),
            new SitemapEntry($"{baseUrl}/Home/About", "0.8", "monthly"),
            new SitemapEntry($"{baseUrl}/Home/Contact", "0.8", "monthly"),
            new SitemapEntry($"{baseUrl}/Home/Privacy", "0.5", "yearly"),
            new SitemapEntry($"{baseUrl}/Home/Terms", "0.5", "yearly")
        };
        var xml = new StringBuilder();
        xml.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        xml.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");
        foreach (var page in pages)
        {
            xml.AppendLine("  <url>");
            xml.AppendLine($"    <loc>{XmlEscape(page.Location)}</loc>");
            xml.AppendLine($"    <lastmod>{lastMod}</lastmod>");
            xml.AppendLine($"    <changefreq>{page.ChangeFreq}</changefreq>");
            xml.AppendLine($"    <priority>{page.Priority}</priority>");
            xml.AppendLine("  </url>");
        }
        xml.AppendLine("</urlset>");
        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    private static string XmlEscape(string value) =>
        System.Security.SecurityElement.Escape(value) ?? value;

    private sealed record SitemapEntry(string Location, string Priority, string ChangeFreq);
}
