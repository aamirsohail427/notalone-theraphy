using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Models.Dtos.StoreProcedureDto
{
    public class ConversationUsersDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long ConversationId { get; set; }
        public int UnReadMessages { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastMessage { get; set; }


    }
}
