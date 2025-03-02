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
        public DateTime PregnancyWeek { get; set; }
        public double HeadCircumference { get; set; }
        public double Weight { get; set; }
        public double Lenght { get; set; }
        public double SacDiameter { get; set; }
        public double HearRate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public bool IsAlert { get; set; }
        public Children? Childrent { get; set; }
        public GrowthIndex? GrowthIndex { get; set; }
    }
}
