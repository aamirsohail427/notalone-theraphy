using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize(Roles = RoleStrings.ClientOnly)]
    public class DoctorsController : Controller
    {
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly ISessionManager _sessionManager;
        private readonly ISpecialtyLookupService _specialtyLookupService;

        public DoctorsController(IDoctorProfileService doctorProfileService, ISessionManager sessionManager, ISpecialtyLookupService specialtyLookupService)
        {
            _doctorProfileService = doctorProfileService;
            _sessionManager = sessionManager;
            _specialtyLookupService = specialtyLookupService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DoctorViewModel model = new DoctorViewModel();
            var specialtyLookups = await _specialtyLookupService.GetAllAsync();

            if (specialtyLookups.Status is 1)
                if (!(specialtyLookups.Data is null))
                    model.SpecialtyLookups.AddRange(specialtyLookups.Data.Where(x => x.IsActive == true)
                        .Select(item => new SelectListItem { Text = item.Title, Value = item.SpecialtyLookupId.ToString() }));

            string message = "";
            string doctorId = "";
            
            if (!(TempData["PaymentStatus"] is null))
                message = Convert.ToString(TempData["PaymentStatus"]);

            if(!(TempData["PaymentStatus"] is null))
                doctorId = Convert.ToString(TempData["DoctorId"]);
            
            ViewBag.Message = message;
            ViewBag.DoctorId = doctorId;
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            //var result = await _doctorProfileService.GetDoctorProfileDetailByUserAsync(new RequestDto<long>() { Key = id });
            var result = await _doctorProfileService.GetDoctorDetailAsync(new RequestDto<long>() { Key = id });
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> GetDoctors(GetDoctorsByFiltersRequestDto getDoctorsByFiltersRequestDto)
        {
            //var data = await _doctorProfileService.GetDoctorsByFiltersAsync(getDoctorsByFiltersRequestDto);
            var data = await _doctorProfileService.GetDoctorsByDoctorNameAndStateAsync(getDoctorsByFiltersRequestDto);
            return PartialView("~/Views/Doctors/PartialViews/_DoctorsDetail.cshtml", data.Data);
        }

        [HttpPost]
        public IActionResult GetAppointmentReasonForm()
        {
            return PartialView("~/Views/Doctors/PartialViews/_Q2AppointmentFlow.cshtml");
        }

        [HttpPost]
        public IActionResult GetLocationForm()
        {
            return PartialView("~/Views/Doctors/PartialViews/_Q3AppointmentFlow.cshtml");
        }

        [HttpPost]
        public IActionResult GetConcernForm()
        {
            return PartialView("~/Views/Doctors/PartialViews/_Q4AppointmentFlow.cshtml");
        }

        [HttpPost]
        public IActionResult GetDoctorsDetail()
        {
            return PartialView("~/Views/Doctors/PartialViews/_DoctorsDetail.cshtml");
        }
    }
}