using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaReal5.Models;

namespace HotelEstrellaReal5.Controllers
{
    public class ComodidadesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public ComodidadesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Comodidades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comodidades.ToListAsync());
        }

        // GET: Comodidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comodidade = await _context.Comodidades
                .FirstOrDefaultAsync(m => m.IdComodidad == id);
            if (comodidade == null)
            {
                return NotFound();
            }

            return View(comodidade);
        }

        // GET: Comodidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comodidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComodidad,Nombre,Descripcion")] Comodidade comodidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comodidade);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Comodidad Creada Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            return View(comodidade);
        }

        // GET: Comodidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comodidade = await _context.Comodidades.FindAsync(id);
            if (comodidade == null)
            {
                return NotFound();
            }
            return View(comodidade);
        }

        // POST: Comodidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComodidad,Nombre,Descripcion")] Comodidade comodidade)
        {
            if (id != comodidade.IdComodidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comodidade);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComodidadeExists(comodidade.IdComodidad))
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
            return View(comodidade);
        }

        // GET: Comodidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comodidade = await _context.Comodidades
                .FirstOrDefaultAsync(m => m.IdComodidad == id);
            if (comodidade == null)
            {
                return NotFound();
            }

            return View(comodidade);
        }

        // POST: Comodidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comodidade = await _context.Comodidades.FindAsync(id);
            if (comodidade != null)
            {
                _context.Comodidades.Remove(comodidade);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Servicio Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool ComodidadeExists(int id)
        {
            return _context.Comodidades.Any(e => e.IdComodidad == id);
        }
    }
}
