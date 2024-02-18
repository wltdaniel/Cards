using Cards.Core.Models;
using Cards.Core.Services.CardUsers;
using Microsoft.EntityFrameworkCore;

namespace Cards.Core.Infrastructure
{
    public class DatabaseSeeder(ICardUserService cardUserService, CardDbContext cardDbContext) : IDatabaseSeeder
    {
        private readonly CardDbContext _cardDbContext = cardDbContext;
        private readonly ICardUserService _cardUserService = cardUserService;

        public async Task SeedAsync()
        {
            await _cardDbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
  
        }

       

        private async Task SeedDefaultUsersAsync()
        {
            if (!await _cardDbContext.CardUsers.AnyAsync())
            {
                await EnsureCardUserAsync("admin@card.com");
                await EnsureCardUserAsync("member@card.com");
            }
        }

        private async Task EnsureCardUserAsync(string email)
        {
            if (await _cardUserService.GetByEmail(email) == null)
            {

                var cardUser = new CardUser
                {
                    Email = email
                };                
              await _cardUserService.Create(cardUser);
            }
        }

        
    }
}
