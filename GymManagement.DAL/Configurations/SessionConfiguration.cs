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
    public class SessionConfiguration : IEntityTypeConfiguration<Sessions>
    {
        public void Configure(EntityTypeBuilder<Sessions> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
                tb.HasCheckConstraint("SessionCapacityChech", "Capacity between  1 and 25");
            });

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.Trainer)
                  .WithMany(x => x.Sessions)
                  .HasForeignKey(x => x.TrainerId);
        }
    }
}
