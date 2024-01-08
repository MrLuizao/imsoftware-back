using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly RegistryService registryService;

    public UserController(RegistryService registryService)
    {
        this.registryService = registryService;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        registryService.RegistryUser(user);
        return CreatedAtAction(nameof(CreateUser), user);
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = registryService.RegisteredUsers();
        return Ok(users);
    }
}

