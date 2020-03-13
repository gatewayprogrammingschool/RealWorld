using System;
using System.ComponentModel.DataAnnotations;

namespace RealWorldWebAPI.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Body { get; set; }
        public User Author { get; set; }
    }
}