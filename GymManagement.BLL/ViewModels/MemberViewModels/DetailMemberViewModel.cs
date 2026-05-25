using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class DetailMemberViewModel
    {
        public string Name { get; set; } = default!;
        public string ? Phone {  get; set; }
        public string Email { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string ?PlanName { get; set; }
        public DateTime ?MemberShipStartDate { get; set; }
        public DateTime? MemberShipEndDate { get; set; }
        public string Address { get; set; } = default!;
        public string  ?Photo { get; set; }

    }
}
