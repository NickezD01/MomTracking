using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Payment : Base
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int OrderId { get; set; }
        public int MethodId { get; set; }
        public int TransactionId { get; set; }
        public string Note { get; set; }
        public Order? Order { get; set; }
        public UserAccount? Account { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public TransactionHistory? TransactionHistory { get; set; }
    }
}
