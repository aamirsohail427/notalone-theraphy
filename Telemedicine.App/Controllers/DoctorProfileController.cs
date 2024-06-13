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
    public class DoctorProfileController : Controller
    {
        private readonly IDoctorProfileService _service;

        public DoctorProfileController(IDoctorProfileService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLicenseById(long id)
        {
            return Json(await _service.DeleteLicenseByIdAsync(new RequestDto<long>() { Key = id }));
        }
    }
}
