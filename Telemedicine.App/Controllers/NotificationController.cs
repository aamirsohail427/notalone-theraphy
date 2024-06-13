using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _service;
        private readonly ISessionManager _sessionManager;

        public NotificationController(INotificationService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            return PartialView("~/Views/Shared/_NotificationDetail.cshtml", result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            return Json(await _service.ReadAllAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() }));
        }
    }
}
