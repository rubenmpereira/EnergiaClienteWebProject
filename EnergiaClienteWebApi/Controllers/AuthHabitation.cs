using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Handlers.Interfaces;
using System.Security.Claims;
using EnergiaClienteWebApi.Models.User;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthHabitation : ActionFilterAttribute
{
    private IEnergiaClienteHandler Handler { get; set; }
    public AuthHabitation(IEnergiaClienteHandler _handler)
    {
        Handler = _handler;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var request = filterContext.HttpContext.Request;
        var identity = filterContext.HttpContext.User.Identity;

        if (identity == null)
        {
            filterContext.Result = new StatusCodeResult(500);
            base.OnActionExecuting(filterContext);
            return;
        }

        var claim = (ClaimsIdentity)identity;
        var email = claim.Name;

        var header = request.Headers.FirstOrDefault(h => h.Key.Equals("habitation"));

        if (string.IsNullOrEmpty(header.Value))
        {
            filterContext.Result = new BadRequestResult();
            base.OnActionExecuting(filterContext);
            return;
        }

        var habitation = int.Parse(header.Value.ToString());

        var autherization = Handler.AutherizeHabitation(new AutherizeHabitationModel()
        {
            email = email,
            habitation = habitation
        });

        if (autherization.Result[0] == false)
            filterContext.Result = new ForbidResult();

        base.OnActionExecuting(filterContext);
    }
}