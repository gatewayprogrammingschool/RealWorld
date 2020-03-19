using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RealWorldWebAPI.Data.Models
{
    public partial class RealWorldContext : DbContext
    {
        public RealWorldContext()
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public RealWorldContext(DbContextOptions<RealWorldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticleTag> ArticleTag { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FavoriteAuthor> FavoriteAuthor { get; set; }
        public virtual DbSet<ArticleFavorite> Favorites { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public IQueryable<Tags> FullTags => Tags.Include(nameof(Models.Tags.ArticleTag));

        public IQueryable<Article> FullArticles => Articles
                .Include(nameof(Article.Author))
                .Include(nameof(Article.ArticleTags))
                .Include(nameof(Article.Favorites))
                .Include(nameof(Article.Comments))
                .Include($"{nameof(Article.ArticleTags)}.{nameof(Models.ArticleTag.Tag)}");

        public IQueryable<User> FullUsers => Users
                .Include(nameof(User.Articles))
                .Include(nameof(User.Comments))
                .Include(nameof(User.Favorites))
                .Include(nameof(User.FavoriteAuthorAuthorUserU))
                .Include(nameof(User.FavoriteAuthorFanUserU));

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=.\\App_Data\\RealWorld.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.HasKey(e => e.ArticleTagUid);

                entity.Property(e => e.ArticleTagUid).HasColumnName("ArticleTagUid");

                entity.Property(e => e.ArticleUid)
                    .IsRequired()
                    .HasColumnName("ArticleUid");

                entity.Property(e => e.TagUid)
                    .IsRequired()
                    .HasColumnName("TagUid");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.ArticleUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ArticleTag)
                    .HasForeignKey(d => d.TagUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.ArticleUid);

                entity.Property(e => e.ArticleUid).HasColumnName("ArticleUid");

                entity.Property(e => e.AuthorUserUid)
                    .IsRequired()
                    .HasColumnName("AuthorUserUid");

                entity.Property(e => e.CreatedAt).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Slug).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.AuthorUserUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ArticleUid)
                    .IsRequired()
                    .HasColumnName("ArticleUid");

                entity.Property(e => e.AuthorUserUid)
                    .IsRequired()
                    .HasColumnName("AuthorUserUid");

                entity.Property(e => e.CreatedAt).IsRequired();

                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.AuthorUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.AuthorUserUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FavoriteAuthor>(entity =>
            {
                entity.HasKey(e => e.FavoriteAuthorUid);

                entity.Property(e => e.FavoriteAuthorUid).HasColumnName("FavoriteAuthorUid");

                entity.Property(e => e.AuthorUserUid)
                    .IsRequired()
                    .HasColumnName("AuthorUserUid");

                entity.Property(e => e.FanUserUid)
                    .IsRequired()
                    .HasColumnName("FanUserUid");

                entity.HasOne(d => d.AuthorUser)
                    .WithMany(p => p.FavoriteAuthorAuthorUserU)
                    .HasForeignKey(d => d.AuthorUserUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.FanUser)
                    .WithMany(p => p.FavoriteAuthorFanUserU)
                    .HasForeignKey(d => d.FanUserUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ArticleFavorite>(entity =>
            {
                entity.HasKey(e => e.ArticleFavoriteUid);

                entity.Property(e => e.ArticleFavoriteUid).HasColumnName("ArticleFavoriteUid");

                entity.Property(e => e.ArticleUid)
                    .IsRequired()
                    .HasColumnName("ArticleUid");

                entity.Property(e => e.UserUid)
                    .IsRequired()
                    .HasColumnName("UserUid");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.ArticleUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserUid)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagUid);

                entity.Property(e => e.TagUid).HasColumnName("TagUid");

                entity.Property(e => e.TagName).IsRequired();

                entity.ToTable("Tags");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserUid);

                entity.Property(e => e.UserUid).HasColumnName("UserUid");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        internal Guid GetTag(string s)
        {
            var existing = this.Tags.FirstOrDefault(t => t.TagName == s);

            if(existing == null)
            {
                existing = Tags.Add(new Tags { TagUid = Guid.NewGuid(), TagName = s }).Entity;
                SaveChanges();
            }

            return existing.TagUid;
        }
    }
}
