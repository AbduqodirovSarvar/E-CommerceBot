using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? ImagePath { get; set; }
        public int TypeId { get; set; }
        public ProductType? Type { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
