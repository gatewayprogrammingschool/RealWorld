using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class Comment : IEqualityComparer<Comment>
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public Guid AuthorUserUid { get; set; }
        public Guid ArticleUid { get; set; }

        public virtual Article Article { get; set; }
        public virtual User AuthorUser { get; set; }

        public bool Equals([AllowNull] Comment x, [AllowNull] Comment y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Comment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
