using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Children
{
    public class ChildrenUpdateRequest
    {
       
        public string FullName { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birth { get; set; }

    }
}
