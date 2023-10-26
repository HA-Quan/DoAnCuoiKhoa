using Services.Models.Entities.DTO;
using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities
{
    [Table("Display")]
    public class Display : BaseEntity
    {
        [Key]
        [Column("displayID")]
        public Guid DisplayID { get; set; }

        [Column("displayType")]
        public string DisplayType { get; set; }

        [Column("specifications")]
        public decimal Specifications { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
