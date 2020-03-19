using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using RealWorldCommon.Models;
using RealWorldWebAPI.Data.Models;
using RealWorldWebAPI.Services;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealWorldWebAPI.Controllers
{
    public partial class UsersController : Controller
    {
        public UserService UserService { get; }

        public UsersController(UserService userService)
        {
            UserService = userService;
        }

        [HttpGet("api/user")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Get()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(token) || !token.Contains(" "))
            {
                return UnprocessableEntity();
            }

            var parts = token.Split(' ');

            var user = UserService.GetCurrentUser(parts[1]);

            return user != null
                ? Ok(new UserResponse { User = new UserModel(user.Email, user.Token, user.Username, user.Bio, user.Image) })
                : (IActionResult)Unauthorized();
        }

        [HttpGet("api/profiles/{username}")]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult GetProfile([FromRoute] string username)
        {
            string[] parts = new string[2];
            var token = Request.Headers["Authorization"].FirstOrDefault();

            if (!(string.IsNullOrWhiteSpace(token) || !token.Contains(" ")))
            {
                parts = token.Split(' ');
            }

            var profile = UserService.GetUserProfile(username, parts[1]);

            return profile != null
                ? Ok(new ProfileResponse(profile))
                : (IActionResult)NotFound();
        }

        [HttpPost("api/users/login")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Login([FromBody] LoginUserRequest userRequest)
        {
            try
            {
                var user = UserService.LoginUser(userRequest.User);

                return user != null
                    ? Ok(new UserResponse { User = new UserModel(user.Email, user.Token, user.Username, user.Bio, user.Image) })
                    : (IActionResult)Unauthorized();
            }
            catch(Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return BadRequest(genericErrorModel);
            }
        }

        [HttpPost("api/users")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post([FromBody] NewUserRequest registerRequest)
        {
            try
            {
                var user = await UserService.RegisterUser(registerRequest.User);

                return Ok(new UserResponse { User = new UserModel(user.Email, user.Token, user.Username, user.Bio, user.Image) });

            }
            catch (ApplicationException ae) when (ae.Message == "409")
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { "User exists." }));
                return UnprocessableEntity(genericErrorModel);
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return UnprocessableEntity(genericErrorModel);
            }
        }

        [HttpPut("api/user")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericErrorModel), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put([FromBody] UpdateUserRequest updateRequest)
        {
            try
            {
                var user = await UserService.UpdateUser(updateRequest.User);

                return Ok(new UserResponse { User = new UserModel(user.Email, null, user.Username, user.Bio, user.Image) });
            }
            catch (ApplicationException ae) when (ae.Message == "Not found.")
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                var genericErrorModel = new GenericErrorModel(new GenericErrorModelErrors(new[] { ex.ToString() }));
                return UnprocessableEntity(genericErrorModel);
            }
        }
    }
}
