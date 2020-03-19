using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealWorldWebAPI.Services
{
    public class CommentsService
    {
        public CommentsService(
            IServiceProvider serviceProvider,
            ILogger<CommentsService> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public RealWorldContext Context => ServiceProvider.GetRequiredService<RealWorldContext>();
        public IServiceProvider ServiceProvider { get; }
        public ILogger<CommentsService> Logger { get; }

        public IEnumerable<Comment> GetAllCommentsForArticle(string slug, int offset, int limit)
        {
            var articleService = ServiceProvider.GetRequiredService<ArticleService>();

            var article = articleService.GetArticle(slug);

            if (article == null) throw new ApplicationException("Not found.");

            return article.Comments.OrderByDescending(c => c.UpdatedAt).Skip(offset).Take(limit);
        }

        public async Task<Comment> AddCommentToArticle(string slug, string token, SingleCommentModel newComment)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var articleService = ServiceProvider.GetRequiredService<ArticleService>();

            var article = articleService.GetArticle(slug);

            if (article == null) throw new ApplicationException("Not found.");

            if (article.Comments.Any(c => c.AuthorUserUid == user.UserUid && c.Body == newComment.Body))
                throw new ApplicationException("Duplicate comment.");

            var newDate = DateTime.UtcNow;

            var comment = new Comment
            {
                Id = Context.Comments.Any() ? Context.Comments.Max(c => c.Id) + 1 : 0,
                ArticleUid = article.ArticleUid,
                AuthorUserUid = user.UserUid,
                Body = newComment.Body,
                CreatedAt = newDate,
                UpdatedAt = newDate
            };

            article.Comments.Add(comment);

            var count = await Context.SaveChangesAsync();

            return count > 0
                ? comment
                : null;
        }

        public async Task<Comment> UpdateCommentOnArticle(int id, string token, SingleCommentModel newComment)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var comment = Context.Comments.FirstOrDefault(c => c.AuthorUserUid == user.UserUid && c.Id == id);

            if (comment == null) throw new ApplicationException("Not found.");

            var newDate = DateTime.UtcNow;

            comment.Body = newComment.Body;
            comment.UpdatedAt = newDate;

            Context.Update(comment);

            var count = await Context.SaveChangesAsync();

            return count > 0
                ? comment
                : null;
        }

        public async Task<bool> RemoveComment(int id, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var comment = Context.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null) throw new ApplicationException("Not found.");

            if (comment.AuthorUserUid != user.UserUid) throw new ApplicationException("Unauthorized");

            Context.Remove(comment);

            var count = await Context.SaveChangesAsync();

            return count > 0;
        }

        public Comment GetComment(int id)
        {
            var comment = Context.Comments.FirstOrDefault(c => c.Id == id);

            return comment;
        }
    }
}
