using System.Threading;
using System.Threading.Tasks;

namespace EmergencyService
{
    class MedicalDepartment : Department
    {
        protected override async Task StartProcessAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
            });
        }
    }
}
