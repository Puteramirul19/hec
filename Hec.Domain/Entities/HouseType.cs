using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class HouseType : Entity
    {
        public string HouseTypeCode { get; set; }
        public string HouseTypeName { get; set; }
        public string PremiseType { get; set; }
        public string PremiseCode { get; set; }
        public string Average { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }

        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int? FileSize { get; set; }

        public string FileHeaderId { get; set; }
        public string FileHeaderName { get; set; }
        public string FileHeaderExtension { get; set; }
        public int? FileHeaderSize { get; set; }

        public Guid HouseCategoryId { get; set; }
        [JsonIgnore]
        public virtual HouseCategory HouseCategories { get; set; }
    }
}

