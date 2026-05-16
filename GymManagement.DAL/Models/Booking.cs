using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Booking
    {
        //SK
        public int Id { get; set; }
        public bool IsAttended {  get; set; }
        public DateTime BookingDate { get; set; }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }
        public Sessions Session { get; set; } = null!;
        public int SessionId { get; set; }

    }
}
