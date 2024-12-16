public interface IElectricityConsumedDayRepository
{
    Task Add(Guid mounthId, ElectricityConsumedDayEntity electricityConsumedDay);

    Task Init();

}