using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class ArticleFavorite : IEqualityComparer<ArticleFavorite>
    {
        [Key]
        public Guid ArticleFavoriteUid { get; set; }
        public Guid ArticleUid { get; set; }
        public Guid UserUid { get; set; }

        public virtual Article Article { get; set; }
        public virtual User User { get; set; }

        public bool Equals([AllowNull] ArticleFavorite x, [AllowNull] ArticleFavorite y)
        {
            return x.ArticleFavoriteUid == y.ArticleFavoriteUid;
        }

        public int GetHashCode([DisallowNull] ArticleFavorite obj)
        {
            return obj.ArticleFavoriteUid.GetHashCode();
        }
    }
}
