using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.PlansViewModel
{
    public class PlanToUpdateViewModel
    {
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = " The description length must be at least 5 characters and at most 200")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Duration days is required")]
        [Range(1,365,ErrorMessage ="Duration days must be between 1 and 365")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage ="The Price Field is required")]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

    }
}
