using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Enum
{
    public class EnumType
    {
        /// <summary>
        /// Phân quyền
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum Role : byte
        {
            /// <summary>
            /// Tất cả
            /// </summary>
            All = 0,

            /// <summary>
            /// Khách hàng thành viên
            /// </summary>
            Member = 1,

            /// <summary>
            /// Nhân viên
            /// </summary>
            Staff = 2,

            /// <summary>
            /// Quản lý
            /// </summary>
            Management = 3,

            /// <summary>
            /// Quản trị
            /// </summary>
            Admin = 4

        }
        /// <summary>
        /// Trạng thái xóa
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public class DeleteFlag
        {
            /// <summary>
            /// Đang hoạt động
            /// </summary>
            public static bool Using = false;

            /// <summary>
            /// Đã xóa
            /// </summary>
            public static bool Deleted = true;
        }

        /// <summary>
        /// Trạng thái sử dụng
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public class Status
        {
            /// <summary>
            /// Đang sử dụng
            /// </summary>
            public static bool Using = true;

            /// <summary>
            /// Ngừng sử dụng
            /// </summary>
            public static bool StopUsing = false;
        }

        /// <summary>
        /// Loại card đồ họa 
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public class CardType
        {
            /// <summary>
            /// Card rời
            /// </summary>
            public static bool RemovableCard = true;

            /// <summary>
            /// Card liền
            /// </summary>
            public static bool OnboardCard = false;
        }

        public enum Sort
        {
            /// <summary>
            /// Sắp xếp từ cũ đến mới
            /// </summary>
            TimeAsc = 1,

            /// <summary>
            /// Sắp xếp từ mới về cũ
            /// </summary>
            TimeDesc = 2,

            /// <summary>
            /// Sắp xếp giá tăng dần
            /// </summary>
            PriceAsc = 3,

            /// <summary>
            /// Sắp xếp giá giảm dần
            /// </summary>
            PriceDesc = 4,

            /// <summary>
            /// Sắp xếp theo lượt xem tăng dần
            /// </summary>
            ViewAsc = 5,

            /// <summary>
            /// Sắp xếp theo lượt xem giảm dần
            /// </summary>
            ViewDesc = 6,

            /// <summary>
            /// Sắp xếp theo đánh gía tăng dần
            /// </summary>
            RateAsc = 7,

            /// <summary>
            /// Sắp xếp theo đánh gía giảm dần
            /// </summary>
            RateDesc = 8,

            /// <summary>
            /// Sắp xếp theo tên từ a-z
            /// </summary>
            NameAsc = 9,

            /// <summary>
            /// Sắp xếp theo tên từ z-a
            /// </summary>
            NameDesc = 10,
            /// <summary>
            /// Sắp xếp theo trạng thái
            /// </summary>
            StatusAsc = 11,
            /// <summary>
            /// Sắp xếp theo trạng thái
            /// </summary>
            StatusDesc = 12,
            /// <summary>
            /// Sắp xếp theo số lượng tăng dần
            /// </summary>
            QuantityAsc = 13,
            /// <summary>
            /// Sắp xếp theo số lượng giảm dần
            /// </summary>
            QuantityDesc = 14,
            /// <summary>
            /// Sắp xếp theo lượt bán tăng dần
            /// </summary>
            NumberSellAsc = 15,
            /// <summary>
            /// Sắp xếp theo lượt bán giảm dần
            /// </summary>
            NumberSellDesc = 16,

            /// <summary>
            /// Sắp xếp theo tên đăng nhập từ a-z
            /// </summary>
            UsernameAsc = 17,

            /// <summary>
            /// Sắp xếp theo tên đăng nhập từ z-a
            /// </summary>
            UsernameDesc = 18,

        }

        public enum SortAcount
        {
            /// <summary>
            /// Sắp xếp theo username tăng dần
            /// </summary>
            UsernameAsc = 1,
            /// <summary>
            /// Sắp xếp theo username giảm dần
            /// </summary>
            UsernameDesc = 2,
            /// <summary>
            /// Sắp xếp theo họ tên tăng dần
            /// </summary>
            FullNameAsc = 3,
            /// <summary>
            /// Sắp xếp theo họ tên giảm dần
            /// </summary>
            FullNameDesc = 4,
            /// <summary>
            /// Sắp xếp theo chức vụ tăng dần
            /// </summary>
            RoleAsc = 5,
            /// <summary>
            /// Sắp xếp theo chức vụ giảm dần
            /// </summary>
            RoleDesc = 6,
            /// <summary>
            /// Sắp xếp theo trạng thái tăng dần
            /// </summary>
            StatusAsc = 7,
            /// <summary>
            /// Sắp xếp theo trạng thái giảm dần
            /// </summary>
            StatusDesc = 8,
            TimeAsc = 9, 
            TimeDesc = 10,
        }

        public enum SortPromotion {
            /// <summary>
            /// Sắp xếp theo mã voucher tăng dần
            /// </summary>
            CodeAsc = 1,
            /// <summary>
            /// Sắp xếp theo mã voucher giảm dần
            /// </summary>
            CodeDesc = 2,
            /// <summary>
            /// Sắp xếp theo giá trị voucher tăng dần
            /// </summary>
            DiscountAsc = 3,
            /// <summary>
            /// Sắp xếp theo giá trị voucher giảm dần
            /// </summary>
            DiscountDesc = 4,
            /// <summary>
            /// Sắp xếp theo ngày bắt đầu voucher tăng dần
            /// </summary>
            DayStartAsc = 5,
            /// <summary>
            /// Sắp xếp theo ngày bắt đầu voucher giảm dần
            /// </summary>
            DayStartDesc = 6,
            /// <summary>
            /// Sắp xếp theo ngày kết thúc voucher tăng dần
            /// </summary>
            DayExpiredAsc = 7,
            /// <summary>
            /// Sắp xếp theo ngày kết thúc voucher giảm dần
            /// </summary>
            DayExpiredDesc = 8,
            /// <summary>
            /// Sắp xếp theo điều kiện áp dụng tăng dần
            /// </summary>
            ProvisoAsc = 9,
            /// <summary>
            /// Sắp xếp theo điều kiện áp dụng giảm dần
            /// </summary>
            ProvisoDesc = 10,
            /// <summary>
            /// Sắp xếp theo số lượng tăng dần
            /// </summary>
            QuantityAsc = 11,
            /// <summary>
            /// Sắp xếp theo số lượng giảm dần
            /// </summary>
            QuantityDesc = 12,
            /// <summary>
            /// Sắp xếp theo số lượng đã sử dụng tăng dần
            /// </summary>
            NumUsedAsc = 13,
            /// <summary>
            /// Sắp xếp theo số lượng đã sử dụng giảm dần
            /// </summary>
            NumUsedDesc = 14,
            TimeAsc = 15,
            TimeDesc = 16,

        }


        /// <summary>
        /// Nhu cầu sử dụng
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum DemandType
        {
            /// <summary>
            /// Laptop sinh viên - văn phòng
            /// </summary>
            Office = 0,

            /// <summary>
            /// Laptop Gaming
            /// </summary>
            Gaming = 1,

            /// <summary>
            /// Laptop đồ họa
            /// </summary>
            Graphics = 2,

            /// <summary>
            /// Laptop mỏng nhẹ
            /// </summary>
            ThinAndLight = 3,

            /// <summary>
            /// Sản phẩm hot
            /// </summary>
            Hot = 4,

            /// <summary>
            /// All
            /// </summary>
            All = 5,
        }

        /// <summary>
        /// Tình trạng sản phẩm
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum Condition : byte
        {
            /// <summary>
            /// Mới 100%
            /// </summary>
            New = 0,

            /// <summary>
            /// NewOutlet: Mới 99%
            /// </summary>
            NewOutlet = 1,

            /// <summary>
            /// Đã qua sử dụng
            /// </summary>
            Used = 2
        }

        /// <summary>
        /// Trạng thái sản phẩm
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum StatusProduct : byte
        {
            /// <summary>
            /// Ngừng bán
            /// </summary>
            StopSelling = 0,

            /// <summary>
            /// Đang bán
            /// </summary>
            Selling = 1,

            /// <summary>
            /// Hàng sắp về
            /// </summary>
            PrepairCome = 2,

            /// <summary>
            /// Tất cả
            /// </summary>
            All = 3,
        }

        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum StatusOrder : byte
        {
            /// <summary>
            /// Tất cả
            /// </summary>
            All = 0,

            /// <summary>
            /// Chưa duyệt
            /// </summary>
            NotApproved = 1,

            /// <summary>
            /// Đã duyệt
            /// </summary>
            Approved = 2,

            /// <summary>
            /// Đang giao hàng
            /// </summary>
            Delivering = 3,

            /// <summary>
            /// Đã nhận hàng
            /// </summary>
            Delivered = 4,

            /// <summary>
            /// Đã hủy
            /// </summary>
            Cancelled = 5
        }

        /// <summary>
        /// Phương thức mua hàng
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum DeliveryMethod : byte
        {
            /// <summary>
            /// Mua tại cửa hàng
            /// </summary>
            ReceiveAtStore = 0,

            /// <summary>
            /// Giao hàng tận nơi
            /// </summary>
            ReceiveAtHome = 1,
        }

        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum PaymentMethod : byte
        {
            /// <summary>
            /// Thanh toán khi nhận hàng
            /// </summary>
            ShipCod = 0,

            /// <summary>
            /// Thanh toán trực tuyến
            /// </summary>
            PaymentOnline = 1,
        }

        /// <summary>
        /// Tiêu chí lấy top sản phẩm
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum ByType : byte
        {
            /// <summary>
            /// Số lượt xem
            /// </summary>
            NumberView = 0,

            /// <summary>
            /// Số lượt mua
            /// </summary>
            NumberSell = 1,

            /// <summary>
            /// Khách hàng
            /// </summary>
            Member = 2,

            /// <summary>
            /// Nhân viên
            /// </summary>
            Staff = 3,

            /// <summary>
            /// Sản phẩm
            /// </summary>
            Product = 4,

            ///// <summary>
            ///// Theo năm
            ///// </summary>
            //ByYear = 5,

            ///// <summary>
            ///// Theo tháng
            ///// </summary>
            //ByMonth = 6,

        }

        /// <summary>
        /// Lọc theo khoảng giá
        /// </summary>
        /// Author: HAQUAN (13/09/2023)
        public enum SortByPrice : byte
        {
            /// <summary>
            /// Nhỏ hơn 10 triệu
            /// </summary>
            Less10M = 1,

            /// <summary>
            /// từ 10 -> 15 triệu
            /// </summary>
            From10To15 = 2,

            /// <summary>
            /// từ 15 -> 20 triệu
            /// </summary>
            From15To20 = 3,

            /// <summary>
            /// từ 20 -> 30 triệu
            /// </summary>
            From20To30 = 4,

            /// <summary>
            /// từ 30 -> 50 triệu
            /// </summary>
            From30To50 = 5,

            /// <summary>
            /// trên 50 triệu
            /// </summary>
            Over50 = 6,

            /// <summary>
            /// Tất cả
            /// </summary>
            All = 0,
        }

        /// <summary>
        /// Mã lỗi nội bộ
        /// </summary>
        /// Author: HAQUAN (14/09/2023)
        public enum ErrorCode
        {
            /// <summary>
            /// Lỗi do exception chưa xác định được
            /// </summary>
            Exception = 1,

            /// <summary>
            /// Lỗi từ database
            /// </summary>
            Database = 2,

            /// <summary>
            /// Lỗi do validate dữ liệu thất bại
            /// </summary>
            Validate = 3,

            /// <summary>
            /// Lỗi trùng lập
            /// </summary>
            Duplicate = 4,

            /// <summary>
            /// Lỗi không tìm thấy bản ghi với ID tương ứng
            /// </summary>
            NotFoundByID = 5,

            /// <summary>
            /// Lỗi sai tên đăng nhập hoặc mật khẩu
            /// </summary>
            LoginFail = 6,

            /// <summary>
            /// Lỗi không thể tạo mới renewToken
            /// </summary>
            RenewTokenFail = 7,

            /// <summary>
            /// Lỗi mật khẩu cũ không đúng
            /// </summary>
            PasswordOldInvalid = 8,

        }
    }
}
