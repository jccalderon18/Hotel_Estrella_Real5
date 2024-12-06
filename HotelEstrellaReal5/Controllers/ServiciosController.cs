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
    public class ServiciosController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public ServiciosController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal2Context = _context.Servicios.Include(s => s.IdEstadoNavigation);
            return View(await hotelEstrellaReal2Context.ToListAsync());
        }

        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(s => s.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1");
            return View();
        }

        // POST: Servicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdServicio,Nombre,Valor,Descripcion,IdEstado")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Servicio Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", servicio.IdEstado);
            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", servicio.IdEstado);
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdServicio,Nombre,Valor,Descripcion,IdEstado")] Servicio servicio)
        {
            if (id != servicio.IdServicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.IdServicio))
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
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", servicio.IdEstado);
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(s => s.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Servicio Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.IdServicio == id);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int id, string estado)
        {
            var servicio = _context.Servicios.FirstOrDefault(s => s.IdServicio == id);

            if (servicio == null)
            {
                // Actualizar el estado
                servicio.IdEstadoNavigation.Estado1 = estado;

                _context.SaveChanges();
                return Json(new { success = true });
            }

            // Retornar una respuesta JSON indicando que no se encontró el Servicio
            return Json(new { success = false });
        }
    }
}
