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
    [Table("Storage")]
    public class Storage : BaseEntity
    {
        [Key]
        [Column("storageID")]
        public Guid StorageID { get; set; }

        [Column("storageType")]
        public string StorageType { get; set; }

        [Column("specifications")]
        public int Specifications { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
