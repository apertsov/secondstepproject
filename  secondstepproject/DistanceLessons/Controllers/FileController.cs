using System;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using System.IO;

namespace DistanceLessons.Controllers
{
    

    [Authorize(Roles = "Teacher")]
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
