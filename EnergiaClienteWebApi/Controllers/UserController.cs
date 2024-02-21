using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.Handlers.Interfaces;
using EnergiaClienteWebApi.RequestModels.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;


[Route("[controller]/[action]")]
public class UserController(IEnergiaClienteHandler _handler) : ControllerBase
{
    private readonly IEnergiaClienteHandler Handler = _handler;

    private string Email()
    {
        if (User.Identity == null)
            throw new Exception(); //define exeption

        var identity = (ClaimsIdentity)User.Identity;
        if (string.IsNullOrEmpty(identity.Name))
            throw new Exception(); //define exeption

        return identity.Name;
    }
    public ActionResult<dbResponse<T>> ReturnResult<T>(dbResponse<T> result)
    {
        var code = result.Status.StatusCode;
        return code switch
        {
            200 => new OkObjectResult(result),
            400 => new BadRequestObjectResult(result),
            401 => new UnauthorizedObjectResult(result),
            404 => new NotFoundObjectResult(result),
            _ => StatusCode(code, result)
        };
    }

    [Authorize]
    [HttpGet(Name = "GetUserDetails")]
    public ActionResult<dbResponse<User>> GetUserDetails()
    {
        var result = Handler.GetUserDetails(new GetUserDetailsRequestModel()
        {
            email = Email()
        });

        return ReturnResult(result);
    }

    [HttpPost(Name = "Auth")]
    public IResult Auth([FromBody] AuthRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.email))
            return Results.BadRequest();

        if (string.IsNullOrEmpty(requestModel.password))
            return Results.BadRequest();

        var result = Handler.AuthenticateUser(requestModel);

        if (result.Result[0] == false)
            return Results.Unauthorized();

        var claimsPrincipal =
        new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, requestModel.email)],
            BearerTokenDefaults.AuthenticationScheme)
            );

        return Results.SignIn(claimsPrincipal);
    }

}