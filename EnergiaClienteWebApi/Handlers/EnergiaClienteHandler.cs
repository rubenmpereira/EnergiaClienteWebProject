using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Handlers;

public static class EnergiaClienteHandler
{
    public static dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        return EnergiaClienteDatabase.GetReadings(requestModel);
    }

    public static dbResponse<decimal> CalculateAmountPay(CalculateInvoiceRequestModel requestModel)
    {
        var readings = EnergiaClienteDatabase.GetReadings(new GetReadingsRequestModel() { habitation = requestModel.habitation, quantity = 1 });

        if (readings.Result == null) return new dbResponse<decimal>() { Status = readings.Status };

        var last = readings.Result[0];

        var cost = EnergiaClienteDatabase.GetCostKwh();

        decimal amountponta = (requestModel.ponta - last.Ponta) * cost.costkwhPonta;
        decimal amountcheias = (requestModel.cheias - last.Cheias) * cost.costkwhCheias;
        decimal amountvazio = (requestModel.vazio - last.Vazio) * cost.costkwhVazio;

        decimal amount = amountponta + amountcheias + amountvazio;

        return new dbResponse<decimal>() { Result = new List<decimal> { amount } };
    }

}