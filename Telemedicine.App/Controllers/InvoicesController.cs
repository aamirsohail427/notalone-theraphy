using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.Models.Dtos;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _service;
        private readonly ISessionManager _sessionManager;

        public InvoicesController(IInvoiceService service, ISessionManager sessionManager)
        {
            _service = service;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
            {
                var data = await _service.GetForPatientAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
                return PartialView("~/Views/Invoices/PartialViews/_InvoicesDetail.cshtml", data.Data);
            }
            return Json(Responses.SomethingWentWrong<string>("Invoices", null));
        }
    }
}