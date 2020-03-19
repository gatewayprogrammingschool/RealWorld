using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class ArticleTag : IEqualityComparer<ArticleTag>
    {
        [Key]
        public Guid ArticleTagUid { get; set; }
        public Guid ArticleUid { get; set; }
        public Guid TagUid { get; set; }

        public virtual Article Article { get; set; }
        public virtual Tags Tag { get; set; }

        public bool Equals([AllowNull] ArticleTag x, [AllowNull] ArticleTag y)
        {
            return x.ArticleTagUid == y.ArticleTagUid;
        }

        public int GetHashCode([DisallowNull] ArticleTag obj)
        {
            return obj.ArticleTagUid.GetHashCode();
        }
    }
}
