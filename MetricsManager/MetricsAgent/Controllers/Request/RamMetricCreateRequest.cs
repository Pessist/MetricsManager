using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers.Request
{
    public class RamMetricCreateRequest
    {
        public RamMetricCreateRequest(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
        }

        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
