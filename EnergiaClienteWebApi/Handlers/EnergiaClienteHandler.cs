using System.Text;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Handlers.Interfaces;

namespace EnergiaClienteWebApi.Handlers;

public class EnergiaClienteHandler : IEnergiaClienteHandler
{
    private IEnergiaClienteDatabase Database { get; set; }

    public EnergiaClienteHandler(IEnergiaClienteDatabase _database)
    {
        Database = _database;
    }

    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        return Database.GetReadings(requestModel);
    }

    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel)
    {
        return Database.GetReadingByDate(requestModel);
    }

    public dbResponse<decimal> UploadNewReading(InsertReadingRequestModel requestModel)
    {
        var response = Database.InsertReading(requestModel);

        if (response.Status.Error)
            return new dbResponse<decimal>() { Status = response.Status };

        var oldReadingResult = GetpreviousMonthReading(new GetReadingByDateRequestModel()
        {
            habitation = requestModel.HabitationId,
            month = requestModel.Month,
            year = requestModel.Year
        });

        var reading = new Reading()
        {
            Ponta = requestModel.Ponta,
            Cheias = requestModel.Cheias,
            Vazio = requestModel.Vazio
        };

        Reading oldReading = new Reading();

        if (oldReadingResult.Result.Count > 0)
            oldReading = oldReadingResult.Result[0];

        return new dbResponse<decimal>(CalculateAmount(reading, oldReading));
    }

    public dbResponse<Reading> GetpreviousMonthReading(GetReadingByDateRequestModel requestModel)
    {
        return Database.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = requestModel.habitation,
            month = requestModel.month > 1 ? requestModel.month - 1 : 12,
            year = requestModel.month > 1 ? requestModel.year : requestModel.year - 1
        });
    }

    public dbResponse<string> Billing(BillingRequestModel requestModel)
    {
        var startDate = new DateTime(requestModel.billingYear, requestModel.billingMonth, 9);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var limitDate = startDate.AddMonths(2).AddDays(3);

        var amount = CalculateBillingAmount(requestModel.habitationId, requestModel.billingMonth, requestModel.billingYear);

        byte[] document = Encoding.ASCII.GetBytes(""); // generate invoice pdf document

        var request = new InsertInvoiceRequestModel()
        {
            number = "FAT-" + requestModel.habitationId + "DT" + requestModel.billingYear + "-" + requestModel.billingMonth,
            habitation = requestModel.habitationId,
            startDate = startDate,
            endDate = endDate,
            limitDate = limitDate,
            value = amount,
            document = document
        };

        return Database.InsertInvoice(request);
    }

    public dbResponse<int> GetHabitationIds()
    {
        return Database.GetHabitationIds();
    }

    public dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel)
    {
        return Database.GetInvoices(requestModel);
    }

    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalRequestModel requestModel)
    {
        return Database.GetUnpaidTotal(requestModel);
    }

    public dbResponse<User> GetUserDetails(GetUserDetailsRequestModel requestModel)
    {
        return Database.GetUserDetails(requestModel);
    }

    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsRequestModel requestModel)
    {
        return Database.GetHolderDetails(requestModel);
    }

    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsRequestModel requestModel)
    {
        return Database.GetHabitationDetails(requestModel);
    }

    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerRequestModel requestModel)
    {
        return Database.UpdateHabitationPower(requestModel);
    }

    public dbResponse<string> UpdateHolderName(UpdateHolderNameRequestModel requestModel)
    {
        return Database.UpdateHolderName(requestModel);
    }

    public dbResponse<string> UpdateHolderNif(UpdateHolderNifRequestModel requestModel)
    {
        return Database.UpdateHolderNif(requestModel);
    }

    public dbResponse<string> UpdateHolderContact(UpdateHolderContactRequestModel requestModel)
    {
        return Database.UpdateHolderContact(requestModel);
    }

    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelRequestModel requestModel)
    {
        return Database.UpdateHabitationTensionLevel(requestModel);
    }

    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleRequestModel requestModel)
    {
        return Database.UpdateHabitationSchedule(requestModel);
    }

    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseRequestModel requestModel)
    {
        return Database.UpdateHabitationPhase(requestModel);
    }

    private Reading GenerateEstimatedReading(int habitation, int billingMonth, int billingYear, Reading lastMonth)
    {
        var realReadingsResult = Database.GetRealReadings(new GetReadingsRequestModel()
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

        InsertReadingRequestModel estimated = new()
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
        var readingResult = Database.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = habitation,
            month = billingMonth,
            year = billingYear
        });

        var oldReadingResult = GetpreviousMonthReading(new GetReadingByDateRequestModel()
        {
            habitation = habitation,
            month = billingMonth,
            year = billingYear
        });

        Reading oldReading = new Reading();

        if (oldReadingResult.Result.Count > 0)
            oldReading = oldReadingResult.Result[0];

        Reading reading;

        if (readingResult.Result.Count == 0)
            reading = GenerateEstimatedReading(habitation, billingMonth, billingYear, oldReading);
        else
            reading = readingResult.Result[0];

        return CalculateAmount(reading, oldReading);
    }

}