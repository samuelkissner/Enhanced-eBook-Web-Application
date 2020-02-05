using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class DeleteDbFileModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteDbFileModel(AppDbContext context)
        {
            _context = context;
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

            return RedirectToPage("/Index");
        }
    }
}
