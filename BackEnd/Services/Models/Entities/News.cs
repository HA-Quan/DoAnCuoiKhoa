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
    [Table("News")]
    public class News : BaseEntity
    {
        [Key]
        [Column("newsID")]
        public Guid NewsID { get; set; }

        [Column("title")]
        public string Title { get; set; }
        [Column("image")]
        public string? Image { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;

    }
}
