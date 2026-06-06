using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.HomePageViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.interfaces
{
    public  interface IHomePageService
    {
        public Task<Result<HomePageViewModel>> GetAnalyticAsync(CancellationToken ct = default);
    }
}
