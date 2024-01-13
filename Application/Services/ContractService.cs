using Models;
using BankDbConnection;
using Bogus.DataSets;

namespace Services
{
    public class ContractService
    {
        public Contract? GetContract(Guid employeeGuid)
        {
            using var db = new BankContext();
            return db.Contract.FirstOrDefault(c => c.EmployeeId == employeeGuid);
        }
        public void SetContract(Guid idContract)
        {
            using var db = new BankContext();
            var contract = db.Contract.FirstOrDefault(c => c.Id == idContract);
            if (contract != null)
            {
                var employee = db.Employee.FirstOrDefault(e => e.Id == contract.EmployeeId);
                if (employee != null)
                {
                    var salary = db.Currency.FirstOrDefault(c => c.Id == employee!.CurrencyIdSalary);
                    if(salary != null)
                    {
                        contract.MainContract = $"{contract.NameCompany}\n" +
                        $"{contract.Adress}\n" +
                        $"{contract.City} {contract.MailIndex}\n\n" +
                        $"{employee.StartWorkDate}\n\n" +
                        "Договор о найме сотрудника\n\n" +
                        $"Между {contract.NameCompany}, в дальнейшем называемой \"Работодатель\", и {employee.Name}, в дальнейшем называемым \"Сотрудник\", заключен следующий договор:\n" +
                        $"1. Описание работы: Сотрудник принят на должность {employee.JobPositionType}. Он обязуется выполнять свои обязанности согласно требованиям, предъявляемым к этой должности Работодателем.\n" +
                        $"2. Заработная плата: Работодатель обязуется выплачивать Сотруднику заработную плату в размере {salary.Value} {salary.TypeCurrency} в соответствии с установленным графиком выплаты зарплаты.\n\n" +
                        $"3. График работы: Рабочее время {contract.WorkTime}.\n\n" +
                        $"4. Прочие условия: Стороны также договариваются о {contract.OtherCondition}.\n\n" +
                        $"5. Срок действия: Настоящий договор вступает в силу с момента подписания обеими сторонами и действует до {employee.EndContractDate}.\n\n" +
                        "6. Заключительные положения: Любые изменения или дополнения к настоящему договору действительны, если они оформлены в письменной форме и подписаны обеими сторонами.\n\n" +
                        "Подписи сторон:\n\n" +
                        "Работодатель: _______________________\n\n" +
                        "Сотрудник: _______________________\n\n";
                        db.SaveChanges();
                    }
                }
            }
        }
        public void AddContract(Contract contract)
        {
            using var db = new BankContext();
            if (db.Contract.Any(c => c.EmployeeId == contract.EmployeeId))
                throw new ArgumentException("Работник уже имеет контракт");
            db.Contract.Add(contract);
            db.SaveChanges();
        }
        public void UpdateContract(Contract contract)
        {
            using var db = new BankContext();
            if(!db.Contract.Any(c => c.EmployeeId == contract.EmployeeId))
                throw new ArgumentNullException("Идентификатор работника не совпадает с изменяемым работником");
            if(!db.Contract.Any(c => c.Id == contract.Id))
                throw new ArgumentNullException("Идентификатор контракта не совпадает с изменяемым контрактом");
            db.Contract.Update(contract);
            db.SaveChanges();
            
        }
        public void DeleteContract(Guid employeeGuid)
        {
            using var db = new BankContext();
            var contract = db.Contract.FirstOrDefault(c => c.EmployeeId == employeeGuid);
            if(contract != null)
            {
                db.Contract.Remove(contract);
                db.SaveChanges();
            }
        }
    }
}
