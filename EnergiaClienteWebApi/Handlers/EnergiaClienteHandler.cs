using System.Text;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Handlers.Interfaces;
using EnergiaClienteWebApi.RequestModels.User;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Models.User;

namespace EnergiaClienteWebApi.Handlers;

public class EnergiaClienteHandler : IEnergiaClienteHandler
{
    private IEnergiaClienteDatabase Database { get; set; }

    public int habitation { get; set; }

    public EnergiaClienteHandler(IEnergiaClienteDatabase _database)
    {
        Database = _database;
    }

    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        return Database.GetReadings(new()
        {
            habitation = habitation,
            quantity = requestModel.quantity
        });
    }

    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel)
    {
        return Database.GetReadingByDate(new()
        {
            habitation = habitation,
            month = requestModel.month,
            year = requestModel.year
        });
    }

    public dbResponse<decimal> UploadNewReading(UploadNewReadingRequestModel requestModel)
    {
        var today = DateTime.Now;
        int month = today.AddMonths(-1).Month;
        int year = today.AddMonths(-1).Year;

        if (today.Day < 5 || today.Day > 7)
            return new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You can not upload a new reading in this date",
                    StatusCode = 400
                }
            };

        var readingResult = Database.GetReadingByDate(new GetReadingByDateModel()
        {
            habitation = habitation,
            month = month,
            year = year
        });

        if (readingResult.Result.Count > 0)
            return new dbResponse<decimal>()
            {
                Status = new StatusObject()
                {
                    Error = true,
                    ErrorMessage = "You already uploaded your reading this month",
                    StatusCode = 400
                }
            };

        var response = Database.InsertReading(new InsertReadingModel()
        {
            Cheias = requestModel.cheias,
            Ponta = requestModel.ponta,
            Vazio = requestModel.vazio,
            Estimated = false,
            HabitationId = habitation,
            Month = month,
            Year = year,
            ReadingDate = today
        });

        if (response.Status.Error)
            return new dbResponse<decimal>() { Status = response.Status };

        var oldReadingResult = GetpreviousMonthReading(new()
        {
            month = month,
            year = year
        });

        var reading = new Reading()
        {
            Ponta = requestModel.ponta,
            Cheias = requestModel.cheias,
            Vazio = requestModel.vazio
        };

        Reading oldReading = new Reading();

        if (oldReadingResult.Result.Count > 0)
            oldReading = oldReadingResult.Result[0];

        return new dbResponse<decimal>(CalculateAmount(reading, oldReading));
    }

    public dbResponse<Reading> GetpreviousMonthReading(GetReadingByDateRequestModel requestModel)
    {
        return Database.GetReadingByDate(new()
        {
            habitation = habitation,
            month = requestModel.month > 1 ? requestModel.month - 1 : 12,
            year = requestModel.month > 1 ? requestModel.year : requestModel.year - 1
        });
    }

    public dbResponse<string> Billing(BillingRequestModel requestModel)
    {
        var startDate = new DateTime(requestModel.billingYear, requestModel.billingMonth, 9);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var limitDate = startDate.AddMonths(2).AddDays(3);

        var amount = CalculateBillingAmount(habitation, requestModel.billingMonth, requestModel.billingYear);

        byte[] document = Encoding.ASCII.GetBytes(""); // generate invoice pdf document

        return Database.InsertInvoice(new()
        {
            number = "FAT-" + habitation + "DT" + requestModel.billingYear + "-" + requestModel.billingMonth,
            habitation = habitation,
            startDate = startDate,
            endDate = endDate,
            limitDate = limitDate,
            value = amount,
            document = document
        });
    }

    public dbResponse<int> GetHabitationIds()
    {
        return Database.GetHabitationIds();
    }

    public dbResponse<Invoice> GetInvoices()
    {
        return Database.GetInvoices(new()
        {
            habitation = habitation
        });
    }

    public dbResponse<decimal> GetUnpaidTotal()
    {
        return Database.GetUnpaidTotal(new()
        {
            habitation = habitation
        });
    }

    public dbResponse<User> GetUserDetails(GetUserDetailsRequestModel requestModel)
    {
        return Database.GetUserDetails(new()
        {
            email = requestModel.email
        });
    }

    public dbResponse<Holder> GetHolderDetails()
    {
        return Database.GetHolderDetails(new()
        {
            habitation = habitation
        });
    }

    public dbResponse<Habitation> GetHabitationDetails()
    {
        return Database.GetHabitationDetails(new()
        {
            habitation = habitation
        });
    }

    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerRequestModel requestModel)
    {
        return Database.UpdateHabitationPower(new()
        {
            habitation = habitation,
            power = requestModel.power
        });
    }

    public dbResponse<string> UpdateHolderName(UpdateHolderNameRequestModel requestModel)
    {
        return Database.UpdateHolderName(new()
        {
            habitation = habitation,
            fullName = requestModel.fullName
        });
    }

    public dbResponse<string> UpdateHolderNif(UpdateHolderNifRequestModel requestModel)
    {
        return Database.UpdateHolderNif(new()
        {
            habitation = habitation,
            nif = requestModel.nif
        });
    }

    public dbResponse<string> UpdateHolderContact(UpdateHolderContactRequestModel requestModel)
    {
        return Database.UpdateHolderContact(new()
        {
            habitation = habitation,
            contact = requestModel.contact
        });
    }

    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelRequestModel requestModel)
    {
        return Database.UpdateHabitationTensionLevel(new()
        {
            habitation = habitation,
            tensionLevel = requestModel.tensionLevel
        });
    }

    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleRequestModel requestModel)
    {
        return Database.UpdateHabitationSchedule(new()
        {
            habitation = habitation,
            schedule = requestModel.schedule
        });
    }

    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseRequestModel requestModel)
    {
        return Database.UpdateHabitationPhase(new()
        {
            habitation = habitation,
            phase = requestModel.phase
        });
    }

    private Reading GenerateEstimatedReading(int billingMonth, int billingYear, Reading lastMonth)
    {
        var realReadingsResult = Database.GetRealReadings(new()
        {
            habitation = habitation,
            quantity = 6
        });

        var readings = realReadingsResult.Result ?? new();

        decimal[] pontaValues = new decimal[5];
        decimal[] cheiasValues = new decimal[5];
        decimal[] vazioValues = new decimal[5];

        for (int i = 0; i < readings.Count - 1; i++)
        {
            var monthDif = ((readings[i].Year - readings[i + 1].Year) * 12) + readings[i].Month - readings[i + 1].Month;

            pontaValues[i] = (readings[i].Ponta - readings[i + 1].Ponta) / monthDif;
            var cheiasDif = readings[i].Cheias - readings[i + 1].Cheias;
            var vazioDif = readings[i].Vazio - readings[i + 1].Vazio;

            cheiasValues[i] = cheiasDif / monthDif;
            vazioValues[i] = vazioDif / monthDif;
        }

        InsertReadingModel estimated = new()
        {
            Ponta = (int)Math.Round(pontaValues.Average(), 0) + lastMonth.Ponta,
            Cheias = (int)Math.Round(cheiasValues.Average(), 0) + lastMonth.Cheias,
            Vazio = (int)Math.Round(vazioValues.Average(), 0) + lastMonth.Vazio,
            HabitationId = habitation,
            Month = billingMonth,
            Year = billingYear,
            Estimated = true,
            ReadingDate = DateTime.Now
        };

        Database.InsertReading(estimated);

        var reading = new Reading()
        {
            Ponta = estimated.Ponta,
            Cheias = estimated.Cheias,
            Vazio = estimated.Vazio
        };

        return reading;
    }

    private decimal CalculateAmount(Reading reading, Reading oldReading)
    {
        var cost = Database.GetCostKwh();

        decimal amountponta = (reading.Ponta - oldReading.Ponta) * cost.costkwhPonta;
        decimal amountcheias = (reading.Cheias - oldReading.Cheias) * cost.costkwhCheias;
        decimal amountvazio = (reading.Vazio - oldReading.Vazio) * cost.costkwhVazio;

        decimal amount = amountponta + amountcheias + amountvazio;

        return amount;
    }

    private decimal CalculateBillingAmount(int habitation, int billingMonth, int billingYear)
    {
        var readingResult = Database.GetReadingByDate(new()
        {
            habitation = habitation,
            month = billingMonth,
            year = billingYear
        });

        var oldReadingResult = GetpreviousMonthReading(new()
        {
            month = billingMonth,
            year = billingYear
        });

        Reading oldReading = new Reading();

        if (oldReadingResult.Result.Count > 0)
            oldReading = oldReadingResult.Result[0];

        Reading reading;

        if (readingResult.Result.Count == 0)
            reading = GenerateEstimatedReading(billingMonth, billingYear, oldReading);
        else
            reading = readingResult.Result[0];

        return CalculateAmount(reading, oldReading);
    }

    public dbResponse<bool> AutherizeHabitation(AutherizeHabitationModel requestModel)
    {
        return Database.AutherizeHabitation(requestModel);
    }

    public dbResponse<bool> AuthenticateUser(AuthRequestModel requestModel)
    {
        return Database.AuthenticateUser(new AuthenticateUserModel()
        {
            email = requestModel.email,
            password = requestModel.password
        });
    }
}