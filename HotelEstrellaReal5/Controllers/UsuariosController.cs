using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelEstrellaReal5.Models;
using System.Text;
using System.Security.Cryptography;

namespace HotelEstrellaReal5.Controllers
{
    public class UsuariosController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public UsuariosController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var hotelEstrellaReal3Context = _context.Usuarios.Include(u => u.IdClienteNavigation).Include(u => u.IdRolNavigation);
            return View(await hotelEstrellaReal3Context.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdClienteNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Email");
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreCompleto,Email,Clave,ConfirmarClave,FotoPerfilUrl,FechaRegistro,IdCliente,IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.Clave == usuario.ConfirmarClave)
                {
                    usuario.Clave = ConvertirSha256(usuario.Clave);
                    usuario.ConfirmarClave = ConvertirSha256(usuario.ConfirmarClave);

                }
                else
                {
                    TempData["ResultError"] = "! Las Contraseñas no coinciden !";
                    return View(usuario);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["ResultOk"] = "! Usuario Creado Exitosamente !";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Email", usuario.IdCliente);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Email", usuario.IdCliente);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreCompleto,Email,Clave,ConfirmarClave,FotoPerfilUrl,FechaRegistro,IdCliente,IdRol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            // Recuperar el usuario original de la base de datos para preservar la fecha de registro
            var usuarioOriginal = await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuarioOriginal == null)
            {
                return NotFound();
            }

            // Preservar la fecha de registro original
            usuario.FechaRegistro = usuarioOriginal.FechaRegistro;

            if (ModelState.IsValid)
            {
                try
                {
                    if (usuario.Clave == usuario.ConfirmarClave)
                    {
                        usuario.Clave = ConvertirSha256(usuario.Clave);
                        usuario.ConfirmarClave = ConvertirSha256(usuario.ConfirmarClave);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["ResultOk"] = "! Datos Actualizados Exitosamente !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Email", usuario.IdCliente);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdClienteNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            TempData["ResultOk"] = "! Usuario Eliminado Exitosamente !";
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        private string ConvertirSha256(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
