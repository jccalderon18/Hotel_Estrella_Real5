using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelEstrellaReal5.Models.Validaciones_Permisos
{
    public class PermisosRol : ActionFilterAttribute
    {
        private ROL Idrol;

        public PermisosRol(ROL idrol)
        {
            Idrol = idrol;
        }

        public override void OnActionExecuting(ActionExecutingContext filtercontext)
        {
            // Verifica si hay un usuario en la sesión
            var usuarioSession = filtercontext.HttpContext.Session.GetString("Usuario");

            if (usuarioSession == null)
            {
                // Redirigir si no hay usuario en la sesión
                filtercontext.Result = new RedirectResult("~/Inicio/Paginappal");
            }
            else
            {
                // Si hay un usuario, intenta deserializarlo
                Usuario oUsuario = JsonConvert.DeserializeObject<Usuario>(usuarioSession);

                // Verifica el rol
                if (oUsuario?.IdRol != (int)Idrol)
                {
                    // Redirigir si el rol no coincide
                    filtercontext.Result = new RedirectResult("~/Inicio/Paginappal");
                }
            }

            base.OnActionExecuting(filtercontext);
        }
    }
}
