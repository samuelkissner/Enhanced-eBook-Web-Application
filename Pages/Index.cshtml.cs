using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
      
        public IndexModel(AppDbContext context)
        {
            _context = context;
           
        }

        public IList<Ebook> DatabaseFiles { get; private set; }
        

        public async Task OnGetAsync()
        {
            DatabaseFiles = await _context.Ebook.AsNoTracking().ToListAsync();
            
        }

        public async Task<IActionResult> OnGetDownloadDbAsync(int? id)
        {
            if (id == null)
            {
                return Page();
            }

            var requestFile = await _context.Ebook.SingleOrDefaultAsync(m => m.ID == id);

            if (requestFile == null)
            {
                return Page();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(requestFile.Content, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(requestFile.UntrustedName));
        }

        
    }
}
