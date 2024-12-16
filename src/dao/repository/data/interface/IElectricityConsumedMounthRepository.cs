public interface IElectricityConsumedMounthRepository
{
    Task Add(ElectricityConsumedMounthEntity electricityConsumedMounth);

    Task<List<ElectricityConsumedMounthEntity>> GetByPeriodDate(string periodDate);

    Task<List<ElectricityConsumedMounthEntity>> GetByPeriodDateStartEnd(string periodDateStart, string periodDateEnd);

    Task RenameClientById(Guid id, string name);

    Task Init();

}