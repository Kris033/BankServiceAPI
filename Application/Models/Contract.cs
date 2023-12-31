namespace Models
{
    public struct Contract
    {
        public Contract(
            Employee employee,
            string nameCompany,
            string adress,
            string city,
            string mailIndex,
            string workTime,
            string otherCondition
            )
        {
            Employee = employee;
            NameCompany = nameCompany;
            Adress = adress;
            City = city;
            MailIndex = mailIndex;
            WorkTime = workTime;
            OtherCondition = otherCondition;
        }
        public Employee Employee { get; set; }
        public string NameCompany { get; private set; }
        public string Adress { get; private set; }
        public string City { get; private set; }
        public string MailIndex { get; private set; }
        public string WorkTime { get; private set; }
        public string OtherCondition { get; private set; }
        public string GetContract()
            => $"{NameCompany}\n" +
               $"{Adress}\n" +
               $"{City} {MailIndex}\n\n" +
               $"{Employee.GetStartDateWork}\n\n" +
               "Договор о найме сотрудника\n\n" +
               $"Между {NameCompany}, в дальнейшем называемой \"Работодатель\", и {Employee.Name}, в дальнейшем называемым \"Сотрудник\", заключен следующий договор:\n" +
               $"1. Описание работы: Сотрудник принят на должность {Employee.JobPositionType}. Он обязуется выполнять свои обязанности согласно требованиям, предъявляемым к этой должности Работодателем.\n" +
               $"2. Заработная плата: Работодатель обязуется выплачивать Сотруднику заработную плату в размере {Employee.Salary.Value} {Employee.Salary.TypeCurrency} в соответствии с установленным графиком выплаты зарплаты.\n\n" +
               $"3. График работы: Рабочее время {WorkTime}.\n\n" +
               $"4. Прочие условия: Стороны также договариваются о {OtherCondition}.\n\n" +
               $"5. Срок действия: Настоящий договор вступает в силу с момента подписания обеими сторонами и действует до {Employee.GetEndContractDate}.\n\n" +
               "6. Заключительные положения: Любые изменения или дополнения к настоящему договору действительны, если они оформлены в письменной форме и подписаны обеими сторонами.\n\n" +
               "Подписи сторон:\n\n" +
               "Работодатель: _______________________\n\n" +
               "Сотрудник: _______________________\n\n";
    }
}
