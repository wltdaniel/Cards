using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Models
{
    public class CardUser
    {
       public CardUser() { 
            Id=Guid.NewGuid().ToString();  
        }  
        public  string? Id { get; set; }
        public string? Email { get; set; }
        public ICollection<CardItem>? CardItems { get; set; }

    }
}
