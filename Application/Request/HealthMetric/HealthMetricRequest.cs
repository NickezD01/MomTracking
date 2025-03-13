using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.HealthMetric
{
    public class HealthMetricRequest
    {
        public int ChildrentId { get; set; }
        public int PregnancyWeek { get; set; }
        public double HeadCircumference { get; set; }
        public double Weight { get; set; }
        public double Lenght { get; set; }
        public double? BPD { get; set; }
        public double? AC { get; set; }
        public double? FL { get; set; }
        public double HearRate { get; set; }
        public string? Note { get; set; }
    }
}
