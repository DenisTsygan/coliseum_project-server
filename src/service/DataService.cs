public class DataService
{

    private readonly IElectricityConsumedDayRepository _ecDayRepository;

    private readonly IElectricityConsumedMounthRepository _ecMounthRepository;



    public DataService(IElectricityConsumedDayRepository electricityConsumedDayRepository,
        IElectricityConsumedMounthRepository electricityConsumedMounthRepository
        )
    {
        _ecDayRepository = electricityConsumedDayRepository;
        _ecMounthRepository = electricityConsumedMounthRepository;
    }

    public async Task<ElectricityConsumedMounthEntity> GetECMByPeriodName(string periodName)
    {
        return await _ecMounthRepository.GetByPeriodDate(periodName);
    }

    public async Task InitData()
    {
        await _ecDayRepository.Init();
    }

}