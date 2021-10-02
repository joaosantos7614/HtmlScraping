using FaroTemperature.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using ScrapySharp.Network;
using System.Text.RegularExpressions;

namespace FaroTemperature.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        static ScrapingBrowser _browser = new ScrapingBrowser();
        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
        public IActionResult Index()
        {
            TemperatureModel temperature = new();
            //string urlToScrape = "http://centronautico.cm-faro.pt/";
            string urlToScrape = "https://www.aviationweather.gov/metar/data?ids=lppt&format=raw&hours=0&taf=off&layout=off";
            HtmlNode scrapedHtml = GetHtml(urlToScrape);
            string innerHtml = scrapedHtml.InnerHtml;
            //Match match = Regex.Match(innerHtml, "<td class=\"category\">Air<\\/td>\\s*<td class=\"ws[0-9]*\">([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match match = Regex.Match(innerHtml, @"<code>LPPT\s\S*\s\S*\s\S*\s\S*\s(\S*)\/", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            temperature.TempString = match.Groups[1].ToString();
            temperature.ScrapingDate = DateTime.Now;
            return View(temperature);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
