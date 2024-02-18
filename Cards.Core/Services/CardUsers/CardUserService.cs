using Cards.Core.Infrastructure;
using Cards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Services.CardUsers
{
    public class CardUserService(CardDbContext _context) : ICardUserService
    {
        public async Task<CardUser?> Create(CardUser user)
        {
            _context.CardUsers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<CardUser?> GetByEmail(string email)
        {
            return await _context.CardUsers.FirstOrDefaultAsync(a => a.Email==email);
        }

        public async Task<CardUser?> GetById(string id)
        {
            return await _context.CardUsers.FirstOrDefaultAsync(a=>a.Id == id);
        }

        
    }
}
