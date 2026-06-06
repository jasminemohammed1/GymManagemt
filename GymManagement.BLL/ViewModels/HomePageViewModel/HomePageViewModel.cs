using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.HomePageViewModel
{
    public class HomePageViewModel
    {
        public int CountAllMember {  get; set; }
        public int CountActiveMember { get; set; }
        public int CountAllTrainers { get; set; }
        public int CountOnGoingSessions { get; set; }
        public int CountUpComingSesions { get; set; }
        public int CountCompletedSessions {  get; set; }
    }
}
