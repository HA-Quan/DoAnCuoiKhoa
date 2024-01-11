﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class OrderItemModel {
        public Guid DetailID { get; set; }
        public Guid ProductID { get; set; }
        public Guid RateID { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
        public int Amount { get; set; }
        public byte Condition { get; set; }
        public long Price { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
