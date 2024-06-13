using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ISpecialtyLookupService _specialtyLookupService;
        private readonly ILookupService _lookupService;
        private readonly ISessionManager _sessionManager;
        private readonly IUserService _userService;
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly IStateService _stateService;
        private readonly IUserStateService _userStateService;

        public UserController(ISpecialtyLookupService specialtyLookupService, ILookupService lookupService, ISessionManager sessionManager,
            IUserService userService, IDoctorProfileService doctorProfileService, IStateService stateService, IUserStateService userStateService)
        {
            _specialtyLookupService = specialtyLookupService;
            _lookupService = lookupService;
            _sessionManager = sessionManager;
            _userService = userService;
            _doctorProfileService = doctorProfileService;
            _stateService = stateService;
            _userStateService = userStateService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            UserViewModel model = new UserViewModel();
            var educationTypes = await _lookupService.GetByTypeAsync(new RequestDto<string>() { Key = LookupTypeStrings.EducationType });

            if (educationTypes.Status is 1)
                if (!(educationTypes.Data is null))
                    model.EducationTypes.AddRange(educationTypes.Data
                        .Select(item => new SelectListItem { Text = item.LongTitle, Value = item.LookupId.ToString() }));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            UserViewModel model = new UserViewModel();

            var user = await _userService.PopulateUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            if (user.Status is 1)
                if (!(user.Data is null))
                    model.UserRequestDto = user.Data;

            if (_sessionManager.GetUserRoleId() == Convert.ToInt32(UserRoleTypes.Therapist))
            {
                var specialtyLookups = await _specialtyLookupService.GetAllAsync();

                if (specialtyLookups.Status is 1)
                    if (!(specialtyLookups.Data is null))
                        model.SpecialtyLookups.AddRange(specialtyLookups.Data.Where(x => x.IsActive == true)
                            .Select(item => new SelectListItem { Text = item.Title, Value = item.SpecialtyLookupId.ToString() }));

                var doctorProfile = await _doctorProfileService.GetByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                if (doctorProfile.Status is 1)
                    if (!(doctorProfile.Data is null))
                    {
                        model.UserRequestDto.SpecialtyLookupId = doctorProfile.Data.SpecialtyLookupId;
                        model.UserRequestDto.LicensedUrl = doctorProfile.Data.LicensedUrl;
                    }

            }

            var genderTypes = await _lookupService.GetByTypeAsync(new RequestDto<string>() { Key = LookupTypeStrings.GenderType });
            var preferredLanguageTypes = await _lookupService.GetByTypeAsync(new RequestDto<string>() { Key = LookupTypeStrings.PreferredLanguageType });
            var salutationTypes = await _lookupService.GetByTypeAsync(new RequestDto<string>() { Key = LookupTypeStrings.SalutationType });
            var states = await _stateService.GetAllAsync();
            var userStates = await _userStateService.GetAllByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });

            if (genderTypes.Status is 1)
                if (!(genderTypes.Data is null))
                    model.GenderTypes.AddRange(genderTypes.Data
                        .Select(item => new SelectListItem { Text = item.LongTitle, Value = item.LookupId.ToString() }));

            if (preferredLanguageTypes.Status is 1)
                if (!(preferredLanguageTypes.Data is null))
                    model.PreferredLanguageTypes.AddRange(preferredLanguageTypes.Data
                        .Select(item => new SelectListItem { Text = item.LongTitle, Value = item.LookupId.ToString() }));

            if (salutationTypes.Status is 1)
                if (!(salutationTypes.Data is null))
                    model.SalutationTypes.AddRange(salutationTypes.Data
                        .Select(item => new SelectListItem { Text = item.LongTitle, Value = item.LookupId.ToString() }));

            if (states.Status is 1)
                if (!(states.Data is null))
                    model.States.AddRange(states.Data
                        .Select(item => new SelectListItem { Text = item.Title + " (" + item.Code + ")" + "", Value = item.StateId.ToString() }));

            if (userStates.Status is 1)
                if (!(userStates.Data is null))
                    if (userStates.Data.Count > 0)
                    {
                        if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
                        {
                            model.PatientStateId = userStates.Data.FirstOrDefault().StateId;
                        }

                        if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
                        {
                            model.DoctorLicensedStates = new List<int>();
                            foreach (var item in userStates.Data)
                            {
                                model.DoctorLicensedStates.Add(item.StateId);
                            }
                        }
                    }

            return PartialView("~/Views/User/PartialViews/_PersonalInformation.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserRequestDto userRequestDto, IFormFile file, List<int> doctorLicensedStates, int patientStateId)
        {
            long userId = _sessionManager.GetUserId();
            if (_sessionManager.GetUserRoleId() == Convert.ToInt32(UserRoleTypes.Therapist))
            {
                var files = Request.Form.Files;
                foreach (IFormFile source in files)
                {
                    string pathToUpload = userId + "/";
                    string fileUrl = await AzureBlobHandler.Upload(source, pathToUpload);
                    if (!(string.IsNullOrEmpty(fileUrl)))
                    {
                        userRequestDto.LicensedUrl = fileUrl;
                    }
                }

                DoctorProfileRequestDto doctorProfileRequestDto = new DoctorProfileRequestDto()
                {
                    UserId = userId,
                    LicensedUrl = userRequestDto.LicensedUrl,
                    SpecialtyLookupId = userRequestDto.SpecialtyLookupId
                };
                await _doctorProfileService.SaveAsync(userId, doctorProfileRequestDto);

                if (!(doctorLicensedStates is null))
                    if (doctorLicensedStates.Any())
                    {
                        List<UserStateRequestDto> requestDto = new List<UserStateRequestDto>();
                        foreach (var item in doctorLicensedStates)
                        {
                            requestDto.Add(new UserStateRequestDto()
                            {
                                UserId = _sessionManager.GetUserId(),
                                StateId = item
                            });
                        }

                        if (requestDto.Any())
                            await _userStateService.SaveAsync(_sessionManager.GetUserId(), requestDto);

                    }
            }
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
            {
                if (patientStateId > 0)
                {
                    await _userStateService.SaveAsync(_sessionManager.GetUserId(), new List<UserStateRequestDto>()
                    {
                        new UserStateRequestDto()
                        {
                            StateId = patientStateId,
                            UserId = _sessionManager.GetUserId()
                        }
                    });
                }
            }
            var result = await _userService.UpdateAsync(userId, userRequestDto);
            if (result.Status is 1)
            {
                var claims = _sessionManager.RefreshCookieClaims(new UserSessionRequestDto()
                {
                    LoginId = Convert.ToString(_sessionManager.GetLoginId()),
                    UserId = Convert.ToString(_sessionManager.GetUserId()),
                    UserRoleId = Convert.ToString(_sessionManager.GetUserRoleId()),
                    Username = userRequestDto.FirstName + " " + userRequestDto.LastName,
                    ProfilePicture = _sessionManager.GetProfilePicture(),
                    Role = _sessionManager.GetRole(),
                    TimeZoneId = _sessionManager.GetTimezoneId(),
                    TokenId = _sessionManager.GetTokenId()
                });

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
        {
            long userId = _sessionManager.GetUserId();
            var files = Request.Form.Files;
            foreach (IFormFile source in files)
            {
                string[] extentions = { ".png", ".jpg", ".jpeg" };
                var extention = Path.GetExtension(source.FileName);
                if (extentions.Contains(extention))
                {
                    string pathToUpload = userId + "/";
                    string fileUrl = await AzureBlobHandler.Upload(source, pathToUpload);
                    if (!(string.IsNullOrEmpty(fileUrl)))
                    {
                        var result = await _userService.UpdateProfilePictureAsync(userId, new RequestDto<string>() { Key = fileUrl });
                        if (result.Status is 1)
                        {
                            string pictureUrl = AzureConfigurations.AZURE_BLOB_BASE_URL + fileUrl;

                            var claims = _sessionManager.RefreshCookieClaims(new UserSessionRequestDto()
                            {
                                LoginId = Convert.ToString(_sessionManager.GetLoginId()),
                                UserId = Convert.ToString(_sessionManager.GetUserId()),
                                UserRoleId = Convert.ToString(_sessionManager.GetUserRoleId()),
                                Username = _sessionManager.GetUsername(),
                                ProfilePicture = pictureUrl,
                                Role = _sessionManager.GetRole(),
                                TimeZoneId = _sessionManager.GetTimezoneId(),
                                TokenId = _sessionManager.GetTokenId()
                            });

                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                            var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                            return Json(new ResponseDto<string>()
                            {
                                Status = 1,
                                Message = result.Message,
                                Data = pictureUrl
                            });
                        }
                        return Json(result);
                    }
                    return Json(new ResponseDto<string>()
                    {
                        Status = 0,
                        Message = "Something went wrong",
                        Data = null
                    });
                }
                return Json(new ResponseDto<string>()
                {
                    Status = 0,
                    Message = "Please upload .png, .jpg, .jpeg file",
                    Data = null
                });
            }
            return Json(new ResponseDto<string>()
            {
                Status = 0,
                Message = "Something went wrong",
                Data = null
            });
        }
    }
}