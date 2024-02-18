using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Infrastructure
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
