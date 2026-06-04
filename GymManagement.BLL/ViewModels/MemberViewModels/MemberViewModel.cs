using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public string ?Photo { get; set; }
        public string Name { get; set; } = default!;

        // details member 
        public string ? MemberShipStartDate { get; set; }
        public string? MemberShipEndDate { get; set; }
        public string ?PlanName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Addresss { get; set; }

    }
}
