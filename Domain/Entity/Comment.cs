using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Comment : Base
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } // Added content property
        public bool IsEdited { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public UserAccount? Account { get; set; }
        public Post? Post { get; set; }
    }
}
