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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private IRamMetricsRepository _repository;
        private ILogger<RamMetricsController> _logger;

        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] RamMetricCreateRequest request)
        {
            var metrics = _repository.GetTimeByPeriod(request.FromTime, request.ToTime);
            var response = new TimePeriodRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto
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
