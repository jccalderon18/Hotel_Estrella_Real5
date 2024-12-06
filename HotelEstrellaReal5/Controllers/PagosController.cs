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
    public class PagosController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public PagosController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal3Context = _context.Pagos.Include(p => p.IdReservaNavigation);
            return View(await hotelEstrellaReal3Context.ToListAsync());
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdPago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva");
            return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPago,MedioPago,PrecioInicial,Adelanto,PrecioRestante,CostoPenalidad,SubTotal,Total,Observacion,FechaRegistro,IdReserva")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Pago Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", pago.IdReserva);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", pago.IdReserva);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPago,MedioPago,PrecioInicial,Adelanto,PrecioRestante,CostoPenalidad,SubTotal,Total,Observacion,FechaRegistro,IdReserva")] Pago pago)
        {
            if (id != pago.IdPago)
            {
                return NotFound();
            }

            // Recuperamos el pago original de la base de datos
            var pagoOriginal = await _context.Pagos.AsNoTracking().FirstOrDefaultAsync(p => p.IdPago == id);
            if (pagoOriginal == null)
            {
                return NotFound(); // Si no encontramos el pago original, retornamos un error 404.
            }

            // Preservamos la fecha original de pago
            pago.FechaRegistro = pagoOriginal.FechaRegistro; // La fecha original no debe cambiar.

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.IdPago))
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
            ViewData["IdReserva"] = new SelectList(_context.Reservas, "IdReserva", "IdReserva", pago.IdReserva);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.IdReservaNavigation)
                .FirstOrDefaultAsync(m => m.IdPago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Pago Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        // Método para obtener la información detallada de una reserva y los cálculos del pago
        [HttpPost]
        public async Task<IActionResult> GetClienteReserva(int idCliente)
        {
            // Inicializar el mensaje de validación
            ViewData["ValidacionPago"] = "";

            // Buscar el cliente con las reservas relacionadas
            var cliente = await _context.Clientes
                .Include(c => c.Reservas)
                    .ThenInclude(r => r.IdHabitacionNavigation) // Incluir la habitación de la reserva
                .Include(c => c.Reservas)
                    .ThenInclude(r => r.DetallesReservaServicios) // Incluir los servicios asociados a la reserva
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente); // Buscar por ID del cliente

            // Verificar si el cliente existe
            if (cliente == null)
            {
                ViewData["ValidacionPago"] = "Cliente no encontrado.";
                return View();
            }

            // Mostrar las reservas asociadas al cliente
            var reservas = cliente.Reservas.ToList(); // Todas las reservas asociadas

            if (reservas.Count == 0)
            {
                ViewData["ValidacionPago"] = "No se encontraron reservas para este cliente.";
                return View();
            }

            // Devolver las reservas asociadas al cliente (esto puede ser una lista para que el cliente elija)
            return Json(new
            {
                success = true,
                reservas = reservas.Select(r => new
                {
                    r.IdReserva,
                    r.FechaEntrada,
                    r.FechaSalida,
                    Habitacion = r.IdHabitacionNavigation.Nombre // Muestra nombre de habitación asociada
                }).ToList()
            });
        }

        // Método POST para calcular los totales de una reserva seleccionada
        [HttpPost]
        public IActionResult CalcularPrecios(int idReserva)
        {
            var reserva = _context.Reservas
                .Include(r => r.IdHabitacionNavigation)
                .Include(r => r.DetallesReservaServicios)
                .ThenInclude(d => d.IdServicioNavigation)
                .Include(r => r.DetallesReservaHuespeds)
                .ThenInclude(d => d.IdHuespedNavigation)
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdClienteNavigation)
                .FirstOrDefault(r => r.IdReserva == idReserva);

            if (reserva == null)
            {
                return Json(new { success = false, message = "Reserva no encontrada" });
            }

            var noches = (reserva.FechaSalida - reserva.FechaEntrada).Days;
            var precioPorNoche = reserva.IdHabitacionNavigation.Precio;
            var precioInicial = precioPorNoche * noches;
            var adelanto = precioInicial * 0.5m;
            var precioRestante = precioInicial - adelanto;
            // Cálculo de penalidad
            DateTime fechaActual = DateTime.Now;
            int diasDeRetraso = 0;

            if (fechaActual > reserva.FechaSalida)
            {
                diasDeRetraso = (fechaActual - reserva.FechaSalida).Days;
            }

            var costoPenalidad = diasDeRetraso * 200000;  // Penalidad de 200,000 por cada día de retraso
            var subtotal = precioRestante + costoPenalidad;
            var iva = subtotal * 0.19m;
            var total = subtotal + iva;

            return Json(new
            {
                success = true,
                nombreCliente = reserva.IdClienteNavigation.NombreCompleto,
                identificacionCliente = reserva.IdClienteNavigation.IdCliente,
                nombreHabitacion = reserva.IdHabitacionNavigation.Nombre,
                fechaEntrada = reserva.FechaEntrada.ToString("dd/MM/yyyy"),
                fechaSalida = reserva.FechaSalida.ToString("dd/MM/yyyy"),
                Servicios = reserva.DetallesReservaServicios.Select(d => new
                {
                    d.IdServicioNavigation.Nombre,
                    d.IdServicioNavigation.Valor
                }).ToList(),
                PrecioPorNoche = precioPorNoche,
                PrecioInicial = precioInicial,
                Adelanto = adelanto,
                PrecioRestante = precioRestante,
                CostoPenalidad = costoPenalidad,
                Subtotal = subtotal,
                iva = iva,
                Total = total
            });
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.IdPago == id);
        }
    }
}
