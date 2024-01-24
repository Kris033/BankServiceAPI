using ExportTool;
using Microsoft.AspNetCore.Mvc;
using Models.Exports;
using Models.Requests;
using Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("Bank/[controller]/[action]")]
    public class ListsController : Controller
    {
        [HttpGet]
        public async Task<IEnumerable<ClientExportModel>> GetClients([FromQuery] GetFilterRequest? filterRequest = null)
           => await new ExportClientsService()
                .ConvertatorToExportListModel(
                await new ClientService()
               .GetClients(filterRequest));
        
        [HttpGet]
        public async Task<IEnumerable<EmployeeExportModel>> GetEmployees([FromQuery] GetFilterRequest? filterRequest = null)
            => await new ExportEmployeeService().ConvertatorToExportListModel(
                await new EmployeeService().GetEmployees(filterRequest));

        [HttpGet]
        public async Task<IEnumerable<AccountExportModel>> GetClientAccounts(Guid idClient)
            => await new ExportAccountsService()
            .ConvertatorToExportListModel(
                await new AccountService()
                .GetAccountsClient(idClient));
        

    }
}
