﻿using Services.Models.Entities.DTO;
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
    [Table("OrderDetail")]
    public class OrderDetail : BaseEntity
    {
        [Key]
        [Column("detailID")]
        public Guid DetailID { get; set; }

        [ForeignKey("OrderProduct")]
        [Column("orderID")]
        public Guid OrderID { get; set; }

        [ForeignKey("Product")]
        [Column("productID")]
        public Guid ProductID { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("price")]
        public long Price { get; set; }
        [Column("delFlag")]
        public bool DelFalg { get; set; } = EnumType.DeleteFlag.Using;

    }
}
