using System.Threading;
using System.Threading.Tasks;

namespace EmergencyService.Departments
{
    class PoliceDepartment : Department
    {
        protected override async Task StartProcessAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(6000);
            });
        }
    }
}
