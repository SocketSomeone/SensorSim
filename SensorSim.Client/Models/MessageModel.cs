using Newtonsoft.Json;

namespace SensorSim.Client.Models;

public class MessageModel
{
    [JsonProperty("id")] public Guid Id { get; set; }
    
    [JsonProperty("text")] public string Text { get; set; }
    
    [JsonProperty("author")] public UserModel Author { get; set; }
    
    [JsonProperty("date")] public DateTime Date { get; set; }
}

public class CreateMessageModel
{
    [JsonProperty("text")] public string Text { get; set; }
}