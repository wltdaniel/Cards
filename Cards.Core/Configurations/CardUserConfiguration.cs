using Cards.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Configurations
{
    public class CardUserConfiguration : IEntityTypeConfiguration<CardUser>
    {
        public void Configure(EntityTypeBuilder<CardUser> builder)
        {
            builder.ToTable("CardUser");         
        }
    }
}
