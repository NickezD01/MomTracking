using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.UserAccount
{
    public class UpdateUserRoleRequest
    {
        //public int Id { get; set; }
        public Role Role { get; set; }
    }
}
