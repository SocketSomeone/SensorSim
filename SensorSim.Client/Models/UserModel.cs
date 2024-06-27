using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SensorSim.Client.Models;

public class UserModel
{
    [JsonProperty("id")] public Guid Id { get; set; }

    [JsonProperty("username")] public string Username { get; set; }

    [JsonProperty("password")] public string Password { get; set; }
}

public class RegisterModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}