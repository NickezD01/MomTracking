using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Payment : Base
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid OrderId { get; set; }
        public Guid MethodId { get; set; }
        public Guid TransactionId { get; set; }
        public string Note { get; set; }
        public Order? Order { get; set; }
        public UserAccount? Account { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public TransactionHistory? TransactionHistory { get; set; }
    }
}
