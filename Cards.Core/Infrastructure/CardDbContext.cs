using Cards.Core.Configurations;
using Cards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Infrastructure
{
    public class CardDbContext(DbContextOptions<CardDbContext> options) : DbContext(options)
    {

        public DbSet<CardItem> CardItems => Set<CardItem>();
        public DbSet<CardUser> CardUsers => Set<CardUser>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
         
           builder.ApplyConfiguration(new CardUserConfiguration());
           builder.ApplyConfiguration(new CardDbConfiguration());
       
        }
    }
}
