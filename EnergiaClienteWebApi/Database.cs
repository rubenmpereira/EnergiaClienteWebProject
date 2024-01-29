namespace EnergiaClienteWebApi.Database;

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

public class dbResponse<T>
{
    public dbResponse()
    {
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
        this.StatusCode=200;
    }
    public bool Error { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public class EnergiaCliente
{
    //private static SqlConnection connection = new SqlConnection("Data Source=MSIRUBEN\\SQLEXPRESS;Initial Catalog=EnergiaClienteDados;Integrated Security=True;TrustServerCertificate=True");

    private static SqlConnection connection = new SqlConnection("Data Source=192.168.1.8,1433;Initial Catalog=EnergiaClienteDados;User ID=sa;Password=1Secure*Password1;TrustServerCertificate=True");
    //get from db
    private static decimal costkwhPonta => 0.24m;
    private static decimal costkwhCheias => 0.1741m;
    private static decimal costkwhVazio => 0.1072m;

    public static dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel)
    {
        //set parameters
        var param = new SqlParameter("habitacao", requestModel.habitation);

        //execute stored procedure
        var response = RunSelectProcedure("UltimasFaturas", new SqlParameter[1] { param });

        if (response.Count == 0)
            return new dbResponse<Invoice> { Status = new StatusObject() { Error = true, ErrorMessage = "Not found", StatusCode = 404 } };

        //mapping
        var invoices = new List<Invoice>();
        foreach (DataRow row in response)
        {
            var invoice = new Invoice();
            invoice.Number = GetParam<string>(row["numero"]);
            invoice.StartDate = GetParam<DateTime>(row["dataInicio"]);
            invoice.EndDate = GetParam<DateTime>(row["dataFim"]);
            invoice.Paid = GetParam<bool>(row["pago"]);
            invoice.Value = GetParam<decimal>(row["valor"]);
            invoice.LimitDate = GetParam<DateTime>(row["dataLimite"]);
            invoice.HabitationId = GetParam<int>(row["idHabitacao"]);
            var documentString = row["documento"].ToString();
            invoice.Document = Encoding.ASCII.GetBytes(documentString != null ? documentString : "");
            invoices.Add(invoice);
        }

        return new dbResponse<Invoice>
        {
            Result = invoices,
            Status = new StatusObject(false)
        };
    }

    public static dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        //set parameters
        var parameters = new SqlParameter[2] {
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
            var reading = new Reading();
            reading.Id = GetParam<int>(row["id"]);
            reading.Vazio = GetParam<int>(row["vazio"]);
            reading.Ponta = GetParam<int>(row["ponta"]);
            reading.Cheias = GetParam<int>(row["cheias"]);
            reading.Month = GetParam<int>(row["mes"]);
            reading.Year = GetParam<int>(row["ano"]);
            reading.ReadingDate = GetParam<DateTime>(row["dataLeitura"]);
            reading.HabitationId = GetParam<int>(row["idHabitacao"]);
            reading.Estimated = GetParam<bool>(row["estimada"]);

            readings.Add(reading);
        }

        return new dbResponse<Reading>
        {
            Result = readings,
            Status = new StatusObject(false)
        };
    }

    public static dbResponse<decimal> CalculateAmountPay(int habitation, Reading reading)
    {
        var readings = GetReadings(new GetReadingsRequestModel() { habitation = habitation, quantity = 1 });

        if (readings.Result == null) return new dbResponse<decimal>() { Status = readings.Status };

        var last = readings.Result[0];

        decimal amount = (reading.Ponta - last.Ponta) * costkwhPonta + (reading.Cheias - last.Cheias) * costkwhCheias + (reading.Vazio - last.Vazio) * costkwhVazio;

        return new dbResponse<decimal>() { Result = new List<decimal> { amount } };
    }

    private static DataRowCollection RunSelectProcedure(string procedure, SqlParameter[] parameters)
    {
        var dataAdapter = new SqlDataAdapter(procedure, connection);
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        dataAdapter.SelectCommand.Parameters.AddRange(parameters);
        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet.Tables[0].Rows;
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