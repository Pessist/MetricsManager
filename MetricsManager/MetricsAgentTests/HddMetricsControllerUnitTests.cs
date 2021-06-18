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
    public class HddMetricsControllerUnitTests
    {
        private readonly HddMetricsController _controller;
        private readonly Mock<IHddMetricsRepository> _mock;

        public HddMetricsControllerUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();
            var logMock = new Mock<ILogger<HddMetricsController>>();
            _controller = new HddMetricsController(_mock.Object, logMock.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ShouldCall_GetTimeByPeriod_From_Repository()
        {
            //Arrange
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            _mock.Setup(repository => repository.GetTimeByPeriod(fromTime, toTime)).Returns(new List<HddMetric>()).Verifiable();
            HddMetricCreateRequest request = new HddMetricCreateRequest(fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAgent(request);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
