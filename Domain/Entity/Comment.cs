using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Comment : Base
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PostId { get; set; }
        public bool IsEdited { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public UserAccount? Account { get; set; }
        public Post? Post { get; set; }
    }
}
