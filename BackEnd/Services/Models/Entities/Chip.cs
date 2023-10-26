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
    [Table("Chip")]
    public class Chip : BaseEntity
    {
        [Key]
        [Column("chipID")]
        public Guid ChipID { get; set; }

        [Column("chipType")]
        public string ChipType { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
