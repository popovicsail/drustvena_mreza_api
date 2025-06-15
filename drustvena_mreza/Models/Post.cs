namespace drustvena_mreza.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User? Author { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublishing { get; set; }

        public Post(int id, string? content, DateTime dateOfPublishing)
        {
            Id = id;
            Content = content;
            DateOfPublishing = dateOfPublishing;
        }
    }
}