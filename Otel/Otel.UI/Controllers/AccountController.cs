using Mailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otel.DAL.DataContext.Entities;
using Otel.UI.Models;

namespace Otel.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        // REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        // LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        // LOGOUT
        [HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // <-- redirect to Home page
        }


        // FORGOT PASSWORD
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Güvenlik: email bulunmasa bile aynı mesaj göster
                ViewBag.Message = "If the email exists, a reset link has been sent.";
                return View();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { resetToken = resetToken, email = user.Email },
                Request.Scheme
            );

            _mailService.SendMail(new Mail
            {
                ToEmail = user.Email,
                Subject = "Reset Password",
                TextBody = $"Click the link to reset your password: {resetLink}",
                HtmlBody = $"<a href='{resetLink}'>Reset Password</a>"
            });

            ViewBag.Message = "If the email exists, a reset link has been sent.";
            return View();
        }

        // RESET PASSWORD
        [HttpGet]
        public IActionResult ResetPassword(string resetToken, string email)
        {
            if (string.IsNullOrEmpty(resetToken) || string.IsNullOrEmpty(email))
                return BadRequest();

            var model = new ResetPasswordViewModel
            {
                ResetToken = resetToken,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}
