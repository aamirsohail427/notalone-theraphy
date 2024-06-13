using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Telemedicine.Utilities;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class StripeController : Controller
    {
        public IActionResult Index()
        {
            //StripeConfiguration.ApiKey = StripeConfigurations.PUBLISHABLE_KEY;
            //var options = new CustomerCreateOptions
            //{

            //    Description = "My First Test Customer (created for API docs)",
            //};
            //var service = new CustomerService();
            //service.Create(options);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, long aptId, DateTime aptStartDateTime, DateTime aptEndDateTime)
        {
            StripeConfiguration.ApiKey = StripeConfigurations.SECRET_KEY;
            var customerOptions = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken,

            };
            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);

            var chargeOptions = new ChargeCreateOptions
            {
                Customer = customer.Id,
                Description = "Appointment Fee",
                Amount = Convert.ToInt64(Configurations.AppointmentFee) * 100,
                Currency = "usd",
            };
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);
            if (charge.Status == "succeeded")
            {
                
                return RedirectToAction(nameof(AppointmentsController.BookAnAppointment), "Appointments", new { id= aptId, startDateTime = aptStartDateTime, endDateTime= aptEndDateTime, referenceNumber = charge.Id, amount = charge.Amount });
            }
            return RedirectToAction(nameof(AccessDeniedController.Index), "AccessDenied");
        }

        //public async Task<bool> Addnewpayment(string paymentTypeStrings, long PaymentAmount, int Agencyid, int Numberofusersadded, string transactionid, int PaymentMethod)
        //{
        //    Model.Model.Payment AddnewPayment = new Model.Model.Payment();
        //    AddnewPayment.AgencyId = Agencyid;
        //    AddnewPayment.PaidForNumberOfUsers = Numberofusersadded;
        //    AddnewPayment.PaymentAmount = PaymentAmount.ToString();
        //    AddnewPayment.PaymentType = PaymentType;
        //    AddnewPayment.TransactionId = transactionid.ToString();
        //    AddnewPayment.PaymentDate = DateTime.UtcNow;
        //    AddnewPayment.NextPaymentDate = DateTime.UtcNow.AddDays(30);
        //    AddnewPayment.PaymentMethod = PaymentMethod;
        //    var addpayment = await _BillingService.CreateNewPayment(AddnewPayment);
        //    var agency = await _agencyService.GetAgencyByUserAsync(Agencyid);
        //    var usersadd = agency.NoOfUsersAdded == null ? 10 : agency.NoOfUsersAdded + Numberofusersadded;
        //    agency.NoOfUsersAdded = usersadd;
        //    var update = await _agencyService.UpdateAgencyAsync(agency, Convert.ToInt32(agency.AgencyId));
        //    return true;

        //}
    }
}
