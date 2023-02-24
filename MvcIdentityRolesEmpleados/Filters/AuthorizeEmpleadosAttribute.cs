using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcIdentityRolesEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute
        , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRoute("Manager", "Login");
            }
            else
            {
                //VALIDAMOS EL ROLE CON SU OFICIO
                if (user.IsInRole("DIRECTOR") == false
                    && user.IsInRole("PRESIDENTE") == false)
                {
                    context.Result = this.GetRoute("Manager", "ErrorAcceso");
                }
            }
        }

        //CREAMOS UN METODO DE AYUDA POR SI REDIRECCIONAMOS A MAS
        //SITIOS ADEMAS DEL LOGIN
        private RedirectToRouteResult GetRoute
            (string controlador, string vista)
        {
            RouteValueDictionary ruta = 
                new RouteValueDictionary(new {
                    controller = controlador,
                    action = vista
                });
            return new RedirectToRouteResult(ruta);
        }
    }
}
