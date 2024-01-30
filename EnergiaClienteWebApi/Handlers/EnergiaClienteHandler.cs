using System.Text;
using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Handlers;

public static class EnergiaClienteHandler
{
    public static dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        return EnergiaClienteDatabase.GetReadings(requestModel);
    }

    public static dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel)
    {
        return EnergiaClienteDatabase.GetReadingByDate(requestModel);
    }

    public static dbResponse<decimal> UploadNewReading(Reading reading)
    {
        EnergiaClienteDatabase.InsertReading(reading);

        Reading oldReading = GetpreviousMonthReading(reading.HabitationId, reading.Month, reading.Year);

        return new dbResponse<decimal>(CalculateAmount(reading, oldReading));
    }

    public static Reading GetpreviousMonthReading(int habitation, int month, int year)
    {
        var Readingresult = EnergiaClienteDatabase.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = habitation,
            month = month > 1 ? month - 1 : 12,
            year = month > 1 ? year : year - 1
        });

        Reading reading;

        if (Readingresult.Result == null)
            reading = new Reading();
        else
            reading = Readingresult.Result[0];

        return reading;
    }

    private static Reading GenerateEstimatedReading(int habitation, int billingMonth, int billingYear, Reading lastMonth)
    {
        var realReadingsResult = EnergiaClienteDatabase.GetRealReadings(new GetReadingsRequestModel()
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

        Reading estimated = new()
        {
            Ponta = (int)Math.Round(pontaValues.Average(), 0) + lastMonth.Ponta,
            Cheias = (int)Math.Round(cheiasValues.Average(), 0) + lastMonth.Cheias,
            Vazio = (int)Math.Round(vazioValues.Average(), 0) + lastMonth.Vazio,
            HabitationId = habitation,
            Month = billingMonth,
            Year = billingYear,
            Estimated = true,
            ReadingDate = new DateTime()
        };

        EnergiaClienteDatabase.InsertReading(estimated);

        return estimated;
    }

    private static decimal CalculateAmount(Reading reading, Reading oldReading)
    {
        var cost = EnergiaClienteDatabase.GetCostKwh();//get Cost Kwh for this habitation

        decimal amountponta = (reading.Ponta - oldReading.Ponta) * cost.costkwhPonta;
        decimal amountcheias = (reading.Cheias - oldReading.Cheias) * cost.costkwhCheias;
        decimal amountvazio = (reading.Vazio - oldReading.Vazio) * cost.costkwhVazio;

        decimal amount = amountponta + amountcheias + amountvazio;

        return amount;
    }

    private static decimal CalculateBillingAmount(int habitation, int billingMonth, int billingYear)
    {
        var readingresult = EnergiaClienteDatabase.GetReadingByDate(new GetReadingByDateRequestModel()
        {
            habitation = habitation,
            month = billingMonth,
            year = billingYear
        });

        Reading oldReading = GetpreviousMonthReading(habitation, billingMonth, billingYear);
        Reading reading;

        if (readingresult.Result == null)
            reading = GenerateEstimatedReading(habitation, billingMonth, billingYear, oldReading);
        else
            reading = readingresult.Result[0];

        return CalculateAmount(reading, oldReading);
    }

    public static void Billing(int habitationId, int billingMonth, int billingYear)
    {
        var startDate = new DateTime(billingYear, billingMonth, 9);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var limitDate = startDate.AddMonths(2).AddDays(3);

        var amount = CalculateBillingAmount(habitationId, billingMonth, billingYear);

        byte[] document = Encoding.ASCII.GetBytes(""); // generate invoice pdf document

        var request = new BillingRequestModel()
        {
            number = "FAT-" + habitationId + "DT" + billingYear + "-" + billingMonth,
            habitation = habitationId,
            startDate = startDate,
            endDate = endDate,
            limitDate = limitDate,
            value = amount,
            document = document
        };

        EnergiaClienteDatabase.Billing(request);
    }

}