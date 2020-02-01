using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreSqlDb.Models
{
    public class Ebook
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        //indicates storage size (in MB) for ebook
        public double Size { get; set; }

        public byte[] EbookContents { get; set; }
    }
}

