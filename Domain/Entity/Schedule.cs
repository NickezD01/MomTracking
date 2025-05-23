﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Schedule : Base
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public bool IsNoti { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Description { get; set; }
        public UserAccount? Account { get; set; }
    }
}
