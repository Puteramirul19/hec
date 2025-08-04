using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Tip : Entity
    {
        public string Title {  get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Guid TipCategoryId { get; set; }
        public virtual TipCategory TipCategory { get; set; }
        public int DoneCount { get; set; }

        public string FileThumbId { get; set; }
        public string FileThumbName { get; set; }
        public string FileThumbExtension { get; set; }
        public int? FileThumbSize { get; set; }

        public string FilePopupId { get; set; }
        public string FilePopupName { get; set; }
        public string FilePopupExtension { get; set; }
        public int? FilePopupSize { get; set; }
    }
}
