using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class FileByteArray: BaseEntity
    {
        public byte[] Content { get; set; }
        // Foreign Key

        public int FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
      

    }
}
