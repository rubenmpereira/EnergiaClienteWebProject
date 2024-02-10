using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.Models.EnergiaCliente;

namespace EnergiaClienteTests.EnergiaCliente;

public class ControllerTests
{

    [Fact]
    public void GetReadingsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadings(It.IsAny<GetReadingsModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetReadings(new GetReadingsRequestModel());

        mockHandler.Verify(mock => mock.GetReadings(It.IsAny<GetReadingsModel>()), Times.Once);
    }

    [Fact]
    public void UploadNewReadingCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();
        //var mockDatetime = new Mock<DateTime>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>());
        mockHandler.Setup(handler => handler.UploadNewReading(It.IsAny<InsertReadingModel>()))
        .Returns(new dbResponse<decimal>(10m));

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UploadNewReading(new UploadNewReadingRequestModel());

        if (DateTime.Now.Day >= 5 && DateTime.Now.Day <= 7)
        {
            mockHandler.Verify(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
            mockHandler.Verify(handler => handler.UploadNewReading(It.IsAny<InsertReadingModel>()), Times.Once);
        }
        else
        {
        Assert.NotNull(response);
        Assert.NotNull(response.Result);
        Assert.Equal(typeof(BadRequestObjectResult), response.Result.GetType());
        }
    }

    [Fact]
    public void UploadNewReadingAlreadyUploaded()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>(new Reading() { Ponta = 1, Cheias = 1, Vazio = 1 }));
        mockHandler.Setup(handler => handler.UploadNewReading(It.IsAny<InsertReadingModel>()));

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UploadNewReading(new UploadNewReadingRequestModel());

        if (DateTime.Now.Day >= 5 && DateTime.Now.Day <= 7)
        {
            mockHandler.Verify(mock => mock.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
            mockHandler.Verify(mock => mock.UploadNewReading(It.IsAny<InsertReadingModel>()), Times.Never);
        }

        Assert.NotNull(response);
        Assert.NotNull(response.Result);
        Assert.Equal(typeof(BadRequestObjectResult), response.Result.GetType());
    }

    [Fact]
    public void GetReadingByDateCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetReadingByDate(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetReadingByDate(new GetReadingByDateRequestModel());

        mockHandler.Verify(mock => mock.GetReadingByDate(It.IsAny<GetReadingByDateModel>()), Times.Once);
    }

    [Fact]
    public void GetpreviousMonthReadingCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetpreviousMonthReading(It.IsAny<GetReadingByDateModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetpreviousMonthReading(new GetReadingByDateRequestModel());

        mockHandler.Verify(mock => mock.GetpreviousMonthReading(It.IsAny<GetReadingByDateModel>()), Times.Once);
    }

    [Fact]
    public void GetInvoicesCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetInvoices(It.IsAny<GetInvoicesModel>()))
        .Returns(new dbResponse<Invoice>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetInvoices();

        mockHandler.Verify(mock => mock.GetInvoices(It.IsAny<GetInvoicesModel>()), Times.Once);
    }

    [Fact]
    public void GetUnpaidTotalCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetUnpaidTotal(It.IsAny<GetUnpaidTotalModel>()))
        .Returns(new dbResponse<decimal>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetUnpaidTotal();

        mockHandler.Verify(mock => mock.GetUnpaidTotal(It.IsAny<GetUnpaidTotalModel>()), Times.Once);
    }

    [Fact]
    public void GetUserDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetUserDetails(It.IsAny<GetUserDetailsModel>()))
        .Returns(new dbResponse<User>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        //var response = controller.GetUserDetails(new GetUserDetailsRequestModel());

        mockHandler.Verify(mock => mock.GetUserDetails(It.IsAny<GetUserDetailsModel>()), Times.Once);
    }

    [Fact]
    public void GetHolderDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHolderDetails(It.IsAny<GetHolderDetailsModel>()))
        .Returns(new dbResponse<Holder>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHolderDetails();

        mockHandler.Verify(mock => mock.GetHolderDetails(It.IsAny<GetHolderDetailsModel>()), Times.Once);
    }

    [Fact]
    public void GetHabitationDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHabitationDetails(It.IsAny<GetHabitationDetailsModel>()))
        .Returns(new dbResponse<Habitation>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHabitationDetails();

        mockHandler.Verify(mock => mock.GetHabitationDetails(It.IsAny<GetHabitationDetailsModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationPowerCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationPower(new UpdateHabitationPowerRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationPower(It.IsAny<UpdateHabitationPowerModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHolderNameCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHolderName(new UpdateHolderNameRequestModel());

        mockHandler.Verify(mock => mock.UpdateHolderName(It.IsAny<UpdateHolderNameModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHolderNifCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHolderNif(new UpdateHolderNifRequestModel());

        mockHandler.Verify(mock => mock.UpdateHolderNif(It.IsAny<UpdateHolderNifModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationTensionLevelCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationTensionLevel(new UpdateHabitationTensionLevelRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationTensionLevel(It.IsAny<UpdateHabitationTensionLevelModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationScheduleCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationSchedule(new UpdateHabitationScheduleRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationSchedule(It.IsAny<UpdateHabitationScheduleModel>()), Times.Once);
    }

    [Fact]
    public void UpdateHabitationPhaseCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()))
        .Returns(new dbResponse<string>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UpdateHabitationPhase(new UpdateHabitationPhaseRequestModel());

        mockHandler.Verify(mock => mock.UpdateHabitationPhase(It.IsAny<UpdateHabitationPhaseModel>()), Times.Once);
    }

}