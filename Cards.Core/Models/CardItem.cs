using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Models
{
    public class CardItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public Status Status { get; set; } = Status.ToDo;
        public string? CardOwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public CardUser User { get; set; } = null!;

    }
}
