using EnergiaClienteWebApi.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class Validate : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var args = context.ActionArguments;

        if (args.Count == 0)
            return;

        var requestModel = args.First().Value ?? new object();
        var queryKeys = context.HttpContext.Request.Query.Keys;
        var missing = requestModel.GetType().GetProperties().Where(p => !queryKeys.Contains(p.Name));

        if (missing.Count() > 0)
        {
            context.Result = new BadRequestObjectResult(new dbResponse<string>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "There was a missing parameter: " + string.Join(",", missing.Select(x => x.Name).ToArray()),
                    StatusCode = 400
                }
            });
            return;
        }

        base.OnActionExecuting(context);
    }
}