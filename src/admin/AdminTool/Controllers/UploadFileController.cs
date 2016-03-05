using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AdminTool.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadFile()
        {
            ViewData["Success"] = "";
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(string Title)
        {
            string extension = string.Empty;
            string path = string.Empty;
            if (Request.Files["file"].ContentLength > 0)
            {
                try
                {
                    extension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                    if (extension.ToUpper().Equals(".STL") || extension.ToUpper().Equals(".X3D"))
                    {
                        //  path = string.Format("{0}/{1}{2}", Server.MapPath("~/documents/Files"), "test", extension);
                        path = Path.Combine(Server.MapPath("~/documents/Files"), Path.GetFileName(Request.Files["file"].FileName));
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);

                        }
                        Request.Files["file"].SaveAs(path);
                        ViewData["Success"] = "Success";

                    }
                    else
                    {
                        ViewData["Success"] = "Please upload X3D format.";
                    }
                }
                catch (Exception)
                {
                    ViewData["Success"] = "Upload Failed";
                }

            }
            GenerateHtmlFile(Path.GetFileName(Request.Files["file"].FileName), Title);
            return View();
        }
        private  void GenerateHtmlFile(string filePath,string Title)
        {
            string ApplicationDate = DateTime.Now.ToString("dd_MMM_yyyy");
            StringBuilder sb = new StringBuilder();
            // sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<html");
            sb.AppendLine("<meta http-equiv='X - UA - Compatible' content='IE = edge'/>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title></title>");
            sb.AppendLine("<script type='text/javascript' src='http://www.x3dom.org/download/x3dom.js'>");
            sb.AppendLine("</script>");
            sb.AppendLine("<link rel='stylesheet' type='text/css' href='http://www.x3dom.org/download/x3dom.css'/>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<form");
            sb.AppendLine("<div>");
            sb.AppendLine("<x3d width='500px' height='400px'>");
            sb.AppendLine("<scene>");
            sb.AppendLine("<inline url='" + filePath + "'/>");
            sb.AppendLine("</scene>");
            sb.AppendLine("</x3d>");
            sb.AppendLine("</div>");
            sb.AppendLine("</form>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            String filename = String.Concat(ApplicationDate, ".html");
            string path = Server.MapPath("~/documents/Files");
            path = path + "\\" + filename;
            // System.IO.File.WriteAllText(path + @"\" + filename, sb.ToString());
            System.IO.File.WriteAllText(path, sb.ToString());


            // Consuming web api 
            using (var client = new HttpClient())
            {
                string name = string.Empty;
                string url = string.Empty;
                if(string.IsNullOrEmpty(Title.Trim()))
                {
                    name = filePath;
                }
                else
                {
                    name = Title;
                }
                
                url = "/documents/Files/" + filename; 
                // New code:
                // string uri = "http://10.64.98.69:9810/api/values?name=xyz&url=yahoo.com";
                client.BaseAddress = new Uri("http://10.64.98.69:9810");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               // client.GetAsync(uri);
                HttpResponseMessage response = client.GetAsync("/api/values?name="+ name + "&url="+ url + "").Result;
                if (response.IsSuccessStatusCode)
                {
                    ViewData["Success"] = "Data saved sucessfully.";
                }

            }

        }

    }
}