using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AskUs.Data;
using AskUs.Models;
using Microsoft.AspNetCore.Authorization;

namespace AskUs.Controllers
{
    public class InquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inquiries
        public async Task<IActionResult> Index()
        {
              return _context.Inquiry != null ? 
                          View(await _context.Inquiry.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Inquiry'  is null.");
        }

        // GET: Inquiries/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            //optional param with view name. not included since same as controller method name.
            return View();
        }

        // POST: Inquiries/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Inquiry.Where( i => i.Question.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Inquiries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inquiry == null)
            {
                return NotFound();
            }

            var inquiry = await _context.Inquiry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inquiry == null)
            {
                return NotFound();
            }

            return View(inquiry);
        }

        // GET: Inquiries/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquiries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inquiry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inquiry);
        }

        // GET: Inquiries/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inquiry == null)
            {
                return NotFound();
            }

            var inquiry = await _context.Inquiry.FindAsync(id);
            if (inquiry == null)
            {
                return NotFound();
            }
            return View(inquiry);
        }

        // POST: Inquiries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Inquiry inquiry)
        {
            if (id != inquiry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inquiry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InquiryExists(inquiry.Id))
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
            return View(inquiry);
        }

        // GET: Inquiries/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inquiry == null)
            {
                return NotFound();
            }

            var inquiry = await _context.Inquiry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inquiry == null)
            {
                return NotFound();
            }

            return View(inquiry);
        }

        // POST: Inquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inquiry == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inquiry'  is null.");
            }
            var inquiry = await _context.Inquiry.FindAsync(id);
            if (inquiry != null)
            {
                _context.Inquiry.Remove(inquiry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InquiryExists(int id)
        {
          return (_context.Inquiry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
