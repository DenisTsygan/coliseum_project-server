public class ElectricityConsumedDayRepository : IElectricityConsumedDayRepository
{
    private readonly ServiceDbContext _dbContext;
    public ElectricityConsumedDayRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //No check
    public async Task Add(Guid mounthId, ElectricityConsumedDayEntity electricityConsumedDay)
    {
        var ecm = _dbContext.ElectricityConsumedMounthEntities.FirstOrDefault(ecm => ecm.Id == mounthId) ?? throw new Exception("Not found ElectricityMounth");
        ecm.ElectricyConsumedDays.Add(electricityConsumedDay);
        await _dbContext.SaveChangesAsync();

        //await _dbContext.ElectricityConsumedDayEntities.AddAsync(electricityConsumedDay);
        //await _dbContext.SaveChangesAsync();
    }

    public async Task Init()
    {


    }
}