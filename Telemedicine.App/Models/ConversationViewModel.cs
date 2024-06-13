using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Models.Dtos.ResponseDto;
using Telemedicine.Models.Dtos.StoreProcedureDto;

namespace Telemedicine.App.Models
{
    public class ConversationViewModel
    {
        public List<ConversationUsersDto> conversationUsers { get; set; }
        public List<ConversationResponseDto> Conversations { get; set; }
    }
}
