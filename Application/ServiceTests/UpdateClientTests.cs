using Models;
using Services;
using Xunit;

namespace ServiceTests
{
    public class UpdateClientTests
    {
        [Fact]
        public void UpdateAccountPositiveTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Act
            var clientAccounts = clientService.GetFirstClientWithAccounts();
            var firstAccount = clientAccounts.Value.FirstOrDefault();
            firstAccount!.BankAccount.ExChange(CurrencyType.LeiMD);
            clientService.ChangeAccountClient(clientAccounts.Key, firstAccount);
        }
        [Fact]
        public void UpdateAccountThrowTest()
        {
            //Arrange
            ClientService clientService = new ClientService();

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                var clientAccounts = clientService.GetFirstClientWithAccounts();
                var firstAccount = clientAccounts.Value.FirstOrDefault();
                firstAccount!.BankAccount.ExChange(CurrencyType.LeiMD);
                var newAccount = new Account(clientAccounts.Key, "2135 8693 2112 7652", firstAccount.BankAccount);
                clientService.ChangeAccountClient(clientAccounts.Key, newAccount);
            });
            
        }
    }
}
