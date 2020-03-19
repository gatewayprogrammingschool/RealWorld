using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealWorldWebAPI.Data.Models;
using RealWorldCommon.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Policy;

namespace RealWorldWebAPI.Services
{
    public class UserService
    {
        private static Microsoft.Extensions.Caching.Memory.MemoryCache _usersCache =
            new Microsoft.Extensions.Caching.Memory.MemoryCache(
                new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()
                {
                    ExpirationScanFrequency = TimeSpan.FromSeconds(60)
                });
        private static ConcurrentDictionary<string, (Guid, string)> _currentUsers = 
            new ConcurrentDictionary<string, (Guid, string)>();

        public UserService(
            IServiceProvider serviceProvider,
            ILogger<UserService> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public RealWorldContext Context => ServiceProvider.GetRequiredService<RealWorldContext>();
        public IServiceProvider ServiceProvider { get; }
        public ILogger<UserService> Logger { get; }

        public User GetCurrentUser(string token)
        {
            (Guid uid, _) = _currentUsers.Values.FirstOrDefault(tuple =>
            {
                (_, string tok) = tuple;
                return token == tok;
            });

            var user = _usersCache.Get<User>(uid);
            return user;
        }

        public ProfileModel GetUserProfile(string username, string token)
        {
            var user = GetCurrentUser(token);

            var profileUser = Context.Users.FirstOrDefault(u =>
                u.Username == username);

            if (profileUser != null)
            {
                return new ProfileModel
                {
                    Username = profileUser.Username,
                    Bio = profileUser.Bio,
                    Image = profileUser.Image,
                    Following = user?.FavoriteAuthorAuthorUserU.Any(fa => fa.AuthorUserUid == profileUser.UserUid) ?? false
                };
            }

            return null;
        }

        public User LoginUser(LoginUserModel loginUser)
        {
            User ProcessLogin()
            {
                byte[] data = UnicodeEncoding.Unicode.GetBytes(loginUser.Password);
                SHA512 shaM = new SHA512Managed();
                var hashed = Convert.ToBase64String(shaM.ComputeHash(data));

                var user = Context.FullUsers.FirstOrDefault(
                    u => u.Email == loginUser.Email && u.Password == hashed);

                return user ?? throw new ApplicationException("Unauthorized.");
            }

            User LocalLoginUser(ICacheEntry entry)
            {
                return entry.SetValue(ProcessLogin()).Value as User;
            }

            User user = null;

            if (_currentUsers.TryGetValue(loginUser.Email, out (Guid uid, string token) uid))
            {
                user = _usersCache.GetOrCreate<User>(uid, entry => LocalLoginUser(entry));
            }
            else
            {
                user = ProcessLogin();
                _usersCache.Set(user.UserUid, user);
            }

            if(user != null)
            {
                user.Token ??= Guid.NewGuid().ToString();

                if (!_currentUsers.Keys.Any(k => k == loginUser.Email))
                {
                    _currentUsers.TryAdd(user.Email, (user.UserUid, user.Token));
                }
                else
                {
                    _currentUsers[user.Email] = (user.UserUid, user.Token);
                }
            }

            Logger.LogInformation($"Log In {loginUser.Email} success: { user != null}");

            return user;
        }

        public async Task<User> RegisterUser(NewUserModel newUser)
        {
            var existing = Context.Users.Any(u => u.Email == newUser.Email || u.Username == newUser.Username);

            if (existing)
            {
                throw new ApplicationException("409");
            }

            byte[] data = UnicodeEncoding.Unicode.GetBytes(newUser.Password);
            SHA512 shaM = new SHA512Managed();
            var hashed = Convert.ToBase64String(shaM.ComputeHash(data));

            var user = Context.Users.Add(new User
            {
                Email = newUser.Email,
                Username = newUser.Username,
                Password = hashed
            });

            var count = await Context.SaveChangesAsync();

            user.Entity.Token = Guid.NewGuid().ToString();

            if (count == 1)
            {
                var temp = Context.FullUsers.First(u => u.UserUid == user.Entity.UserUid);
                _currentUsers.TryAdd(user.Entity.Email, (user.Entity.UserUid, user.Entity.Token));
                _usersCache.Set(temp.UserUid, temp);
                return user.Entity;
            }

            throw new ApplicationException("Not registered.");
        }

        public async Task<User> UpdateUser(UpdateUserModel updateUser)
        {
            var existing = await Context.Users.FirstOrDefaultAsync(u =>
                u.Email == updateUser.Email || u.Username == updateUser.Username);

            if (existing != null)
            {
                existing.Image = updateUser.Image ?? existing.Image;
                existing.Bio = updateUser.Bio ?? existing.Bio;

                Context.Update(existing);
                var count = await Context.SaveChangesAsync();

                return count == 1
                    ? existing
                    : null;
            }
            else
            {
                throw new ApplicationException("Not found.");
            }
        }
    }
}
