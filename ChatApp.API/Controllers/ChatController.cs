using BusinessLogicLayer.Services;
using ChatApp.API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Global.Modals.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        protected ResponseDTO _response;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
            this._response = new ResponseDTO();
        }



        // Fetch all the previous chat history between two users
        [HttpPost(EndpointConstants.PreviousChat)]
        public async Task<IActionResult> PreviousChatHistory(GroupDTO group)
        {
            var result = await _chatService.GetPrivateChatHistory(group);
            if (result == null)
            {
                return BadRequest("No Chat History Available");
            }
            return Ok(result);
        }



        //public async Task<ResponseDTO> PreviousChatHistory1(GroupDTO group)
        //{
        //    try
        //    {
        //        var result = await _chatService.GetPrivateChatHistory(group);
        //        _response.Message = "Chat Found";
        //        _response.Result = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ex.Message;
        //    }
        //    return _response;

        //}




    }
}
