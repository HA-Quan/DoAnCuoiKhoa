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
    [Table("Warehouse")]
    public class Warehouse : BaseEntity
    {
        [Key]
        [Column("warehouseID")]
        public Guid WarehouseID { get; set; }

        [ForeignKey("ImportDetail")]
        [Column("importDetailID")]
        public Guid ImportDetailID { get; set; }

        [ForeignKey("Product")]
        [Column("productID")]
        public Guid ProductID { get; set; }

        [Column("priceImport")]
        public long PriceImport { get; set; }
        [Column("isSold")]
        public bool IsSold { get; set; } = false;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
