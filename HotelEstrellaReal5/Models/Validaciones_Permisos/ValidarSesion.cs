using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HotelEstrellaReal5.Models.Validaciones_Permisos
{
    public class ValidarSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filtercontext)
        {
            if (filtercontext.HttpContext.Session.GetString("Usuario") == null)
            {

                filtercontext.Result = new RedirectResult("/Acceso/Login");
            }
            base.OnActionExecuting(filtercontext);
        }
    }
}
