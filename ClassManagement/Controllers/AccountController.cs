using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClassManagement.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace ClassManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly InfrastructureDBContext _conetxt;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
       
        public AccountController(InfrastructureDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger, IEmailService emailService, IConfiguration configuration)
        {
            this._conetxt = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailService = emailService;
            this._configuration = configuration;

        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Location =ResponseCacheLocation.None, NoStore =true)]
        public IActionResult Login()          //string returnUrl
        {
            var CaptchCode = _emailService.GenerateCaptchaCode();
            ViewBag.Captch = CaptchCode;
            // return View(model);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserRegistration UserObj)
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValide = (ValidateCaptcha(EncodedResponse) == "true" ? true : false);

            if (UserObj.UserName != null && UserObj.Password != null && IsCaptchaValide == true)
            {
                var result = await _signInManager.PasswordSignInAsync(UserObj.UserName, UserObj.Password, false  , lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Login Successfully {UserObj.UserName}";
                    //ModelState.AddModelError(string.Empty, $"Login Successfully {UserObj.UserName}");
                    return RedirectToAction("Index", "EmployeeDetails");
                }
                if (result.IsLockedOut)
                { 
                    var forgetPassword = Url.Action(nameof(ForgetPassWord), "Account", new { }, Request.Scheme);
                    var Content = string.Format("Your account is locked out, to reset your password, please click this link");
                   _emailService.SendEmailConfirmation(UserObj.UserName,forgetPassword,Content);
                    ModelState.AddModelError("", "The account is locked out please Forget Your Password!");
                         return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                    TempData["Success"] = "Invalid Email and Password ";

                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalide Google Recaptcha!");
            }

            return View(UserObj);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration()
        {
            var CaptchCode = _emailService.GenerateCaptchaCode();
            ViewBag.Captch = CaptchCode;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(UserRegistration Userobj)
        {
            if (ModelState.IsValid)
            {
                if (Userobj.CaptchCode == Userobj.OTP)
                {
                    var user = new IdentityUser
                    {
                        UserName = Userobj.Email,
                        Email = Userobj.Email,
                    };
                    string otp_Check = _emailService.GenerateOtp();
                    TempData["Otp"] = otp_Check;
                    ViewBag.OtpValue= otp_Check;

                    TempData["TimeStamp"] = DateTime.Now;
                    var Content = string.Format($"OTP is {otp_Check} for Registration, please Enter Properly");
                    bool EmailStatus = _emailService.SendEmailConfirmation(Userobj.Email, otp_Check,Content);
                    if (EmailStatus)
                    {
                        var UserIsExist = await _userManager.FindByEmailAsync(Userobj.Email);
                        if (UserIsExist == null)
                        {
                            var result = await _userManager.CreateAsync(user, Userobj.Password);
                            if (result.Succeeded)
                            {
                                if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                                {
                                    return RedirectToAction("ListUsers", "Administration");
                                }

                                var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                var ConfirmLink = Url.Action(nameof(ConfirmEmail), "Account", new { Token, email = user.Email }, Request.Scheme);
                                var _Content = string.Format("Your account Verification Link, please click this link And Confirm Registration Successfully!");
                                bool EmailResponse = _emailService.SendEmailConfirmation(user.Email, ConfirmLink, _Content);
                                if (EmailResponse)
                                {
                                    await _userManager.AddToRoleAsync(user, UserRoleModel.User);
                                    return RedirectToAction("OtpValidation", "Account", new { Email = Userobj.Email });
                                }
                                else
                                {
                                    foreach (IdentityError error in result.Errors)
                                    {
                                        ModelState.AddModelError(string.Empty, error.Description);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User IsAlready Exist yet");

                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalide OTP");
                }

            }

            return View(Userobj);
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Validete The Captcha Code & Checkit 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// 
        public static string ValidateCaptcha(string EncodedResponse)
        {
            // bool IscaptchaCode = false;
            var client = new System.Net.WebClient();
            const string secretKey = "6LfsehEkAAAAAKpWLUeQ5ydRxITxkLev9pmYvvHN";
            var Client = new WebClient();
            var jsonResult = Client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, EncodedResponse));
            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<captchaResponse>(jsonResult);
            return (captchaResponse.Success.ToLower());
        }


        [HttpGet]
        public ActionResult OtpValidation()   //string Email
        {
            return View();

        }

        [HttpGet]
        public IActionResult OtpConfirm(string strOtp)
        {
            string otp = (string)TempData["Otp"];
            ViewBag.Otp = otp;
            HttpContext.Session.SetString("Otp", otp);
            if (strOtp == ViewBag.Otp)
            {
                TempData["Data"] = "Your One Time Password Verifed Successfully";
                return View();
            }
            else if ((DateTime.Now - Convert.ToDateTime(TempData["TimeStamp"])).TotalMinutes > 5)
            {
                TempData["Data"] = "OTP TimedOut";
                return RedirectToAction("Registration", "Account");
            }
            else
            {
                TempData["Data"] = "Invalide Otp !";
                return RedirectToAction("OtpValidation","Account");
            }
           
        }


        [HttpGet]
        public IActionResult ForgetPassWord()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassWord(ForgotPassWordViewModel forgotPassWordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPassWordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(forgotPassWordViewModel.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var Callback = Url.Action(nameof(ResetPassword), "Account", new {token =Token, Email= user.Email }, Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Reset password token", Callback, null);
            var Content = String.Format("Reset Password Token");
             _emailService.SendEmailConfirmation(user.Email,Callback,Content);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { ToKen = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.ToKen, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword changePasswordObj)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user, changePasswordObj.CurrentPassword, changePasswordObj.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("ChangePasswordConfirmation");
            }
            return View(changePasswordObj);
        }

        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectionUrl = Url.Action("ExternalLoginBack", "Account",
                new { ReturnUrl = returnUrl });
            var Properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectionUrl);

            return new ChallengeResult(provider, Properties);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginBack(string returnUrl = null, string remoteError = null)
        {
           
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, "Error from the External Provider");
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error Loading in External Login Information");
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor:true);
                     //    string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "EmployeeDetails");
                //return RedirectToAction(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(ForgetPassWord));
            }
            else
            {
                var Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (Email != null)
                {
                    var user = await _userManager.FindByEmailAsync(Email);

                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            EmailConfirmed=true
                        };
                           await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "EmployeeDetails");
                }
                ViewBag.ErrorTitle = $"Email Claim not Recieved from : {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please Contact To Support Team on Admin.protector@1303@gmail.com ";
                return View("Error");
                
            }

        }

       
    }
}
