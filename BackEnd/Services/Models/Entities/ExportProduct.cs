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
    [Table("ExportProduct")]
    public class ExportProduct : BaseEntity
    {
        [Key]
        [Column("exportID")]
        public Guid ExportID { get; set; }

        [ForeignKey("OrderDetail")]
        [Column("detailID")]
        public Guid DetailID { get; set; }

        [ForeignKey("Warehouse")]
        [Column("warehouseID")]
        public Guid WarehouseID { get; set; }
        
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
