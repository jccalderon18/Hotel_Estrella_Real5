using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HotelEstrellaReal5.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar si la imagen de perfil ya está almacenada en la sesión
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserProfilePicture")))
            {
                // Si no hay imagen, asigna la imagen predeterminada
                HttpContext.Session.SetString("UserProfilePicture", "/images/img_pre_per_2.PNG");
            }

            // Recuperar la imagen de perfil desde la sesión y pasarla a ViewData
            string userProfilePicture = HttpContext.Session.GetString("UserProfilePicture");
            ViewData["UserProfilePicture"] = userProfilePicture;

            // Llamada al base para ejecutar la acción del controlador
            base.OnActionExecuting(context);

            // Obtener el nombre del usuario de las reclamaciones (si está autenticado)
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";

            // Obtener el nombre del usuario de las reclamaciones
            if (claimUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email) != null)
            {
                nombreUsuario = claimUser.Claims.Where(x => x.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            }
            // Almacenar el nombre en ViewData
            ViewData["Email"] = nombreUsuario;
        }
    }
}
