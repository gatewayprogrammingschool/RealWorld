using System;
using System.ComponentModel.DataAnnotations;

namespace RealWorldWebAPI.Data.Models
{
    public class Profile
    {
        [Key]
        public Guid ProfileUID { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
    }
}