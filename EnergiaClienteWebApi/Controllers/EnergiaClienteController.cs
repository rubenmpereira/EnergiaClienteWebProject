using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Handlers.Interfaces;

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
        if (result.Status.Error == true)
        {
            if (result.Status.StatusCode == 404)
                return new NotFoundObjectResult(result);
            else
                return new BadRequestObjectResult(result);
        }

        return new OkObjectResult(result);
    }

    [HttpGet(Name = "GetReadings")]
    public ActionResult<dbResponse<Reading>> GetReadings([FromQuery] GetReadingsRequestModel requestModel)
    {
        var result = Handler.GetReadings(requestModel);

        return ReturnResult(result);
    }

    [HttpPost(Name = "UploadNewReading")]
    public ActionResult<dbResponse<decimal>> UploadNewReading([FromQuery] UploadNewReadingRequestModel requestModel)
    {
        var today = DateTime.Now;
        int month = today.AddMonths(-1).Month;
        int year = today.AddMonths(-1).Year;

        if (today.Day < 5 || today.Day > 7)
            return new BadRequestObjectResult(new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You can not upload a new reading in this date",
                    StatusCode = 400
                }
            });

        var readingResult = Handler.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = requestModel.habitation,
            month = month,
            year = year
        });

        if (readingResult.Result.Count > 0)
            return new BadRequestObjectResult(new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You already uploaded your reading this month",
                    StatusCode = 400
                }
            });

        var result = Handler.UploadNewReading(new InsertReadingRequestModel()
        {
            Cheias = requestModel.cheias,
            Ponta = requestModel.ponta,
            Vazio = requestModel.vazio,
            Estimated = false,
            HabitationId = requestModel.habitation,
            Month = month,
            Year = year,
            ReadingDate = today
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetReadingByDate")]
    public ActionResult<dbResponse<Reading>> GetReadingByDate([FromQuery] GetReadingByDateRequestModel requestModel)
    {
        var result = Handler.GetReadingByDate(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetpreviousMonthReading")]
    public ActionResult<dbResponse<Reading>> GetpreviousMonthReading([FromQuery] GetReadingByDateRequestModel requestModel)
    {
        var result = Handler.GetpreviousMonthReading(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetInvoices")]
    public ActionResult<dbResponse<Invoice>> GetInvoices([FromQuery] GetInvoicesRequestModel requestModel)
    {
        var result = Handler.GetInvoices(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetUnpaidTotal")]
    public ActionResult<dbResponse<decimal>> GetUnpaidTotal([FromQuery] GetUnpaidTotalRequestModel requestModel)
    {
        var result = Handler.GetUnpaidTotal(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetUserDetails")]
    public ActionResult<dbResponse<User>> GetUserDetails([FromQuery] GetUserDetailsRequestModel requestModel)
    {
        var result = Handler.GetUserDetails(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetHolderDetails")]
    public ActionResult<dbResponse<Holder>> GetHolderDetails([FromQuery] GetHolderDetailsRequestModel requestModel)
    {
        var result = Handler.GetHolderDetails(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetHabitationDetails")]
    public ActionResult<dbResponse<Habitation>> GetHabitationDetails([FromQuery] GetHabitationDetailsRequestModel requestModel)
    {
        var result = Handler.GetHabitationDetails(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHabitationPower")]
    public ActionResult<dbResponse<string>> UpdateHabitationPower([FromQuery] UpdateHabitationPowerRequestModel requestModel)
    {
        var result = Handler.UpdateHabitationPower(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHolderName")]
    public ActionResult<dbResponse<string>> UpdateHolderName([FromQuery] UpdateHolderNameRequestModel requestModel)
    {
        var result = Handler.UpdateHolderName(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHolderNif")]
    public ActionResult<dbResponse<string>> UpdateHolderNif([FromQuery] UpdateHolderNifRequestModel requestModel)
    {
        var result = Handler.UpdateHolderNif(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHolderContact")]
    public ActionResult<dbResponse<string>> UpdateHolderContact([FromQuery] UpdateHolderContactRequestModel requestModel)
    {
        var result = Handler.UpdateHolderContact(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHabitationTensionLevel")]
    public ActionResult<dbResponse<string>> UpdateHabitationTensionLevel([FromQuery] UpdateHabitationTensionLevelRequestModel requestModel)
    {
        var result = Handler.UpdateHabitationTensionLevel(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHabitationSchedule")]
    public ActionResult<dbResponse<string>> UpdateHabitationSchedule([FromQuery] UpdateHabitationScheduleRequestModel requestModel)
    {
        var result = Handler.UpdateHabitationSchedule(requestModel);

        return ReturnResult(result);
    }

    [HttpGet(Name = "UpdateHabitationPhase")]
    public ActionResult<dbResponse<string>> UpdateHabitationPhase([FromQuery] UpdateHabitationPhaseRequestModel requestModel)
    {
        var result = Handler.UpdateHabitationPhase(requestModel);

        return ReturnResult(result);
    }

}