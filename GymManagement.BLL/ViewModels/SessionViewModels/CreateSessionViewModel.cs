using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.SessionViewModels
{
    public class CreateSessionViewModel
    {
        [Required(ErrorMessage = "Category is required")]
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Trainer is required")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        [StringLength(500, MinimumLength =10 , ErrorMessage = "Description length must be between 10 - 500")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Capacity field is required")]
        [Range(1,25,ErrorMessage ="Capacity must be between 1 - 25")]
        public int Capacity { get; set; }

        [Required(ErrorMessage ="Start Date is required")]
        [Display(Name = "StartDate & Time")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required")]
        [Display(Name = "EndDate & Time")]
        public  DateTime EndDate {get;set;}
    }
}
