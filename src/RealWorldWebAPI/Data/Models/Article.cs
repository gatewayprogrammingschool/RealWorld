using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RealWorldWebAPI.Data.Models
{
    public partial class Article : IEqualityComparer<Article>
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
            Comments = new HashSet<Comment>();
            Favorites = new HashSet<ArticleFavorite>();
        }

        [Key]
        public Guid ArticleUid { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Guid AuthorUserUid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User Author { get; set; }
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ArticleFavorite> Favorites { get; set; }

        [NotMapped]
        public int FavoritesCount => Favorites.Count;

        public bool Equals([AllowNull] Article x, [AllowNull] Article y)
        {
            return x.ArticleUid == y.ArticleUid;
        }

        public int GetHashCode([DisallowNull] Article obj)
        {
            return obj.ArticleUid.GetHashCode();
        }

        internal bool GetFavorited(User currentUser)
        {
            if (currentUser == null) return false;

            return Favorites.Any(af => af.UserUid == currentUser.UserUid);
        }
    }
}
