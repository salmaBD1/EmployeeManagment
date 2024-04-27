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
    public class BookingsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Salle);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Salle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            // Only fetch salles that have a status of "free"
            var availableSalles = _context.Salles.Where(s => s.Status == "free").ToList();

            ViewData["SalleId"] = new SelectList(availableSalles, "Id", "Numero");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroMembre,CustomerName,BookingFrom,BookingTo,SalleId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if the selected salle is available
                var selectedSalle = _context.Salles.FirstOrDefault(s => s.Id == booking.SalleId && s.Status == "free");

                if (selectedSalle != null)
                {
                    // Check if the number of members is less than or equal to the salle capacity
                    if (booking.NumeroMembre <= selectedSalle.Capacite)
                    {
                        // Update salle status to "not free"
                        selectedSalle.Status = "not free";

                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("NumeroMembre", "Number of members exceeds salle capacity.");
                    }
                }
                else
                {
                    ModelState.AddModelError("SalleId", "Selected salle is not available.");
                }
            }

            // If the model state is not valid, re-populate the salle dropdown
            var availableSalles = _context.Salles.Where(s => s.Status == "free").ToList();
            ViewData["SalleId"] = new SelectList(availableSalles, "Id", "Numero", booking.SalleId);
            return View(booking);
        }
        [Authorize(Roles = "admin")]
        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["SalleId"] = new SelectList(_context.Salles, "Id", "Id", booking.SalleId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroMembre,CustomerName,BookingFrom,BookingTo,SalleId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["SalleId"] = new SelectList(_context.Salles, "Id", "Id", booking.SalleId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Salle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
