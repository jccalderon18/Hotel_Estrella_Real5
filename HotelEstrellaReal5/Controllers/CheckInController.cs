using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaReal5.Models;
using System.Runtime.ConstrainedExecution;

namespace HotelEstrellaReal5.Controllers
{
    public class CheckInController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public CheckInController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: CheckIn
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal5Context = _context.CheckIns.Include(c => c.IdHabitacionNavigation).Include(c => c.IdHuespedNavigation).Include(c => c.IdReservaNavigation);
            return View(await hotelEstrellaReal5Context.ToListAsync());
        }

        // GET: CheckIn/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkIn = await _context.CheckIns
                .Include(c => c.IdHabitacionNavigation)
                .Include(c => c.IdHuespedNavigation)
                .Include(c => c.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdCheckIn == id);
            if (checkIn == null)
            {
                return NotFound();
            }

            return View(checkIn);
        }

        // GET: CheckIn/Create
        public IActionResult Create()
        {
            // Obtener las reservas confirmadas
            var reservasConfirmadas = _context.Reservas
                .Where(r => r.IdEstadoReservaNavigation.NombreEstado == "Confirmada")
                .Select(r => new
                {
                    r.IdReserva,
                    r.NombreCompleto
                })
                .ToList();

            // Convertir a SelectListItem
            var selectListReservas = reservasConfirmadas.Select(r => new SelectListItem
            {
                Value = r.IdReserva.ToString(),
                Text = r.NombreCompleto
            }).ToList();

            // Obtener las habitaciones disponibles
            var habitaciones = _context.Habitaciones
                .Select(h => new SelectListItem
                {
                    Value = h.IdHabitacion.ToString(),
                    Text = h.Nombre
                })
                .ToList();

            // Pasar las listas al ViewBag
            ViewBag.Reservas = selectListReservas;
            ViewBag.Habitaciones = habitaciones;


            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Descripcion");
            ViewData["IdHuesped"] = new SelectList(_context.Huespedes, "IdHuesped", "Email");
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "NombreCompleto");
            return View();
        }

        // POST: CheckIn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCheckIn,IdReserva,IdHuesped,IdHabitacion,FechaRegistro")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                // Actualizar el estado de la reserva a "En Ejecución"
                var reserva = await _context.Reservas.FindAsync(checkIn.IdReserva);
                if (reserva != null)
                {
                    reserva.IdEstadoReservaNavigation.NombreEstado = "En Ejecución"; // Cambiar el estado de la reserva
                    _context.Update(reserva);
                }

                _context.Add(checkIn);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Ckeck-in Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Descripcion", checkIn.IdHabitacion);
            ViewData["IdHuesped"] = new SelectList(_context.Huespedes, "IdHuesped", "Email", checkIn.IdHuesped);
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "NombreCompleto", checkIn.IdReserva);
            return View(checkIn);
        }

        // GET: CheckIn/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkIn = await _context.CheckIns.FindAsync(id);
            if (checkIn == null)
            {
                return NotFound();
            }
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Descripcion", checkIn.IdHabitacion);
            ViewData["IdHuesped"] = new SelectList(_context.Huespedes, "IdHuesped", "Email", checkIn.IdHuesped);
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "NombreCompleto", checkIn.IdReserva);
            return View(checkIn);
        }

        // POST: CheckIn/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCheckIn,IdReserva,IdHuesped,IdHabitacion,FechaRegistro")] CheckIn checkIn)
        {
            if (id != checkIn.IdCheckIn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkIn);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckInExists(checkIn.IdCheckIn))
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
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Descripcion", checkIn.IdHabitacion);
            ViewData["IdHuesped"] = new SelectList(_context.Huespedes, "IdHuesped", "Email", checkIn.IdHuesped);
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "NombreCompleto", checkIn.IdReserva);
            return View(checkIn);
        }

        // GET: CheckIn/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkIn = await _context.CheckIns
                .Include(c => c.IdHabitacionNavigation)
                .Include(c => c.IdHuespedNavigation)
                .Include(c => c.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdCheckIn == id);
            if (checkIn == null)
            {
                return NotFound();
            }

            return View(checkIn);
        }

        // POST: CheckIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkIn = await _context.CheckIns.FindAsync(id);
            if (checkIn != null)
            {
                _context.CheckIns.Remove(checkIn);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Ckeck-in Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool CheckInExists(int id)
        {
            return _context.CheckIns.Any(e => e.IdCheckIn == id);
        }

        // Método para obtener los detalles de la reserva seleccionada
        public JsonResult GetReservaDetails(int reservaId)
        {
            var reserva = _context.Reservas
                .Include(r => r.IdHabitacionNavigation)
                .Include(r => r.DetallesReservaHuespeds)
                    .ThenInclude(d => d.IdHuespedNavigation)
                .FirstOrDefault(r => r.IdReserva == reservaId);

            if (reserva != null)
            {
                var detalle = new
                {
                    FechaEntrada = reserva.FechaEntrada.ToString("yyyy-MM-dd"),
                    FechaSalida = reserva.FechaSalida.ToString("yyyy-MM-dd"),
                    NumeroHabitacion = reserva.IdHabitacionNavigation?.Nombre,
                    Huespedes = reserva.DetallesReservaHuespeds.Select(h => new
                    {
                        NombreCompleto = h.IdHuespedNavigation?.NombreCompleto,
                        Identificacion = h.IdHuespedNavigation?.Identificacion,
                        Email = h.IdHuespedNavigation?.Email,
                    }).ToList()
                };
                return Json(detalle);
            }

            return Json(new { Error = "Reserva no encontrada." });
        }

    }
}

