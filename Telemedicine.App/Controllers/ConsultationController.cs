using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Utilities;
using OpenTokSDK;
using System.Net;
using Microsoft.AspNetCore.Http;
using Telemedicine.App.Models;
using Telemedicine.Services.Interfaces;
using Telemedicine.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class ConsultationController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConsultationService _service;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISessionManager _sessionManager;

        public ConsultationController(IHttpContextAccessor httpContextAccessor, IConsultationService service, IPatientService patientService,
            IAppointmentService appointmentService, ISessionManager sessionManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _service = service;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public async Task<IActionResult> Video(string sid, long aid, long pid, long did)
        {
            ConsultationViewModel model = new ConsultationViewModel();
            model.AppointmentId = aid;
            model.ApiKey = TokboxConfigurations.API_KEY;
            model.SessionId = sid;
            model.Token = OpenTokHandler.GenerateToken(sid);
            model.PatientId = pid;
            model.DoctorId = did;

            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
            {
                var result = await _patientService.GetPatientProfileDetailByUserAsync(new RequestDto<long>() { Key = pid });
                if (result.Status is 1)
                    if (!(result.Data is null))
                    {
                        var appointments = await _patientService.GetAppointmentsWithSOAPNotesByUserAsync(new RequestDto<long>() { Key = pid });
                        if (appointments.Status is 1)
                            if (!(appointments.Data is null))
                                result.Data.Appointments = appointments.Data;
                    }
                model.PatientProfileDetail = result.Data;
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Connect(long id)
        {
            string sessionId = "";
            var result = await _appointmentService.GetByIdAsync(new RequestDto<long>() { Key = id });
            if (result.Status is 1)
                if (!(result.Data is null))
                {
                    if(!(result.Data.Consultations is null))
                        if(result.Data.Consultations.Any())
                        {
                            var consultation = result.Data.Consultations.FirstOrDefault();
                            return RedirectToAction(nameof(ConsultationController.Video), "Consultation", new { sid = consultation.SessionId, aid = result.Data.AppointmentId, pid = result.Data.UserId, did = result.Data.CreatedById });
                        }
                    
                }
            return Content("");
        }

        [HttpGet]
        public IActionResult GenerateVideoSession()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string sessionId = OpenTokHandler.CreateSession(ipAddress);
            string token = OpenTokHandler.GenerateToken(sessionId);
            return RedirectToAction(nameof(ConsultationController.Video), "Consultation", new { sessionId = sessionId, token = token });
        }
    }
}
