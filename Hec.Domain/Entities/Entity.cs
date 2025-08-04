using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Hec.Entities
{
    public abstract class Entity
    {
        private Guid _id = Guid.NewGuid();

        [Key]
        public Guid Id { get { return _id; } set { _id = value; } }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}