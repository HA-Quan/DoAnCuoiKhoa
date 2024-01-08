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
    [Table("ImportDetail")]
    public class ImportDetail : BaseEntity
    {
        [Key]
        [Column("importDetailID")]
        public Guid ImportDetailID { get; set; }

        [ForeignKey("ImportProduct")]
        [Column("importID")]
        public Guid ImportID { get; set; }

        [ForeignKey("Product")]
        [Column("productID")]
        public Guid ProductID { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("priceImport")]
        public long PriceImport { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
