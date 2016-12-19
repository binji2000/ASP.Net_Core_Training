using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EComm.Web.ViewModels;
using System.Security.Claims;

namespace EComm.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login(string ReturnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid) return View(lvm);

            bool auth = (lvm.Username == "test" && lvm.Password == "password");

            if (!auth) return View(lvm);
            
            var principal = new ClaimsPrincipal(
              new ClaimsIdentity(new List<Claim>
              {
                  new Claim(ClaimTypes.Name, lvm.Username),
                  new Claim(ClaimTypes.Role, "Admin")
              }, "FormsAuthentication"));

            await HttpContext.Authentication.SignInAsync("Cookies", principal);

            if (lvm.ReturnUrl != null) return LocalRedirect(lvm.ReturnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
