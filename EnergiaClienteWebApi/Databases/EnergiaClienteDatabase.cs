namespace EnergiaClienteWebApi.Databases;

using System.Data;
using System.Text;
using EnergiaClienteWebApi.RequestModels;
using Microsoft.Data.SqlClient;


public record Reading
{
    public int Id { get; set; }
    public int Vazio { get; set; }
    public int Ponta { get; set; }
    public int Cheias { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime ReadingDate { get; set; }
    public int HabitationId { get; set; }
    public bool Estimated { get; set; }
}

public record Habitation
{
    public decimal CostPontaKwh { get; set; }
    public decimal CostCheiasKwh { get; set; }
    public decimal CostVazioKwh { get; set; }

}

public record Invoice
{
    public string? Number { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Paid { get; set; }
    public decimal Value { get; set; }
    public DateTime LimitDate { get; set; }
    public int HabitationId { get; set; }
    public byte[]? Document { get; set; }
}

public record CostKwh(decimal costkwhPonta, decimal costkwhCheias, decimal costkwhVazio);

public class dbResponse<T>
{
    public dbResponse()
    {
        this.Status = new StatusObject(false);
    }
    public dbResponse(T value)
    {
        this.Result = new List<T>() { value };
        this.Status = new StatusObject(false);
    }
    public dbResponse(List<T> values)
    {
        this.Result = values;
        this.Status = new StatusObject(false);
    }
    public StatusObject Status { get; set; }
    public List<T>? Result { get; set; }
}

public class StatusObject
{
    public StatusObject() { }
    public StatusObject(bool _error)
    {
        this.Error = _error;
        this.ErrorMessage = "";
        if (_error == false)
            this.StatusCode = 200;
    }
    public bool Error { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public class EnergiaClienteDatabase
{
    private static SqlConnection connection = new SqlConnection("Data Source=192.168.1.8,1433;Initial Catalog=EnergiaClienteDados;User ID=sa;Password=1Secure*Password1;TrustServerCertificate=True");

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

    private static DataRowCollection RunSelectProcedure(string procedure, List<SqlParameter> parameters)
    {
        var dataAdapter = new SqlDataAdapter(procedure, connection);
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        dataAdapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet.Tables[0].Rows;
    }
    private static bool RunInsertProcedure(string procedure, List<SqlParameter> parameters)
    {
        var command = new SqlCommand(procedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddRange(parameters.ToArray());

        try
        {
            connection.Open();
            return command.ExecuteNonQuery() > 0 ? true : false;
        }
        catch (Exception ex)
        {
            var x = ex;
            return false;
        }
        finally
        {
            connection.Close();
        }
    }
    private static T? GetParam<T>(object value)
    {
        object x = value != null ? value : "";

        string? param = x.ToString();

        T? result;

        try
        {
            result = (T?)Convert.ChangeType(param, typeof(T));
        }
        catch
        {
            result = default(T);
        }

        return result != null ? result : default(T);
    }

}