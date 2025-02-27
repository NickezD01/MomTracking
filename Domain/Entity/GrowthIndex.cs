using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class GrowthIndex : Base
    {
        public Guid Id { get; set; }
        public Guid HealthMetricId { get; set; }
        public double DevelopmentScore { get; set; }
        public double GrowthRate { get; set; }
        public HealthMetric? HealthMetric { get; set; }
    }
}
