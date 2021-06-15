using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers.Request
{
    public class CpuMetricCreateRequest
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }
}
