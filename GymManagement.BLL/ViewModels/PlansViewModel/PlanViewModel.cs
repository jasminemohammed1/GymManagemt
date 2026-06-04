using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.PlansViewModel
{
    public class PlanViewModel
    {
        public string Name { get; set; } = default!;
        public int Duration { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Id { get; set; }
        public bool IsActive { get; set; }

    }
}
