using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EnhancedEbookWebApp.Data;
using EnhancedEbookWebApp.Models;
using Microsoft.Extensions.Logging;

namespace EnhancedEbookWebApp.Pages
{
    public class DeleteDbFileModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteDbFileModel> logger;

        public DeleteDbFileModel(AppDbContext context, ILogger<DeleteDbFileModel> logger)
        {
            _context = context;
            this.logger = logger;
        }

        [BindProperty]
        public Ebook RemoveFile { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            RemoveFile = await _context.Ebook.SingleOrDefaultAsync(m => m.ID == id);

            if (RemoveFile == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            RemoveFile = await _context.Ebook.FindAsync(id);

            if (RemoveFile != null)
            {
                _context.Ebook.Remove(RemoveFile);
                await _context.SaveChangesAsync();
            }

            //logging
            string clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            logger.LogInformation($"An ebook (id= {RemoveFile.ID}, title= \"{RemoveFile.Title}\", author= {RemoveFile.Author}, filename= {RemoveFile.UntrustedName})  was removed from the database by a client with the following ip address: {clientIP}");

            return RedirectToPage("/Index");
        }
    }
}
