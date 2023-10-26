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
    [Table("Supplier")]
    public class Supplier : BaseEntity
    {
        [Key]
        [Column("supplierID")]
        public Guid SupplierID { get; set; }

        [Column("supplierName")]
        public string SupplierName { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
