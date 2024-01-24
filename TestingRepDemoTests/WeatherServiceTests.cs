using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingRepDemo.Models.Dtos;
using TestingRepDemo.Services;

namespace TestingRepDemoTests
{
    [TestClass]
    public class WeatherServiceTests
    {
        [TestMethod]
        public async Task GetWeatherForCityAsync_ReturnsCorrectWeather()
        {
            // Arrange
            WeatherDto weather = new WeatherDto()
            {
                Temperature = "8C",
                Wind = "8 mph",
                Description = "Description",
            };
            string responseString = JsonSerializer.Serialize(weather);

            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync( new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseString)
                } );

            HttpClient mockClient = new HttpClient(mockHandler.Object);
            WeatherService weatherService = new WeatherService(mockClient);

            // Act
            var result = await weatherService.GetWeatherForCityAsync("test-city");

            // Assert
            Assert.AreEqual("8C", result.Temperature);
            Assert.AreEqual("8 mph", result.Wind);
            Assert.AreEqual("Description", result.Description);
        }
    }
}
