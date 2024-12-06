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
    public class EstadoHabitacionesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public EstadoHabitacionesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: EstadoHabitaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadoHabitaciones.ToListAsync());
        }

        // GET: EstadoHabitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoHabitacione = await _context.EstadoHabitaciones
                .FirstOrDefaultAsync(m => m.IdEstadoHabitacion == id);
            if (estadoHabitacione == null)
            {
                return NotFound();
            }

            return View(estadoHabitacione);
        }

        // GET: EstadoHabitaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoHabitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadoHabitacion,NombreEstadoHabitacion,Descripcion")] EstadoHabitacione estadoHabitacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoHabitacione);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! EstadoHabitación Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            return View(estadoHabitacione);
        }

        // GET: EstadoHabitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoHabitacione = await _context.EstadoHabitaciones.FindAsync(id);
            if (estadoHabitacione == null)
            {
                return NotFound();
            }
            return View(estadoHabitacione);
        }

        // POST: EstadoHabitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadoHabitacion,NombreEstadoHabitacion,Descripcion")] EstadoHabitacione estadoHabitacione)
        {
            if (id != estadoHabitacione.IdEstadoHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoHabitacione);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoHabitacioneExists(estadoHabitacione.IdEstadoHabitacion))
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
            return View(estadoHabitacione);
        }

        // GET: EstadoHabitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoHabitacione = await _context.EstadoHabitaciones
                .FirstOrDefaultAsync(m => m.IdEstadoHabitacion == id);
            if (estadoHabitacione == null)
            {
                return NotFound();
            }

            return View(estadoHabitacione);
        }

        // POST: EstadoHabitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoHabitacione = await _context.EstadoHabitaciones.FindAsync(id);
            if (estadoHabitacione != null)
            {
                _context.EstadoHabitaciones.Remove(estadoHabitacione);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! EstadoHabitación Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoHabitacioneExists(int id)
        {
            return _context.EstadoHabitaciones.Any(e => e.IdEstadoHabitacion == id);
        }
    }
}
