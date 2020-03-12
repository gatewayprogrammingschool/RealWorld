using System;
using System.ComponentModel.DataAnnotations;

namespace RealWorldWebAPI.Data.Models
{
    public class User
    {
        [Key]
        public Guid UserUID { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
    }
}