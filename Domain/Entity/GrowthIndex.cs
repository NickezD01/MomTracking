using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class GrowthIndex : Base
    {
        public int Id { get; set; }
        public int HealthMetricId { get; set; }
        public double DevelopmentScore { get; set; }
        public double GrowthRate { get; set; }
        public HealthMetric? HealthMetric { get; set; }
    }
}
