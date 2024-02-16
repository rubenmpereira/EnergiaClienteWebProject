using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.Handlers.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[TypeFilter(typeof(AuthHabitation))]
[Validate]
[Route("[controller]/[action]")]
public class EnergiaClienteController : ControllerBase
{
    private IEnergiaClienteHandler Handler { get; set; }
    public EnergiaClienteController(IEnergiaClienteHandler _handler)
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

    [HttpGet(Name = "GetReadings")]
    public ActionResult<dbResponse<Reading>> GetReadings([FromQuery] GetReadingsRequestModel requestModel)
    {
        return ReturnResult(Handler.GetReadings(requestModel));
    }

    [HttpPost(Name = "UploadNewReading")]
    public ActionResult<dbResponse<decimal>> UploadNewReading([FromQuery] UploadNewReadingRequestModel requestModel)
    {
        return ReturnResult(Handler.UploadNewReading(requestModel));
    }

    [HttpGet(Name = "GetReadingByDate")]
    public ActionResult<dbResponse<Reading>> GetReadingByDate([FromQuery] GetReadingByDateRequestModel requestModel)
    {
        return ReturnResult(Handler.GetReadingByDate(requestModel));
    }

    [HttpGet(Name = "GetpreviousMonthReading")]
    public ActionResult<dbResponse<Reading>> GetpreviousMonthReading([FromQuery] GetReadingByDateRequestModel requestModel)
    {
        return ReturnResult(Handler.GetpreviousMonthReading(requestModel));
    }

    [HttpGet(Name = "GetInvoices")]
    public ActionResult<dbResponse<Invoice>> GetInvoices()
    {
        return ReturnResult(Handler.GetInvoices());
    }

    [HttpGet(Name = "GetUnpaidTotal")]
    public ActionResult<dbResponse<decimal>> GetUnpaidTotal()
    {
        return ReturnResult(Handler.GetUnpaidTotal());
    }

    [HttpGet(Name = "GetHolderDetails")]
    public ActionResult<dbResponse<Holder>> GetHolderDetails()
    {
        return ReturnResult(Handler.GetHolderDetails());
    }

    [HttpGet(Name = "GetHabitationDetails")]
    public ActionResult<dbResponse<Habitation>> GetHabitationDetails()
    {
        return ReturnResult(Handler.GetHabitationDetails());
    }

    [HttpPatch(Name = "UpdateHabitationPower")]
    public ActionResult<dbResponse<string>> UpdateHabitationPower([FromQuery] UpdateHabitationPowerRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHabitationPower(requestModel));
    }

    [HttpPatch(Name = "UpdateHolderName")]
    public ActionResult<dbResponse<string>> UpdateHolderName([FromQuery] UpdateHolderNameRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHolderName(requestModel));
    }

    [HttpPatch(Name = "UpdateHolderNif")]
    public ActionResult<dbResponse<string>> UpdateHolderNif([FromQuery] UpdateHolderNifRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHolderNif(requestModel));
    }

    [HttpPatch(Name = "UpdateHolderContact")]
    public ActionResult<dbResponse<string>> UpdateHolderContact([FromQuery] UpdateHolderContactRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHolderContact(requestModel));
    }

    [HttpPatch(Name = "UpdateHabitationTensionLevel")]
    public ActionResult<dbResponse<string>> UpdateHabitationTensionLevel([FromQuery] UpdateHabitationTensionLevelRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHabitationTensionLevel(requestModel));
    }

    [HttpPatch(Name = "UpdateHabitationSchedule")]
    public ActionResult<dbResponse<string>> UpdateHabitationSchedule([FromQuery] UpdateHabitationScheduleRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHabitationSchedule(requestModel));
    }

    [HttpPatch(Name = "UpdateHabitationPhase")]
    public ActionResult<dbResponse<string>> UpdateHabitationPhase([FromQuery] UpdateHabitationPhaseRequestModel requestModel)
    {
        return ReturnResult(Handler.UpdateHabitationPhase(requestModel));
    }

}