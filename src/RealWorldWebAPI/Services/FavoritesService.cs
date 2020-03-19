using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace RealWorldWebAPI.Services
{
    public class FavoritesService
    {
        public FavoritesService(
            IServiceProvider serviceProvider,
            ILogger<FavoritesService> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public RealWorldContext Context => ServiceProvider.GetRequiredService<RealWorldContext>();
        public IServiceProvider ServiceProvider { get; }
        public ILogger<FavoritesService> Logger { get; }

        public async Task<bool> AddFavorite(string slug, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            if (user.Favorites.Any(a => a.Article.Slug == slug)) return true;

            var article = ServiceProvider.GetRequiredService<ArticleService>().GetArticle(slug);

            if (article == null) throw new ApplicationException("Not found.");

            var newFavorite = new ArticleFavorite
            {
                UserUid = user.UserUid,
                ArticleUid = article.ArticleUid,
                ArticleFavoriteUid = Guid.NewGuid()
            };

            Context.Favorites.Add(newFavorite);

            var count = await Context.SaveChangesAsync();

            return count > 0;
        }

        public async Task<bool> RemoveFavorite(string slug, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var toDelete = Context.Favorites.Where(af => af.User.UserUid == user.UserUid && af.Article.Slug == slug).ToList();

            if (toDelete == null) throw new ApplicationException("Not found.");

            toDelete.ForEach(td => Context.Remove(td));

            var count = await Context.SaveChangesAsync();

            return count > 0;
        }

        public async Task<bool> RemoveFavoriteProfile(string username, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            var profileUser = Context.Users.FirstOrDefault(p => p.Username == username);

            if (profileUser == null) throw new ApplicationException("Not found.");

            var toDelete =user.FavoriteAuthorAuthorUserU.Where(fa => fa.AuthorUserUid == profileUser.UserUid && fa.FanUserUid == user.UserUid).ToList();

            Context.ChangeTracker.AcceptAllChanges();

            toDelete.ForEach(td =>
            {
                Context.Remove(td);
            });            

            var count = await Context.SaveChangesAsync();

            toDelete.ForEach(td =>
            {
                user.FavoriteAuthorAuthorUserU.Remove(td);
            });

            return count > 0;
        }

        public async Task<bool> AddFavoriteProfile(string username, string token)
        {
            var user = ServiceProvider.GetRequiredService<UserService>().GetCurrentUser(token);

            if (user == null) throw new ApplicationException("Not logged in.");

            if (user.FavoriteAuthorAuthorUserU.Any(fa => fa.AuthorUser.Username == username)) return true;

            var profileUser = Context.Users.FirstOrDefault(p => p.Username == username);

            if (profileUser == null) return false;

            var fa = new FavoriteAuthor
            {
                FavoriteAuthorUid = Guid.NewGuid(),
                AuthorUserUid = profileUser.UserUid,
                FanUserUid = user.UserUid,
            };

            Context.FavoriteAuthor.Add(fa);

            user.FavoriteAuthorAuthorUserU.Add(fa);

            var count = await Context.SaveChangesAsync();

            return count > 0;
        }
    }
}
