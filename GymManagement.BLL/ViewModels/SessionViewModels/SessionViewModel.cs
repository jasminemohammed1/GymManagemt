using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string TrainerName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; } = default!;
        public int AvailableSlots { get; set; }
        public int Capacity { get; set; }

        public TimeSpan Duration => EndDate - StartDate;
        public string TimeRangeDisplay => $"{StartDate: hh: mm tt} - {EndDate: hh mm tt}";
        public string DateDisplay => $"{StartDate: MMM dd , yyyy}";
        public string Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return "Up Coming";
                else if (EndDate > DateTime.Now && StartDate < DateTime.Now)
                    return "On Going";
                else
                    return "Completed";

            }
        }
            

    }
}
