using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Global.Modals.Dtos
{
    public class FileDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedTo { get; set; }
        public string Content { get; set; }
        public DateTime DateUploaded { get; set; }

        // content
        public string FileContent { get; set; }
    }
}
