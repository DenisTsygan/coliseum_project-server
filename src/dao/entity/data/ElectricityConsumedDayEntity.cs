using System.Text.Json.Serialization;

public class ElectricityConsumedDayEntity
{
    public Guid Id { get; set; }

    [JsonPropertyName("periodDate")]
    public int Day { get; set; }// день месяца  

    public double AllElectricyConsumed { get; set; }// kW·h кВт·ч общее потребление за месяц

    public double[] ElectricyConsumedHours { get; set; } = [];
    public Guid MounthId { get; set; } // Внешний ключ на ElectricityConsumedMounthEntity

    [JsonIgnore]
    public ElectricityConsumedMounthEntity? ElectricyConsumedMounth { get; set; }

}