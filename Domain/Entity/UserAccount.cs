using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class UserAccount : Base
    {
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsEmailVerified { get; set; } = false;
        public string? ImgUrl { get; set; }
        public Role Role { get; set; }
        //public double? walletAmount { get; set; }
        //
        public List<EmailVerification>? EmailVerifications { get; set; }
        /*public int? TransportServiceId { get; set; }
        public TransportService? TransportService { get; set; }*/
        //public List<OrderFish>? OrderFishes { get; set; }
        public List<Subscription>? Subscriptions{ get; set; }
        public List<Payment>? Payments { get; set; }
        public List<Children>? Childrents { get; set; }
        public List<Schedule>? Schedules { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Notification>? Notifications { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Order>? Orders { get; set; }
    }
    public enum Role
    {
        Customer,
        Manager
    }
}
