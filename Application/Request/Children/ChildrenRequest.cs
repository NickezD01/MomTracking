using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Children
{
    public class ChildrenRequest
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birth { get; set; }
        
    }
}
