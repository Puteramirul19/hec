using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class State : Entity
    {
        [Index, MaxLength(10)] public string Code { get; set; }
        [Index, MaxLength(10)] public string EhrmsCode { get; set; }
        public string Name { get; set; }
        public DayOfWeek Weekend1 { get; set; }
        public DayOfWeek Weekend2 { get; set; }

        /// <summary>
        /// Corresponding tblState ID used in PAP/PowerAlert
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Integration Id is required.")]
        [UIHint("Integer")]
        public int IntegrationId { get; set; }
    }
}
