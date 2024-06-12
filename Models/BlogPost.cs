using System;
using System.ComponentModel.DataAnnotations;

namespace MyPersonelBlog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
