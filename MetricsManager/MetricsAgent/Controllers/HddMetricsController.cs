using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.Controllers.Request;
using MetricsAgent.Controllers.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private IHddMetricsRepository _repository;
        private ILogger<HddMetricsController> _logger;

        public HddMetricsController(IHddMetricsRepository repository, ILogger<HddMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("left/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] HddMetricCreateRequest request)
        {
            var metrics = _repository.GetTimeByPeriod(request.FromTime, request.ToTime);
            var response = new TimePeriodHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto
                {
                    Time = DateTimeOffset.FromUnixTimeSeconds(metric.Time),
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            _logger.LogInformation($"Get CPU metrics: From Time = {request.FromTime} To Time = {request.ToTime}");

            return Ok(response);
        }
    }
}

