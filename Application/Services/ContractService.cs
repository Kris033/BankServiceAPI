using Models;
using BankDbConnection;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class ContractService : IContractService
    {
        public async Task<Contract?> Get(Guid contractId)
        {
            using var db = new BankContext();
            return await db.Contract.FirstOrDefaultAsync(c => c.Id == contractId);
        }
        public async Task SetContract(Guid idContract)
        {
            using var db = new BankContext();
            var contract = await db.Contract.FirstOrDefaultAsync(c => c.Id == idContract);
            if (contract != null)
            {
                var employee = await db.Employee.FirstOrDefaultAsync(e => e.Id == contract.EmployeeId);
                if (employee != null)
                {
                    var salary = await db.Currency.FirstOrDefaultAsync(c => c.Id == employee!.CurrencyIdSalary);
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
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task Add(Contract contract)
        {
            using var db = new BankContext();
            if (await db.Contract.AnyAsync(c => c.EmployeeId == contract.EmployeeId))
                throw new ArgumentException("Работник уже имеет контракт");
            db.Contract.Add(contract);
            await db.SaveChangesAsync();
        }
        public async Task Update(Contract contract)
        {
            using var db = new BankContext();
            if(!await db.Contract.AnyAsync(c => c.EmployeeId == contract.EmployeeId))
                throw new ArgumentNullException("Идентификатор работника не совпадает с изменяемым работником");
            if(!await db.Contract.AnyAsync(c => c.Id == contract.Id))
                throw new ArgumentNullException("Идентификатор контракта не совпадает с изменяемым контрактом");
            db.Contract.Update(contract);
            await db.SaveChangesAsync();
            
        }
        public async Task Delete(Guid idContract)
        {
            using var db = new BankContext();
            var contract = await db.Contract.FirstOrDefaultAsync(c => c.Id == idContract);
            if (contract != null)
            {
                db.Contract.Remove(contract);
                await db.SaveChangesAsync();
            }
        }
    }
}
