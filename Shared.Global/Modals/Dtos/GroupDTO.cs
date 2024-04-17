using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Global.Modals.Dtos
{
    public class GroupDTO
    {
        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        // added
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
