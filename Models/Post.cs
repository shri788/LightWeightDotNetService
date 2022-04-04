namespace LightWeightDotNetService.Models
{
    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }

    public class Api
    {
        public int Id { get; set; }
        public string? API { get; set; }
        public string? Description { get; set; }
        public string? Auth { get; set; }
        public bool? HTTPS { get; set; }
        public string? Cors { get; set; }
        public string? Link { get; set; }
        public string? Category { get; set; }

    }
}
