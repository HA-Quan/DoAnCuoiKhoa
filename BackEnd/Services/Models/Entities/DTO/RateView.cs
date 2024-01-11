using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class RateView {
        public Guid RateID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public byte Condition { get; set; }
        public string? Comment { get; set; }
        public List<string>? CommentImage { get; set; }
        public string? ReplyComment { get; set; }
        public string RateBy { get; set; }
        public int NumStar {  get; set; }
        public int RateCount { get; set; }
        public float RateAverage { get; set; }
        public DateTime RateDate { get; set; }
    }
}
