using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestingRepDemo.Services;

namespace TestingRepDemoTests
{
    [TestClass]
    public class IpInfoServiceTests
    {
        [TestMethod]
        public async Task GetCityAsync_ReturnsCorrectCity()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"city\": \"test-city\"}")
                } );
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            IpInfoService service = new IpInfoService(mockClient);

            // Act
            string result = await service.GetCityAsync("1.1.1.1");

            // Assert
            Assert.AreEqual("test-city", result);
        }
    }
}
