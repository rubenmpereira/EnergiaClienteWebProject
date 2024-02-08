using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.RequestModels;
using Moq;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using EnergiaClienteWebApi.Domains;

namespace EnergiaClienteTests.EnergiaCliente;

public class DatabaseTests
{

    [Fact]
    public void GetReadingsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("id", typeof(int)),
            new DataColumn("vazio", typeof(int)),
            new DataColumn("ponta", typeof(int)),
            new DataColumn("cheias", typeof(int)),
            new DataColumn("mes", typeof(int)),
            new DataColumn("ano", typeof(int)),
            new DataColumn("dataLeitura", typeof(DateTime)),
            new DataColumn("idHabitacao", typeof(int)),
            new DataColumn("estimada", typeof(bool)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["id"] = 1;
        row["vazio"] = 100;
        row["ponta"] = 100;
        row["cheias"] = 100;
        row["mes"] = 1;
        row["ano"] = 2024;
        row["dataLeitura"] = new DateTime(2024, 1, 1);
        row["idHabitacao"] = 1;
        row["estimada"] = false;
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetReadings(new GetReadingsRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetReadingsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetReadings(new GetReadingsRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetInvoicesOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("numero", typeof(string)),
            new DataColumn("dataInicio", typeof(DateTime)),
            new DataColumn("dataFim", typeof(DateTime)),
            new DataColumn("pago", typeof(bool)),
            new DataColumn("valor", typeof(decimal)),
            new DataColumn("dataLimite", typeof(DateTime)),
            new DataColumn("idHabitacao", typeof(int)),
            new DataColumn("documento", typeof(string)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["numero"] = "1";
        row["dataInicio"] = new DateTime(2024, 1, 1);
        row["dataFim"] = new DateTime(2024, 1, 1);
        row["pago"] = true;
        row["valor"] = 1m;
        row["dataLimite"] = new DateTime(2024, 1, 1);
        row["idHabitacao"] = 1;
        row["documento"] = "doc";
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetInvoices(new GetInvoicesRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetInvoicesNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetInvoices(new GetInvoicesRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetRealReadingsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("id", typeof(int)),
            new DataColumn("vazio", typeof(int)),
            new DataColumn("ponta", typeof(int)),
            new DataColumn("cheias", typeof(int)),
            new DataColumn("mes", typeof(int)),
            new DataColumn("ano", typeof(int)),
            new DataColumn("dataLeitura", typeof(DateTime)),
            new DataColumn("idHabitacao", typeof(int)),
            new DataColumn("estimada", typeof(bool)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["id"] = 1;
        row["vazio"] = 100;
        row["ponta"] = 100;
        row["cheias"] = 100;
        row["mes"] = 1;
        row["ano"] = 2024;
        row["dataLeitura"] = new DateTime(2024, 1, 1);
        row["idHabitacao"] = 1;
        row["estimada"] = false;
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetRealReadings(new GetReadingsRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetRealReadingsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetRealReadings(new GetReadingsRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetUnpaidTotalOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("Total", typeof(decimal)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["Total"] = 100m;
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetUnpaidTotal(new GetUnpaidTotalRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUnpaidTotalNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetUnpaidTotal(new GetUnpaidTotalRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void InsertReadingOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.InsertReading(new InsertReadingRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void InsertReadingInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.InsertReading(new InsertReadingRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void InsertInvoiceOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.InsertInvoice(new InsertInvoiceRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void InsertInvoiceInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.InsertInvoice(new InsertInvoiceRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetUserDetailsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("email", typeof(string)),
            new DataColumn("contacto", typeof(string)),
            new DataColumn("nomeCompleto", typeof(string)),
            new DataColumn("genero", typeof(bool)),
            new DataColumn("nif", typeof(string)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["email"] = "exemple@email.com";
        row["contacto"] = "917767887";
        row["nomeCompleto"] = "test name";
        row["genero"] = 0;
        row["nif"] = "123456789";
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetUserDetails(new GetUserDetailsRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUserDetailsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetUserDetails(new GetUserDetailsRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetHolderDetailsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("idHabitacao", typeof(int)),
            new DataColumn("contacto", typeof(string)),
            new DataColumn("nomeCompleto", typeof(string)),
            new DataColumn("nif", typeof(string)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["idHabitacao"] = 1;
        row["contacto"] = "917767887";
        row["nomeCompleto"] = "test name";
        row["nif"] = "123456789";
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHolderDetails(new GetHolderDetailsRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHolderDetailsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHolderDetails(new GetHolderDetailsRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetHabitationDetailsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("userEmail", typeof(string)),
            new DataColumn("power", typeof(decimal)),
            new DataColumn("phase", typeof(string)),
            new DataColumn("tensionLevel", typeof(string)),
            new DataColumn("schedule", typeof(string)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["userEmail"] = "exemple@email.com";
        row["power"] = 6.7m;
        row["phase"] = "mono";
        row["tensionLevel"] = "baixo";
        row["schedule"] = "simples";
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHabitationDetails(new GetHabitationDetailsRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHabitationDetailsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHabitationDetails(new GetHabitationDetailsRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHabitationPowerOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPowerInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHolderNameOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderName(new UpdateHolderNameRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNameInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderName(new UpdateHolderNameRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHolderNifOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderNif(new UpdateHolderNifRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNifInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderNif(new UpdateHolderNifRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHolderContactOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderContact(new UpdateHolderContactRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderContactInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHolderContact(new UpdateHolderContactRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHabitationTensionLevelOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationTensionLevelInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHabitationScheduleOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationScheduleInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHabitationPhaseOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(true);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPhaseInternalErrorResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        mockDatabaseFunctions.Setup(functions => functions.RunInsertProcedure(It.IsAny<string>(), It.IsAny<List<SqlParameter>>()))
        .Returns(false);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetHabitationIdsOkResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();
        DataColumn[] columns =
        [
            new DataColumn("id", typeof(int)),
        ];
        table.Columns.AddRange(columns);

        var row = table.NewRow();
        row["id"] = 1;
        table.Rows.Add(table.NewRow());

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHabitationIds();

        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHabitationIdsNotFoundResult()
    {
        var mockDatabaseFunctions = new Mock<IDatabaseFunctions>();

        DataTable table = new DataTable();

        mockDatabaseFunctions.Setup(functions => functions.RunSelectProcedure(It.IsAny<string>()))
        .Returns(table.Rows);

        var database = new EnergiaClienteDatabase(mockDatabaseFunctions.Object);

        var response = database.GetHabitationIds();

        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

}