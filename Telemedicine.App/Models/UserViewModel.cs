using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;

namespace Telemedicine.App.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            SpecialtyLookups = new List<SelectListItem>();
            GenderTypes = new List<SelectListItem>();
            PreferredLanguageTypes = new List<SelectListItem>();
            SalutationTypes = new List<SelectListItem>();
            UserRequestDto = new UserRequestDto();
            EducationTypes = new List<SelectListItem>();
            States = new List<SelectListItem>();
        }
        public string File { get; set; }
        public List<SelectListItem> SpecialtyLookups { get; set; }
        public List<SelectListItem> GenderTypes { get; set; }
        public List<SelectListItem> PreferredLanguageTypes { get; set; }
        public List<SelectListItem> SalutationTypes { get; set; }
        public List<SelectListItem> EducationTypes { get; set; }
        public List<SelectListItem> States { get; set; }
        public UserRequestDto UserRequestDto { get; set; }
        public ResetPasswordRequestDto ResetPasswordRequestDto { get; set; }
        public DoctorExperienceRequestDto DoctorExperienceRequestDto { get; set; }
        public DoctorEducationRequestDto DoctorEducationRequestDto { get; set; }
        public DoctorAwardRequestDto DoctorAwardRequestDto { get; set; }
        public SOAPNoteRequestDto SOAPNoteRequestDto { get; set; }
        public PatientAttachmentRequestDto PatientAttachmentRequestDto { get; set; }
        public List<int> DoctorLicensedStates { get; set; }
        public int PatientStateId { get; set; }
    }
}
