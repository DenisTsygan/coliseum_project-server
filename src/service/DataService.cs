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

    public async Task<List<ElectricityConsumedMounthEntity>> GetECMByPeriodName(string periodName)
    {
        return await _ecMounthRepository.GetByPeriodDate(periodName);
    }

    public async Task InitData()
    {
        await _ecMounthRepository.Init();
        await _ecDayRepository.Init();
    }

    public async Task RenameClientById(Guid ecmId, string newName)
    {
        await _ecMounthRepository.RenameClientById(ecmId, newName);
    }

}