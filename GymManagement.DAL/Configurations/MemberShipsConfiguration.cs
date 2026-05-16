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
    public class MemberShipsConfiguration : IEntityTypeConfiguration<MemberShips>
    {
        public void Configure(EntityTypeBuilder<MemberShips> builder)
        {
            builder.HasOne(x => x.Member)
                    .WithMany(x => x.MemberShips)
                    .HasForeignKey(x => x.MemberId);

            builder.HasOne(x => x.Plan)
                    .WithMany(x => x.MemberShips)
                    .HasForeignKey(x => x.PlanId);


        }
    }
}
