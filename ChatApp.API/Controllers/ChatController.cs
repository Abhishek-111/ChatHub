using BusinessLogicLayer.Services;
using ChatApp.API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Global.Modals.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
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

        
        // Returns a file based on file Id
       //[HttpGet("file-download/{fileId:int}")]
       [HttpGet(EndpointConstants.FileDownload+"/{fileId:int}")]
        public async Task<IActionResult> DownloadFile([FromRoute] int fileId)
        {
            return Ok(await _chatService.DownloadFile(fileId));
        }

        // Returns top 2 dates
        [HttpPost(EndpointConstants.Dates)]
        public async Task<IActionResult> GetTopDates(DateDTO date)
        {
            var result = await _chatService.GetTop2Dates(date);
            return Ok(result);
        }











        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = String.Empty;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                //filename = DateTime.Now.Ticks.ToString() + extension;
                filename = Path.GetFileNameWithoutExtension(file.FileName) + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using(var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch(Exception ex)
            {

            }
            return filename;
            //return exactpath;

        }




    }
}
