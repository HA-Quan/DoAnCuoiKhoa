const Enum = {
  // Trạng thái chức năng
  Mode: {
    Add: 1, // Thêm mới
    Edit: 2, // Sửa
    DeleteSingle: 3, // Xóa 1,
    DeleteMultiple: 4 // Xóa nhiều,
  },

  // Mã phím
  KeyCode: {
    Enter: 13,      // keycode khi nhấn phím Enter
    ArrowUp: 38,    // keycode khi nhấn phím Up
    ArrowDown: 40,  // keycode khi nhấn phím Down
    ESC: 27,        // keycode khi nhấn phím Esc
    Tab: 9,         // keycode khi nhấn phím Tab
  },

  // Trạng thái
  Status: {
    Using: 1,     // sử dụng
    StopUsing: 0,  // ngừng sử dụng
    All: 2 // 
  },
  // mã lỗi
  Error: {
    Duplicode: 4, // lỗi trùng mã 
    InputValid: 6, // lỗi dữ liệu đầu vào
    PasswordOldInvalid: 8, // lỗi mật khẩu cũ k khớp
    Login: 401 // lỗi hết phiên đăng nhập
  },

  FirstPage: 1, // trang hiển thị mặc định

  FirstIndexFocus: 0, // vị trí đầu tiên focus

  PageSize: 20, // số sản phẩm hiển thị trong 1 trang mặc định

  // danh sách số lượng danh hiệu có thể có của page
  ListSize: [
    {
      key: 1,
      value: 10
    },
    {
      key: 2,
      value: 20
    },
    {
      key: 3,
      value: 30
    },
    {
      key: 4,
      value: 50
    },
    {
      key: 5,
      value: 100
    }
  ],

  Position: { // vị trí tọa độ
    Limited: 610, // tọa độ giới hạn
    Bottom: 40,   // tọa độ bên dưới
    Top: -145     // tọa độ bên trên
  },
  Sort: {
    Default: 0,
    TimeAsc: 1,
    TimeDesc: 2,
    PriceAsc: 3,
    PriceDesc: 4,
    ViewAsc: 5,
    ViewDesc: 6,
    RateAsc: 7,
    RateDesc: 8,
    NameAsc: 9,
    NameDesc: 10,
    StatusAsc: 11,
    StatusDesc: 12,
    QuantityAsc: 13,
    QuantityDesc: 14,
    NumberSellAsc: 15,
    NumberSellDesc: 16,
    UsernameAsc: 17,
    UsernameDesc: 18,
  },
  SortAccount: {
    UsernameAsc: 1,
    UsernameDesc: 2,
    FullNameAsc: 3,
    FullNameDesc: 4,
    RoleAsc: 5,
    RoleDesc: 6,
    StatusAsc: 7,
    StatusDesc: 8,
    TimeAsc: 9,
    TimeDesc: 10,
    All: 0
  },
  SortPromotion: {
    Default: 0,
    CodeAsc: 1,
    CodeDesc: 2,
    DiscountAsc: 3,
    DiscountDesc: 4,
    DayStartAsc: 5,
    DayStartDesc: 6,
    DayExpiredAsc: 7,
    DayExpiredDesc: 8,
    ProvisoAsc: 9,
    ProvisoDesc: 10,
    QuantityAsc: 11,
    QuantityDesc: 12,
    NumUsedAsc: 13,
    NumUsedDesc: 14,
    TimeAsc: 15,
    TimeDesc: 16
  },
  DemandType: {
    Office: 0,
    Gaming: 1,
    Graphics: 2,
    ThinAndLight: 3,
    All: 5
  },
  PriceRange: {
    Less10M: 1,
    From10To15: 2,
    From15To20: 3,
    From20To30: 4,
    From30To50: 5,
    Over50: 6,
    All: 0
  },
  Role: {
    All: 0,
    Member: 1,
    Staff: 2,
  },
  StatusOrder: {
    All: 0,
    NotApproved: 1,
    Delivered: 4,
    Cancel: 5
  },
  StatusProduct: {
    StopSelling: 0,
    Selling: 1,
    PrepairCome: 2
  },
  StatusRate: {
    NotReply: 1,
    Replyed: 2
  },
  Step: {
    SelectProduct: 1,
    InfoOrder: 2,
    Payment: 3,
    Done: 4,
  },
  Condition: {
    New: 0,
    NewOutlet: 1,
    Used: 2
  }
};
export default Enum;
