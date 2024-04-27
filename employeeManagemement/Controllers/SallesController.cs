using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using employeeManagemement.Data;
using employeeManagemement.Models;
using Microsoft.AspNetCore.Authorization;


namespace employeeManagemement.Controllers
{
    [Authorize(Roles = "admin")]
    public class SallesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SallesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Salles
        public async Task<IActionResult> Index()
        {
              return _context.Salles != null ? 
                          View(await _context.Salles.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Salles'  is null.");
        }

        // GET: Salles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Salles == null)
            {
                return NotFound();
            }

            var salle = await _context.Salles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salle == null)
            {
                return NotFound();
            }

            return View(salle);
        }

        // GET: Salles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Status,Capacite")] Salle salle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salle);
        }

        // GET: Salles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Salles == null)
            {
                return NotFound();
            }

            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound();
            }
            return View(salle);
        }

        // POST: Salles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,Status,Capacite")] Salle salle)
        {
            if (id != salle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalleExists(salle.Id))
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
            return View(salle);
        }

        // GET: Salles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Salles == null)
            {
                return NotFound();
            }

            var salle = await _context.Salles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salle == null)
            {
                return NotFound();
            }

            return View(salle);
        }

        // POST: Salles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Salles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Salles'  is null.");
            }
            var salle = await _context.Salles.FindAsync(id);
            if (salle != null)
            {
                _context.Salles.Remove(salle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalleExists(int id)
        {
          return (_context.Salles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
