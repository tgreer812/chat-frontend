namespace ChatFrontend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
    }

    public class Chat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Contents { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public User? User { get; set; }
        public string? Username { get; set; } // Add this field for the username from the API
    }

    public class ChatCreateRequest
    {
        public int UserId { get; set; }
        public string? Contents { get; set; }
    }

    public class ChatUpdateRequest
    {
        public string? Contents { get; set; }
    }
}
