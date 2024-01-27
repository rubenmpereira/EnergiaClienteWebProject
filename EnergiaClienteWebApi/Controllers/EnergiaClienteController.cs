using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Database;
using EnergiaClienteWebApi.RequestModels;

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
        return new OkObjectResult(EnergiaCliente.GetReadings(new GetReadingsRequestModel() { habitation = id, quantity = 10 }));
    }

    [HttpPost(Name = "CalculateInvoice")]
    public ActionResult<dbResponse<decimal>> CalculateInvoice([FromQuery] int id, [FromBody] Reading reading)
    {
        return new OkObjectResult(EnergiaCliente.CalculateAmountPay(id, reading));
    }
}