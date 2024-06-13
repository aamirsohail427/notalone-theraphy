using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _service;
        private readonly ILookupService _lookupService;
        private readonly ISessionManager _sessionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConsultationService _consultationService;
        private readonly IInvoiceService _invoiceService;
        private readonly INotificationService _notificationService;

        public AppointmentsController(IAppointmentService service, ILookupService lookupService, ISessionManager sessionManager,
            IHttpContextAccessor httpContextAccessor, IConsultationService consultationService, IInvoiceService invoiceService,
            INotificationService notificationService)
        {
            _service = service;
            _lookupService = lookupService;
            _sessionManager = sessionManager;
            _httpContextAccessor = httpContextAccessor;
            _consultationService = consultationService;
            _invoiceService = invoiceService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppointmentViewModel model = new AppointmentViewModel();

            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
            {
                var appointmentTypes = await _lookupService.GetByTypeAsync(new RequestDto<string>() { Key = LookupTypeStrings.AppointmentType });
                if (appointmentTypes.Status is 1)
                    if (!(appointmentTypes.Data is null))
                        model.AppointmentTypes.AddRange(appointmentTypes.Data
                            .Select(item => new SelectListItem { Text = item.LongTitle, Value = item.LookupId.ToString() }));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlots(CreateSlotRequestDto createSlotRequestDto)
        {
            return Json(await _service.CreateSlotsAsync(_sessionManager.GetUserId(), createSlotRequestDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
                return Json(await _service.GetForDoctorAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() }));

            if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
                return Json(await _service.GetForPatientAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() }));

            return Json(new ResponseDto<string>()
            {
                Status = 0,
                Message = "Something went wrong",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Book(long id, DateTime startDateTime, DateTime endDateTime)
        {
            CheckOverlapAppointmentRequestDto checkOverlapAppointmentRequestDto = new CheckOverlapAppointmentRequestDto()
            {
                UserId = _sessionManager.GetUserId(),
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };
            var checkOverlapAppointments = await _service.CheckOverlapAppointmentsAsync(checkOverlapAppointmentRequestDto);
            if ((checkOverlapAppointments.Status is 1) && !(checkOverlapAppointments.Data is null) && (checkOverlapAppointments.Data.Count is 0))
            {
                //return Json(new ResponseDto<bool>()
                //{
                //    Status = 1,
                //    Message = "Success",
                //    Data = true
                //});
                var result = await _service.BookAsync(_sessionManager.GetUserId(), new RequestDto<long>() { Key = id });
                if (result.Status is 1)
                    if (!(result.Data is null))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        string sessionId = OpenTokHandler.CreateSession(ipAddress);

                        await _consultationService.SaveAsync(new ConsultationRequestDto()
                        {
                            AppointmentId = id,
                            SessionId = sessionId
                        });

                        await _invoiceService.SaveAsync(new InvoiceRequestDto()
                        {
                            AppointmentId = id,
                            AmountPaid = Convert.ToDecimal(Configurations.AppointmentFee),
                            ReferenceNumber = KeyGenerator.GetUniqueKey(20)
                        });

                        await _notificationService.SaveAsync(new NotificationRequestDto()
                        {
                            NotificationTypeId = Convert.ToInt32(NotificationTypes.Appointment),
                            FromUserId = _sessionManager.GetUserId(),
                            ToUserId = result.Data.CreatedById,
                            Message = NotificationMessages.AppointmentBooked(_sessionManager.GetUsername())
                        });
                    }
                return Json(result);
            }

            return Json(new ResponseDto<bool>()
            {
                Status = 0,
                Message = "Your appoinment is being overlaped with this appointment. Please try different slot.",
                Data = false
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUpcomings()
        {
            var result = await _service.GetUpcomingsAsync(new RequestDto<long>() { Key = _sessionManager.GetUserId() });
            if (result.Status is 1)
                if (!(result.Data is null))
                {
                    result.Data.ForEach(item => item.StartTime = item.StartDate.ToTimezoneFormatTime(_sessionManager.GetTimezoneId()));
                    result.Data.ForEach(item => item.EndTime = item.EndDate.ToTimezoneFormatTime(_sessionManager.GetTimezoneId()));
                }
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> BookAnAppointment(long id, DateTime startDateTime, DateTime endDateTime, string referenceNumber, decimal amount)
        {
            var result = await _service.BookAsync(_sessionManager.GetUserId(), new RequestDto<long>() { Key = id });
            if (result.Status is 1)
                if (!(result.Data is null))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    string sessionId = OpenTokHandler.CreateSession(ipAddress);

                    await _consultationService.SaveAsync(new ConsultationRequestDto()
                    {
                        AppointmentId = id,
                        SessionId = sessionId
                    });

                    await _invoiceService.SaveAsync(new InvoiceRequestDto()
                    {
                        AppointmentId = id,
                        AmountPaid = (amount/100),
                        ReferenceNumber = referenceNumber
                    });

                    await _notificationService.SaveAsync(new NotificationRequestDto()
                    {
                        FromUserId = _sessionManager.GetUserId(),
                        ToUserId = result.Data.CreatedById,
                        Message = NotificationMessages.AppointmentBooked(_sessionManager.GetUsername()),
                        NotificationTypeId = Convert.ToInt32(NotificationTypes.Appointment)
                    });
                }
            TempData["PaymentStatus"] = "Your payment was successful and you have booked your appointment successfully";
            TempData["DoctorId"] = Convert.ToString(result.Data.CreatedById);
            return RedirectToAction(nameof(DoctorsController.Index), "Doctors");

            //CheckOverlapAppointmentRequestDto checkOverlapAppointmentRequestDto = new CheckOverlapAppointmentRequestDto()
            //{
            //    UserId = _sessionManager.GetUserId(),
            //    StartDateTime = startDateTime,
            //    EndDateTime = endDateTime
            //};
            //var checkOverlapAppointments = await _service.CheckOverlapAppointmentsAsync(checkOverlapAppointmentRequestDto);
            //if ((checkOverlapAppointments.Status is 1) && !(checkOverlapAppointments.Data is null) && (checkOverlapAppointments.Data.Count is 0))
            //{

            //}

            //return RedirectToAction(nameof(AccessDeniedController.Index), "AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> CancelAppointment(long id)
        {
            var apt = await _service.GetByIdAsync(new RequestDto<long>() { Key = id });
            var result = await _service.CancelAppointmentAsync(new RequestDto<long>() { Key = id });
            if(result.Status is 1)
            {
                long toUserId = 0;
                
                if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Therapist))
                {
                    toUserId = apt.Data.UserId.Value;
                }

                if (_sessionManager.GetRole().Equals(UserRoleTypeStrings.Client))
                {
                    toUserId = apt.Data.CreatedById;
                }
                await _notificationService.SaveAsync(new NotificationRequestDto()
                {
                    NotificationTypeId = Convert.ToInt32(NotificationTypes.Appointment),
                    FromUserId = _sessionManager.GetUserId(),
                    ToUserId = toUserId,
                    Message = NotificationMessages.AppointmentCanceled(_sessionManager.GetUsername())
                });
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(long id)
        {
            return Json(await _service.DeleteAppointmentAsync(new RequestDto<long>() { Key = id }));
        }
    }
}