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
    public class UserController : ControllerBase
    {
        private readonly IChatService _chatService;
       // protected ResponseDTO _response;
        public UserController(IChatService chatService)
        {
            _chatService = chatService;
           // this._response = new ResponseDTO();
        }

        // Register a user for Chatting 
        [HttpPost(EndpointConstants.RegisterUser)]
        public async Task<IActionResult> RegisterUser(UserDTO user)
        {
            var userId = await _chatService.AddUser(user);
            if (userId == 0)
            {
                return BadRequest(new { message = $"User '{user.Name}' is Already Online" });
            }
            return Ok(new { message = $"{userId}" });
        }

        [HttpGet(EndpointConstants.GetAllUser)]
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = await _chatService.GetAllUsers();
            if (allUsers != null)
            {
                return Ok(allUsers);
            }
            return BadRequest(new { message = "No Users Available" });
        }







        //public async Task<ResponseDTO> RegisterUser1(UserDTO user)
        //{
        //    try
        //    {
        //        var userId = await _chatService.AddUser(user);
        //        if (userId == 0)
        //        {
        //            _response.IsSuccess = false;
        //            _response.Message = $"User '{user.Name}' is Already Online";
        //        }
        //        else
        //        {
        //            _response.GeneratedId = userId;
        //            _response.Message = "User Added";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.Message = ex.Message;
        //        _response.IsSuccess = false;
        //    }
        //    return _response;
        //}


        // Fetch all the Registered users from the database



        //public async Task<ResponseDTO> AllUsers2()
        //{
        //    try
        //    {
        //        var allUsers = await _chatService.GetAllUsers();
        //        _response.Result = allUsers;
        //        _response.Message = "Success";

        //    }
        //    catch (Exception ex)
        //    {
        //        _response.Message = ex.Message;
        //        _response.IsSuccess = false;
        //    }
        //    return _response;

        //}




    }
}
