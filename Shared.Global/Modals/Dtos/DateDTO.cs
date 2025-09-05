using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Global.Modals.Dtos
{
    public class DateDTO
    {
        public string from { get; set; }
        public string to { get; set; }
        public DateTime timeStamp { get; set; }
        public bool passed { get; set; }
    }
}
