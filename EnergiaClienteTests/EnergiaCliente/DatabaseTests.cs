using EnergiaClienteWebApi.Databases;
using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.RequestModels;
using Moq;
using Microsoft.Data.SqlClient;
using System.Data;

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

}