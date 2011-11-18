using System.Collections.Generic;
using System.Web;
using System.IO;
using DistanceLessons.Controllers;

namespace DistanceLessons.Models
{

    public class FileRepository
    {
        public List<FileDescription> GetAllFileDescription()
        {
            string uploadFolder = HttpContext.Current.Server.MapPath("../Uploads");
            string[] files = Directory.GetFiles(uploadFolder);
            List<FileDescription> fileDescriptions = new List<FileDescription>();

            foreach (string file in files)
            {
                FileInfo fileinfo = new FileInfo(file);
                fileDescriptions.Add(
                    new FileDescription
                    {
                        Name = fileinfo.Name,
                        Size = fileinfo.Length / 1024,
                        WebPath = fileinfo.Name,
                        DateCreated = fileinfo.CreationTime
                    });
            }

            return fileDescriptions;
        }

        public FileDescription GetFileByName(string filename)
        {
            string uploadFolder = HttpContext.Current.Server.MapPath("../Uploads");
            string[] files = Directory.GetFiles(uploadFolder);
            FileDescription fileDescriptions = new FileDescription();

            foreach (string file in files)
            {
                FileInfo fileinfo = new FileInfo(file);
                if (fileinfo.Name==filename){
                fileDescriptions.Name = fileinfo.Name;
                fileDescriptions.Size = fileinfo.Length / 1024;
                fileDescriptions.WebPath = fileinfo.Name;
                fileDescriptions.DateCreated = fileinfo.CreationTime;
                break;    
                }
            }

            return fileDescriptions;
        }
    }
}