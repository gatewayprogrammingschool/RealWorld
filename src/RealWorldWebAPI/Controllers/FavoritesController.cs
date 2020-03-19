using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using RealWorldWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorldWebAPI.Controllers
{
    public partial class FavoritesController  : Controller
    {
        public FavoritesController(
            ArticleService articleService,
            FavoritesService favoritesService,
            UserService userService)
        {
            ArticleService = articleService;
            FavoritesService = favoritesService;
            UserService = userService;
        }

        public ArticleService ArticleService { get; }
        public FavoritesService FavoritesService { get; }
        public UserService UserService { get; }

        [HttpGet("api/articles/{slug}/favorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddFavoriteArticle([FromRoute] string slug)
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

                var result = await FavoritesService.AddFavorite(slug, token);

                return result 
                    ? Ok()
                    : (IActionResult)Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }


        [HttpPost("api/profiles/{username}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> FollowProfile([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
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

                var result = await FavoritesService.AddFavoriteProfile(username, token);

                return result
                    ? Ok()
                    : (IActionResult)Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpDelete("api/articles/{slug}/favorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RemoveFavoriteArticle([FromRoute] string slug)
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

                var result = await FavoritesService.RemoveFavorite(slug, token);

                return result
                    ? Ok()
                    : (IActionResult)Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpDelete("api/profiles/{username}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UnfollowProfile([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
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

                var result = await FavoritesService.RemoveFavoriteProfile(username, token);

                return result
                    ? Ok()
                    : (IActionResult)Unauthorized();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

    }
}
