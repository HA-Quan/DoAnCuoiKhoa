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
    [Table("ImportProduct")]
    public class ImportProduct : BaseEntity
    {
        [Key]
        [Column("importID")]
        public Guid ImportID { get; set; }

        [ForeignKey("Product")]
        [Column("productID")]
        public Guid ProductID { get; set; }

        [ForeignKey("Supplier")]
        [Column("supplierID")]
        public Guid SupplierID { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("totalPrice")]
        public long TotalPrice { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
