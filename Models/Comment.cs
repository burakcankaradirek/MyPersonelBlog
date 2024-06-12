using System;

namespace MyPersonelBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
