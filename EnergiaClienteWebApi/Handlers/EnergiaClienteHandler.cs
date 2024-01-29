using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Handlers;

public static class EnergiaClienteHandler
{
    public static dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        return EnergiaClienteDatabase.GetReadings(requestModel);
    }

    public static dbResponse<decimal> CalculateAmountPay(int habitation, Reading reading)
    {
        var readings = EnergiaClienteDatabase.GetReadings(new GetReadingsRequestModel() { habitation = habitation, quantity = 1 });

        if (readings.Result == null) return new dbResponse<decimal>() { Status = readings.Status };

        var last = readings.Result[0];

        var cost = EnergiaClienteDatabase.GetCostKwh();

        decimal amount = (reading.Ponta - last.Ponta) * cost.costkwhPonta + (reading.Cheias - last.Cheias) * cost.costkwhCheias + (reading.Vazio - last.Vazio) * cost.costkwhVazio;

        return new dbResponse<decimal>() { Result = new List<decimal> { amount } };
    }

}