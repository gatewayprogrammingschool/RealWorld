using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using RealWorldWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealWorldWebAPI.Services
{
    public class ArticleService
    {
        public ArticleService(
            IServiceProvider serviceProvider,
            ILogger<ArticleService> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public RealWorldContext Context => ServiceProvider.GetRequiredService<RealWorldContext>();
        public IServiceProvider ServiceProvider { get; }
        public ILogger<ArticleService> Logger { get; }

        public Article GetArticle(string slug)
        {
            var existing = Context.FullArticles
                .FirstOrDefault(a => a.Slug == slug);

            if (existing == null) throw new ApplicationException("Not found.");

            return existing;
        }

        public IEnumerable<Article> ListArticles(ListArticlesCriteria criteria, string value, int offset, int limit)
        {
            var articles = criteria switch {
                ListArticlesCriteria.Author => 
                    Context.FullArticles
                        .Where(a => a.Author.Username == value),
                ListArticlesCriteria.Tag => 
                    Context.FullArticles
                        .Where(a => a.ArticleTags.Any(t => t.Tag.TagName == value)),
                ListArticlesCriteria.FavoritedBy => 
                    Context.FullArticles
                        .Where(a => a.Favorites.Any(af => af.User.Email == value || af.User.Username == value)),
                _ => Context.FullArticles
            };

            return articles.OrderByDescending(a => a.UpdatedAt).Skip(offset).Take(limit);
        }

        public async Task<Article> DeleteArticle(string slug, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var existing = Context.FullArticles.FirstOrDefault(a => a.Slug == slug);

            if (existing == null) throw new ApplicationException("Not found.");

            Context.ArticleTag.RemoveRange(existing.ArticleTags);
            Context.Favorites.RemoveRange(existing.Favorites);
            Context.Comments.RemoveRange(existing.Comments);
            Context.Articles.Remove(existing);

            var count = await Context.SaveChangesAsync();

            return count > 0 
                ? existing
                : null;
        }

        public async Task<Article> CreateArticle(NewArticleModel newArticle, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var newUid = Guid.NewGuid();

            var article = new Article
            {
                ArticleUid = newUid,
                AuthorUserUid = user.UserUid,
                Body = newArticle.Body,
                Description = newArticle.Description,
                Title = newArticle.Title,
                ArticleTags = newArticle.TagList.Select(s => new ArticleTag { TagUid = Context.GetTag(s) }).ToList(),
                UpdatedAt = DateTime.UtcNow,
                Slug = HttpUtility.UrlEncode($"{user.Username}_{newArticle.Title}")
            };

            Context.Articles.Add(article);
            var count = await Context.SaveChangesAsync();

            article.Author = user;

            return count > 0
                ? article
                : null;
        }

        public async Task<Article> UpdateArticle(UpdateArticleModel updateArticle, string slug, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var existing = Context.FullArticles
                        .FirstOrDefault(a => a.Slug == slug);

            if (existing == null) throw new ApplicationException("Not found.");

            existing.Body = updateArticle.Body ?? existing.Body;
            existing.Title = updateArticle.Title ?? existing.Title;
            existing.Description = updateArticle.Description ?? existing.Description;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.Slug = HttpUtility.UrlEncode($"{user.Username}_{existing.Title}");

            Context.Update(existing);

            var count = await Context.SaveChangesAsync();

            return count == 1
                ? existing
                : null;
        }                                                                                                  public IEnumerable<Tags> GetTags(TagSort sort)
        {

            return sort switch
            {
                TagSort.Count => Context.FullTags.OrderByDescending(t => t.ArticleTag.Count).ThenBy(t => t.TagName),
                _ => Context.FullTags.OrderBy(t => t.TagName)
            };
        }
    }
}
