using System;
using System.Threading.Tasks;

namespace EmergencyService
{
    abstract class Department : IDepartment
    {
        public event Action<int, bool> EndProcessHandler;

        public async Task ProcessAsync(int callId)
        {
            try
            {
                await StartProcessAsync();
            }
            catch (Exception)
            {
                OnEndProcessHandler(callId, false);
                return;
            }

            OnEndProcessHandler(callId, true);
        }

        private void OnEndProcessHandler(int callId, bool isSuccess)
        {
            EndProcessHandler?.Invoke(callId, isSuccess);
        }

        protected abstract Task StartProcessAsync();
    }
}
