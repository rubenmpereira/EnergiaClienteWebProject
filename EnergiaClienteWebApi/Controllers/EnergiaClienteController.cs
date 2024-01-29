using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Handlers;

[Route("[controller]/[action]")]
public class EnergiaClienteController : ControllerBase
{
    //private IDatabase DB { get; set; } 
    public EnergiaClienteController()
    {

    }

    [HttpGet(Name = "GetReadings")]
    public ActionResult<dbResponse<Reading>> GetReadings([FromQuery] int id)
    {
        var result = EnergiaClienteHandler.GetReadings(new GetReadingsRequestModel() { habitation = id, quantity = 10 });

        if (result.Status.Error == true)
        {
            if (result.Status.StatusCode == 404)
                return new NotFoundResult();
            else
                return new BadRequestObjectResult(result);
        }

        return new OkObjectResult(result);
    }

    [HttpPost(Name = "CalculateInvoice")]
    public ActionResult<dbResponse<decimal>> CalculateInvoice([FromQuery] int id, [FromBody] Reading reading)
    {
        var result = EnergiaClienteHandler.CalculateAmountPay(id, reading);

        if (result.Status.Error == true)
        {
            if (result.Status.StatusCode == 404)
                return new NotFoundResult();
            else
                return new BadRequestObjectResult(result);
        }

        return new OkObjectResult(result);
    }
}