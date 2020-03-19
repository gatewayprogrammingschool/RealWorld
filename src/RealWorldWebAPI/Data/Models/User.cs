using RealWorldCommon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class User : IEqualityComparer<User>
    {
        public User()
        {
            Articles = new HashSet<Article>();
            Comments = new HashSet<Comment>();
            FavoriteAuthorAuthorUserU = new HashSet<FavoriteAuthor>();
            FavoriteAuthorFanUserU = new HashSet<FavoriteAuthor>();
            Favorites = new HashSet<ArticleFavorite>();
        }

        [Key]
        public Guid UserUid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FavoriteAuthor> FavoriteAuthorAuthorUserU { get; set; }
        public virtual ICollection<FavoriteAuthor> FavoriteAuthorFanUserU { get; set; }
        public virtual ICollection<ArticleFavorite> Favorites { get; set; }

        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            return x.UserUid == y.UserUid;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.UserUid.GetHashCode();
        }

        public ProfileModel Profile =>
            new ProfileModel
            {
                Bio = Bio,
                Image = Image,
                Username = Username
            };

        [NotMapped]
        public string Token { get; set; }
    }
}
