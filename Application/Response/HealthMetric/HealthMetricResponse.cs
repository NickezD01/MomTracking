﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class HealthMetricResponse
    {
        public int ChildrenId { get; set; }
        public int PregnancyWeek { get; set; }
        public double HeadCircumference { get; set; }
        public double Weight { get; set; }
        public double Lenght { get; set; }
        public double SacDiameter { get; set; }
        public double HearRate { get; set; }
        public string? Note { get; set; }
    }
}
