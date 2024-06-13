using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class SOAPNoteController : Controller
    {
        private readonly ISOAPNoteService _service;
        private readonly ISessionManager _sessionManager;
        private readonly INotificationService _notificationService;

        public SOAPNoteController(ISOAPNoteService service, ISessionManager sessionManager, INotificationService notificationService)
        {
            _service = service;
            _sessionManager = sessionManager;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(SOAPNoteRequestDto sOAPNoteRequestDto)
        {
            var result = await _service.SaveAsync(_sessionManager.GetUserId(), sOAPNoteRequestDto);
            if(result.Status is 1)
                if(!(result.Data is null))
                {
                    await _notificationService.SaveAsync(new NotificationRequestDto()
                    {
                        FromUserId = _sessionManager.GetUserId(),
                        ToUserId = sOAPNoteRequestDto.PatientId,
                        Message = NotificationMessages.SOAPNoteAdded(_sessionManager.GetUsername()),
                        NotificationTypeId = Convert.ToInt32(NotificationTypes.SOAPNote)
                    });
                }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(long id)
        {
            return Json(await _service.GetByIdAsync(new RequestDto<long>() { Key = id }));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByAppointment(long id)
        {
            return Json(await _service.GetAllByAppointmentAsync(new RequestDto<long>() { Key = id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            return PartialView("~/Views/User/PartialViews/_SOAPNoteDetail.cshtml", result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(long id)
        {
            return Json(await _service.DeleteByIdAsync(new RequestDto<long>() { Key = id }));
        }
    }
}
