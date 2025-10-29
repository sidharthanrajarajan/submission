using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UAMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult CreateAccount([FromBody] object accountDto)
        {
            return Ok("Account created successfully (Admin/Employee only).");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult UpdateAccount(int id, [FromBody] object accountDto)
        {
            return Ok($"Account {id} updated successfully (Admin/Employee only).");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAccount(int id)
        {
            return Ok($"Account {id} deleted successfully (Admin only).");
        }

        [HttpGet("{id?}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult GetAccount(int? id = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var isEmployee = User.IsInRole("Employee");
            var isCustomer = User.IsInRole("Customer");

            if (isCustomer)
            {
                if (id == null || id.ToString() != currentUserId)
                    return Forbid("You can only access your own account details.");
            }

            if (isAdmin || isEmployee)
            {
                return Ok("Full account list (Admin/Employee access).");
            }

            return Ok($"Account details for user ID {currentUserId}.");
        }
    }
}
