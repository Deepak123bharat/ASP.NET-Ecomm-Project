using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabjiMandi.Entities
{
    public class BaseEntity
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50),MinLength(2)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
