using Cards.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Tests
{
    public class TestContextDbFactory : IDbContextFactory<CardDbContext>
    {
        public CardDbContext CreateDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
