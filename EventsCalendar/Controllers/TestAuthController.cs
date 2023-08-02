using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsCalendar.Controllers;

[Route("api/test-auth")]
public class TestAuthController : Controller
{
	[Authorize]
	[HttpGet]
	public IActionResult Test()
	{
		var user =HttpContext.User.Claims;
		return Ok("Test messange");
	}
}