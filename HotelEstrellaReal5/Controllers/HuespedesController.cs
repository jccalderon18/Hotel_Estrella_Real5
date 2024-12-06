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
    public class HuespedesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public HuespedesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Huespedes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Huespedes.ToListAsync());
        }

        // GET: Huespedes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var huespede = await _context.Huespedes
                .FirstOrDefaultAsync(m => m.IdHuesped == id);
            if (huespede == null)
            {
                return NotFound();
            }

            return View(huespede);
        }

        // GET: Huespedes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Huespedes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHuesped,Identificacion,NombreCompleto,Email,Telefono,FechaRegistro")] Huespede huespede)
        {
            if (ModelState.IsValid)
            {
                _context.Add(huespede);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Huesped Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            return View(huespede);
        }

        // GET: Huespedes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var huespede = await _context.Huespedes.FindAsync(id);
            if (huespede == null)
            {
                return NotFound();
            }
            return View(huespede);
        }

        // POST: Huespedes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHuesped,Identificacion,NombreCompleto,Email,Telefono,FechaRegistro")] Huespede huespede)
        {
            if (id != huespede.IdHuesped)
            {
                return NotFound();
            }

            // Buscar el huésped original para obtener su FechaRegistro
            var huespedeOriginal = await _context.Huespedes.AsNoTracking().FirstOrDefaultAsync(h => h.IdHuesped == id);

            if (huespedeOriginal == null)
            {
                return NotFound();
            }

            // Preservar la fecha de registro original
            huespede.FechaRegistro = huespedeOriginal.FechaRegistro;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(huespede);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HuespedeExists(huespede.IdHuesped))
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
            return View(huespede);
        }

        // GET: Huespedes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var huespede = await _context.Huespedes
                .FirstOrDefaultAsync(m => m.IdHuesped == id);
            if (huespede == null)
            {
                return NotFound();
            }

            return View(huespede);
        }

        // POST: Huespedes/Delete/5
        // POST: Huespedes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. Primero, eliminamos las relaciones en la tabla intermedia `Detalles_Reserva_Huesped`
            var detallesReservaHuesped = _context.DetallesReservaHuespeds
                .Where(drh => drh.IdHuesped == id)
                .ToList();

            // Eliminar las filas correspondientes en la tabla intermedia
            _context.DetallesReservaHuespeds.RemoveRange(detallesReservaHuesped);

            // 2. Luego, eliminamos el huésped de la tabla `Huespedes`
            var huespede = await _context.Huespedes.FindAsync(id);
            if (huespede != null)
            {
                _context.Huespedes.Remove(huespede);
            }

            // Guardamos todos los cambios realizados en la base de datos
            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Huesped Eliminado Exitosamente !";
            // Redirigimos al índice de la lista de huéspedes
            return RedirectToAction(nameof(Index));
        }

        private bool HuespedeExists(int id)
        {
            return _context.Huespedes.Any(e => e.IdHuesped == id);
        }
    }
}
