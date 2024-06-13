using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService _service;
        private readonly ISessionManager _sessionManager;
        private readonly IUserService _userService;

        public FeedbacksController(IFeedbackService service, ISessionManager sessionManager, IUserService userService)
        {
            _service = service;
            _sessionManager = sessionManager;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Doctor(long id)
        {
            FeedbackViewModel model = new FeedbackViewModel();
            var result = await _userService.GetByIdAsync(new RequestDto<long>() { Key = id });
            if (result.Status is 1)
                if (!(result.Data is null))
                    model.UserResponseDto = result.Data;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(FeedbackRequestDto feedbackRequestDto)
        {
            feedbackRequestDto.FromUserId = _sessionManager.GetUserId();
            return Json(await _service.SaveAsync(feedbackRequestDto));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
            {
                var data = await _service.GetForDoctorAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                return PartialView("~/Views/Feedbacks/PartialViews/_FeedbacksDetail.cshtml", data.Data);
            }
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
            {
                var data = await _service.GetForPatientAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                return PartialView("~/Views/Feedbacks/PartialViews/_FeedbacksDetail.cshtml", data.Data);
            }
            return Json(Responses.SomethingWentWrong<string>("Feedback", null));
        }
    }
}