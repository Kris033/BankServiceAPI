namespace Models.Validations
{
    public static partial class EmployeeExtensions
    {
        public static void Validation(this Employee employee)
        {
            employee.ValidationPerson();
            if (employee.StartWorkDate > employee.EndContractDate)
                throw new ArgumentOutOfRangeException("Дата начало работы не может быть больше чем дата конца работы");
            if (employee.EndContractDate <= DateTime.Today)
                throw new ArgumentOutOfRangeException("Контракт у данного работника истёк.");
        }
    }
}
