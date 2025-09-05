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
       
        public UserController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Register a user for Chatting 
        [HttpPost(EndpointConstants.RegisterUser)]
        public async Task<IActionResult> RegisterUser(UserDTO user)
        {
            var userId = await _chatService.AddUser(user);
            return Ok(new { message = $"{userId}" });
        }

        // Returns all the Users from the database Users table
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

    }
}
