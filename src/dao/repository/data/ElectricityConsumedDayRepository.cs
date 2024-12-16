public class ElectricityConsumedDayRepository : IElectricityConsumedDayRepository
{
    private readonly ServiceDbContext _dbContext;
    public ElectricityConsumedDayRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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

        Console.WriteLine("Test");
        var ecmId_1 = Guid.Parse("00000000-0000-0000-0000-100000000000");
        var ecmId_2 = Guid.Parse("00000000-0000-0000-0000-200000000000");
        var ecmId_3 = Guid.Parse("00000000-0000-0000-0000-300000000000");
        Random random = new Random();
        for (int count_mounth_id = 0; count_mounth_id < 3; count_mounth_id++)
        {
            var days = new List<ElectricityConsumedDayEntity>();
            for (int count_day = 0; count_day < 30; count_day++)
            {
                var hours = new double[24];
                for (int k = 0; k < 24; k++)
                {
                    hours[k] = Math.Round(random.NextDouble() * (10 - 1) + 1, 2);
                }
                var MounthIdConst = ecmId_1;
                if (count_mounth_id == 1)
                {
                    MounthIdConst = ecmId_2;
                }
                else if (count_mounth_id == 2)
                {
                    MounthIdConst = ecmId_3;
                }

                days.Add(new ElectricityConsumedDayEntity
                {
                    Id = Guid.NewGuid(),
                    Day = count_day + 1,
                    AllElectricyConsumed = Math.Round(random.NextDouble() * (1000 - 1) + 1, 2),
                    ElectricyConsumedHours = hours,
                    MounthId = MounthIdConst
                });
            }
            Console.WriteLine("count_mounth_id = " + count_mounth_id + " | " + days.Count);
            /*foreach (var item in days)
            {
                Console.WriteLine("day = " + item.Day + " ;mounthId = " + item.MounthId + ";all = " + item.AllElectricyConsumed);
            }*/
            Console.WriteLine(days.Count);
            await _dbContext.ElectricityConsumedDayEntities.AddRangeAsync(days);
            days.Clear();
        }
        await _dbContext.SaveChangesAsync();
    }
}