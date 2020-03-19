using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class FavoriteAuthor : IEqualityComparer<FavoriteAuthor>
    {
        [Key]
        public Guid FavoriteAuthorUid { get; set; }
        public Guid AuthorUserUid { get; set; }
        public Guid FanUserUid { get; set; }

        public virtual User AuthorUser { get; set; }
        public virtual User FanUser { get; set; }

        public bool Equals([AllowNull] FavoriteAuthor x, [AllowNull] FavoriteAuthor y)
        {
            return x.FavoriteAuthorUid == y.FavoriteAuthorUid;
        }

        public int GetHashCode([DisallowNull] FavoriteAuthor obj)
        {
            return obj.FavoriteAuthorUid.GetHashCode();
        }
    }
}
