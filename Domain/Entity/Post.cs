using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Post : Base
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public UserAccount? Account { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
