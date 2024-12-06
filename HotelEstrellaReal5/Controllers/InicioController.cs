using HotelEstrellaReal5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelEstrellaReal5.Controllers
{
    public class InicioController : BaseController
    {
        private readonly Models.HotelEstrellaReal5Context _context;

        public InicioController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }
        public IActionResult Paginappal()
        {

            // Obtener todas las habitaciones de la base de datos, incluyendo la categoría y el estado
            var habitaciones = _context.Habitaciones
                .Include(h => h.IdEstadoHabitacionNavigation) // Incluir estado de la habitación
                .Include(h => h.IdCategoriaNavigation) // Incluir categoría de la habitación
                .ToList();

            // Calcular la cantidad de habitaciones en cada estado
            var totalHabitaciones = habitaciones.Count();
            var habitacionesDisponibles = habitaciones.Count(h => h.IdEstadoHabitacionNavigation.NombreEstadoHabitacion == "Disponible");
            var habitacionesOcupadas = habitaciones.Count(h => h.IdEstadoHabitacionNavigation.NombreEstadoHabitacion == "Ocupada");
            var habitacionesReservadas = habitaciones.Count(h => h.IdEstadoHabitacionNavigation.NombreEstadoHabitacion == "Reservada");
            var habitacionesInhabilitadas = habitaciones.Count(h => h.IdEstadoHabitacionNavigation.NombreEstadoHabitacion == "Inhabilitada");
            var habitacionesLimpieza = habitaciones.Count(h => h.IdEstadoHabitacionNavigation.NombreEstadoHabitacion == "Limpieza");

            // Preparar los datos para los gráficos y vista
            ViewBag.TotalHabitaciones = totalHabitaciones;
            ViewBag.HabitacionesDisponibles = habitacionesDisponibles;
            ViewBag.HabitacionesOcupadas = habitacionesOcupadas;
            ViewBag.HabitacionesReservadas = habitacionesReservadas;
            ViewBag.HabitacionesInhabilitadas = habitacionesInhabilitadas;
            ViewBag.HabitacionesLimpieza = habitacionesLimpieza;

            // Pasar las habitaciones completas (incluyendo categoría y estado) a la vista
            ViewBag.Habitaciones = habitaciones;



            return View();
        }

        public IActionResult Dashboard()
        {
            // Obtén todas las encuestas con las calificaciones de cada categoría
            var encuestas = _context.EncuestaSatisfaccions.ToList();

            // Calcula los promedios de cada calificación
            var promedioLimpieza = encuestas.Average(e => e.CalificacionLimpieza ?? 0);
            var promedioServicios = encuestas.Average(e => e.CalificacionServicios ?? 0);
            var promedioComida = encuestas.Average(e => e.CalificacionComida ?? 0);
            var promedioHabitacion = encuestas.Average(e => e.CalificacionHabitacion ?? 0);
            var promedioAtencion = encuestas.Average(e => e.CalificacionAtencion ?? 0);
            var promedioInstalaciones = encuestas.Average(e => e.CalificacionInstalaciones ?? 0);

            // Crear un objeto anónimo con los promedios para pasar a la vista
            var promedios = new
            {
                Limpieza = promedioLimpieza,
                Servicios = promedioServicios,
                Comida = promedioComida,
                Habitacion = promedioHabitacion,
                Atencion = promedioAtencion,
                Instalaciones = promedioInstalaciones
            };
            return View(promedios);
        }

        public IActionResult PaginappalClientes()
        {
            return View();
        }

        public IActionResult RestauranteyBar()
        {
            return View();
        }

        public IActionResult AuditorioyEventos()
        {
            return View();
        }

        public IActionResult HabitacionesClientes()
        {
            return View();
        }

        public IActionResult ServiciosClientes()
        {
            return View();
        }

        public IActionResult ReservasClientes()
        {


            return View();
        }
    }
}
