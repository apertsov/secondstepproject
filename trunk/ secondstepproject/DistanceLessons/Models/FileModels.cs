using System;
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