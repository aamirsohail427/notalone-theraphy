using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;
using TimeZoneConverter;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionManager _sessionManager;

        public DashboardController(IUserService userService, ISessionManager sessionManager)
        {
            _userService = userService;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Admin))
            {
                var result = await _userService.AdminDashboardStatsByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                model.AdminDashboardStatsDto = result.Data;
            }
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
            {
                var result = await _userService.DoctorDashboardStatsByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                model.DoctorDashboardStatsDto = result.Data;
            }
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
            {
                var result = await _userService.PatientDashboardStatsByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                model.PatientDashboardStatsDto = result.Data;
            }
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = RoleStrings.AdminOnly)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            return PartialView("~/Views/Dashboard/PartialViews/_UsersDetail.cshtml",result.Data);
        }
    }
}