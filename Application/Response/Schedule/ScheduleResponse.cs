using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Schedule
{
    public class ScheduleResponse
    {
        public int AccountId { get; set; }
        public int Id { get; set; }
        public bool IsNoti { get; set; }
        public string? Description { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
