using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaReal5.Models;
using Newtonsoft.Json;

namespace HotelEstrellaReal5.Controllers
{
    public class ReservasController : BaseController
    {   
        private readonly HotelEstrellaReal5Context _context;

        public ReservasController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal5Context = _context.Reservas.Include(r => r.IdCategoriaNavigation).Include(r => r.IdClienteNavigation).Include(r => r.IdEstadoReservaNavigation).Include(r => r.IdHabitacionNavigation);
            return View(await hotelEstrellaReal5Context.ToListAsync());


        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.IdCategoriaNavigation)
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdHabitacionNavigation)
                .Include(r => r.DetallesReservaServicios)
                .ThenInclude(d => d.IdServicioNavigation)
                .Include(r => r.DetallesReservaHuespeds)
                .ThenInclude(d => d.IdHuespedNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            ViewBag.Servicios = _context.Servicios.ToList();

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre");
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente");
            ViewData["IdEstadoReserva"] = new SelectList(_context.EstadoReservas, "IdEstadoReserva", "NombreEstado");
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Nombre");
            ViewBag.Servicios = _context.Servicios.ToList();
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReserva,FechaEntrada,FechaSalida,NombreCompleto,IdCliente,ComprobanteAdelanto,FechaRegistro,IdCategoria,IdEstadoReserva,IdHabitacion")] Reserva reserva, List<int> serviciosSeleccionados)
        {
             if (reserva.FechaEntrada < DateTime.Now)
            {
                ModelState.AddModelError("FechaEntrada", "La fecha de entrada no puede ser anterior a la fecha actual.");
            }

            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                ModelState.AddModelError("FechaSalida", "La fecha de salida debe ser al menos un día después de la fecha de entrada.");
            }

            if (reserva.FechaSalida > reserva.FechaEntrada.AddDays(30))
            {
                ModelState.AddModelError("FechaSalida", "La fecha de salida no puede ser mayor a 30 días después de la fecha de entrada.");
            }


            if (ModelState.IsValid)
            {
                // Si es válido, agregamos la nueva reserva al contexto de la base de datos
                _context.Add(reserva);
                await _context.SaveChangesAsync(); // Guardamos los cambios en la base de datos

                // Asociamos los servicios seleccionados a la nueva reserva
                foreach (var idServicio in serviciosSeleccionados)
                {
                    _context.DetallesReservaServicios.Add(new DetallesReservaServicio
                    {
                        IdReserva = reserva.IdReserva, // Asociamos la reserva a este servicio
                        IdServicio = idServicio // Asociamos el servicio por su id
                    });
                }

                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Reserva Creada Exitosamente !";
                return RedirectToAction(nameof(Index)); // Redirigimos a la acción Index después de la creación exitosa

            }
               

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", reserva.IdCategoria);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", reserva.IdCliente);
            ViewData["IdEstadoReserva"] = new SelectList(_context.EstadoReservas, "IdEstadoReserva", "NombreEstado", reserva.IdEstadoReserva);
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Nombre", reserva.IdHabitacion);
            ViewBag.Servicios = _context.Servicios.ToList();
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos la reserva con el id especificado y cargamos todas sus relaciones
            var reserva = await _context.Reservas
                 .Include(r => r.IdClienteNavigation)
                 .Include(r => r.IdEstadoReservaNavigation)
                 .Include(r => r.IdHabitacionNavigation)
                 .Include(r => r.IdCategoriaNavigation)
                 .Include(r => r.DetallesReservaServicios)
                 .ThenInclude(d => d.IdServicioNavigation)
                 .Include(r => r.DetallesReservaHuespeds)
                 .ThenInclude(d => d.IdHuespedNavigation)
                 .FirstOrDefaultAsync(m => m.IdReserva == id); // Traemos la primera reserva que coincida con el id
                        
            if (reserva == null)
            {
                return NotFound();
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", reserva.IdCategoria);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", reserva.IdCliente);
            ViewData["IdEstadoReserva"] = new SelectList(_context.EstadoReservas, "IdEstadoReserva", "NombreEstado", reserva.IdEstadoReserva);
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Nombre", reserva.IdHabitacion);
            // Usamos ViewBag para pasar los servicios con sus valores a la vista
            ViewBag.Servicios = _context.Servicios.Select(s => new { s.IdServicio, s.Nombre, s.Valor }).ToList();
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,FechaEntrada,FechaSalida,NombreCompleto,IdCliente,ComprobanteAdelanto,FechaRegistro,IdCategoria,IdEstadoReserva,IdHabitacion")] Reserva reserva, List<int> serviciosSeleccionados, string huespedesData)
        {
            if (reserva.FechaEntrada < DateTime.Now)
            {
                ModelState.AddModelError("FechaEntrada", "La fecha de entrada no puede ser anterior ni coincidir con la fecha actual.");
            }

            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                ModelState.AddModelError("FechaSalida", "La fecha de salida debe ser al menos un día después de la fecha de entrada.");
            }

            if (reserva.FechaSalida > reserva.FechaEntrada.AddDays(30))
            {
                ModelState.AddModelError("FechaSalida", "La fecha de salida no puede ser mayor a 30 días después de la fecha de entrada.");
            }

            if (id != reserva.IdReserva)
            {
                return NotFound();
            }

            var reservaOriginal = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.IdReserva == id);

            if (reservaOriginal == null)
            {
                return NotFound(); // Si no se encuentra la reserva original, retornamos NotFound
            }

            // Preservamos las fechas originales de la reserva
            reserva.FechaEntrada = reservaOriginal.FechaEntrada; // No se debe modificar
            reserva.FechaSalida = reservaOriginal.FechaSalida; // No se debe modificar
            reserva.FechaRegistro = reservaOriginal.FechaRegistro; // No se debe modificar

            // Si el modelo es válido, procedemos a actualizar la reserva
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva); // Actualizamos los datos de la reserva
                    await _context.SaveChangesAsync(); // Guardamos los cambios en la base de datos

                    // Eliminar los servicios existentes asociados a la reserva
                    var detallesExistentes = await _context.DetallesReservaServicios
                        .Where(d => d.IdReserva == reserva.IdReserva)
                        .ToListAsync();
                    _context.DetallesReservaServicios.RemoveRange(detallesExistentes); // Eliminamos los servicios antiguos

                    // Asociamos los nuevos servicios seleccionados a la reserva
                    foreach (var idServicio in serviciosSeleccionados)
                    {
                        _context.DetallesReservaServicios.Add(new DetallesReservaServicio
                        {
                            IdReserva = reserva.IdReserva, // Asociamos la reserva al servicio
                            IdServicio = idServicio // Asociamos el servicio por su id
                        });
                    }

                    // Eliminar los huéspedes existentes
                    var huespedesExistentes = _context.Huespedes.Where(h => h.IdHuesped == reserva.IdReserva);
                    _context.Huespedes.RemoveRange(huespedesExistentes);

                    // Deserializamos los nuevos huéspedes y los asociamos
                    var huespedes = JsonConvert.DeserializeObject<List<Huespede>>(huespedesData);
                    foreach (var huesped in huespedes)
                    {
                        huesped.IdHuesped = reserva.IdReserva;
                        _context.Huespedes.Add(huesped);
                    }
                    await _context.SaveChangesAsync();  // Guardamos los cambios nuevamente para insertar los nuevos servicios

                    // Establecemos un mensaje temporal de éxito para ser mostrado al usuario
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.IdReserva)) // Verificamos si la reserva aún existe en la base de datos
                    {
                        return NotFound(); // Si no existe, retornamos NotFound
                    }
                    else
                    {
                        throw; // Si ocurre otro error, lo lanzamos
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirigimos a la lista de reservas después de la edición exitosa
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Nombre", reserva.IdCategoria);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", reserva.IdCliente);
            ViewData["IdEstadoReserva"] = new SelectList(_context.EstadoReservas, "IdEstadoReserva", "NombreEstado", reserva.IdEstadoReserva);
            ViewData["IdHabitacion"] = new SelectList(_context.Habitaciones, "IdHabitacion", "Nombre", reserva.IdHabitacion);
            ViewBag.Servicios = _context.Servicios.ToList();
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos la reserva con el id especificado y cargamos todas sus relaciones
            var reserva = await _context.Reservas
                .Include(r => r.IdClienteNavigation)
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdHabitacionNavigation)
                .Include(r => r.DetallesReservaServicios)
                .ThenInclude(d => d.IdServicioNavigation)
                .Include(r => r.DetallesReservaHuespeds)
                .ThenInclude(d => d.IdHuespedNavigation)
                .FirstOrDefaultAsync(m => m.IdReserva == id); // Traemos la primera reserva que coincida con el id

            if (reserva == null)
            {
                return NotFound();
            }

             return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var reserva = await _context.Reservas
                    .Include(r => r.DetallesReservaServicios) // Cargamos los servicios asociados a la reserva
                    .FirstOrDefaultAsync(r => r.IdReserva == id); // Buscamos la reserva por su id

                if (reserva == null)
                {
                    TempData["Error"] = "La reserva no existe.";
                    return RedirectToAction("Index");
                }

                // Eliminamos los servicios asociados a la reserva
                _context.DetallesReservaServicios.RemoveRange(reserva.DetallesReservaServicios);

                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Reserva eliminada con éxito.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                // Aquí se maneja la excepción
                TempData["Error"] = "No se puede eliminar la reserva porque está asociada a otras entidades.";
            }
            return RedirectToAction("Index");
        }


        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
        }

        [HttpGet]
        public JsonResult ObtenerClientePorDocumento(int documento)
        {
            try
            {
                // Buscar el cliente por documento
                var cliente = _context.Clientes
                    .FirstOrDefault(c => c.IdCliente == documento);  // Suponiendo que "Documento" es el campo en la tabla Clientes

                if (cliente != null)
                {
                    // Si el cliente existe, devolver el nombre
                    return Json(new { success = true, nombre = cliente.NombreCompleto });
                }
                else
                {
                    // Si el cliente no existe, devolver un mensaje de error
                    return Json(new { success = false, message = "Para realizar una reserva, tienes que estar registrado como cliente." });
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                return Json(new { success = false, message = "Error al verificar el documento", error = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult ObtenerHabitacionesPorCategoria(int idCategoria, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Validamos si el idCategoria es un valor válido
                if (idCategoria <= 0)
                {
                    return Json(new { success = false, message = "ID de categoría inválido" });
                }

                // Intentamos obtener todas las habitaciones que pertenecen a la categoría seleccionada
                var habitaciones = _context.Habitaciones
                    .Include(h => h.IdCategoriaNavigation) // Incluye la categoría asociada a cada habitación
                    .Include(h => h.DetallesHabitacionComodidads) // Incluye los detalles/comodidades de la habitación
                    .ThenInclude(d => d.IdComodidadNavigation) // Incluye las comodidades asociadas a la habitación
                    .Where(h => h.IdCategoria == idCategoria) // Filtra las habitaciones por la categoría seleccionada
                    .ToList(); // Obtenemos todas las habitaciones de esa categoría

                // Filtrar habitaciones disponibles según las fechas seleccionadas
                var habitacionesDisponibles = new List<object>();

                var reservasExistentes = _context.Reservas
                    .Where(r => r.FechaEntrada < fechaFin && r.FechaSalida > fechaInicio)
                    .ToList();  // Obtenemos todas las reservas en el rango de fechas

                // Ahora, iteramos sobre las habitaciones y comprobamos si están ocupadas
                foreach (var habitacion in habitaciones)
                {
                    var reserva = reservasExistentes
                        .FirstOrDefault(r => r.IdHabitacion == habitacion.IdHabitacion);

                    if (reserva == null)
                    {
                        // La habitación está disponible, agregamos los detalles de la habitación
                        var comodidades = habitacion.DetallesHabitacionComodidads
                            .Select(d => new { d.IdComodidadNavigation.Descripcion })
                            .ToList();

                        habitacionesDisponibles.Add(new
                        {
                            habitacion.IdHabitacion,
                            habitacion.Nombre,
                            habitacion.Descripcion,
                            comodidades = comodidades
                        });
                    }
                }

                // Si no hay habitaciones disponibles en esa categoría, devolvemos un mensaje adecuado
                if (habitacionesDisponibles.Count == 0)
                {
                    return Json(new { success = false, message = "No hay habitaciones disponibles en las fechas seleccionadas." });
                }

                // Si todo va bien, retornamos las habitaciones disponibles con sus comodidades
                return Json(new
                {
                    success = true,
                    habitaciones = habitacionesDisponibles
                });
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, lo capturamos y lo devolvemos
                return Json(new { success = false, message = "Error al obtener las habitaciones", error = ex.Message });
            }
        }
    }
}
