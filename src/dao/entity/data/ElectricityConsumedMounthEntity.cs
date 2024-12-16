public class ElectricityConsumedMounthEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;// имя клиента
    public string PeriodDate { get; set; } = String.Empty;// format mounth-year

    public decimal Period { get; set; }// hours количество отроботанных

    public double AllElectricyConsumed { get; set; }// kW·h кВт·ч общее потребление за месяц

    public List<ElectricityConsumedDayEntity> ElectricyConsumedDays { get; set; } = [];

}