using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DistanceLessons.Models;
using System.IO;

namespace DistanceLessons.Controllers
{
    public class FileDescription
    {
        public string Name { get; set; }
        public string WebPath { get; set; }
        public long Size { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class FileController : Controller
    {
        //
        // GET: /Upload/
        FileRepository fileRepository = new FileRepository();

        public ActionResult Index()
        {
            return View(fileRepository.GetAllFileDescription());
        }


        public void Upload()
        {
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];
                if (file.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads")
                        , Path.GetFileName(file.FileName));
                    file.SaveAs(filePath);
                }
            }

            RedirectToAction("Index");
        }

    }
}
