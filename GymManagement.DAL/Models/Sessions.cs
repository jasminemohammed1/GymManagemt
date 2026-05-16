using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
   public class Sessions : BaseEntity
    {
        public string Description { get; set; } = default!;
         public int Capacity { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public Trainer Trainer { get; set; } = default!;
        public int TrainerId { get;  set; }
    }
}
