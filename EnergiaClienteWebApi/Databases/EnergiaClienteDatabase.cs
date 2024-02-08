using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Databases.Interfaces;

namespace EnergiaClienteWebApi.Databases;

public class EnergiaClienteDatabase : IEnergiaClienteDatabase
{
    public IDatabaseFunctions functions { get; set; }

    public EnergiaClienteDatabase(IDatabaseFunctions _functions)
    {
        functions = _functions;
    }

    private CostKwh costKwh => new CostKwh(0.24m, 0.1741m, 0.1072m);

    public CostKwh GetCostKwh()
    {
        return costKwh;
    }

    public dbResponse<Invoice> GetInvoices(GetInvoicesRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = functions.RunSelectProcedure("UltimasFaturas", new List<SqlParameter>() { param });

        if (response.Count == 0)
            return new dbResponse<Invoice>() { Status = new StatusObject(404) };

        var invoices = new List<Invoice>();
        foreach (DataRow row in response)
        {
            var invoice = new Invoice
            {
                Number = functions.GetParam<string>(row["numero"]),
                StartDate = functions.GetParam<DateTime>(row["dataInicio"]),
                EndDate = functions.GetParam<DateTime>(row["dataFim"]),
                Paid = functions.GetParam<bool>(row["pago"]),
                Value = functions.GetParam<decimal>(row["valor"]),
                LimitDate = functions.GetParam<DateTime>(row["dataLimite"]),
                HabitationId = functions.GetParam<int>(row["idHabitacao"])
            };
            var documentString = row["documento"].ToString();
            invoice.Document = Encoding.ASCII.GetBytes(documentString != null ? documentString : "");
            invoices.Add(invoice);
        }

        return new dbResponse<Invoice>(invoices);
    }

    public dbResponse<Reading> GetReadings(GetReadingsRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>() {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("quantidade", requestModel.quantity)
            };

        var response = functions.RunSelectProcedure("UltimasLeituras", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject(404) };

        var readings = new List<Reading>();
        foreach (DataRow row in response)
        {
            var reading = new Reading
            {
                Id = functions.GetParam<int>(row["id"]),
                Vazio = functions.GetParam<int>(row["vazio"]),
                Ponta = functions.GetParam<int>(row["ponta"]),
                Cheias = functions.GetParam<int>(row["cheias"]),
                Month = functions.GetParam<int>(row["mes"]),
                Year = functions.GetParam<int>(row["ano"]),
                ReadingDate = functions.GetParam<DateTime>(row["dataLeitura"]),
                HabitationId = functions.GetParam<int>(row["idHabitacao"]),
                Estimated = functions.GetParam<bool>(row["estimada"])
            };

            readings.Add(reading);
        }

        return new dbResponse<Reading>(readings);
    }

    public dbResponse<Reading> GetRealReadings(GetReadingsRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("quantidade", requestModel.quantity)
            };

        var response = functions.RunSelectProcedure("UltimasLeiturasReais", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject(404) };

        var readings = new List<Reading>();
        foreach (DataRow row in response)
        {
            var reading = new Reading
            {
                Id = functions.GetParam<int>(row["id"]),
                Vazio = functions.GetParam<int>(row["vazio"]),
                Ponta = functions.GetParam<int>(row["ponta"]),
                Cheias = functions.GetParam<int>(row["cheias"]),
                Month = functions.GetParam<int>(row["mes"]),
                Year = functions.GetParam<int>(row["ano"]),
                ReadingDate = functions.GetParam<DateTime>(row["dataLeitura"]),
                HabitationId = functions.GetParam<int>(row["idHabitacao"]),
                Estimated = functions.GetParam<bool>(row["estimada"])
            };

            readings.Add(reading);
        }

        return new dbResponse<Reading>(readings);
    }

    public dbResponse<Reading> GetReadingByDate(GetReadingByDateRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>() {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("mes", requestModel.month),
                new SqlParameter("ano", requestModel.year)
            };

        var response = functions.RunSelectProcedure("ReceberLeitura", parameters);

        if (response.Count == 0)
            return new dbResponse<Reading> { Status = new StatusObject(404) };

        DataRow row = response[0];

        var reading = new Reading
        {
            Id = functions.GetParam<int>(row["id"]),
            Vazio = functions.GetParam<int>(row["vazio"]),
            Ponta = functions.GetParam<int>(row["ponta"]),
            Cheias = functions.GetParam<int>(row["cheias"]),
            Month = functions.GetParam<int>(row["mes"]),
            Year = functions.GetParam<int>(row["ano"]),
            ReadingDate = functions.GetParam<DateTime>(row["dataLeitura"]),
            HabitationId = functions.GetParam<int>(row["idHabitacao"]),
            Estimated = functions.GetParam<bool>(row["estimada"])
        };

        return new dbResponse<Reading>(reading);
    }

    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = functions.RunSelectProcedure("TotalPorPagar", new List<SqlParameter>() { param });
        if (response.Count == 0)
            return new dbResponse<decimal> { Status = new StatusObject(404) };

        DataRow row = response[0];

        decimal value = 0;

        value = functions.GetParam<decimal>(row["Total"]);

        return new dbResponse<decimal>(value);
    }

    public dbResponse<string> InsertReading(InsertReadingRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.HabitationId),
                new SqlParameter("estimada", requestModel.Estimated),
                new SqlParameter("vazio", requestModel.Vazio),
                new SqlParameter("ponta", requestModel.Ponta),
                new SqlParameter("cheias", requestModel.Cheias),
                new SqlParameter("mes", requestModel.Month),
                new SqlParameter("ano", requestModel.Year),
                new SqlParameter("dataLeitura", requestModel.ReadingDate),
            };

        var response = functions.RunInsertProcedure("AdicionarLeitura", parameters);

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

    public dbResponse<string> InsertInvoice(InsertInvoiceRequestModel requestModel)
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

        var parameterValue = new SqlParameter("valor", SqlDbType.Decimal)
        {
            Value = requestModel.value,
            Precision = 8,//this didnt work try something else...
            Scale = 4
        };//decimal is being stored as int - FIX THIS!
        parameters.Add(parameterValue);

        var response = functions.RunInsertProcedure("Faturacao", parameters);

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

    public dbResponse<User> GetUserDetails(GetUserDetailsRequestModel requestModel)
    {
        var param = new SqlParameter("email", requestModel.email);

        var response = functions.RunSelectProcedure("DetalhesUtilizador", new List<SqlParameter>() { param });

        if (response.Count == 0)
            return new dbResponse<User> { Status = new StatusObject(404) };

        DataRow row = response[0];

        User user = new User()
        {
            email = functions.GetParam<string>(row["email"]),
            contact = functions.GetParam<string>(row["contacto"]),
            fullName = functions.GetParam<string>(row["nomeCompleto"]),
            gender = functions.GetParam<bool>(row["genero"]),
            nif = functions.GetParam<string>(row["nif"])
        };

        return new dbResponse<User>(user);
    }

    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = functions.RunSelectProcedure("DetalhesUtilizador", new List<SqlParameter>() { param });

        if (response.Count == 0)
            return new dbResponse<Holder> { Status = new StatusObject(404) };

        DataRow row = response[0];

        Holder holder = new Holder()
        {
            HabitationId = functions.GetParam<int>(row["idHabitacao"]),
            contact = functions.GetParam<string>(row["contacto"]),
            fullName = functions.GetParam<string>(row["nomeCompleto"]),
            nif = functions.GetParam<string>(row["nif"])
        };

        return new dbResponse<Holder>(holder);
    }

    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsRequestModel requestModel)
    {
        var param = new SqlParameter("habitacao", requestModel.habitation);

        var response = functions.RunSelectProcedure("DetalhesHabitacao", new List<SqlParameter>() { param });

        if (response.Count == 0)
            return new dbResponse<Habitation> { Status = new StatusObject(404) };

        DataRow row = response[0];

        Habitation habitation = new Habitation()
        {
            userEmail = functions.GetParam<string>(row["userEmail"]),
            power = functions.GetParam<decimal>(row["power"]),
            phase = functions.GetParam<string>(row["phase"]),
            tensionLevel = functions.GetParam<string>(row["tensionLevel"]),
            schedule = functions.GetParam<string>(row["schedule"])
        };

        habitation.costKwh = GetCostKwh();

        return new dbResponse<Habitation>(habitation);
    }

    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("potencia", requestModel.power),
            };

        var response = functions.RunInsertProcedure("AlterarPotenciaHabitacao", parameters);

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

    public dbResponse<string> UpdateHolderName(UpdateHolderNameRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("nomeCompleto", requestModel.fullName),
            };

        var response = functions.RunInsertProcedure("AlterarNomeTitular", parameters);

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

    public dbResponse<string> UpdateHolderNif(UpdateHolderNifRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("nif", requestModel.nif),
            };

        var response = functions.RunInsertProcedure("AlterarNifTitular", parameters);

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

    public dbResponse<string> UpdateHolderContact(UpdateHolderContactRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("contacto", requestModel.contact),
            };

        var response = functions.RunInsertProcedure("AlterarContactoTitular", parameters);

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

    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("nivelTensao", requestModel.tensionLevel),
            };

        var response = functions.RunInsertProcedure("AlterarNivelHabitacao", parameters);

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

    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("horario", requestModel.schedule),
            };

        var response = functions.RunInsertProcedure("AlterarHorarioHabitacao", parameters);

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

    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseRequestModel requestModel)
    {
        var parameters = new List<SqlParameter>()
            {
                new SqlParameter("habitacao", requestModel.habitation),
                new SqlParameter("fase", requestModel.phase),
            };

        var response = functions.RunInsertProcedure("AlterarFaseHabitacao", parameters);

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

    public dbResponse<int> GetHabitationIds()
    {
        var response = functions.RunSelectProcedure("ListaIdHabitacao");

        if (response.Count == 0)
            return new dbResponse<int> { Status = new StatusObject(404) };

        var ids = new List<int>();
        foreach (DataRow row in response)
        {
            ids.Add(functions.GetParam<int>(row["id"]));
        }

        return new dbResponse<int>(ids);
    }
}