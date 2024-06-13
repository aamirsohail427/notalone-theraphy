using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;
using TimeZoneConverter;

namespace Telemedicine.App.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _service;
        private readonly IUserRoleService _userRoleService;
        private readonly ISessionManager _sessionManager;

        public RegisterController(IRegisterService service, IUserRoleService userRoleService, ISessionManager sessionManager)
        {
            _service = service;
            _userRoleService = userRoleService;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RegisterViewModel model = new RegisterViewModel();

            var userRoles = await _userRoleService.GetAllAsync();
            if(userRoles.Status is 1)
                model.UserRoles.AddRange(userRoles.Data.Where(x=> x.IsActive == true)
                    .Select(item => new SelectListItem { Text = item.Title, Value = item.UserRoleId.ToString() }));

            return View(model);
        }

        [HttpPost]
        public IActionResult GetRegistrationForm(int UserRoleId)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.RegisterRequestDto.UserRoleId = UserRoleId;
            return PartialView("~/Views/Register/PartialViews/_RegistrationForm.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto, string timezone)
        {
            var result = await _service.RegisterAsync(registerRequestDto);
            if(result.Status is 1)
                if(!(result.Data is null))
                {
                    var user = result.Data;
                    string userRole = "";

                    if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Admin))
                        userRole = UserRoleTypeStrings.Admin;

                    if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Therapist))
                        userRole = UserRoleTypeStrings.Therapist;

                    if (user.UserRoleId == Convert.ToInt32(UserRoleTypes.Client))
                        userRole = UserRoleTypeStrings.Client;

                    long loginId = 0;
                    if (result.Data.Logins.Any())
                        loginId = result.Data.Logins.FirstOrDefault().LoginId;

                    var claims = _sessionManager.RefreshCookieClaims(new UserSessionRequestDto()
                    {
                        LoginId = Convert.ToString(loginId),
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
                }
            return Json(result);
        }
    }
}