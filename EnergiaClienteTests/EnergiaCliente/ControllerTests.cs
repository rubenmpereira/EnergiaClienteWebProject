using EnergiaClienteWebApi.RequestModels;
using EnergiaClienteWebApi.Domains;
using Moq;
using EnergiaClienteWebApi.Handlers.Interfaces;

namespace EnergiaClienteTests.EnergiaCliente;

public class ControllerTests
{

    [Fact]
    public void GetReadingsCallsHandler()
    {
        var mockHandlerFunctions = new Mock<IEnergiaClienteHandler>();

        mockHandlerFunctions.Setup(handler => handler.GetReadings(It.IsAny<GetReadingsRequestModel>()))
        .Returns(new dbResponse<Reading>());

        var controller = new EnergiaClienteController(mockHandlerFunctions.Object);

        var response = controller.GetReadings(new GetReadingsRequestModel());

        mockHandlerFunctions.Verify(mock => mock.GetReadings(It.IsAny<GetReadingsRequestModel>()), Times.Once);
    }


}