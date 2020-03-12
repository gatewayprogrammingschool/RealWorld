using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RealWorldWebAPI.Data.Models
{
    public class ArticleList : DbSet<Article>
    {
        public int ArticlesCount => base.AsQueryable().Count();
    }
}