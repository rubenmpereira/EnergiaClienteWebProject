using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.Models;
using EnergiaClienteWebApi.Handlers.Interfaces;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.RequestModels.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using EnergiaClienteWebApi.Models.User;


[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private IEnergiaClienteHandler Handler { get; set; }
    public UserController(IEnergiaClienteHandler _handler)
    {
        Handler = _handler;
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
        var identity = (ClaimsIdentity)User.Identity;
        var email = identity.Name;

        var result = Handler.GetUserDetails(new GetUserDetailsModel()
        {
            email = email
        });

        return ReturnResult(result);
    }

    [HttpPost(Name = "Auth")]
    public IResult Auth([FromBody] AuthRequestModel requestModel)
    {
        var result = Handler.AuthenticateUser(new AuthenticateUserModel()
        {
            email = requestModel.email,
            password = requestModel.password
        });

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