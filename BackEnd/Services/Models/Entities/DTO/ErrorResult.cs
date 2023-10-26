using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    /// <summary>
    /// Thông tin lỗi trả về cho client
    /// </summary>
    public class ErrorResult
    {
        #region Property

        /// <summary>
        /// Định danh của mã lỗi nội bộ
        /// </summary>
        public EnumType.ErrorCode ErrorCode { get; set; } = EnumType.ErrorCode.Exception;

        /// <summary>
        /// Thông báo chi tiết lỗi cho người dùng
        /// </summary>
        public List<string>? UserMsg { get; set; }

        /// <summary>
        /// Thông báo chi tiết lỗi đang gặp phải cho Dev
        /// </summary>
        public List<string>? DevMsg { get; set; }

        /// <summary>
        /// Thông tin chi tiết hơn về vấn đề. Ví dụ: Đường dẫn mô tả về mã lỗi
        /// </summary>
        public string? MoreInfo { get; set; }

        /// <summary>
        /// Mã để tra cứu thông tin log: ELK, file log,...
        /// </summary>
        public string? TraceId { get; set; }

        #endregion
    }

}
