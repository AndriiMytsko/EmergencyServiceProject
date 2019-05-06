using System;
using System.Threading.Tasks;

namespace EmergencyService
{
    interface IDepartment
    {
        event Action<int, bool> EndProcessHandler;
        Task ProcessAsync(int callId);
    }
}
