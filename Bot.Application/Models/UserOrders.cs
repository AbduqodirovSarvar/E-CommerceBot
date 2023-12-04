using Bot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Models
{
    public class UserOrders
    {
        public long UserId { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
