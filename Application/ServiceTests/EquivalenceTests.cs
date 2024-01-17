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
            var dictionaryAccountClients = await dataGenerator.GenerationDictionaryAccount(5);
            var keyClient = dictionaryAccountClients.FirstOrDefault().Key;

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

            //Act
            var dictionaryAccountClients = await dataGenerator.GenerationDictionaryAccount(5);

            //Assert
            foreach(var clientAndHisAccounts in dictionaryAccountClients )
            {
                foreach (var account in clientAndHisAccounts.Value)
                {
                    Assert.Equal(clientAndHisAccounts.Key, account.Client);
                }
            }
            
        }
        [Fact]
        public async Task CheckEmployeeOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = await dataGenerator.GenerationEmployees(5);
            var employee = employeeList.FirstOrDefault();

            //Assert
            Assert.Equal(employee, employeeList[0]);
        }
        [Fact]
        public async Task GetHashCodeOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = await dataGenerator.GenerationEmployees(5);
            var employee = employeeList.First();

            //Assert
            Assert.Equal(
                employee.GetHashCode(),
                employeeList
                    .First(e => e.Equals(employee))
                    .GetHashCode());
        }
    }
}
