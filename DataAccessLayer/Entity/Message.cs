using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class Message : BaseEntity
    {
        
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
