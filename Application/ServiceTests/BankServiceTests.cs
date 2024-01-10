using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class BankServiceTests
    {
        [Fact]
        public void AddPersonInToBlackListTest()
        {
            //Arrange
            BankService service = new BankService();
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var employee = generator.GenerationEmployees(1).First();
            service.AddToBlackList((Person)employee);

            //Assert
            Assert.True(service.IsPersonInBlackList(employee));
        }
    }
}
