using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

[Authorize(Roles = "admin")] // Authorize only Admins to access this controller
public class AppRolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AppRolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var roles = _roleManager.Roles;
        return View(roles);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IdentityRole model)
    {
        // AVOID DUPLICATE ROLE
        if (!await _roleManager.RoleExistsAsync(model.Name))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Name));
        }

        return RedirectToAction("Index");
    }
}
