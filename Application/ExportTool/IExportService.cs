namespace ExportTool
{
    public interface IExportService<TModel, VModelExport> 
        where TModel : class
        where VModelExport : class
    {
        string PathToDirectory { get; set; }
        string FileName { get; set; }
        Task ExportForCsv(List<TModel> items);
        Task ImportFromCsvToDb();
        Task<string> ExportForJson(List<TModel> items);
        Task ImportFromJsonToDb(string itemsJson);
        Task<VModelExport> ConvertatorToExportModel(TModel item);
        Task<List<VModelExport>> ConvertatorToExportListModel(List<TModel> items);
    }
}
