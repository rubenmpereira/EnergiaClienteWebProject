
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;

namespace EnergiaClienteWebApi.Databases;

public class EnergiaClienteDatabase : DatabaseFunctions
{

    private static CostKwh costKwh => new CostKwh(0.24m, 0.1741m, 0.1072m); //get from db

    public static CostKwh GetCostKwh()
    {
        return costKwh;
    }

    public static dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = RunSelectProcedure("UltimasFaturas", new List<SqlParameter>() { param });

        if (response.Count == 0)
            return new dbResponse<Invoice> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        var invoices = new List<Invoice>();
        foreach (DataRow row in response)
        {
            var invoicesss = new Invoice();
            invoicesss.Paid = true;

            var invoice = new Invoice
            {
                Number = GetParam<string>(row["numero"]),
                StartDate = GetParam<DateTime>(row["dataInicio"]),
                EndDate = GetParam<DateTime>(row["dataFim"]),
                Paid = GetParam<bool>(row["pago"]),
                Value = GetParam<decimal>(row["valor"]),
                LimitDate = GetParam<DateTime>(row["dataLimite"]),
                HabitationId = GetParam<int>(row["idHabitacao"])
            };
            var documentString = row["documento"].ToString();
            invoice.Document = Encoding.ASCII.GetBytes(documentString != null ? documentString : "");
            invoices.Add(invoice);
        }

        return new dbResponse<Invoice>(invoices);
    }

    public static dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        //set parameters
        var parameters = new List<SqlParameter>() {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("quantidade", requestModel.quantity)
            };

        //execute stored procedure
        var response = RunSelectProcedure("UltimasLeituras", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        //mapping
        var readings = new List<Reading>();
        foreach (DataRow row in response)
        {
            var reading = new Reading
            {
                Id = GetParam<int>(row["id"]),
                Vazio = GetParam<int>(row["vazio"]),
                Ponta = GetParam<int>(row["ponta"]),
                Cheias = GetParam<int>(row["cheias"]),
                Month = GetParam<int>(row["mes"]),
                Year = GetParam<int>(row["ano"]),
                ReadingDate = GetParam<DateTime>(row["dataLeitura"]),
                HabitationId = GetParam<int>(row["idHabitacao"]),
                Estimated = GetParam<bool>(row["estimada"])
            };

            readings.Add(reading);
        }

        return new dbResponse<Reading>(readings);
    }

    public static dbResponse<Reading> GetRealReadings(GetReadingsRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("quantidade", requestModel.quantity)
            };

        var response = RunSelectProcedure("UltimasLeiturasReais", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        var readings = new List<Reading>();
        foreach (DataRow row in response)
        {
            var reading = new Reading
            {
                Id = GetParam<int>(row["id"]),
                Vazio = GetParam<int>(row["vazio"]),
                Ponta = GetParam<int>(row["ponta"]),
                Cheias = GetParam<int>(row["cheias"]),
                Month = GetParam<int>(row["mes"]),
                Year = GetParam<int>(row["ano"]),
                ReadingDate = GetParam<DateTime>(row["dataLeitura"]),
                HabitationId = GetParam<int>(row["idHabitacao"]),
                Estimated = GetParam<bool>(row["estimada"])
            };

            readings.Add(reading);
        }

        return new dbResponse<Reading>(readings);
    }

    public static dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>() {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("mes", requestModel.month),
                new SqlParameter("ano", requestModel.year)
            };

        var response = RunSelectProcedure("ReceberLeitura", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        DataRow row = response[0];

        var reading = new Reading
        {
            Id = GetParam<int>(row["id"]),
            Vazio = GetParam<int>(row["vazio"]),
            Ponta = GetParam<int>(row["ponta"]),
            Cheias = GetParam<int>(row["cheias"]),
            Month = GetParam<int>(row["mes"]),
            Year = GetParam<int>(row["ano"]),
            ReadingDate = GetParam<DateTime>(row["dataLeitura"]),
            HabitationId = GetParam<int>(row["idHabitacao"]),
            Estimated = GetParam<bool>(row["estimada"])
        };

        return new dbResponse<Reading>(reading);
    }

    public static dbResponse<decimal> GetUnpaidTotal(GetInvoicesRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = RunSelectProcedure("TotalPorPagar", new List<SqlParameter>() { param });
        if (response.Count == 0)
            return new dbResponse<decimal> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        DataRow row = response[0];

        decimal value = 0;

        value = GetParam<decimal>(row["Total"]);

        return new dbResponse<decimal>(value);
    }

    public static dbResponse<string> InsertReading(Reading model)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", model.HabitationId),
                new SqlParameter("estimada", model.Estimated),
                new SqlParameter("vazio", model.Vazio),
                new SqlParameter("ponta", model.Ponta),
                new SqlParameter("cheias", model.Cheias),
                new SqlParameter("mes", model.Month),
                new SqlParameter("ano", model.Year),
                new SqlParameter("dataLeitura", model.ReadingDate),
            };

        var response = RunInsertProcedure("AdicionarLeitura", parameters);

        var result = new dbResponse<string>();

        if (response == false)
            result.Status = new StatusObject()
            {
                Error = true,
                ErrorMessage = "SQL failed to run insert command",
                StatusCode = 500
            };
        return result;
    }

    public static dbResponse<string> Billing(BillingRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("numero", requestModel.number),
                new SqlParameter("pago", false),
                new SqlParameter("dataini", requestModel.startDate),
                new SqlParameter("datafim", requestModel.endDate),
                new SqlParameter("datalim", requestModel.limitDate),
                new SqlParameter("documento", requestModel.document)
            };

        var parameterValue = new SqlParameter("valor", SqlDbType.Decimal);//decimal is being stored as int - FIX THIS!
        parameterValue.Value = requestModel.value;
        parameterValue.Precision = 8;//this didnt work try something else...
        parameterValue.Scale = 4;
        parameters.Add(parameterValue);

        var response = RunInsertProcedure("Faturacao", parameters);

        var result = new dbResponse<string>();

        if (response == false)
            result.Status = new StatusObject()
            {
                Error = true,
                ErrorMessage = "SQL failed to run insert command",
                StatusCode = 500
            };
        return result;
    }

}