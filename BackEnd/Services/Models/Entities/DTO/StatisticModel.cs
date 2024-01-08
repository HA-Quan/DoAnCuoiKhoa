using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class StatisticModel
    {
        public long TotalCapital { get; set; }
        public long TotalRevenue { get; set; }
        public long? CapitalLastMonth { get; set; }
        public long? RevenueLastMonth { get; set; }
        public List<AreaChartModel> AreaChart { get; set; } = new List<AreaChartModel>();
        public List<PieChartModel> PieChart { get; set; } = new List<PieChartModel>();
        public List<TopAccountModel> TopMembers { get; set; } = new List<TopAccountModel>();
        public List<TopAccountModel> TopStaffs { get; set; } = new List<TopAccountModel>();
        public List<TopProductModel> TopProducts { get; set; } = new List<TopProductModel>();
    }
}
