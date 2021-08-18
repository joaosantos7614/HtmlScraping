using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using ScrapySharp.Network;
using System.Text.RegularExpressions;

namespace Faro_Weather.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public string TempString { get; set; }
        public void OnGet()
        {
            
            string urlToScrape = "http://centronautico.cm-faro.pt/";
            HtmlNode scrapedHtml = GetHtml(urlToScrape);
            string innerHtml = scrapedHtml.InnerHtml;
            Match match = Regex.Match(innerHtml, "<td class=\"category\">Air<\\/td>\\s*<td class=\"ws[0-9]*\">([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            TempString = match.Groups[1].ToString();
            //if (match.Success && match.Groups.Count >= 1)
            //{
            //    Group temperature = match.Groups[1];
            //    TempString = Convert.ToString(temperature);
            //}
        }
        static ScrapingBrowser _browser = new ScrapingBrowser();
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}

/*
namespace Scraping_Tutorial
{
    class Program
    {
        static ScrapingBrowser _browser = new ScrapingBrowser();
        static void Main(string[] args)
        {
            string urlToScrape = "http://centronautico.cm-faro.pt/";
            HtmlNode scrapedHtml = GetHtml(urlToScrape);
            string innerHtml = scrapedHtml.InnerHtml;
            Match matches = Regex.Match(innerHtml, "<td class=\"category\">Air<\\/td>\\s*<td class=\"ws[0-9]*\">([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (matches.Success && matches.Groups.Count >= 1)
            {
                Group temperature = matches.Groups[1];
                Console.WriteLine($"A temperatura actual em Faro é: {temperature} C");
            }
        }
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }

    }
}
*/