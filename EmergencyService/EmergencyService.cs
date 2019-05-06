using System.Collections.Generic;
using System.Linq;

namespace EmergencyService
{
    class EmergencyService
    {
        private IList<IOperator> operators;
        private IDictionary<DepartmentType, IDepartment> departments;

        public EmergencyService(
            IList<IOperator> operators, 
            IDictionary<DepartmentType, IDepartment> departments)
        {
            this.operators = operators;
            this.departments = departments;
        }

        public void CallEmergencyService(DepartmentType type, string userName)
        {
            IOperator freeOperator;
            do
            {
                freeOperator = GetFreeOperator();
            } while (freeOperator == null);

            var department = GetDepartment(type);
            freeOperator.RecieveCall(department, userName);
        }

        private IOperator GetFreeOperator()
        {
            return operators.FirstOrDefault(x => !x.IsBusy);
        }

        private IDepartment GetDepartment(DepartmentType type)
        {
            return departments[type];
        }
    }
}