using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
   public class Trainer : GymUser
    {
        //hiredate = CreatedAt
        public Speciality speciality {  get; set; }
        public ICollection<Sessions> Sessions { get; set; }  = new List<Sessions>();    
    }
}
