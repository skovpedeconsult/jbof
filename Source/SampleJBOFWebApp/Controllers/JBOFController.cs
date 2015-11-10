using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleJBOFWebApp.Controllers
{
    [Authorize]
    public class JBOFController : Controller
    {
        HtmlParser _parser = new HtmlParser();
        static Regex fileExtension = new Regex(@"\.(.+)$");

        public async Task<ActionResult> Index(string url = "")
        {
            if (!fileExtension.IsMatch(url)) url = url + "index.html";
            string path = HostingEnvironment.MapPath("~/App_Data/html/" + url);
            try
            {
                IHtmlDocument parsed;
                using (var fs = System.IO.File.OpenRead(path)) parsed = await _parser.ParseAsync(fs);
                IElement siteTitle = parsed.QuerySelector(".site-title");
                siteTitle.FirstChild.TextContent += " " + ((ClaimsIdentity)User.Identity).Name;
                return Content(parsed.ToHtml(), "text/html");
            }
            catch (IOException)
            {
                throw new HttpException(404, "Not found");
            }
        }

        public Task<ActionResult> Premium(string url = "")
        {
            //Do stuff for the premium area here.
            return null;
        }

    }
}