using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class HealthMetric : Base
    {
        public int Id { get; set; }
        public int ChildrentId { get; set; }
        public int? PregnancyWeek { get; set; }
        public double? HeadCircumference { get; set; }
        public double? Weight { get; set; }
        public double? Lenght { get; set; }
        public double? BPD { get; set; }
        public double? AC { get; set; }
        public double? FL { get; set; }
        public double? HearRate { get; set; }
        public string? Note { get; set; }
        public bool Status { get; set; } = true;
        public bool IsAlert { get; set; }
        public Children? Childrent { get; set; }
        public GrowthIndex? GrowthIndex { get; set; }
    }
}
