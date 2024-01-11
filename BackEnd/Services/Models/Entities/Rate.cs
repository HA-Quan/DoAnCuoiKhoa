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
    [Table("Rate")]
    public class Rate : BaseEntity
    {
        [Key]
        [Column("rateID")]
        public Guid RateID { get; set; }

        [ForeignKey("Product")]
        [Column("productID")]
        public Guid ProductID { get; set; }
        [ForeignKey("Account")]
        [Column("accountID")]
        public Guid AccountID { get; set; }

        [Column("numStar")]
        public byte NumStar { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }
        [Column("commentReply")]
        public string? CommentReply { get; set; }
        [Column("listImg")]
        public string? ListImg { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
