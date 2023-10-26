using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class OrderModel
    {
        public OrderProduct Order { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }
    }
}
