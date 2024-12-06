using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaReal5.Models;
using System.Security.Claims;

namespace HotelEstrellaReal5.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public ClientesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            // Obtener el rol desde la sesión
            var rol = HttpContext.Session.GetString("Rol");

            // Pasa el rol a la vista usando ViewData
            ViewData["UsarRol"] = rol;

            // Obtiene el email del usuario autenticado usando el Claim "email"
            var nombre = User?.FindFirst(ClaimTypes.Name)?.Value;

            // Si el rol es Cliente, solo mostrar los registros del cliente actual
            if (rol == "5")
            {
                // Filtrar los clientes por el UserId o el identificador que esté asociado al cliente
                var cliente = await _context.Clientes
                    .Where(c => c.NombreCompleto == nombre)  // Asume que "UserId" es un campo en la entidad Cliente
                    .Include(c => c.IdEstadoNavigation)
                    .ToListAsync();

                return View(cliente);
            }


            var hotelEstrellaReal3Context = _context.Clientes.Include(c => c.IdEstadoNavigation);
            return View(await hotelEstrellaReal3Context.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,NombreCompleto,Telefono,Email,FechaNacimiento,FechaRegistro,IdEstado")] Cliente cliente)
        {

            // Verificar si el cliente ya existe
            if (await _context.Clientes.AnyAsync(c => c.IdCliente == cliente.IdCliente))
            {
                // Agregar un error al modelo para que se pueda mostrar en la vista
                ModelState.AddModelError("IdCliente", "Ya existe un cliente con este documento.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Cliente Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", cliente.IdEstado);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", cliente.IdEstado);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,NombreCompleto,Telefono,Email,FechaNacimiento,FechaRegistro,IdEstado")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            // Recuperar el cliente original desde la base de datos
            var clienteOriginal = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.IdCliente == id);

            if (clienteOriginal == null)
            {
                return NotFound();
            }

            // Preservar los campos originales del cliente
            cliente.FechaRegistro = clienteOriginal.FechaRegistro; // No se debe modificar
            cliente.IdCliente = clienteOriginal.IdCliente; // No se debe modificar

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", cliente.IdEstado);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Cliente Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int id, int estado)
        {
            // Buscar el cliente por IdCliente
            var cliente = _context.Clientes.FirstOrDefault(c => c.IdCliente == id);

            if (cliente != null)
            {
                // Asignar el nuevo estado al cliente
                cliente.IdEstado = estado;

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                // Retornar una respuesta JSON indicando éxito
                return Json(new { success = true });
            }

            // Retornar una respuesta JSON indicando que no se encontró el cliente
            return Json(new { success = false });
        }
    }
}
