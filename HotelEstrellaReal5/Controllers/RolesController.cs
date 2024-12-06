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
    public class RolesController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public RolesController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal2Context = _context.Roles.Include(r => r.IdEstadoNavigation);
            return View(await hotelEstrellaReal2Context.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1");
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,Nombre,Descripcion,IdEstado")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Rol Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", role.IdEstado);
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", role.IdEstado);
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,Nombre,Descripcion,IdEstado")] Role role)
        {
            if (id != role.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.IdRol))
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
            ViewData["IdEstado"] = new SelectList(_context.Estados, "IdEstado", "Estado1", role.IdEstado);
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .Include(r => r.IdEstadoNavigation)
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Rol Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.IdRol == id);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int id, int estado)
        {
            // Buscar el rol por IdRol
            var rol = _context.Roles.FirstOrDefault(r => r.IdRol == id);

            if (rol != null)
            {
                // Asignar el nuevo estado al rol
                rol.IdEstado = estado;

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                // Retornar una respuesta JSON indicando éxito
                return Json(new { success = true });
            }

            // Retornar una respuesta JSON indicando que no se encontró el rol
            return Json(new { success = false });
        }
    }
}
