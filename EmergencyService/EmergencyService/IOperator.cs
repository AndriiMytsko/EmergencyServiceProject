namespace EmergencyService
{
    interface IOperator
    {
        bool IsBusy { get; }
        string Name { get; }
        void RecieveCall(IDepartment department, string userName);
    }
}
