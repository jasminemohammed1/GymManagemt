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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();
            builder.Property(x => x.Phone).HasColumnType("varchar").HasMaxLength(11);
            builder.OwnsOne(x => x.Address, OTB =>
            {
                OTB.Property(x => x.City).HasMaxLength(30).HasColumnType("varchar").HasColumnName("City");
                OTB.Property(x => x.Street).HasMaxLength(30).HasColumnType("varchar").HasColumnName("Street");

            });

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone like '010%' or Phone like '012%' or Phone like '015%' or Phone like '011%' or Phone like '010%'");
                
            });
        }
    }

}
