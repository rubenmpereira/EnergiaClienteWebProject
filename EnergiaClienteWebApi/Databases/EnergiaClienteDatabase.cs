using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using EnergiaClienteWebApi.Domains;
using EnergiaClienteWebApi.Models.EnergiaCliente;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Models.User;

namespace EnergiaClienteWebApi.Databases;

public class EnergiaClienteDatabase(IDatabaseFunctions _functions) : IEnergiaClienteDatabase
{
    public readonly IDatabaseFunctions functions = _functions;

    private CostKwh costKwh => new CostKwh(0.24m, 0.1741m, 0.1072m);

    public CostKwh GetCostKwh()
    {
        return costKwh;
    }

    public dbResponse<Invoice> GetInvoices(GetInvoicesModel model)
    {
        var param = new SqlParameter("habitacao", model.habitation);

        var response = functions.RunSelectProcedure("UltimasFaturas", [param]);

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

    public dbResponse<Reading> GetReadings(GetReadingsModel model)
    {
        var parameters = new List<SqlParameter>() {
                new("habitacao", model.habitation),
                new("quantidade", model.quantity)
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

    public dbResponse<Reading> GetRealReadings(GetReadingsModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("quantidade", model.quantity)
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

    public dbResponse<Reading> GetReadingByDate(GetReadingByDateModel model)
    {
        var parameters = new List<SqlParameter>() {
                new("habitacao", model.habitation),
                new("mes", model.month),
                new("ano", model.year)
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

    public dbResponse<decimal> GetUnpaidTotal(GetUnpaidTotalModel model)
    {
        var param = new SqlParameter("habitacao", model.habitation);

        var response = functions.RunSelectProcedure("TotalPorPagar", [param]);
        if (response.Count == 0)
            return new dbResponse<decimal> { Status = new StatusObject(404) };

        DataRow row = response[0];

        decimal value = 0;

        value = functions.GetParam<decimal>(row["Total"]);

        return new dbResponse<decimal>(value);
    }

    public dbResponse<string> InsertReading(InsertReadingModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.HabitationId),
                new("estimada", model.Estimated),
                new("vazio", model.Vazio),
                new("ponta", model.Ponta),
                new("cheias", model.Cheias),
                new("mes", model.Month),
                new("ano", model.Year),
                new("dataLeitura", model.ReadingDate),
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

    public dbResponse<string> InsertInvoice(InsertInvoiceModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("numero", model.number),
                new("pago", false),
                new("dataini", model.startDate),
                new("datafim", model.endDate),
                new("datalim", model.limitDate),
                new("documento", model.document)
            };

        var parameterValue = new SqlParameter("valor", SqlDbType.Decimal)
        {
            Value = model.value,
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

    public dbResponse<User> GetUserDetails(GetUserDetailsModel model)
    {
        var param = new SqlParameter("email", model.email);

        var response = functions.RunSelectProcedure("DetalhesUtilizador", [param]);

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

    public dbResponse<bool> AutherizeHabitation(AutherizeHabitationModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("email", model.email),
            };

        var response = functions.RunSelectProcedure("AutorizarHabitacao", parameters);

        if (response.Count == 0)
            return new dbResponse<bool> { Status = new StatusObject(500) };

        var value = response[0]["success"].ToString();

        if (string.IsNullOrEmpty(value))
            return new dbResponse<bool> { Status = new StatusObject(500) };

        return new dbResponse<bool>(bool.Parse(value));
    }

    public dbResponse<bool> AuthenticateUser(AuthenticateUserModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("palavrapasse", model.password),
                new("email", model.email),
            };

        var response = functions.RunSelectProcedure("AuthenticarUtilizador", parameters);

        if (response.Count == 0)
            return new dbResponse<bool> { Status = new StatusObject(500) };

        var value = response[0]["success"].ToString();

        if (string.IsNullOrEmpty(value))
            return new dbResponse<bool> { Status = new StatusObject(500) };

        return new dbResponse<bool>(bool.Parse(value));
    }

    public dbResponse<Holder> GetHolderDetails(GetHolderDetailsModel model)
    {
        var param = new SqlParameter("habitacao", model.habitation);

        var response = functions.RunSelectProcedure("DetalhesTitular", [param]);

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

    public dbResponse<Habitation> GetHabitationDetails(GetHabitationDetailsModel model)
    {
        var param = new SqlParameter("habitacao", model.habitation);

        var response = functions.RunSelectProcedure("DetalhesHabitacao", [param]);

        if (response.Count == 0)
            return new dbResponse<Habitation> { Status = new StatusObject(404) };

        DataRow row = response[0];

        Habitation habitation = new Habitation()
        {
            userEmail = functions.GetParam<string>(row["emailUtilizador"]),
            power = functions.GetParam<decimal>(row["potencia"]),
            phase = functions.GetParam<string>(row["fase"]),
            tensionLevel = functions.GetParam<string>(row["nivelTensao"]),
            schedule = functions.GetParam<string>(row["horario"]),
            costKwh = GetCostKwh()
        };

        return new dbResponse<Habitation>(habitation);
    }

    public dbResponse<string> UpdateHabitationPower(UpdateHabitationPowerModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("potencia", model.power),
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

    public dbResponse<string> UpdateHolderName(UpdateHolderNameModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("nomeCompleto", model.fullName),
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

    public dbResponse<string> UpdateHolderNif(UpdateHolderNifModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("nif", model.nif),
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

    public dbResponse<string> UpdateHolderContact(UpdateHolderContactModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("contacto", model.contact),
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

    public dbResponse<string> UpdateHabitationTensionLevel(UpdateHabitationTensionLevelModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("nivelTensao", model.tensionLevel),
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

    public dbResponse<string> UpdateHabitationSchedule(UpdateHabitationScheduleModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("horario", model.schedule),
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

    public dbResponse<string> UpdateHabitationPhase(UpdateHabitationPhaseModel model)
    {
        var parameters = new List<SqlParameter>()
            {
                new("habitacao", model.habitation),
                new("fase", model.phase),
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