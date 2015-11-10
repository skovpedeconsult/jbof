using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using System.Net.Http;
using AngleSharp.Dom.Html;
using System.IO;
using System.Text.RegularExpressions;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }
        static  async Task MainAsync(string[] args)
        {
            var targetUri = new Uri(args[0]);
            string targetHost = targetUri.Host;
            Queue<Uri> pending = new Queue<Uri>();
            ISet<Uri> completed = new HashSet<Uri>();
            pending.Enqueue(targetUri);
            var parser = new HtmlParser();
            
            while(pending.Any()) {
                Uri current = pending.Dequeue();
                Console.WriteLine(current + " " + pending.Count);
                completed.Add(current);
                using (var client = new HttpClient { BaseAddress = targetUri })
                {
                    var response = await client.GetAsync(current);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(current + " " + response.StatusCode);
                        continue;
                    }
                    string content = await response.Content.ReadAsStringAsync();
                    IHtmlDocument parsedHtml = await parser.ParseAsync(content);
                    List<Uri> todo = parsedHtml.Links
                        .Select(s => new Uri(current,s.Attributes["href"].Value))
                        .Where(s => s.Host == targetHost)
                        .Except(completed)
                        .ToList();

                    var rewrites = parsedHtml.Links
                        .Select(s => new { s, u = new Uri(current, s.Attributes["href"].Value) })
                        .Where(s => s.u.Host == targetHost)
                        .Where(s => !s.s.Attributes["href"].Value.StartsWith("#"))
                        .Select(s => new { s.s, s.u, f = ToLink(s.u) }).ToList();

                    foreach(var a in rewrites) a.s.Attributes["href"].Value = a.f;

                    string targetFile = "result" + ToFileName(current.AbsolutePath, current.Query);
                    Directory.CreateDirectory(Directory.GetParent(targetFile).FullName);
                    File.WriteAllText(targetFile, parsedHtml.ToHtml());
                    Console.WriteLine(targetFile);
                    foreach (var a in todo) pending.Enqueue(a);

                }
            }
        }

        static Regex fileExtension = new Regex(@"\.(.+)$");

        static string ToLink(Uri uri)
        {
            return ToFileName(uri.AbsolutePath, uri.Query).Replace("index.html", "") + uri.Fragment;
        }

        static string ToFileName(string path, string query)
        {
            if (path.EndsWith("/")) path = path + "index.html";

            var split = path.Split('/');
            var filename = split.Last();
            if (query.Any())
            {
                string noExtension = fileExtension.Replace(path, "");
                string extension = fileExtension.Match(path).Groups[1].Value;
                query = query.Replace('=', '-');
                path = noExtension + "-" + query.Substring("?".Length) + "." + extension;
            }
            return path;
        }

    }
}
