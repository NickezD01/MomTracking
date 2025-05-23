﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Children
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birth { get; set; }
        public UserAccount Account { get; set; }

        public List<HealthMetric> HealthMetrics { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
