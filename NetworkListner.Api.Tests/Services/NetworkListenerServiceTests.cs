using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NetworkListener.Api.Services;
using SubModels.Models;
using System.Threading.Tasks;
using Xunit;
using SubModels.Helper;

namespace NetworkListener.Api.Tests.Services
{
    public class NetworkListenerServiceTests
    {
        private readonly NetworkListenerService _networkListenerService;
        private readonly Mock<ILogger<NetworkListenerService>> _mockLogger = new Mock<ILogger<NetworkListenerService>>();
        private readonly Mock<IConfiguration> _mockConfigurationService = new Mock<IConfiguration>();
        private readonly Mock<IRestService> _mockRestService = new Mock<IRestService>();

        public NetworkListenerServiceTests()
        {
            _networkListenerService = new NetworkListenerService(_mockLogger.Object, _mockConfigurationService.Object, _mockRestService.Object);
        }
        [Fact]
        public void InvokeParllelServices_Retuns_NetworkListenerModel()
        {
            //Arrange
            var services = "GEOIP,RDAP,RDNS,PING";
            var IpAddress = "8.8.8.8";

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
            //Act
            _mockConfigurationService.SetupGet(x => x[It.Is<string>(s => s == "IPServiceBaseURL")]).Returns(It.IsAny<string>());
            _mockConfigurationService.SetupGet(x => x[It.Is<string>(s => s == "PingServiceBaseURL")]).Returns(It.IsAny<string>());
            _mockConfigurationService.SetupGet(x => x[It.Is<string>(s => s == "RDNSServiceBaseURL")]).Returns(It.IsAny<string>());
            _mockRestService.Setup(l => l.GetInternalServiceAsync<GeoIpModel>(It.IsAny<string>())).Returns(Task.FromResult(geoIpResults));
            _mockRestService.Setup(l => l.GetInternalServiceAsync<RdnsModel>(It.IsAny<string>())).Returns(Task.FromResult(rdnsResults));
            _mockRestService.Setup(l => l.GetInternalServiceAsync<PingModel>(It.IsAny<string>())).Returns(Task.FromResult(pingIpResults));

            var response = _networkListenerService.InvokeParallelServices(IpAddress, services);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.GeoIp);
            Assert.NotNull(response.Ping);
            Assert.NotNull(response.Rdns);

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

    }
}
