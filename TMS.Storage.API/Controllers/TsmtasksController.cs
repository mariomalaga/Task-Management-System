using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    public class TsmtasksController : Controller
    {
        private readonly TSMTasksContext _context;

        public TsmtasksController(TSMTasksContext context)
        {
            _context = context;
        }

        // GET: Tsmtasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tsmtask.ToListAsync());
        }

        // GET: Tsmtasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tsmtask = await _context.Tsmtask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tsmtask == null)
            {
                return NotFound();
            }

            return View(tsmtask);
        }

        // GET: Tsmtasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tsmtasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,FinishDate,State")] Tsmtask tsmtask)
        {
            if (ModelState.IsValid)
            {
                tsmtask.Id = Guid.NewGuid();
                _context.Add(tsmtask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tsmtask);
        }

        // GET: Tsmtasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tsmtask = await _context.Tsmtask.FindAsync(id);
            if (tsmtask == null)
            {
                return NotFound();
            }
            return View(tsmtask);
        }

        // POST: Tsmtasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,StartDate,FinishDate,State")] Tsmtask tsmtask)
        {
            if (id != tsmtask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tsmtask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TsmtaskExists(tsmtask.Id))
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
            return View(tsmtask);
        }

        // GET: Tsmtasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tsmtask = await _context.Tsmtask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tsmtask == null)
            {
                return NotFound();
            }

            return View(tsmtask);
        }

        // POST: Tsmtasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tsmtask = await _context.Tsmtask.FindAsync(id);
            _context.Tsmtask.Remove(tsmtask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TsmtaskExists(Guid id)
        {
            return _context.Tsmtask.Any(e => e.Id == id);
        }
    }
}
