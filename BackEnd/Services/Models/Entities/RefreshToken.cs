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
    [Table("RefreshToken")]
    public class RefreshToken : BaseEntity
    {
        [Key]
        [Column("refreshTokenID")]
        public Guid RefreshTokenID { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("jwtID")]
        public Guid JwtID { get; set; }

        [Column("isUsed")]
        public bool IsUsed { get; set; }

        [Column("isRevoked")]
        public bool IsRevoked { get; set; }

        [Column("issuedAt")]
        public DateTime IssuedAt { get; set; }
        [Column("expiredAt")]
        public DateTime ExpiredAt { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
