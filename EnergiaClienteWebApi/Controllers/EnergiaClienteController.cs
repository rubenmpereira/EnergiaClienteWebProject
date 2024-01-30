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

    [HttpPost(Name = "UploadNewReading")]
    public ActionResult<dbResponse<decimal>> UploadNewReading([FromQuery] UploadNewReadingRequestModel requestModel)
    {
        var today = new DateTime();
        int month = today.AddMonths(-1).Month;
        int year = today.AddMonths(-1).Year;

        if (today.Day < 5 || today.Day > 7) // Check if there is better way to do this
            return new BadRequestObjectResult(new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You can not upload a new reading in this date",
                    StatusCode = 400
                }
            });

        var readingResult = EnergiaClienteHandler.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = requestModel.habitation,
            month = month,
            year = year
        });

        if (readingResult != null) // test this if works!
            return new BadRequestObjectResult(new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You already uploaded your reading this month",
                    StatusCode = 400
                }
            });

        var result = EnergiaClienteHandler.UploadNewReading(new Reading()
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