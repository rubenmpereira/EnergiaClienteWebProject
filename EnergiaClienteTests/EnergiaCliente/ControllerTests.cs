using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EnergiaClienteWebApi.RequestModels.EnergiaCliente;
using EnergiaClienteWebApi.RequestModels.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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

        mockHandler.Setup(handler => handler.UploadNewReading(It.IsAny<UploadNewReadingRequestModel>()))
        .Returns(new dbResponse<decimal>(10m));

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.UploadNewReading(new UploadNewReadingRequestModel());

        mockHandler.Verify(handler => handler.UploadNewReading(It.IsAny<UploadNewReadingRequestModel>()), Times.Once);
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

        mockHandler.Setup(handler => handler.GetInvoices())
        .Returns(new dbResponse<Invoice>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetInvoices();

        mockHandler.Verify(mock => mock.GetInvoices(), Times.Once);
    }

    [Fact]
    public void GetUnpaidTotalCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetUnpaidTotal())
        .Returns(new dbResponse<decimal>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetUnpaidTotal();

        mockHandler.Verify(mock => mock.GetUnpaidTotal(), Times.Once);
    }

    [Fact]
    public void GetUserDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();
        var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.Name == "email@gmail.com");
        var mockPrincipal = Mock.Of<ClaimsPrincipal>(p => p.Identity == mockIdentity);
        var mockhttp = Mock.Of<HttpContext>(cc => cc.User == mockPrincipal);
        var mockContext = Mock.Of<ControllerContext>(cc => cc.HttpContext == mockhttp);

        mockHandler.Setup(handler => handler.GetUserDetails(It.IsAny<GetUserDetailsRequestModel>()))
        .Returns(new dbResponse<User>());

        var controller = new UserController(mockHandler.Object)
        {
            ControllerContext = mockContext
        };

        var response = controller.GetUserDetails();

        mockHandler.Verify(mock => mock.GetUserDetails(It.Is<GetUserDetailsRequestModel>(model => model.email == "email@gmail.com")), Times.Once);
    }

    [Fact]
    public void GetHolderDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHolderDetails())
        .Returns(new dbResponse<Holder>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHolderDetails();

        mockHandler.Verify(mock => mock.GetHolderDetails(), Times.Once);
    }

    [Fact]
    public void GetHabitationDetailsCallsHandler()
    {
        var mockHandler = new Mock<IEnergiaClienteHandler>();

        mockHandler.Setup(handler => handler.GetHabitationDetails())
        .Returns(new dbResponse<Habitation>());

        var controller = new EnergiaClienteController(mockHandler.Object);

        var response = controller.GetHabitationDetails();

        mockHandler.Verify(mock => mock.GetHabitationDetails(), Times.Once);
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