﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Schedule
{
    public class ScheduleRequest
    {
        public string? Description { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
