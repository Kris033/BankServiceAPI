namespace Services.Storage
{
    public interface IStorage<TModel> where TModel : class
    {
        void Add(TModel item);
        void Update(TModel item);
        void Delete(TModel item);
    }
}
