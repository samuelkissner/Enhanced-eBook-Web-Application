using System;
using System.ComponentModel.DataAnnotations;



namespace DotNetCoreSqlDb.Models
{
    public class Ebook
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public byte[] Content { get; set; }

        [Display(Name = "File Name")]
        public string UntrustedName { get; set; }


        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }

    }
}



