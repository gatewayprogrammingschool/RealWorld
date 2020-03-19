using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public partial class Tags : IEqualityComparer<Tags>
    {
        [Key]
        public Guid TagUid { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<ArticleTag> ArticleTag { get; set; }

        public bool Equals([AllowNull] Tags x, [AllowNull] Tags y)
        {
            return x.TagUid == y.TagUid;
        }

        public int GetHashCode([DisallowNull] Tags obj)
        {
            return obj.TagUid.GetHashCode();
        }
    }
}
