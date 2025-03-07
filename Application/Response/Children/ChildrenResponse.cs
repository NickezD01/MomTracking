using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response.Children
{
    public class ChildrenResponse
    {
        public int AccountId { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birth { get; set; }
    }
}
