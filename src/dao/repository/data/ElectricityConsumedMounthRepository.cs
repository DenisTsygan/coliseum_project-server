using Microsoft.EntityFrameworkCore;

public class ElectricityConsumedMounthRepository : IElectricityConsumedMounthRepository
{
    private readonly ServiceDbContext _dbContext;
    public ElectricityConsumedMounthRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(ElectricityConsumedMounthEntity electricityConsumedMounth)
    {
        await _dbContext.ElectricityConsumedMounthEntities.AddAsync(electricityConsumedMounth);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ElectricityConsumedMounthEntity> GetByPeriodDate(string periodDate)
    {
        return await _dbContext.ElectricityConsumedMounthEntities
            .AsNoTracking()
            .Include(e => e.ElectricyConsumedDays)
            .FirstOrDefaultAsync(e => e.PeriodDate == periodDate) ?? throw new Exception("Not found by periodDate");
    }

    public async Task<List<ElectricityConsumedMounthEntity>> GetByPeriodDateStartEnd(string periodDateStart, string periodDateEnd)
    {
        return await _dbContext.ElectricityConsumedMounthEntities
            .Include(e => e.ElectricyConsumedDays) // Подгружаем связанные дни
            .Where(e => string.Compare(e.PeriodDate, periodDateStart) >= 0 &&
                        string.Compare(e.PeriodDate, periodDateEnd) <= 0)
            .ToListAsync();
    }

    public async Task RenameClientById(Guid id, string name)
    {
        await _dbContext.ElectricityConsumedMounthEntities
            .Where(ecm => ecm.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(ecm => ecm.Name, name)
            );
    }
}