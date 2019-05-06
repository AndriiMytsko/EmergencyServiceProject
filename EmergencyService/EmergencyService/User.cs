using System;

namespace EmergencyService
{
    class User
    {
        private readonly EmergencyService _emergencyService;

        public string Name { get; set; }

        public User(EmergencyService emergencyService)
        {
            _emergencyService = emergencyService;
        }

        public void Call(DepartmentType department)
        {
            _emergencyService.CallEmergencyService(department, Name);
        }
    }
}
