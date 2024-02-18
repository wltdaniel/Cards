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
    public class CardDbConfiguration : IEntityTypeConfiguration<CardItem>
    {
        public void Configure(EntityTypeBuilder<CardItem> builder)
        {
            builder.ToTable("CardItem");
            builder.HasOne<CardUser>()
                       .WithMany()
                       .HasForeignKey(t => t.CardOwnerId);
            builder.Property(p => p.CardOwnerId).IsRequired();
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
