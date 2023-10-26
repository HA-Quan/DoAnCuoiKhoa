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
    [Table("Promotion")]
    public class Promotion : BaseEntity
    {
        [Key]
        [Column("promotionID")]
        public Guid PromotionID { get; set; }
        [Column("promotionCode")]
        public string PromotionCode { get; set; }

        [Column("discount")]
        public long Discount { get; set; }

        [Column("dayExpired")]
        public DateTime DayExpired { get; set; }
        [Column("dayStart")]
        public DateTime DayStart { get; set; }

        [Column("proviso")]
        public long Proviso { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("numUsed")]
        public int NumUsed { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
