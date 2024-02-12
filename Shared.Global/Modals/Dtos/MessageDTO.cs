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

    }
}
