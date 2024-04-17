using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Global.Modals.Dtos
{
    public class MessageDTO
    {
        public int Id { get; set; }
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

        // for file type
        public int? FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FileContent { get; set; }
        //public IFormFile? File { get; set; }

    }
}
