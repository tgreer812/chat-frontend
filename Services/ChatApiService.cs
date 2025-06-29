using System.Net.Http.Json;
using ChatFrontend.Models;

namespace ChatFrontend.Services
{
    public class ChatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<ChatApiService> _logger;

        public ChatApiService(HttpClient httpClient, IConfiguration configuration, ILogger<ChatApiService> logger)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiBaseUrl"] ?? "https://localhost:5000";
            _logger = logger;
            
            // Log the base URL being used
            _logger.LogInformation($"ChatApiService initialized with base URL: {_baseUrl}");
        }

        // User methods
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<User>>($"{_baseUrl}/api/Users");
                return response ?? new List<User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                throw;
            }
        }

        public async Task<User?> GetUserAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<User>($"{_baseUrl}/api/Users/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user {id}");
                throw;
            }
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            try
            {
                _logger.LogInformation($"Creating user with username: {user.Username}");
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Users", user);
                
                if (response.IsSuccessStatusCode)
                {
                    var createdUser = await response.Content.ReadFromJsonAsync<User>();
                    _logger.LogInformation($"User created successfully: {createdUser?.Id}");
                    return createdUser;
                }
                
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to create user. Status: {response.StatusCode}, Error: {error}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        // Chat methods
        public async Task<List<Chat>> GetChatsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Chat>>($"{_baseUrl}/api/Chats");
                return response ?? new List<Chat>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chats");
                throw;
            }
        }

        public async Task<Chat?> GetChatAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Chats/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Chat>();
            }
            return null;
        }

        public async Task<Chat?> CreateChatAsync(ChatCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/Chats", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Chat>();
            }
            return null;
        }

        public async Task<bool> UpdateChatAsync(int id, ChatUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/Chats/{id}", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteChatAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Chats/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
