using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Handlers.Interfaces;
using System.Security.Claims;
using EnergiaClienteWebApi.Models.User;
using Microsoft.AspNetCore.Mvc.Filters;
using EnergiaClienteWebApi.Domains;

public class AuthHabitation(IEnergiaClienteHandler _handler) : ActionFilterAttribute
{
    private readonly IEnergiaClienteHandler Handler = _handler;

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
            filterContext.Result = new BadRequestObjectResult(
                new dbResponse<string>()
                {
                    Status = new StatusObject()
                    {
                        Error = true,
                        ErrorMessage = "habitation header must be specified",
                        StatusCode = 400
                    }
                });
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

        Handler.habitation = habitation;

        base.OnActionExecuting(filterContext);
    }
}