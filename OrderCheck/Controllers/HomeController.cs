using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderCheck.Models;

namespace OrderCheck.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public HomeController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
