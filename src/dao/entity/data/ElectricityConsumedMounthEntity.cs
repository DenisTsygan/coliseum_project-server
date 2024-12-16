using System.Text.Json;
using System.Text.Json.Serialization;

public class ElectricityConsumedMounthEntity
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    [JsonConverter(typeof(ClientNameConverter))]
    [JsonPropertyName("name")]
    public ClientEntity? Client { get; set; }

    public string PeriodDate { get; set; } = String.Empty;// format mounth-year

    public decimal Period { get; set; }// hours количество отроботанных

    public double AllElectricyConsumed { get; set; }// kW·h кВт·ч общее потребление за месяц

    public List<ElectricityConsumedDayEntity> ElectricyConsumedDays { get; set; } = [];

}

public class ClientNameConverter : JsonConverter<ClientEntity?>
{
    public override ClientEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization is not implemented.");
    }

    public override void Write(Utf8JsonWriter writer, ClientEntity? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Name);
    }
}