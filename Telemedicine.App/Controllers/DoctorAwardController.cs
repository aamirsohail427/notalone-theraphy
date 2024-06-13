﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class DoctorAwardController : Controller
    {
        private readonly IDoctorAwardService _service;
        private readonly ISessionManager _sessionManager;

        public DoctorAwardController(IDoctorAwardService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        [HttpPost]
        public async Task<IActionResult> Save(DoctorAwardRequestDto doctorAwardRequestDto)
        {
            return Json(await _service.SaveAsync(_sessionManager.GetUserId(), doctorAwardRequestDto));
        }

        [HttpPost]
        public async Task<IActionResult> GetById(long id)
        {
            return Json(await _service.GetByIdAsync(new RequestDto<long>() { Key = id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            var result = await _service.GetAllByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            return PartialView("~/Views/User/PartialViews/_AwardDetail.cshtml", result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(long id)
        {
            return Json(await _service.DeleteByIdAsync(new RequestDto<long>() { Key = id }));
        }
    }
}
