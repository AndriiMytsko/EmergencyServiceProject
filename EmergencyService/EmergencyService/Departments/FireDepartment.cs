using System.Threading;
using System.Threading.Tasks;

namespace EmergencyService.Departments
{
    class FireDepartment : Department
    {
        protected override async Task StartProcessAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(9000);
            });
        }
    }
}
