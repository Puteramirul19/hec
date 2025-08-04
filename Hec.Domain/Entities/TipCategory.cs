using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class TipCategory : Entity
    {
        public string Name {  get; set;}
        public bool IsActive { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }
    }
}
