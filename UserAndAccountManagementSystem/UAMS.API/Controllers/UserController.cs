using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Infrastructure.Identity;


namespace UAMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] object userDto)
        {
            return Ok("User created successfully (Admin only).");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id, [FromBody] object userDto)
        {
            return Ok($"User {id} updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            return Ok($" User {id} deleted successfully.");
        }

        [HttpGet("{id?}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult GetUser(int? id = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var isEmployee = User.IsInRole("Employee");
            var isCustomer = User.IsInRole("Customer");

            if (isCustomer)
            {
                if (id == null || id.ToString() != currentUserId)
                    return StatusCode(StatusCodes.Status403Forbidden,
                        "You can only access your own account information.");
            }

            if (isAdmin || isEmployee)
            {
                return Ok("Full user list (Admin/Employee access).");
            }

            return Ok($"User profile for ID {currentUserId}.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsersWithRoles()
        {
            var users = _userManager.Users.ToList();
            var userRolesList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesList.Add(new
                {
                    user.Id,
                    user.UserName,
                    Roles = roles
                });
            }

            return Ok(userRolesList);
        }
    }
}