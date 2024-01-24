using Models;

namespace Services.Interfaces
{
    public interface IContractService : IModelService<Contract>
    {
        Task SetContract(Guid idContract);
    }
}
