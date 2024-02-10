using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.Models;
using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers;
using EnergiaClienteWebApi.Models.EnergiaCliente;

namespace EnergiaClienteTests.EnergiaCliente;

public class HandlerTests
{
    [Fact]
    public void GetReadingsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadings(It.IsAny<GetReadingsModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadings(new GetReadingsModel());

        mockDatabase.Verify(database => database.GetReadings(It.IsAny<GetReadingsModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetReadingByDateOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadingByDate(new GetReadingByDateModel());

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UploadNewReadingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertReading(It.IsAny<InsertReadingModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var request = new InsertReadingModel
        {
            Ponta = 20,
            Cheias = 20,
            Vazio = 20,
            Month = 10,
            Year = 2020,
            Estimated = false,
            HabitationId = 1,
            ReadingDate = DateTime.Now
        };

        var response = handler.UploadNewReading(request);

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
        mockDatabase.Verify(database => database.InsertReading(request), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UploadNewReadingNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertReading(It.IsAny<InsertReadingModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var request = new InsertReadingModel
        {
            Ponta = 20,
            Cheias = 20,
            Vazio = 20,
            Month = 10,
            Year = 2020,
            Estimated = false,
            HabitationId = 1,
            ReadingDate = DateTime.Now
        };

        var response = handler.UploadNewReading(request);

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
        mockDatabase.Verify(database => database.InsertReading(request), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UploadNewReadingInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertReading(It.IsAny<InsertReadingModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UploadNewReading(new InsertReadingModel());

        mockDatabase.Verify(database => database.InsertReading(It.IsAny<InsertReadingModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void GetpreviousMonthReadingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetpreviousMonthReading(new GetReadingByDateModel() { month = 10, year = 2020, habitation = 1 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetpreviousMonthReadingPreviousYearOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetpreviousMonthReading(new GetReadingByDateModel() { month = 1, year = 2020, habitation = 1 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 12 && x.year == 2019)), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void BillingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 20, Cheias = 20, Vazio = 20 }));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingModel() { habitationId = 1, billingMonth = 10, billingYear = 2020 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)), Times.Once);
        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void BillingCurrentMonthNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetRealReadings(It.IsAny<GetReadingsModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 20, Cheias = 20, Vazio = 20, HabitationId = 1, Month = 10, Year = 2020 }));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)))
        .Returns(new dbResponse<Reading>());
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingModel() { habitationId = 1, billingMonth = 10, billingYear = 2020 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)), Times.Once);
        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        mockDatabase.Verify(database => database.GetRealReadings(It.IsAny<GetReadingsModel>()), Times.Once);
        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void BillingPreviousMonthNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 20, Cheias = 20, Vazio = 20 }));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)))
        .Returns(new dbResponse<Reading>());
        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingModel() { habitationId = 1, billingMonth = 10, billingYear = 2020 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)), Times.Once);
        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void BillingNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetRealReadings(It.IsAny<GetReadingsModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 0, Cheias = 0, Vazio = 0, HabitationId = 1, Month = 10, Year = 2020 }));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)))
        .Returns(new dbResponse<Reading>());
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)))
        .Returns(new dbResponse<Reading>());
        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingModel() { habitationId = 1, billingMonth = 10, billingYear = 2020 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)), Times.Once);
        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        mockDatabase.Verify(database => database.GetRealReadings(It.IsAny<GetReadingsModel>()), Times.Once);
        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void BillingInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetCostKwh())
        .Returns(new CostKwh(0.24m, 0.1741m, 0.1072m));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 20, Cheias = 20, Vazio = 20 }));
        mockDatabase.Setup(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 10, Cheias = 10, Vazio = 10 }));
        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingModel() { habitationId = 1, billingMonth = 10, billingYear = 2020 });

        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 10)), Times.Once);
        mockDatabase.Verify(database => database.GetReadingByDate(It.Is<GetReadingByDateModel>(x => x.month == 9)), Times.Once);
        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void GetHabitationIdsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHabitationIds())
        .Returns(new dbResponse<int>([102, 116, 137]));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHabitationIds();

        mockDatabase.Verify(database => database.GetHabitationIds(), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetInvoicesOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetInvoices(It.IsAny<GetInvoicesModel>()))
        .Returns(new dbResponse<Invoice>(new Invoice() { Number = "FAT102DT2020-10" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetInvoices(new GetInvoicesModel());

        mockDatabase.Verify(database => database.GetInvoices(It.IsAny<GetInvoicesModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUnpaidTotalOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalModel>()))
        .Returns(new dbResponse<decimal>(120.5m));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUnpaidTotal(new GetUnpaidTotalModel());

        mockDatabase.Verify(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUserDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUserDetails(It.IsAny<GetUserDetailsModel>()))
        .Returns(new dbResponse<User>(new User() { email = "exemple@email.com" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUserDetails(new GetUserDetailsModel());

        mockDatabase.Verify(database => database.GetUserDetails(It.IsAny<GetUserDetailsModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHolderDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsModel>()))
        .Returns(new dbResponse<Holder>(new Holder() { HabitationId = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHolderDetails(new GetHolderDetailsModel());

        mockDatabase.Verify(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHabitationDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsModel>()))
        .Returns(new dbResponse<Habitation>(new Habitation() { userEmail = "exemple@email.com" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHabitationDetails(new GetHabitationDetailsModel());

        mockDatabase.Verify(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHabitationPowerOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPower(new UpdateHabitationPowerModel());

        mockDatabase.Verify(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPowerInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPower(new UpdateHabitationPowerModel());

        mockDatabase.Verify(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNameOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderName(new UpdateHolderNameModel());

        mockDatabase.Verify(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNameInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderName(new UpdateHolderNameModel());

        mockDatabase.Verify(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNifOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderNif(new UpdateHolderNifModel());

        mockDatabase.Verify(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNifInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderNif(new UpdateHolderNifModel());

        mockDatabase.Verify(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderContactOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderContact(new UpdateHolderContactModel());

        mockDatabase.Verify(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderContactInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderContact(new UpdateHolderContactModel());

        mockDatabase.Verify(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationTensionLevelOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelModel());

        mockDatabase.Verify(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationTensionLevelInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelModel());

        mockDatabase.Verify(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationScheduleOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationSchedule(new UpdateHabitationScheduleModel());

        mockDatabase.Verify(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationScheduleInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationSchedule(new UpdateHabitationScheduleModel());

        mockDatabase.Verify(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPhaseOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPhase(new UpdateHabitationPhaseModel());

        mockDatabase.Verify(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPhaseInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPhase(new UpdateHabitationPhaseModel());

        mockDatabase.Verify(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

}