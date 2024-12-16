using System.Text.Json.Serialization;

public class ClientEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<ElectricityConsumedMounthEntity> ElectricityConsumedMounthEntities { get; set; } = [];

}