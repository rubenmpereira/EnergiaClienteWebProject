using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnergiaClienteTests.EnergiaCliente;

public class ControllerTests
{

    [Fact]
    public void GetReadingsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadings(It.IsAny<GetReadingsRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetReadings(new GetReadingsRequestModel());

        mockHandler.Verify(mock => mock.GetReadings(It.IsAny<GetReadingsRequestModel>()), Times.Once);
    }

    [Fact]
    public void UploadNewReadingCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();
        //var mockDatetime = new Mock<DateTime>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>());
        mockHandler.Setup(handler => handler.UploadNewReading(It.IsAny<Reading>()))
        .Returns(new dbResponse<decimal>(10m));

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UploadNewReading(new UploadNewReadingRequestModel());

        if (DateTime.Now.Day >= 5 && DateTime.Now.Day <= 7)
        {
            mockHandler.Verify(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
            mockHandler.Verify(handler => handler.UploadNewReading(It.IsAny<Reading>()), Times.Once);
        }
        else
        {
            Assert.NotNull(response.Value);
            Assert.True(response.Value.Status.Error);
            Assert.Equal(400, response.Value.Status.StatusCode);
        }
    }

    [Fact]
    public void UploadNewReadingAlreadyUploaded()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 1, Cheias = 1, Vazio = 1 }));
        mockHandler.Setup(handler => handler.UploadNewReading(It.IsAny<Reading>()));

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UploadNewReading(new UploadNewReadingRequestModel());

        mockHandler.Verify(mock => mock.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
        mockHandler.Verify(mock => mock.UploadNewReading(It.IsAny<Reading>()), Times.Never);

        Assert.NotNull(response);
        Assert.NotNull(response.Result);
        Assert.Equal(typeof(BadRequestObjectResult), response.Result.GetType());

        //Assert.NotNull(response.Result);
        //Assert.True(response.Value.Status.Error);
        //Assert.Equal(400, response.Value.Status.StatusCode);
        //var result = (ActionResult<dbResponse<decimal>>)response.Result as ActionResult<dbResponse<decimal>>;
        //Assert.NotNull(result);
        //Assert.NotNull(result.Value);
        //var resultvalue = (dbResponse<decimal>)result.Value;
        //Assert.NotNull(resultvalue);
        //Assert.True(result.Value.Status.Error);
        //Assert.Equal(400, result.Value.Status.StatusCode);

    }

    [Fact]
    public void GetReadingByDateCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetReadingByDate(new GetReadingByDateRequestModel());

        mockHandler.Verify(mock => mock.GetReadingByDate(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetpreviousMonthReadingCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetpreviousMonthReading(It.IsAny<GetReadingByDateRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetpreviousMonthReading(new GetReadingByDateRequestModel());

        mockHandler.Verify(mock => mock.GetpreviousMonthReading(It.IsAny<GetReadingByDateRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetInvoicesCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetInvoices(It.IsAny<GetInvoicesRequestModel>()))
        .Returns(new dbResponse<Invoice>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetInvoices(new GetInvoicesRequestModel());

        mockHandler.Verify(mock => mock.GetInvoices(It.IsAny<GetInvoicesRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetUnpaidTotalCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()))
        .Returns(new dbResponse<decimal>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetUnpaidTotal(new GetUnpaidTotalRequestModel());

        mockHandler.Verify(mock => mock.GetUnpaidTotal(It.IsAny<GetUnpaidTotalRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetUserDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()))
        .Returns(new dbResponse<User>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetUserDetails(new GetUserDetailsRequestModel());

        mockHandler.Verify(mock => mock.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetHolderDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()))
        .Returns(new dbResponse<Holder>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHolderDetails(new GetHolderDetailsRequestModel());

        mockHandler.Verify(mock => mock.GetHolderDetails(It.IsAny<GetHolderDetailsRequestModel>()), Times.Once);
    }

    [Fact]
    public void GetHabitationDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()))
        .Returns(new dbResponse<Habitation>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHabitationDetails(new GetHabitationDetailsRequestModel());

        mockHandler.Verify(mock => mock.GetHabitationDetails(It.IsAny<GetHabitationDetailsRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationPowerCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHolderNameCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHolderName(new UpdateHolderNameRequestModel());

        mockHandler.Verify(mock => mock.UpdateHolderName(It.IsAny<UpdateHolderNameRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHolderNifCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHolderNif(new UpdateHolderNifRequestModel());

        mockHandler.Verify(mock => mock.UpdateHolderNif(It.IsAny<UpdateHolderNifRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationTensionLevelCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationScheduleCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleRequestModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationPhaseCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseRequestModel>()), Times.Once);
    }

}