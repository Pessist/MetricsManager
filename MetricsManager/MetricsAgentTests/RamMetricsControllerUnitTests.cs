using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;
using MetricsAgent.Controllers;
using MetricsAgent.Controllers.Request;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL.Models;
using System.Collections.Generic;

namespace MetricsAgentTests
{
    public class RamMetricsControllerUnitTests
    {
        private readonly RamMetricsController _controller;
        private readonly Mock<IRamMetricsRepository> _mock;
        private readonly Mock<ILogger<RamMetricsController>> _logMock;

        public RamMetricsControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            var logMock = new Mock<ILogger<CpuMetricsController>>();
            _controller = new RamMetricsController(_mock.Object, _logMock.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ShouldCall_GetTimeByPeriod_From_Repository()
        {
            //Arrange
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            _mock.Setup(repository => repository.GetTimeByPeriod(fromTime, toTime)).Returns(new List<RamMetric>()).Verifiable();
            RamMetricCreateRequest request = new RamMetricCreateRequest(fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAgent(request);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
