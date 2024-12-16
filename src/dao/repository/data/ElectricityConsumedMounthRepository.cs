using Microsoft.EntityFrameworkCore;

public class ElectricityConsumedMounthRepository : IElectricityConsumedMounthRepository
{
    private readonly ServiceDbContext _dbContext;
    public ElectricityConsumedMounthRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //Not checked
    public async Task Add(ElectricityConsumedMounthEntity electricityConsumedMounth)
    {
        await _dbContext.ElectricityConsumedMounthEntities.AddAsync(electricityConsumedMounth);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Init()
    {

    }


    public async Task<List<ElectricityConsumedMounthEntity>> GetByPeriodDate(string periodDate)
    {
        return await _dbContext.ElectricityConsumedMounthEntities
            .AsNoTracking()
            .Include(e => e.ElectricyConsumedDays)
            .Include(e => e.Client)
            .Where(e => e.PeriodDate == periodDate)
            .ToListAsync();
    }
    //Not checked
    public async Task<List<ElectricityConsumedMounthEntity>> GetByPeriodDateStartEnd(string periodDateStart, string periodDateEnd)
    {
        return await _dbContext.ElectricityConsumedMounthEntities
            .AsNoTracking()
            .Include(e => e.ElectricyConsumedDays) // Подгружаем связанные дни
            .Include(e => e.Client)
            .Where(e => string.Compare(e.PeriodDate, periodDateStart) >= 0 &&
                        string.Compare(e.PeriodDate, periodDateEnd) <= 0)
            .ToListAsync();
    }

    public async Task RenameClientById(Guid id, string name)
    {
        var ecm = await _dbContext.ElectricityConsumedMounthEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(ecm => ecm.Id == id) ?? throw new Exception("Not found ElectricityConsumedMounth by id");
        await _dbContext.Clients
        .AsNoTracking()
        .Where(c => c.Id == ecm.ClientId)
        .ExecuteUpdateAsync(c =>
            c.SetProperty(c => c.Name, name)
        );
    }
}