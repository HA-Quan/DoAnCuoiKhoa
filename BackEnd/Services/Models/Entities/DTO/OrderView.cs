using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class OrderView
    {
        public OrderProduct Order { get; set; }
        public List<OrderDetailModel>? ListOrderDetail { get; set; }
    }
}
