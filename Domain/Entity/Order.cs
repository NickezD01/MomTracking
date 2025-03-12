using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        //public int? PlanId { get; set; }
        public int SubscriptionId { get; set; }
        public decimal? Price { get; set; }
        public string Note { get; set; }
        //public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending, Paid, Canceled
        public bool IsDelete { get; set; }
        public UserAccount? UserAccount { get; set; }
        public Subscription Subscription { get; set; }
        //public SubscriptionPlan? SubscriptionPlans { get; set; }
        public List<Payment>? Payments { get; set; }
    }
    //public enum OrderStatus
    //{
    //    Pending,  // Chờ thanh toán qua VNPay
    //    Paid,     // Đã thanh toán
    //    Canceled  // Đã hủy
    //}

}
