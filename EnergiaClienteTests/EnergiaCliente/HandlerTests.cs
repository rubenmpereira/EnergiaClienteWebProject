using EnergiaClienteWebApi.Databases.Interfaces;
using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers;

namespace EnergiaClienteTests.EnergiaCliente;

public class HandlerTests
{
    [Fact]
    public void GetReadingsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadings(It.IsAny<GetReadingsRequestModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadings(new GetReadingsRequestModel());

        mockDatabase.Verify(database => database.GetReadings(It.IsAny<GetReadingsRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetReadingsNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadings(It.IsAny<GetReadingsRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadings(new GetReadingsRequestModel());

        mockDatabase.Verify(database => database.GetReadings(It.IsAny<GetReadingsRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetReadingByDateOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadingByDate(new GetReadingByDateRequestModel());

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetReadingByDateNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetReadingByDate(new GetReadingByDateRequestModel());

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UploadNewReadingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.InsertReading(It.IsAny<InsertReadingRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UploadNewReading(new InsertReadingRequestModel());

        mockDatabase.Verify(database => database.InsertReading(It.IsAny<InsertReadingRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UploadNewReadingInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.InsertReading(It.IsAny<InsertReadingRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UploadNewReading(new InsertReadingRequestModel());

        mockDatabase.Verify(database => database.InsertReading(It.IsAny<InsertReadingRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void GetpreviousMonthReadingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetpreviousMonthReading(new GetReadingByDateRequestModel() { month = 10, year = 2020, habitation = 1 });

        mockDatabase.Verify(database => database.GetReadingByDate(new GetReadingByDateRequestModel() { habitation = 1, month = 9, year = 2020 }), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetpreviousMonthReadingPreviousYearOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Id = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetpreviousMonthReading(new GetReadingByDateRequestModel() { month = 1, year = 2020, habitation = 1 });

        mockDatabase.Verify(database => database.GetReadingByDate(new GetReadingByDateRequestModel() { habitation = 1, month = 12, year = 2019 }), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetpreviousMonthReadingNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetpreviousMonthReading(new GetReadingByDateRequestModel());

        mockDatabase.Verify(database => database.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void BillingOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingRequestModel());

        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void BillingInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.InsertInvoice(It.IsAny<InsertInvoiceRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.Billing(new BillingRequestModel());

        mockDatabase.Verify(database => database.InsertInvoice(It.IsAny<InsertInvoiceRequestModel>()), Times.Once);
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
    public void GetHabitationIdsNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHabitationIds())
        .Returns(new dbResponse<int>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHabitationIds();

        mockDatabase.Verify(database => database.GetHabitationIds(), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetInvoicesOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetInvoices(It.IsAny<GetInvoicesRequestModel>()))
        .Returns(new dbResponse<Invoice>(new Invoice() { Number = "FAT102DT2020-10" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetInvoices(new GetInvoicesRequestModel());

        mockDatabase.Verify(database => database.GetInvoices(It.IsAny<GetInvoicesRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetInvoicesNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetInvoices(It.IsAny<GetInvoicesRequestModel>()))
        .Returns(new dbResponse<Invoice>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetInvoices(new GetInvoicesRequestModel());

        mockDatabase.Verify(database => database.GetInvoices(It.IsAny<GetInvoicesRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetUnpaidTotalOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()))
        .Returns(new dbResponse<decimal>(120.5m));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUnpaidTotal(new GetUnpaidTotalRequestModel());

        mockDatabase.Verify(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUnpaidTotalNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()))
        .Returns(new dbResponse<decimal>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUnpaidTotal(new GetUnpaidTotalRequestModel());

        mockDatabase.Verify(database => database.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetUserDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()))
        .Returns(new dbResponse<User>(new User() { email = "exemple@email.com" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUserDetails(new GetUserDetailsRequestModel());

        mockDatabase.Verify(database => database.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetUserDetailsNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()))
        .Returns(new dbResponse<User>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetUserDetails(new GetUserDetailsRequestModel());

        mockDatabase.Verify(database => database.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetHolderDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()))
        .Returns(new dbResponse<Holder>(new Holder() { HabitationId = 1 }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHolderDetails(new GetHolderDetailsRequestModel());

        mockDatabase.Verify(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHolderDetailsNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()))
        .Returns(new dbResponse<Holder>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHolderDetails(new GetHolderDetailsRequestModel());

        mockDatabase.Verify(database => database.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void GetHabitationDetailsOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()))
        .Returns(new dbResponse<Habitation>(new Habitation() { userEmail = "exemple@email.com" }));

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHabitationDetails(new GetHabitationDetailsRequestModel());

        mockDatabase.Verify(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void GetHabitationDetailsNotFoundResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()))
        .Returns(new dbResponse<Habitation>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.GetHabitationDetails(new GetHabitationDetailsRequestModel());

        mockDatabase.Verify(database => database.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(404, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count == 0);
    }

    [Fact]
    public void UpdateHabitationPowerOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHabitationPowerInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNameOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderName(new UpdateHolderNameRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHolderNameInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderName(new UpdateHolderNameRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderNifOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderNif(new UpdateHolderNifRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHolderNifInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderNif(new UpdateHolderNifRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHolderContactOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderContact(new UpdateHolderContactRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHolderContactInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHolderContact(new UpdateHolderContactRequestModel());

        mockDatabase.Verify(database => database.UpdateHolderContact(It.IsAny<UpdateHolderContactRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationTensionLevelOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHabitationTensionLevelInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationScheduleOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHabitationScheduleInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

    [Fact]
    public void UpdateHabitationPhaseOkResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()))
        .Returns(new dbResponse<string>());

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
    }

    [Fact]
    public void UpdateHabitationPhaseInternalErrorResult()
    {
        var mockDatabase = new Mock<IEnergiaClienteDatabase>();

        mockDatabase.Setup(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()))
        .Returns(new dbResponse<string>() { Status = new StatusObject(500) });

        var handler = new EnergiaClienteHandler(mockDatabase.Object);

        var response = handler.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        mockDatabase.Verify(database => database.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()), Times.Once);
        Assert.True(response.Status.Error);
        Assert.Equal(500, response.Status.StatusCode);
    }

}