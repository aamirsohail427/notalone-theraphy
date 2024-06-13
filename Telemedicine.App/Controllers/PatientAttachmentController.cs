using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class PatientAttachmentController : Controller
    {
        private readonly IPatientAttachmentService _service;
        private readonly ISessionManager _sessionManager;

        public PatientAttachmentController(IPatientAttachmentService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        [HttpPost]
        public async Task<IActionResult> Save(PatientAttachmentRequestDto patientAttachmentRequestDto)
        {
            var files = Request.Form.Files;
            foreach (IFormFile source in files)
            {
                string[] extentions = { ".png", ".jpg", ".jpeg", ".pdf" };
                var extention = Path.GetExtension(source.FileName);
                if (extentions.Contains(extention))
                {
                    string pathToUpload = _sessionManager.GetUserId() + "/";
                    string fileUrl = await AzureBlobHandler.Upload(source, pathToUpload);
                    if (!(string.IsNullOrEmpty(fileUrl)))
                    {
                        patientAttachmentRequestDto.Url = fileUrl;
                        return Json(await _service.SaveAsync(_sessionManager.GetUserId(), patientAttachmentRequestDto));
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
                    Message = "Please upload .png, .jpg, .jpeg, .pdf file",
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

        [HttpPost]
        public async Task<IActionResult> GetById(long id)
        {
            return Json(await _service.GetByIdAsync(new RequestDto<long>() { Key = id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUser()
        {
            var result = await _service.GetAllByUserAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            return PartialView("~/Views/User/PartialViews/_FileDetail.cshtml", result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(long id)
        {
            return Json(await _service.DeleteByIdAsync(new RequestDto<long>() { Key = id }));
        }
    }
}
