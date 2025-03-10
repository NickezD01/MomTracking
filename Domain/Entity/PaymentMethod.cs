using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        public List<Payment>? Payments { get; set; }

        public string OrderType { get; set; }
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
    }
}
