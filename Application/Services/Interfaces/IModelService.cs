namespace Services.Interfaces
{
    public interface IModelService<TModel>
        where TModel : class
    {
        Task<TModel?> Get(Guid id);
        Task Add(TModel model);
        Task Update(TModel model);
        Task Delete(Guid id);
    }
}