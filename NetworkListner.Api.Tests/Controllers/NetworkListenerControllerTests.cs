using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetworkListener.Api.Controllers;
using SubModels.Models;
using SubModels.Utilities;
using NetworkListener.Api.Services.Contracts;
using Xunit;

namespace NetworkListener.Api.Tests.Controllers
{
    public class NetworkListenerControllerTests
    {
        private readonly NetworkListenerController _networkListenerController;
        private readonly Mock<ILogger<NetworkListenerController>> _mockLogger = new Mock<ILogger<NetworkListenerController>>();
        private readonly Mock<INetworkListenerService> _mockListnerService = new Mock<INetworkListenerService>();

        const string IpAddress = "8.8.8.8";
        const string services = "GEOIP,RDAP,RDNS,PING";
        public NetworkListenerControllerTests()
        {
            _networkListenerController = new NetworkListenerController(_mockLogger.Object, _mockListnerService.Object);
        }
        [Fact]
        public void ExecuteServices_Retuns_Success()
        {
            //Arrange
            var networkListnerModel = LoadData();
            //Act
            _mockListnerService.Setup(l => l.CheckIPv4andIPv6(IpAddress)).Returns(true);
            _mockListnerService.Setup(l => l.InvokeParallelServices(IpAddress, services)).Returns(networkListnerModel);

            var okResults = _networkListenerController.ExecuteServices(IpAddress, services);
            var response = (okResults as ObjectResult)?.Value as NetworkListenerModel;
            //Assert
            Assert.NotNull(response);
            Assert.Equal((okResults as ObjectResult).StatusCode, StatusCodes.Status200OK);
            Assert.Equal(response, networkListnerModel);
            Assert.NotNull(response.GeoIp.Result);

            Assert.True(response.GeoIp.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.GeoIp.HttpStatusCode);
            Assert.NotNull(response.GeoIp.Result);
            Assert.Equal(40.88, response.GeoIp.Result.Latitude);
            Assert.Equal(70.22, response.GeoIp.Result.Longitude);
            Assert.Equal("usa", response.GeoIp.Result.RegionCode);

            Assert.True(response.Ping.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.Ping.HttpStatusCode);
            Assert.NotNull(response.Ping.Result);
            Assert.Equal(22, response.Ping.Result.ExecutionTime);

            Assert.True(response.Rdns.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.Rdns.HttpStatusCode);
            Assert.NotNull(response.Rdns.Result);
            Assert.Equal("google", response.Rdns.Result.DnsName);
        }

        [Fact]
        public void ExecuteServices_Retuns_InternalServerError()
        {
            //Act
            _mockListnerService.Setup(l => l.CheckIPv4andIPv6(IpAddress)).Returns(true);
            _mockListnerService.Setup(l => l.InvokeParallelServices(IpAddress, services)).Throws(new HttpResponseException());
            var results = Assert.Throws<HttpResponseException>(() => _networkListenerController.ExecuteServices(IpAddress, services));
            //Assert
            Assert.Equal(results.StatusCode, StatusCodes.Status500InternalServerError);
        }
        private NetworkListenerModel LoadData()
        {
            var geoIpModel = new GeoIpModel
            {
                CountryName = "united states of america",
                City = "new york",
                Latitude = 40.88,
                Longitude = 70.22,
                RegionCode = "usa"
            };
            var pingModel = new PingModel
            {
                Status = "success",
                ExecutionTime = 22
            };
            var rdns = new RdnsModel
            {
                IpAddress = IpAddress,
                DnsName = "google"
            };
            var geoIpResults = new HttpProcessResults<GeoIpModel>()
            {
                IsSuccess = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Result = geoIpModel
            };
            var pingIpResults = new HttpProcessResults<PingModel>()
            {
                IsSuccess = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Result = pingModel
            };
            var rdnsResults = new HttpProcessResults<RdnsModel>()
            {
                IsSuccess = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Result = rdns
            };

            return new NetworkListenerModel
            {
                GeoIp = geoIpResults,
                Ping = pingIpResults,
                Rdns = rdnsResults
            };
        }
    }
}
