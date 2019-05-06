using EmergencyService.Repository;
using System.Threading;

namespace EmergencyService
{
    class Operator : IOperator
    {
        private readonly ICallRepository callRepository;

        public string Name { get; }
        public bool IsBusy { get; private set; }

        public Operator(
            string name,
            ICallRepository callRepository)
        {
            Name = name;
            this.callRepository = callRepository;
        }

        public void RecieveCall(IDepartment department, string userName)
        {
            SetBusy();
            var callId = RegisterCallRecord(userName, department);
            department.EndProcessHandler += CloseCallRecord;
            department.ProcessAsync(callId);
            DismissBusy();
        }

        private int RegisterCallRecord(string userName, IDepartment department)
        {
            return callRepository.Add(userName, Name, department.GetType().Name, State.InProcess.ToString());
        }

        private void CloseCallRecord(int callId, bool success)
        {
            var status = success ? State.Completed : State.Failed;
            callRepository.UpdateState(callId, status.ToString());
        }

        private void SetBusy()
        {
            IsBusy = true;
        }

        private void DismissBusy()
        {
            IsBusy = false;
        }
    }
}