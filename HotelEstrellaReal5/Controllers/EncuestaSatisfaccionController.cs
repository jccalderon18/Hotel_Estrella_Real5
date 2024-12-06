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
    public class EncuestaSatisfaccionController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public EncuestaSatisfaccionController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: EncuestaSatisfaccion
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal3Context = _context.EncuestaSatisfaccions.Include(e => e.IdReservaNavigation);
            return View(await hotelEstrellaReal3Context.ToListAsync());
        }

        // GET: EncuestaSatisfaccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encuestaSatisfaccion = await _context.EncuestaSatisfaccions
                .Include(e => e.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdEncuesta == id);
            if (encuestaSatisfaccion == null)
            {
                return NotFound();
            }

            return View(encuestaSatisfaccion);
        }

        // GET: EncuestaSatisfaccion/Create
        public IActionResult Create()
        {
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva");
            return View();
        }

        // POST: EncuestaSatisfaccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEncuesta,IdReserva,CalificacionLimpieza,CalificacionServicios,CalificacionComida,CalificacionHabitacion,CalificacionAtencion,CalificacionInstalaciones,FechaRegistro")] EncuestaSatisfaccion encuestaSatisfaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encuestaSatisfaccion);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Encuesta Creada Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", encuestaSatisfaccion.IdReserva);
            return View(encuestaSatisfaccion);
        }

        // GET: EncuestaSatisfaccion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encuestaSatisfaccion = await _context.EncuestaSatisfaccions.FindAsync(id);
            if (encuestaSatisfaccion == null)
            {
                return NotFound();
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", encuestaSatisfaccion.IdReserva);
            return View(encuestaSatisfaccion);
        }

        // POST: EncuestaSatisfaccion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEncuesta,IdReserva,CalificacionLimpieza,CalificacionServicios,CalificacionComida,CalificacionHabitacion,CalificacionAtencion,CalificacionInstalaciones,FechaRegistro")] EncuestaSatisfaccion encuestaSatisfaccion)
        {
            if (id != encuestaSatisfaccion.IdEncuesta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encuestaSatisfaccion);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Encuesta Actualizada Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncuestaSatisfaccionExists(encuestaSatisfaccion.IdEncuesta))
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
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", encuestaSatisfaccion.IdReserva);
            return View(encuestaSatisfaccion);
        }

        // GET: EncuestaSatisfaccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encuestaSatisfaccion = await _context.EncuestaSatisfaccions
                .Include(e => e.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdEncuesta == id);
            if (encuestaSatisfaccion == null)
            {
                return NotFound();
            }

            return View(encuestaSatisfaccion);
        }

        // POST: EncuestaSatisfaccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encuestaSatisfaccion = await _context.EncuestaSatisfaccions.FindAsync(id);
            if (encuestaSatisfaccion != null)
            {
                _context.EncuestaSatisfaccions.Remove(encuestaSatisfaccion);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Encuesta Eliminada Exitosamente !";
            return RedirectToAction(nameof(Index));

        }

        private bool EncuestaSatisfaccionExists(int id)
        {
            return _context.EncuestaSatisfaccions.Any(e => e.IdEncuesta == id);
        }

        // NUEVO MÉTODO: Crear Encuesta con las calificaciones de estrellas
        public IActionResult CrearEncuesta(int idReserva)
        {
            // Obtiene la reserva y el cliente correspondiente por ID
            var reserva = _context.Reservas.Include(r => r.NombreCompleto)
                                           .FirstOrDefault(r => r.IdReserva == idReserva);

            if (reserva == null)
            {
                return NotFound();  // Si no se encuentra la reserva, retorna error 404
            }

            var encuesta = new EncuestaSatisfaccion
            {
                IdReserva = reserva.IdReserva,
                IdReservaNavigation = reserva
            };

            return View(encuesta);  // Retorna la vista de creación con la información de la reserva
        }

        // NUEVO MÉTODO: Procesar respuestas de la encuesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEncuesta([Bind("IdReserva,CalificacionLimpieza,CalificacionServicios,CalificacionComida,CalificacionHabitacion,CalificacionAtencion,CalificacionInstalaciones")] EncuestaSatisfaccion encuestaSatisfaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encuestaSatisfaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Redirige al listado de encuestas
            }

            return View(encuestaSatisfaccion);  // Si el modelo no es válido, vuelve a mostrar la vista de encuesta
        }
    }
}
