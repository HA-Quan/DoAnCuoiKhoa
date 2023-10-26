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
    [Table("Category")]
    public class Category : BaseEntity
    {
        [Key]
        [Column("categoryID")]
        public Guid CategoryID { get; set; }

        [Column("parentID")]
        public Guid ParentID { get; set; } = Guid.Empty;

        [Column("categoryName")]
        public string CategoryName { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("trademark")]
        public string? Trademark { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;
        [Column("delFlag")]
        public bool? DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
