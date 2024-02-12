using KlioBlazor.Shared;
using KlioWeb.Pages.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KlioWeb.Pages
{
    public class LiveModel : PageModel
    {
        public AppState AppState = new AppState();
        public BreadcrumbArea BreadcrumbArea;
        public TV allProgrames;
        public string jsonString;

        public async Task OnGetAsync()
        {
            BreadcrumbArea = new BreadcrumbArea()
            {
                TitleContent = "<h2 class=\"title\">Пряма <span>трансляція</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Наживо</li>"
            };

            WebClient Client = new WebClient();
            using (MemoryStream stream = new MemoryStream(Client.DownloadData(AppState.KlioStreamEPG)))
            {
                stream.Seek(0, SeekOrigin.Begin);

                using (var gzStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        gzStream.CopyTo(outputStream);
                        byte[] outputBytes = outputStream.ToArray();
                        string FileContents = Encoding.UTF8.GetString(outputBytes);
                        string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                        FileContents = FileContents.Remove(0, _byteOrderMarkUtf8.Length);

                        try
                        {
                            var doc = XDocument.Parse(FileContents);
                            XmlSerializer serializer = new XmlSerializer(typeof(TV));
                            allProgrames = (TV)serializer.Deserialize(doc.CreateReader());

                            var options1 = new JsonSerializerOptions
                            {
                                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                                WriteIndented = true
                            };
                            jsonString = JsonSerializer.Serialize(allProgrames.Programmes, options1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
