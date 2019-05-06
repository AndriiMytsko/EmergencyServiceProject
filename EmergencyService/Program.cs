using EmergencyService.Departments;
using EmergencyService.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmergencyService
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            CallRepository callRepository = new CallRepository(connectionString);

            IList<IOperator> operators = new List<IOperator>
            {
                new Operator("Liza", callRepository),
                new Operator("Homer", callRepository),
                new Operator("Bart", callRepository)
            };

            Dictionary<DepartmentType, IDepartment> departments =
                new Dictionary<DepartmentType, IDepartment>();

            var fire = new FireDepartment();
            var police = new PoliceDepartment();
            var medical = new MedicalDepartment();

            departments.Add(DepartmentType.Fire, fire);
            departments.Add(DepartmentType.Police, police);
            departments.Add(DepartmentType.Medical, medical);

            var emergencyService = new EmergencyService(operators, departments);

            var users = CreateUsers(40, emergencyService);
            MakeRandomCall(users);

            while(true)
            {
                Thread.Sleep(3000);
                Console.Clear();

                var calls = callRepository.GetAll();

                foreach (var c in calls)
                {
                    Console.WriteLine($"Id: {c.Id}\tUserName: {c.UserName}\tIDepartment: {c.DepartmentName}" +
                        $"\tOperatorName: {c.OperatorName}\tState: {c.State}");
                }
            }
        }

        private static IList<User> CreateUsers(int count, EmergencyService service)
        {
            var users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                var user = new User(service)
                {
                    Name = GetUserName()
                };
                users.Add(user);
            }
            return users;
        }

        private static void MakeRandomCall(IList<User> users)
        {
            Parallel.ForEach(users, user =>
            {
                var department = GetDepartment();
                user.Call(department);
            });
        }

        static void ShowAllCalls()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var callRepository = new CallRepository(connectionString);
            var calls = callRepository.GetAll();

            var callsDescId = calls.OrderByDescending(c => c.Id);

            foreach (var c in callsDescId)
            {
                Console.WriteLine($"Id: {c.Id}\tUserName: {c.UserName}\tIDepartment: {c.DepartmentName}" +
                    $"\tOperatorName: {c.OperatorName}\tState: {c.State}");
            }
        }

        private static string GetUserName()
        {
            string[] names = { "Liza", "Bob", "Max", "Yra", "Bart",
                    "Oleh", "Vova", "Nazar", "Homer" , "Polina"};

            Thread.Sleep(10);
            int index = new Random().Next(names.Length);
            return names[index];
        }

        public static DepartmentType GetDepartment()
        {
            var department = Enum.GetValues(typeof(DepartmentType));

            DepartmentType dep = (DepartmentType)department.GetValue(new Random().Next(department.Length));
            return dep;
        }
    }
}