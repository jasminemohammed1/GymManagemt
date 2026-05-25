using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasOne(x => x.Session)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.SessionId);

            builder.HasOne(x => x.Member)
                  .WithMany(x => x.Bookings)
                  .HasForeignKey(x => x.MemberId);

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");
            builder.Ignore(x => x.Id);
            builder.HasKey(x => new {x.SessionId, x.MemberId});
        }
    }
}
