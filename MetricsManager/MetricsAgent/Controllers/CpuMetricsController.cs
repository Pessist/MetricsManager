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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private ICpuMetricsRepository _repository;
        private ILogger<CpuMetricsController> _logger;

        public CpuMetricsController(ICpuMetricsRepository repository, ILogger<CpuMetricsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] CpuMetricCreateRequest request)
        {
            var metrics = _repository.GetTimeByPeriod(request.FromTime, request.ToTime);
            var response = new TimePeriodCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto 
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
