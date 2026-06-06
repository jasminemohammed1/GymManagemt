using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.SessionViewModels
{
   public class SessionToUpdateViewModel
    {
        [Required(ErrorMessage = "Trainer is required")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }
        [Required(ErrorMessage = "Description field is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description length must be between 10 - 500")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "StartDate & Time")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required")]
        [Display(Name = "EndDate & Time")]
        public DateTime EndDate { get; set; }

    }
}
