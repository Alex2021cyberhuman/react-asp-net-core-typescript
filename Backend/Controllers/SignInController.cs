using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class SignInController : ControllerBase
{
    [HttpPost("/api/sign-in")]
    public IActionResult Post([FromForm] SignInModel model)
    {
        if (!model.Login.Contains('a') || !model.Password.Contains('1'))
        {
            return BadRequest();
        }

        HttpContext.Response.Cookies.Append("login", "ok");
        return Redirect("/");
    }
}