using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Domain.Entities
{
    public class ProductType
    {
        public int Id { get; set; }
        public string NameUZ { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public string NameRU { get; set; } = null!;
    }
}
