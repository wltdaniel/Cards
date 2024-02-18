using Cards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Services.CardUsers
{
    public interface ICardUserService
    {
       Task<CardUser?> GetById(string id);
        Task<CardUser?> GetByEmail(string email);
        Task<CardUser?> Create(CardUser user);
    }
}
