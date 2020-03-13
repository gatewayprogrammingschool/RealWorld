using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RealWorldWebAPI.Data.Models
{
    public class Tag
    {   
        [Key]
        public Guid TagUID { get; set; }

        [NotNull]
        public string TagName { get; }
    }
}