using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Handlers.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[TypeFilter(typeof(AuthHabitation))]
[Route("[controller]/[action]")]
public class EnergiaClienteController : ControllerBase
{
    private IEnergiaClienteHandler Handler { get; set; }
    public EnergiaClienteController(IEnergiaClienteHandler _handler)
    {
        Handler = _handler;
    }
    private int Habitacao(HttpRequest request)
    {
        var habitation = request.Headers.FirstOrDefault(h => h.Key.Equals("habitation")).Value.ToString();
        if (string.IsNullOrEmpty(habitation))
            return 0;
        return int.Parse(habitation);
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
    public ActionResult<dbResponse<Reading>> GetReadings([FromQuery] GetReadingsRequestModel RequestModel)
    {
        var result = Handler.GetReadings(new GetReadingsModel()
        {
            habitation = Habitacao(Request),
            quantity = RequestModel.quantity
        });

        return ReturnResult(result);
    }

    [HttpPost(Name = "UploadNewReading")]
    public ActionResult<dbResponse<decimal>> UploadNewReading([FromQuery] UploadNewReadingRequestModel RequestModel)
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

        var readingResult = Handler.GetReadingByDate(new GetReadingByDateModel()
        {
            habitation = Habitacao(Request),
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

        var result = Handler.UploadNewReading(new InsertReadingModel()
        {
            Cheias = RequestModel.cheias,
            Ponta = RequestModel.ponta,
            Vazio = RequestModel.vazio,
            Estimated = false,
            HabitationId = 1,
            Month = month,
            Year = year,
            ReadingDate = today
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetReadingByDate")]
    public ActionResult<dbResponse<Reading>> GetReadingByDate([FromQuery] GetReadingByDateRequestModel RequestModel)
    {
        var result = Handler.GetReadingByDate(new GetReadingByDateModel()
        {
            habitation = Habitacao(Request),
            month = RequestModel.month,
            year = RequestModel.year
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetpreviousMonthReading")]
    public ActionResult<dbResponse<Reading>> GetpreviousMonthReading([FromQuery] GetReadingByDateRequestModel RequestModel)
    {
        var result = Handler.GetpreviousMonthReading(new GetReadingByDateModel()
        {
            habitation = Habitacao(Request),
            month = RequestModel.month,
            year = RequestModel.year
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetInvoices")]
    public ActionResult<dbResponse<Invoice>> GetInvoices()
    {
        var result = Handler.GetInvoices(new GetInvoicesModel()
        {
            habitation = 1
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetUnpaidTotal")]
    public ActionResult<dbResponse<decimal>> GetUnpaidTotal()
    {
        var result = Handler.GetUnpaidTotal(new GetUnpaidTotalModel()
        {
            habitation = 1
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetHolderDetails")]
    public ActionResult<dbResponse<Holder>> GetHolderDetails()
    {
        var result = Handler.GetHolderDetails(new GetHolderDetailsModel()
        {
            habitation = 1
        });

        return ReturnResult(result);
    }

    [HttpGet(Name = "GetHabitationDetails")]
    public ActionResult<dbResponse<Habitation>> GetHabitationDetails()
    {
        //get habitation id from header
        var result = Handler.GetHabitationDetails(new GetHabitationDetailsModel()
        {
            habitation = 1
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHabitationPower")]
    public ActionResult<dbResponse<string>> UpdateHabitationPower([FromQuery] UpdateHabitationPowerRequestModel RequestModel)
    {
        var result = Handler.UpdateHabitationPower(new UpdateHabitationPowerModel()
        {
            habitation = Habitacao(Request),
            power = RequestModel.power
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHolderName")]
    public ActionResult<dbResponse<string>> UpdateHolderName([FromQuery] UpdateHolderNameRequestModel RequestModel)
    {
        var result = Handler.UpdateHolderName(new UpdateHolderNameModel()
        {
            habitation = Habitacao(Request),
            fullName = RequestModel.fullName
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHolderNif")]
    public ActionResult<dbResponse<string>> UpdateHolderNif([FromQuery] UpdateHolderNifRequestModel RequestModel)
    {
        var result = Handler.UpdateHolderNif(new UpdateHolderNifModel()
        {
            habitation = Habitacao(Request),
            nif = RequestModel.nif
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHolderContact")]
    public ActionResult<dbResponse<string>> UpdateHolderContact([FromQuery] UpdateHolderContactRequestModel RequestModel)
    {
        var result = Handler.UpdateHolderContact(new UpdateHolderContactModel()
        {
            habitation = Habitacao(Request),
            contact = RequestModel.contact
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHabitationTensionLevel")]
    public ActionResult<dbResponse<string>> UpdateHabitationTensionLevel([FromQuery] UpdateHabitationTensionLevelRequestModel RequestModel)
    {
        var result = Handler.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelModel()
        {
            habitation = Habitacao(Request),
            tensionLevel = RequestModel.tensionLevel
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHabitationSchedule")]
    public ActionResult<dbResponse<string>> UpdateHabitationSchedule([FromQuery] UpdateHabitationScheduleRequestModel RequestModel)
    {
        var result = Handler.UpdateHabitationSchedule(new UpdateHabitationScheduleModel()
        {
            habitation = Habitacao(Request),
            schedule = RequestModel.schedule
        });

        return ReturnResult(result);
    }

    [HttpPatch(Name = "UpdateHabitationPhase")]
    public ActionResult<dbResponse<string>> UpdateHabitationPhase([FromQuery] UpdateHabitationPhaseRequestModel RequestModel)
    {
        var result = Handler.UpdateHabitationPhase(new UpdateHabitationPhaseModel()
        {
            habitation = Habitacao(Request),
            phase = RequestModel.phase
        });

        return ReturnResult(result);
    }

}