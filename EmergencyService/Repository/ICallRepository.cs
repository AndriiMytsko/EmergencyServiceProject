using EmergencyService.Entity;
using System;
using System.Collections.Generic;

namespace EmergencyService.Repository
{
    interface ICallRepository
    {
        IList<Call> GetAll();
        void UpdateState(int callId, string state);
        int Add(string userName, string operatorName, string departmentName, string state);
    }
}
