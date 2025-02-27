using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.HealthMetric
{
    public class HealthMetricRequest
    {
        public double HeadCircumference { get; set; }
        public double Weight { get; set; }
        public double Lenght { get; set; }
        public double SacDiameter { get; set; }
        public double HearRate { get; set; }
    }
}
