using HotelEstrellaReal5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace HotelEstrellaReal5.Controllers
{
    public class PerfilController : BaseController
    {
        private readonly HotelEstrellaReal5Context _context;

        public PerfilController(HotelEstrellaReal5Context context)
        {
            _context = context;
        }
        // Ruta donde se guardarán las imágenes de perfil
        private readonly string _imagenesRuta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "perfil");


        // Acción para mostrar el perfil y la imagen de perfil
        public IActionResult Perfil()
        {
            // Obtener la imagen de perfil desde la sesión
            string userProfilePicture = HttpContext.Session.GetString("UserProfilePicture");

            // Si no existe en la sesión, asignar una imagen predeterminada
            if (string.IsNullOrEmpty(userProfilePicture))
            {
                userProfilePicture = "/Imagenes/img_pre_per_2.png";  // Ruta predeterminada
            }

            // Pasar la ruta de la imagen de perfil a la vista
            ViewData["UserProfilePicture"] = userProfilePicture;

            return View();
        }

            // Acción para subir la foto de perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirFoto(IFormFile fotoPerfil)
        {
            if (fotoPerfil == null || fotoPerfil.Length == 0)
            {
                TempData["Error"] = "Por favor, selecciona una imagen para subir.";
                return RedirectToAction("Perfil");
            }

            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
            var extensionArchivo = Path.GetExtension(fotoPerfil.FileName).ToLower();

            if (!extensionesPermitidas.Contains(extensionArchivo))
            {
                TempData["Error"] = "Formato de imagen no permitido.";
                return RedirectToAction("Perfil");
            }

            const long maxSize = 5 * 1024 * 1024;
            if (fotoPerfil.Length > maxSize)
            {
                TempData["Error"] = "El tamaño máximo permitido es 5MB.";
                return RedirectToAction("Perfil");
            }

            try
            {
                var nombreArchivo = $"{Path.GetFileNameWithoutExtension(fotoPerfil.FileName)}_{Guid.NewGuid()}{extensionArchivo}";
                var rutaArchivo = Path.Combine(_imagenesRuta, nombreArchivo);

                if (!Directory.Exists(_imagenesRuta))
                    Directory.CreateDirectory(_imagenesRuta);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await fotoPerfil.CopyToAsync(stream);
                }

                string urlImagen = $"/imagenes/perfil/{nombreArchivo}";

                // Usa FirstOrDefaultAsync solo si estás seguro de que _context.Usuarios está configurado para EF
                var usuarioActual = _context.Usuarios.FirstOrDefault(u => u.NombreCompleto == User.Identity.Name);
                if (usuarioActual == null)
                {
                    TempData["Error"] = "No se encontró al usuario actual.";
                    return RedirectToAction("Perfil");
                }

                usuarioActual.FotoPerfilUrl = urlImagen;
                _context.Usuarios.Update(usuarioActual);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("UserProfilePicture", urlImagen);
                TempData["Success"] = "Tu foto de perfil fue actualizada con éxito.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al subir la imagen: {ex.Message}";
            }

            return RedirectToAction("Perfil");
        }





        // Acción para cambiar la contraseña
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CambiarContrasena(string currentPassword, string newPassword, string confirmPassword)
        //{
        //    // Obtener el usuario actual
        //    var usuario = await _userManager.GetUserAsync(User);
        //    if (usuario == null)
        //    {
        //        TempData["Error"] = "Usuario no encontrado.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Verificar que la contraseña actual coincida
        //    var passwordValid = await _userManager.CheckPasswordAsync(usuario, currentPassword);
        //    if (!passwordValid)
        //    {
        //        TempData["Error"] = "La contraseña actual es incorrecta.";
        //        return RedirectToAction("MiPerfil");
        //    }

        //    // Verificar que la nueva contraseña y la confirmación coincidan
        //    if (newPassword != confirmPassword)
        //    {
        //        TempData["Error"] = "Las nuevas contraseñas no coinciden.";
        //        return RedirectToAction("MiPerfil");
        //    }

        //    // Cambiar la contraseña
        //    var result = await _userManager.ChangePasswordAsync(usuario, currentPassword, newPassword);
        //    if (result.Succeeded)
        //    {
        //        TempData["Success"] = "Contraseña actualizada con éxito.";
        //        await _signInManager.RefreshSignInAsync(usuario);  // Rehacer el inicio de sesión con la nueva contraseña
        //        return RedirectToAction("MiPerfil");
        //    }

        //    // Si hay algún error en el cambio de contraseña
        //    foreach (var error in result.Errors)
        //    {
        //        TempData["Error"] = error.Description;
        //    }

        //    return RedirectToAction("MiPerfil");

    }
}
