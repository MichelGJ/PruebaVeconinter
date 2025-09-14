using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using VeconinterContacts.ViewModels;

namespace VeconinterContacts.Controllers
{
    public class AuthController : Controller
    {
        // Usuario fijo para la prueba
        private const string AdminEmail = "admin@veconinter.com";
        private const string AdminPass = "Admin!123";

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.Email == AdminEmail && vm.Password == AdminPass)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, AdminEmail),
                    new Claim(ClaimTypes.Name, "Admin Veconinter")
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return Redirect(returnUrl ?? Url.Action("Index", "Clientes")!);
            }

            ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Denied() => Content("Acceso denegado");
    }
}
