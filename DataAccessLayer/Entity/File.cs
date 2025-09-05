using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class File: BaseEntity
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string UploadedBy { get; set; }
        public DateTime DateUploaded { get; set; }


        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
