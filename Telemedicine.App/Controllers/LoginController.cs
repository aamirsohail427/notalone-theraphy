using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;
using TimeZoneConverter;

namespace Telemedicine.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _service;
        private readonly IUserService _userService;
        private readonly ISessionManager _sessionManager;

        public LoginController(ILoginService service, IUserService userService, ISessionManager sessionManager)
        {
            _service = service;
            _userService = userService;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!(TempData["custom-error"] is null))
                ModelState.AddModelError("custom-error", Convert.ToString(TempData["custom-error"]));

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto,string timezone, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var result = await _service.LoginAsync(loginRequestDto);
            if (result.Status is 1)
            {
                if (!(result.Data is null))
                {
                    if (!(result.Data.IsLoginAllowed))
                    {
                        TempData["custom-error"] = ResponseStrings.Unauthorized;
                        return RedirectToAction(nameof(LoginController.Index), "Login");
                    }

                    var userFromService = await _userService.GetByIdAsync(new RequestDto<long>() { Key = result.Data.UserId });
                    if (userFromService.Status is 1)
                    {
                        var user = userFromService.Data;
                        string userRole = "";

                        if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Admin))
                            userRole = UserRoleTypeStrings.Admin;

                        if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Therapist))
                            userRole = UserRoleTypeStrings.Therapist;

                        if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Client))
                            userRole = UserRoleTypeStrings.Client;

                        var claims = _sessionManager.RefreshCookieClaims(new UserSessionRequestDto()
                        {
                            LoginId = Convert.ToString(result.Data.LoginId),
                            UserId = Convert.ToString(user.UserId),
                            UserRoleId = Convert.ToString(user.UserRoleId),
                            Username = "" + user.FirstName + " " + user.LastName + "",
                            ProfilePicture = (string.IsNullOrEmpty(user.ProfilePictureUrl)) ? "" : (AzureConfigurations.AZURE_BLOB_BASE_URL + user.ProfilePictureUrl),
                            Role = userRole,
                            TimeZoneId = TZConvert.GetTimeZoneInfo(timezone).Id,
                            TokenId = OpenTokHandler.GenerateToken(TokboxConfigurations.GLOBAL_SESSION_ID)
                        });

                        var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                        if (!string.IsNullOrEmpty(returnUrl))
                            return LocalRedirect(returnUrl);
                    }
                }
            }
            TempData["custom-error"] = ResponseStrings.InvalidCredentials;
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            resetPasswordRequestDto.LoginId = _sessionManager.GetLoginId();
            return Json(await _service.ResetPasswordAsync(resetPasswordRequestDto));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(long id)
        {
            return Json(await _service.UpdateStatusAsync(new RequestDto<long>() { Key = id }));
        }
    }
}