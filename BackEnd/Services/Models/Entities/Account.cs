using Microsoft.EntityFrameworkCore;
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
    [Table("Account")]
    public class Account : BaseEntity
    {
        [Key]
        [Column("accountID")]
        public Guid AccountID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("status")]
        public bool Status { get; set; } = EnumType.Status.Using;

        [Column("fullName")]
        public string FullName { get; set; }

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("role")]
        public byte Role { get; set; } = (byte) EnumType.Role.Member;

        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;

    }
}
