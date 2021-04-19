using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fieldlevel.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get most recent post for all users
        /// </summary>
        [HttpGet]
        [Route("posts")]
        [ProducesResponseType(typeof(IEnumerable<Post>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMostRecetnPostForAllUsers()
        {
            try
            {
                var recentPosts = await _userManager.GetMostRecentPost();
                return new OkObjectResult(recentPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get Recent Posts Error", ex);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
