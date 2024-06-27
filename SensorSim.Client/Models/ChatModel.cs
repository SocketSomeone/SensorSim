using Newtonsoft.Json;

namespace SensorSim.Client.Models;

public class ChatModel
{
    [JsonProperty("id")] public readonly string Id;
    
    [JsonProperty("name")] public readonly string Name;
    
    [JsonProperty("messages")] public readonly List<MessageModel> Messages;
}

public class CreateChatModel
{
    [JsonProperty("name")] public string Name;
}