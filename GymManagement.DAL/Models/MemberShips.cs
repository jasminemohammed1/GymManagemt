using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class MemberShips : BaseEntity
    {
        
        
        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }
       // StartDate = CreatedAt
        public DateTime EndDate { get; set; }
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;
    }
}
