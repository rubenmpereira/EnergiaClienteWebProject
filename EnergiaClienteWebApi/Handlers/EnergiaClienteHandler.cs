using System.Text;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Handlers.Interfaces;
using EnergiaClienteWebApi.Models.User;

namespace EnergiaClienteWebApi.Handlers;

public class EnergiaClienteHandler : IEnergiaClienteHandler
{
    private IEnergiaClienteDatabase Database { get; set; }

    public EnergiaClienteHandler(IEnergiaClienteDatabase _database)
    {
        Database = _database;
    }

    public dbResponse<Reading> GetReadings(GetReadingsModel model)
    {
        return Database.GetReadings(model);
    }

    public dbResponse<Reading> GetReadingByDate(GetReadingByDateModel model)
    {
        return Database.GetReadingByDate(model);
    }

    public dbResponse<decimal> UploadNewReading(InsertReadingModel model)
    {
        var response = Database.InsertReading(model);

        if (response.Status.Error)
            return new dbResponse<decimal>() { Status = response.Status };

        var oldReadingResult = GetpreviousMonthReading(new GetReadingByDateModel()
        {
            habitation = model.HabitationId,
            month = model.Month,
            year = model.Year
        });

        var reading = new Reading()
        {
            Ponta = model.Ponta,
            Cheias = model.Cheias,
            Vazio = model.Vazio
        };

        Reading oldReading = new Reading();

        if (oldReadingResult.Result.Count > 0)
            oldReading = oldReadingResult.Result[0];

        return new dbResponse<decimal>(CalculateAmount(reading, oldReading));
    }

    public dbResponse<Reading> GetpreviousMonthReading(GetReadingByDateModel model)
    {
        return Database.GetReadingByDate(new GetReadingByDateModel()
        {
            habitation = model.habitation,
            month = model.month > 1 ? model.month - 1 : 12,
            year = model.month > 1 ? model.year : model.year - 1
        });
    }

    public dbResponse<string> Billing(BillingModel model)
    {
        var startDate = new DateTime(model.billingYear, model.billingMonth, 9);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var limitDate = startDate.AddMonths(2).AddDays(3);

        var amount = CalculateBillingAmount(model.habitationId, model.billingMonth, model.billingYear);

        byte[] document = Encoding.ASCII.GetBytes(""); // generate invoice pdf document

        var request = new InsertInvoiceModel()
        {
            number = "FAT-" + model.habitationId + "DT" + model.billingYear + "-" + model.billingMonth,
            habitation = model.habitationId,
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

    public dbResponse<Invoice> GetInvoices(GetInvoicesModel model)
    {
        return Database.GetInvoices(model);
    }

    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalModel model)
    {
        return Database.GetUnpaidTotal(model);
    }

    public dbResponse<User> GetUserDetails(GetUserDetailsModel model)
    {
        return Database.GetUserDetails(model);
    }

    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsModel model)
    {
        return Database.GetHolderDetails(model);
    }

    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsModel model)
    {
        return Database.GetHabitationDetails(model);
    }

    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerModel model)
    {
        return Database.UpdateHabitationPower(model);
    }

    public dbResponse<string> UpdateHolderName(UpdateHolderNameModel model)
    {
        return Database.UpdateHolderName(model);
    }

    public dbResponse<string> UpdateHolderNif(UpdateHolderNifModel model)
    {
        return Database.UpdateHolderNif(model);
    }

    public dbResponse<string> UpdateHolderContact(UpdateHolderContactModel model)
    {
        return Database.UpdateHolderContact(model);
    }

    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelModel model)
    {
        return Database.UpdateHabitationTensionLevel(model);
    }

    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleModel model)
    {
        return Database.UpdateHabitationSchedule(model);
    }

    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseModel model)
    {
        return Database.UpdateHabitationPhase(model);
    }

    private Reading GenerateEstimatedReading(int habitation, int billingMonth, int billingYear, Reading lastMonth)
    {
        var realReadingsResult = Database.GetRealReadings(new GetReadingsModel()
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
        var readingResult = Database.GetReadingByDate(new GetReadingByDateModel()
        {
            habitation = habitation,
            month = billingMonth,
            year = billingYear
        });

        var oldReadingResult = GetpreviousMonthReading(new GetReadingByDateModel()
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

    public dbResponse<bool> AutherizeHabitation(AutherizeHabitationModel model)
    {
        return Database.AutherizeHabitation(model);
    }

    public dbResponse<bool> AuthenticateUser(AuthenticateUserModel model)
    {
        return Database.AuthenticateUser(model);
    }
}