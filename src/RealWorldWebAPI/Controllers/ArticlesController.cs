using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using RealWorldWebAPI.Enums;
using RealWorldWebAPI.Services;

namespace RealWorldWebAPI.Controllers
{
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        public ArticlesController(
            ArticleService articleService,
            FavoritesService favoritesService,
            UserService userService,
            CommentsService commentsService)
        {
            ArticleService = articleService;
            FavoritesService = favoritesService;
            UserService = userService;
            CommentsService = commentsService;
        }

        public ArticleService ArticleService { get; }
        public FavoritesService FavoritesService { get; }
        public UserService UserService { get; }
        public CommentsService CommentsService { get; }

        [HttpGet("api/tags")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult GetTags()
        {
            try
            {
                var tags = ArticleService.GetTags(TagSort.Name);

                return Ok(new TagsResponse 
                { 
                    Tags = tags.Select(t => t.TagName).ToList() ,
                    Counts = tags.Select(t => new { t.TagName, t.ArticleTag.Count }).ToList()
                });
            }
            catch(Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }


        [HttpGet("api/articles/{slug}")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult Get(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                var article = ArticleService.GetArticle(slug);

                return article != null
                    ? Ok(new SingleArticleResponse
                    {
                        Article = new ArticleModel(
                            article.Slug,
                            article.Title,
                            article.Description,
                            article.Body,
                            article.ArticleTags.Select(t => t.Tag.TagName).ToList(),
                            article.CreatedAt,
                            article.UpdatedAt,
                            article.GetFavorited(currentUser),
                            article.FavoritesCount,
                            article.Author.Profile
                            )
                    })
                    : (IActionResult)NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }


        [HttpPost("api/articles")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] NewArticleRequest newArticleRequest)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var article = await ArticleService.CreateArticle(newArticleRequest.Article, token);

                return article != null
                    ? Created($"api/articles/{article.Slug}",
                        new SingleArticleResponse
                    {
                        Article = new ArticleModel(
                            article.Slug,
                            article.Title,
                            article.Description,
                            article.Body,
                            article.ArticleTags.Select(t => t.Tag.TagName).ToList(),
                            article.CreatedAt,
                            article.UpdatedAt,
                            article.GetFavorited(currentUser),
                            article.FavoritesCount,
                            article.Author.Profile
                            )
                    })
                    : (IActionResult)BadRequest("Not created.");
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpPut("api/articles/{slug}")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] string slug, [FromBody] UpdateArticleRequest updateArticleRequest)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var article = await ArticleService.UpdateArticle(updateArticleRequest.Article, slug, token);

                return article != null
                    ? Ok(new SingleArticleResponse
                    {
                        Article = new ArticleModel(
                            article.Slug,
                            article.Title,
                            article.Description,
                            article.Body,
                            article.ArticleTags.Select(t => t.Tag.TagName).ToList(),
                            article.CreatedAt,
                            article.UpdatedAt,
                            article.GetFavorited(currentUser),
                            article.FavoritesCount,
                            article.Author.Profile
                            )
                    })
                    : (IActionResult)BadRequest("Not updated.");
            }
            catch(ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }


        [HttpDelete("api/articles/{slug}")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] string slug)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var article = await ArticleService.DeleteArticle(slug, token);

                return article != null
                    ? Ok(new SingleArticleResponse
                    {
                        Article = new ArticleModel(
                            article.Slug,
                            article.Title,
                            article.Description,
                            article.Body,
                            article.ArticleTags.Select(t => t.Tag.TagName).ToList(),
                            article.CreatedAt,
                            article.UpdatedAt,
                            article.GetFavorited(currentUser),
                            article.FavoritesCount,
                            article.Author.Profile
                            )
                    })
                    : (IActionResult)BadRequest("Not updated.");
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpGet("api/articles")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult ListArticles([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? offset=0, [FromQuery] int? limit=20)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null)
                {
                    favorited = null;
                }

                var criteria = ListArticlesCriteria.None;

                if(!string.IsNullOrWhiteSpace(tag))
                {
                    criteria = ListArticlesCriteria.Tag;
                } 
                else if(!string.IsNullOrWhiteSpace(author))
                {
                    criteria = ListArticlesCriteria.Author;
                }
                else if (!string.IsNullOrWhiteSpace(favorited))
                {
                    criteria = ListArticlesCriteria.FavoritedBy;
                }

                var articles = ArticleService.ListArticles(criteria, criteria switch
                {
                    ListArticlesCriteria.Author => author,
                    ListArticlesCriteria.Tag => tag,
                    ListArticlesCriteria.FavoritedBy => favorited,
                    _ => null
                }, offset ?? 0, limit ?? 20);

                return Ok(new MultipleArticlesResponse
                {
                    Articles = articles.Select(article => 
                        new ArticleModel(
                            article.Slug,
                            article.Title,
                            article.Description,
                            article.Body,
                            article.ArticleTags.Select(t => t.Tag.TagName).ToList(),
                            article.CreatedAt,
                            article.UpdatedAt,
                            article.GetFavorited(currentUser),
                            article.FavoritesCount,
                            article.Author.Profile
                        )).ToList(),
                    ArticlesCount = articles.Count()
                });                
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpGet("api/articles/{slug}/comments")]
        [ProducesResponseType(typeof(MultipleCommentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult ListCommentsForArticle([FromRoute] string slug, [FromQuery] int? offset = 0, [FromQuery] int? limit = 20)
        {
            try
            {
                var comments = CommentsService.GetAllCommentsForArticle(slug, offset ?? 0, limit ?? 20);

                return Ok(new MultipleCommentsResponse
                {
                    Comments = comments.Select(comment =>
                        new CommentModel(
                            (int)comment.Id,
                            comment.CreatedAt,
                            comment.UpdatedAt,
                            comment.Body,
                            comment.AuthorUser.Profile
                        )).ToList(),
                });
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpGet("api/articles/{id:int}/comment")]
        [ProducesResponseType(typeof(MultipleCommentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public IActionResult GetComment([FromQuery] int id)
        {
            try
            {
                var comment = CommentsService.GetComment(id);

                return Ok(new SingleCommentResponse
                {
                    Comment = new CommentModel(
                            (int)comment.Id,
                            comment.CreatedAt,
                            comment.UpdatedAt,
                            comment.Body,
                            comment.AuthorUser.Profile
                        )
                });
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpPost("api/articles/{slug}/comments")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostComment([FromRoute] string slug, [FromBody] SingleCommentRequest newCommentRequest)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var comment = await CommentsService.AddCommentToArticle(slug, token, newCommentRequest.Comment);

                return comment != null
                    ? Created($"api/articles/{comment.Id}/comment",
                        new SingleCommentResponse
                    {
                        Comment = new CommentModel(
                            (int)comment.Id,
                            comment.CreatedAt,
                            comment.UpdatedAt,
                            comment.Body,
                            comment.AuthorUser.Profile
                        )
                    })
                    : (IActionResult)BadRequest("Not created.");
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (ApplicationException ae) when (ae.Message == "Not authorized.")
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }


        [HttpPut("api/articles/{id:int}/comments")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutComment([FromRoute] int id,
            [FromBody] SingleCommentRequest newCommentRequest)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var comment = await CommentsService.UpdateCommentOnArticle(id, token, newCommentRequest.Comment);

                return comment != null
                    ? Ok(new SingleCommentResponse
                    {
                        Comment = new CommentModel(
                            (int)comment.Id,
                            comment.CreatedAt,
                            comment.UpdatedAt,
                            comment.Body,
                            comment.AuthorUser.Profile
                        )
                    })
                    : (IActionResult)BadRequest("Not created.");
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (ApplicationException ae) when (ae.Message == "Not authorized.")
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpDelete("api/articles/{slug}/comments/{id:int}")]
        [ProducesResponseType(typeof(SingleArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteComment([FromRoute] string slug,[FromRoute] int id)
        {
            try
            {
                var tokenHeader = Request.Headers["Authorization"];
                var token = tokenHeader.Count > 0
                    ? tokenHeader.First().Split(' ')[1]
                    : null;

                User currentUser = null;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    currentUser = UserService.GetCurrentUser(token);
                }

                if (currentUser == null) return Unauthorized();

                var article = ArticleService.GetArticle(slug);

                if (article == null) return NotFound();

                var comment = await CommentsService.RemoveComment(id, token);

                return comment 
                    ? Ok()
                    : (IActionResult)BadRequest("Not deleted.");
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (ApplicationException ae) when (ae.Message == "Not authorized.")
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }
    }
}