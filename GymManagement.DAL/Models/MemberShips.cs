using GYMProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class MemberShips
    {
        // SK to allow same member to subscribe same plan that have been registered
        public int Id { get; set; }
        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
