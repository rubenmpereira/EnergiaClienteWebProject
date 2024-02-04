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

        mockDatabase.Verify(mock => mock.GetReadings(It.IsAny<GetReadingsRequestModel>()), Times.Once);
        Assert.False(response.Status.Error);
        Assert.Equal(200, response.Status.StatusCode);
        Assert.True(response.Result != null);
        Assert.True(response.Result.Count > 0);
        Assert.True(response.Result[0].Id == 1);
    }
    
}