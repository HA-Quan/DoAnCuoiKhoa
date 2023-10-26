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
    [Table("OrderProduct")]
    public class OrderProduct : BaseEntity
    {
        [Key]
        [Column("orderID")]
        public Guid OrderID { get; set; }
        [Column("orderBy")]
        public string? OrderBy { get; set; }

        [Column("fullName")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("deliveryMethod")]
        public bool DeliveryMethod { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("paymentMethod")]
        public bool PaymentMethod { get; set; }

        [Column("status")]
        public byte Status { get; set; } = (byte) EnumType.StatusOrder.NotApproved;

        [Column("paymentStatus")]
        public bool PaymentStatus { get; set; }

        [Column("promotionCode")]
        public string? PromotionCode { get; set; }
        [Column("total")]
        public long Total { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;
    }
}
