using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;

namespace DotNetCoreSqlDb.Controllers
{
    public class EbooksController : Controller
    {
        private readonly MyDatabaseContext _context;

        public EbooksController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: Ebooks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ebook.ToListAsync());
        }

        // GET: Ebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebook
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // GET: Ebook/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ebooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Author,Size,File")] Ebook ebook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ebook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ebook);
        }

        // GET: Ebooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebook.FindAsync(id);
            if (ebook == null)
            {
                return NotFound();
            }
            return View(ebook);
        }

        // POST: Ebooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Author,Size,File")] Ebook ebook)
        {
            if (id != ebook.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EbookExists(ebook.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ebook);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Ebook
                .FirstOrDefaultAsync(m => m.ID == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Ebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ebook = await _context.Ebook.FindAsync(id);
            _context.Ebook.Remove(ebook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EbookExists(int id)
        {
            return _context.Ebook.Any(e => e.ID == id);
        }
    }
}
