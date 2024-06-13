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
    public class PatientsController : Controller
    {
        private readonly IPatientService _service;
        private readonly ISessionManager _sessionManager;

        public PatientsController(IPatientService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var result = await _service.GetPatientProfileDetailByUserAsync(new RequestDto<long>() { Key = id });
            if(result.Status is 1)
                if(!(result.Data is null))
                {
                    var soapNotes = await _service.GetSOAPNotesByUserAsync(new RequestDto<long>() { Key = id });
                    if (soapNotes.Status is 1)
                        if (!(soapNotes.Data is null))
                            result.Data.SOAPNotes = soapNotes.Data;
                }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var data = await _service.GetByDoctorAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            return PartialView("~/Views/Patients/PartialViews/_PatientsDetail.cshtml", data.Data);
        }
    }
}