using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.App.Models;
using Telemedicine.Models.Dtos;
using Telemedicine.Models.Dtos.RequestDto;
using Telemedicine.Services.Interfaces;

namespace Telemedicine.App.Controllers
{
    [Authorize]
    public class ConversationsController : Controller
    {

        private readonly ISessionManager _sessionManager;
        private readonly IConversationService _conversationService;
        public ConversationsController(ISessionManager sessionManager, IConversationService conversationService)
        {
            _sessionManager = sessionManager;
            _conversationService = conversationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetConversationUsers()
        {
            var m = new ConversationViewModel();
            var data = await _conversationService.GetConversationsUserAsync(
                                                             new RequestDto<long>
                                                             {
                                                                 Key = _sessionManager.GetUserId()
                                                             });
            m.conversationUsers = data.Data;
            return PartialView("~/Views/Conversations/PartialViews/_ConversationUsers.cshtml", data.Data);
        }

        public async Task<IActionResult> GetConversationByParentId(long parentId)
        {
            var m = new ConversationViewModel();
            var data = await _conversationService.GetByIdAsync(new RequestDto<long> { Key = parentId });
            m.Conversations = data.Data;
            return PartialView("~/Views/Conversations/PartialViews/_ConversationDetail.cshtml", data.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Save(ConversationRequestDto dto)
        {
            await _conversationService.SaveAsync(dto);
            return Json("1");
        }

    }
}