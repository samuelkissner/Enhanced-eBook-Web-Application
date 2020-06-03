using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EnhancedEbookWebApp.Data;
using EnhancedEbookWebApp.Models;
using EnhancedEbookWebApp.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace EnhancedEbookWebApp.Pages
{
    [Authorize]
    public class BufferedSingleFileUploadDbModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BufferedSingleFileUploadDbModel> logger;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".epub" };

        public BufferedSingleFileUploadDbModel(AppDbContext context, 
            IConfiguration config, ILogger<BufferedSingleFileUploadDbModel> logger)
        {
            _context = context;
            this.logger = logger;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

        public string Result { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            // Perform an initial check to catch FileUpload class
            // attribute violations.
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }

            var formFileContent = 
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadDb>(
                    FileUpload.FormFile, ModelState, _permittedExtensions, 
                    _fileSizeLimit);

            // Perform a second check to catch ProcessFormFile method
            // violations. If any validation check fails, return to the
            // page.
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form.";

                return Page();
            }


            var file = new Ebook
            {
                Content = formFileContent,
                UntrustedName = FileUpload.FormFile.FileName,
                Title = FileUpload.Title,
                Author = FileUpload.Author,

                Size = FileUpload.FormFile.Length,
                UploadDT = DateTime.UtcNow
            };

            _context.Ebook.Add(file);
            await _context.SaveChangesAsync();

            //logging
            string clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            logger.LogInformation($"An ebook (id= {file.ID}, title= \"{file.Title}\", author= {file.Author}, filename= {file.UntrustedName})  was added to the database by a client with the following ip address: {clientIP}");


            return RedirectToPage("./Index");
        }
    }

    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(50, MinimumLength = 0)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author")]
        [StringLength(50, MinimumLength = 0)]
        public string Author { get; set; }
    }
}
