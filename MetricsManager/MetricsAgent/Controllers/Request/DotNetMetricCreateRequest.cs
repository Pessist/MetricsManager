﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers.Request
{
    public class DotNetMetricCreateRequest
    {
        public DotNetMetricCreateRequest(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
        }

        public DateTimeOffset FromTime { get; set; }
        public DateTimeOffset ToTime { get; set; }
    }    
}
