using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Notification
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Content { get; set; }
        public NotiType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
        public UserAccount Account { get; set; }
    }
    public enum NotiType
    {
        Reminders,
        Alerts
    }
}
