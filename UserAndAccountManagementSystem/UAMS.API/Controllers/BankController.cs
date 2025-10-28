using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateBank([FromBody] object bankDto)
        {
            return Ok("Bank created successfully (Admin only).");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBank(Guid id, [FromBody] object bankDto)
        {
            return Ok($"Bank {id} updated successfully (Admin only).");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBank(Guid id)
        {
            return Ok($"Bank {id} deleted successfully (Admin only).");
        }

        [HttpGet("{id?}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult GetBank(Guid? id = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var isEmployee = User.IsInRole("Employee");
            var isCustomer = User.IsInRole("Customer");

            if (isCustomer)
            {
                if (id == null || id.ToString() != currentUserId)
                    return Forbid("You can only access your own bank details.");
            }

            if (isAdmin || isEmployee)
            {
                return Ok("Full bank list or details (Admin/Employee access).");
            }

            return Ok($"Bank details for user ID {currentUserId}.");
        }
    }
}
