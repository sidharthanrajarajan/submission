using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] object userDto)
        {
            return Ok("User created successfully (Admin only).");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(Guid id, [FromBody] object userDto)
        {
            return Ok($"User {id} updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(Guid id)
        {
            return Ok($" User {id} deleted successfully.");
        }

        [HttpGet("{id?}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult GetUser(Guid? id = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var isEmployee = User.IsInRole("Employee");
            var isCustomer = User.IsInRole("Customer");

            if (isCustomer)
            {
                if (id == null || id.ToString() != currentUserId)
                    return Forbid("You can only access your own account information.");
            }

            if (isAdmin || isEmployee)
            {
                return Ok("Full user list (Admin/Employee access).");
            }

            return Ok($" User profile for ID {currentUserId}.");
        }
    }
}