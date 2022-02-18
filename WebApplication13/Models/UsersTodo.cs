
namespace WebApplication13.Models
{
    public class UsersTodo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}