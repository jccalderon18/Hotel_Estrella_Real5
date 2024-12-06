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
    public class HabitacionesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public HabitacionesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Habitaciones
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal1Context = _context.Habitaciones.Include(h => h.IdCategoriaNavigation).Include(h => h.IdEstadoHabitacionNavigation);
            return View(await hotelEstrellaReal1Context.ToListAsync());
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdCategoriaNavigation)
                .Include(h => h.IdEstadoHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }

            return View(habitacione);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            ViewData["IdEstadoHabitacion"] = new SelectList(_context.EstadoHabitaciones, "IdEstadoHabitacion", "NombreEstadoHabitacion");
            return View();
        }

        // POST: Habitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHabitacion,Nombre,Precio,Descripcion,IdCategoria,IdEstadoHabitacion")] Habitacione habitacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacione);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Habitación Creada Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", habitacione.IdCategoria);
            ViewData["IdEstadoHabitacion"] = new SelectList(_context.EstadoHabitaciones, "IdEstadoHabitacion", "NombreEstadoHabitacion", habitacione.IdEstadoHabitacion);
            return View(habitacione);
        }

        // GET: Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones.FindAsync(id);
            if (habitacione == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", habitacione.IdCategoria);
            ViewData["IdEstadoHabitacion"] = new SelectList(_context.EstadoHabitaciones, "IdEstadoHabitacion", "NombreEstadoHabitacion", habitacione.IdEstadoHabitacion);
            return View(habitacione);
        }

        // POST: Habitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHabitacion,Nombre,Precio,Descripcion,IdCategoria,IdEstadoHabitacion")] Habitacione habitacione)
        {
            if (id != habitacione.IdHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacione);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacioneExists(habitacione.IdHabitacion))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", habitacione.IdCategoria);
            ViewData["IdEstadoHabitacion"] = new SelectList(_context.EstadoHabitaciones, "IdEstadoHabitacion", "NombreEstadoHabitacion", habitacione.IdEstadoHabitacion);
            return View(habitacione);
        }

        // GET: Habitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdCategoriaNavigation)
                .Include(h => h.IdEstadoHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }

            return View(habitacione);
        }

        // POST: Habitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacione = await _context.Habitaciones.FindAsync(id);
            if (habitacione != null)
            {
                _context.Habitaciones.Remove(habitacione);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Habitación Eliminada Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacioneExists(int id)
        {
            return _context.Habitaciones.Any(e => e.IdHabitacion == id);
        }
    }
}
