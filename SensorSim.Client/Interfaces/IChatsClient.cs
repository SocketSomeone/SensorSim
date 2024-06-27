using Refit;
using SensorSim.Client.Models;

namespace SensorSim.Client.Interfaces;

[Headers("Accept: application/json")]
public interface IChatClient
{
    [Post("/api/chats")]
    public Task CreateChat([Header("Authorization")] string token, [Body] CreateChatModel chat);

    [Get("/api/chats")]
    public Task<List<ChatModel>> GetChatsAsync([Header("Authorization")] string token);
    
    [Get("/api/chats/{id}")]
    public Task<ChatModel> GetChatAsync([Header("Authorization")] string token, string id);
    
    [Delete("/api/chats/{id}")]
    public Task DeleteChatAsync([Header("Authorization")] string token, string id);
    
    [Post("/api/chats/{id}/messages")]
    public Task AddMessageAsync([Header("Authorization")] string token, string id, [Body] CreateMessageModel message);
}