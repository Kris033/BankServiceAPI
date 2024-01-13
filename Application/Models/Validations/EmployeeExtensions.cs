namespace Models.Validations
{
    public static partial class EmployeeExtensions
    {
        public static void Validation(this Employee employee)
        {
            employee.ValidationPerson();
            var dateTimeToday = DateTime.Today;
            var dateTimeStartWork = employee.StartWorkDate.ToDateTime(new TimeOnly());
            var dateTimeEndWork = employee.EndContractDate.ToDateTime(new TimeOnly());
            if (dateTimeStartWork > dateTimeEndWork)
                throw new ArgumentOutOfRangeException("Дата начало работы не может быть больше чем дата конца работы");
            if (dateTimeEndWork <= dateTimeToday)
                throw new ArgumentOutOfRangeException("Контракт у данного работника истёк.");
        }
    }
}
