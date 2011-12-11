using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistanceLessons.Attributes;

namespace DistanceLessons.Models
{
    [Localization]
    public class FileDescription
    {
        public string Name { get; set; }
        public string WebPath { get; set; }
        public long Size { get; set; }
        public DateTime DateCreated { get; set; }
    }
}