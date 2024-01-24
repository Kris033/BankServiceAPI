using Services;
using Xunit;

namespace ServiceTests
{
    public class EquivalenceTests
    {
        [Fact]
        public async Task GetHashCodeNecessityPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var dictionaryAccountClients = await dataGenerator.GetDictionaryAccount(5);
            if (dictionaryAccountClients.Count == 0)
                dictionaryAccountClients = await dataGenerator.GenerationDictionaryAccount(5);
            var keyClient = dictionaryAccountClients.First().Key;

            //Assert
            Assert.Equal(
                keyClient
                    .GetHashCode(),
                dictionaryAccountClients
                    .First(d => d.Key == keyClient).Key
                    .GetHashCode());
        }
        [Fact]
        public async Task CheckAccountOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();
            ClientService clientService = new ClientService();

            //Act
            var dictionaryAccountClients = await dataGenerator.GetDictionaryAccount(5);
            if (dictionaryAccountClients.Count == 0) 
                dictionaryAccountClients = await dataGenerator.GenerationDictionaryAccount(5);

            //Assert
            foreach(var clientAndHisAccounts in dictionaryAccountClients )
            {
                foreach (var account in clientAndHisAccounts.Value)
                {
                    Assert.Equal(clientAndHisAccounts.Key, await clientService.Get(account.ClientId));
                }
            }
            
        }
        [Fact]
        public async Task CheckEmployeeOnEqualPositivTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = await employeeService.GetEmployees();
            var employee = employeeList.FirstOrDefault() 
                ?? await dataGenerator.GenerationEmployee();

            //Assert
            Assert.Equal(employee, employeeList[0]);
        }
        [Fact]
        public async Task GetHashCodeOnEqualPositivTest()
        {
            //Arrange
            EmployeeService employeeService = new EmployeeService();
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = await employeeService.GetEmployees();
            var employee = employeeList.FirstOrDefault()
                ?? await dataGenerator.GenerationEmployee();

            //Assert
            Assert.Equal(
                employee.GetHashCode(),
                employeeList
                    .First(e => e.Equals(employee))
                    .GetHashCode());
        }
    }
}
