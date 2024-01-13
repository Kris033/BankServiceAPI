using Services;
using Xunit;

namespace ServiceTests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var dictionaryAccountClients = dataGenerator.GenerationDictionaryAccount(100);
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
        public void CheckAccountOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var dictionaryAccountClients = dataGenerator.GenerationDictionaryAccount(100);

            //Assert
            foreach(var clientAndHisAccounts in dictionaryAccountClients )
            {
                foreach (var account in clientAndHisAccounts.Value)
                {
                    //Assert.Equal(clientAndHisAccounts.Key, account.Client);
                }
            }
            
        }
        [Fact]
        public void CheckEmployeeOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = dataGenerator.GenerationEmployees(100);
            var employee = employeeList.FirstOrDefault();

            //Assert
            Assert.Equal(employee, employeeList[0]);
        }
        [Fact]
        public void GetHashCodeOnEqualPositivTest()
        {
            //Arrange
            TestDataGenerator dataGenerator = new TestDataGenerator();

            //Act
            var employeeList = dataGenerator.GenerationEmployees(100);
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
