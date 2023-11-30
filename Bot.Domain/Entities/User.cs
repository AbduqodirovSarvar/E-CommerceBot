using Bot.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = null!;
        public Language Language { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
