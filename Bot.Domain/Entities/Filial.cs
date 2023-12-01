using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Domain.Entities
{
    public class Filial
    {
        public int Id { get; set; }
        public string NameEN { get; set; } = null!;
        public string NameUZ { get; set; } = null!;
        public string NameRU { get; set; } = null!;

        public string DescriptionUZ { get; set; } = null!;
        public string DescriptionRU { get; set; } = null!;
        public string DescriptionEN { get; set; } = null!;
        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
