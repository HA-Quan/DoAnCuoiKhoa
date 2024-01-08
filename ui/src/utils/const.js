const Const = {

  DemandCategory: {
    categoryID: null,
    title: "Chọn laptop theo nhu cầu",
    categories: [
      {
        categoryName: "Laptop sinh viên - văn phòng",
        categories: []
      },
      {
        categoryName: "Laptop Gaming",
        categories: []
      },
      {
        categoryName: "Laptop đồ họa",
        categories: []
      },
      {
        categoryName: "Laptop mỏng nhẹ",
        categories: []
      },
    ]
  },
  // Loại thông báo
  TypeToast: {
    Warning: "warning",
    Success: "success",
    Fail: "fail",
  },

  // Hủy sắp sếp
  CancelSort: {
    Label: "Hủy sắp xếp",
    Value: 0,
  },

  ListSort: [
    {
      Label: "Mới nhất",
      Value: 2,
    },
    {
      Label: "Giá tăng dần",
      Value: 3,
    },
    {
      Label: "Giá giảm dần",
      Value: 4,
    },
    {
      Label: "Lượt xem",
      Value: 6,
    },
    {
      Label: "Đánh giá",
      Value: 8,
    },
    {
      Label: "Tên A-Z",
      Value: 9,
    }
  ],

  KeyStrokes: {
    CtrlShiftS: "Ctrl + Shift + S",
    CtrlShiftO: "Ctrl + Shift + O",
    CtrlS: "Ctrl + S"
  },

  GuidEmpty: "00000000-0000-0000-0000-000000000000",
  StatusProduct: [
    {
      Value: 0,
      Label: "Ngừng bán"
    },
    {
      Value: 1,
      Label: "Đang bán"
    },
    {
      Value: 2,
      Label: "Hàng sắp về"
    }
  ],
  CardType: {
    Removable: {
      Label: "Card rời",
      Value: true,
    },
    Onboard: {
      Label: "Card Onboard",
      Value: false,
    }
  },
  Condition: [
    {
      Label: "New 100%",
      Value: 0,
      Detail: "Sản phẩm chưa qua sử dụng, tình trạng mới 100% full-box"
    },
    {
      Label: "New Outlet",
      Value: 1,
      Detail: "Là sản phẩm tồn kho, có thể có chấm xước nhỏ về ngoại hình hoặc tem mác, vỏ hộp... không ảnh hưởng tới chất lượng sản phẩm. Được giảm giá nhiều so với New 100%"
    }, {
      Label: "Used",
      Value: 2,
      Detail: "Sản phẩm đã qua sử dụng"
    }
  ],
  DemandType: [
    {
      Label: "Laptop sinh viên-văn phòng",
      Value: 0
    },
    {
      Label: "Laptop Gaming",
      Value: 1
    },
    {
      Label: "Laptop Đồ họa",
      Value: 2
    },
    {
      Label: "Laptop Mỏng nhẹ-cao cấp",
      Value: 3
    }

  ],
  RoleFilter: [
    {
      Label: "Tất cả",
      Value: 0,
    },
    {
      Label: "Khách hàng thành viên",
      Value: 1,
    },
    {
      Label: "Nhân viên",
      Value: 2,
    },
    {
      Label: "Quản lý",
      Value: 3,
    },
    {
      Label: "Quản trị",
      Value: 4,
    }
  ],

  Role: [
    {
      Label: "Khách hàng thành viên",
      Value: 1,
    },
    {
      Label: "Nhân viên",
      Value: 2,
    },
    {
      Label: "Quản lý",
      Value: 3,
    },
    {
      Label: "Quản trị",
      Value: 4,
    }
  ],

  StatusFilter: [
    {
      Label: "Tất cả",
      Value: ""
    },
    {
      Label: "Đang sử dụng",
      Value: true
    },
    {
      Label: "Ngừng sử dụng",
      Value: false
    }
  ],

  Status: [
    {
      Label: "Đang sử dụng",
      Value: true
    },
    {
      Label: "Ngừng sử dụng",
      Value: false
    }
  ],

  StatusOrder: [
    {
      Label: "Chưa duyệt",
      Value: 1
    },
    {
      Label: "Đã duyệt",
      Value: 2
    },
    {
      Label: "Đang giao hàng",
      Value: 3
    },
    {
      Label: "Đã nhận hàng",
      Value: 4
    },
    {
      Label: "Đã hủy",
      Value: 5
    }
  ],

  StatusOrderFilter: [
    {
      Label: "Tất cả",
      Value: 0
    },
    {
      Label: "Chưa duyệt",
      Value: 1
    },
    {
      Label: "Đã duyệt",
      Value: 2
    },
    {
      Label: "Đang giao hàng",
      Value: 3
    },
    {
      Label: "Đã nhận hàng",
      Value: 4
    },
    {
      Label: "Đã hủy",
      Value: 5
    }
  ],

  CategoryDemand: [
    {
      Icon: "icon-office",
      Text: "Sinh viên, Văn phòng",
      Demand: 0
    },
    {
      Icon: "icon-gaming",
      Text: "Gaming",
      Demand: 1
    },
    {
      Icon: "icon-graphics",
      Text: "Đồ họa, kỹ thuật",
      Demand: 2
    }, {
      Icon: "icon-high-class",
      Text: "Mỏng nhẹ, cao cấp",
      Demand: 3
    }
  ],

  ListStep: [
    {
      Value: 1,
      Icon: "icon-cart",
      Label: "Chọn sản phẩm"
    },
    {
      Value: 2,
      Icon: "icon-info-order",
      Label: "Thông tin đặt hàng"
    },
    {
      Value: 3,
      Icon: "icon-payment",
      Label: "Thanh toán"
    },
    {
      Value: 4,
      Icon: "icon-done",
      Label: "Hoàn tất đặt hàng"
    }
  ],

  DeliveryMethod: {
    AtStore: {
      Label: "Nhận sản phẩm tại cửa hàng",
      Value: false
    },
    AtHome: {
      Label: "Giao hàng tận nơi",
      Value: true
    }
  },

  DeliveryMethodList: [

    {
      Label: "Nhận sản phẩm tại cửa hàng",
      Value: false
    },
    {
      Label: "Giao hàng tận nơi",
      Value: true
    }
  ],

  DeliveryMethodFilter: [
    {
      Label: "Tất cả",
      Value: ""
    },
    {
      Label: "Nhận sản phẩm tại cửa hàng",
      Value: false
    },
    {
      Label: "Giao hàng tận nơi",
      Value: true
    }
  ],

  PaymentMethod: {
    ShipCod: {
      Label: "Thanh toán khi nhận hàng(COD)",
      Value: false
    },
    PaymentOnline: {
      Label: "Thanh toán trực tuyến",
      Value: true
    }
  },

  PaymentMethodList: [
    {
      Label: "Thanh toán khi nhận hàng(COD)",
      Value: false
    },
    {
      Label: "Thanh toán trực tuyến",
      Value: true
    }
  ],

  PaymentMethodFilter: [
    {
      Label: "Tất cả",
      Value: ""
    },
    {
      Label: "Thanh toán khi nhận hàng(COD)",
      Value: false
    },
    {
      Label: "Thanh toán trực tuyến",
      Value: true
    }
  ],

  PaymentStatus: {
    Unpaid: {
      Label: "Chưa thanh toán",
      Value: false
    },
    Paid: {
      Label: "Đã thanh toán",
      Value: true
    }
  },

  UseStatus: {
    Using: {
      Label: "Đang sử dụng",
      Value: true
    },
    StopUsing: {
      Label: "Ngừng sử dụng",
      Value: false
    }
  },

  PaymentStatusFilter: [
    {
      Label: "Tất cả",
      Value: ""
    },
    {
      Label: "Chưa thanh toán",
      Value: false
    },
    {
      Label: "Đã thanh toán",
      Value: true
    }
  ],

  ListAddress: [
    {
      Index: 6,
      Label: "125 Trần Đại Nghĩa, Hai Bà Trưng, HN",
      Link: "https://www.google.com/maps/dir//Laptop88+125+P.+Tr%E1%BA%A7n+%C4%90%E1%BA%A1i+Ngh%C4%A9a+B%C3%A1ch+Khoa+Hai+B%C3%A0+Tr%C6%B0ng,+H%C3%A0+N%E1%BB%99i/@21.0022697,105.8449939,10z/data=!4m8!4m7!1m0!1m5!1m1!1s0x3135ac7150752d8d:0xd8616f0de4ac00c4!2m2!1d105.8449843!2d21.0022797?entry=ttu"
    },
    {
      Index: 7,
      Label: "LK3C5 Nguyễn Văn Lộc - Quận Hà Đông, HN",
      Link: "https://www.google.com/maps/dir//Laptop88+LK3C5,+P.+Nguy%E1%BB%85n+V%C4%83n+L%E1%BB%99c+L%C3%A0ng+Vi%E1%BB%87t+ki%E1%BB%81u+Ch%C3%A2u+%C3%82u+H%C3%A0+%C4%90%C3%B4ng,+H%C3%A0+N%E1%BB%99i/@20.9836258,105.7881235,10z/data=!4m8!4m7!1m0!1m5!1m1!1s0x3135ac7134a76567:0xbc57f7c7e0c3e57b!2m2!1d105.7881235!2d20.9836258?shorturl=1"
    },
    {
      Index: 8,
      Label: "34 Hồ Tùng Mậu, Cầu Giấy, HN",
      Link: "https://www.google.com/maps/dir//Laptop88+34+%C4%90.+H%E1%BB%93+T%C3%B9ng+M%E1%BA%ADu+Mai+D%E1%BB%8Bch+C%E1%BA%A7u+Gi%E1%BA%A5y,+H%C3%A0+N%E1%BB%99i/@21.0368072,105.7785009,10z/data=!4m8!4m7!1m0!1m5!1m1!1s0x313455ac07560f99:0x61f547b52d2ed230!2m2!1d105.7785009!2d21.0368072?shorturl=1"
    },
    {
      Index: 9,
      Label: "277 Nguyễn Văn Cừ, Long Biên, HN",
      Link: "https://www.google.com/maps/dir//Laptop88+277+%C4%90.+Nguy%E1%BB%85n+V%C4%83n+C%E1%BB%AB+Ng%E1%BB%8Dc+L%C3%A2m+Long+Bi%C3%AAn,+H%C3%A0+N%E1%BB%99i+125000/@21.0452715,105.8744968,10z/data=!4m8!4m7!1m0!1m5!1m1!1s0x3135abc6f02a4a77:0x2e2b4dc92ab36552!2m2!1d105.8744968!2d21.0452715?shorturl=1"
    },
    {
      Index: 10,
      Label: "376 Phạm Văn Đồng, Bắc Từ Liêm, HN",
      Link: "https://www.google.com/maps/dir//376+Ph%E1%BA%A1m+V%C4%83n+%C4%90%E1%BB%93ng,+C%E1%BB%95+Nhu%E1%BA%BF,+T%E1%BB%AB+Li%C3%AAm,+H%C3%A0+N%E1%BB%99i/@21.0580448,105.7803529,17z/data=!4m8!4m7!1m0!1m5!1m1!1s0x3135ab2adacdcd0d:0x10814efd95f2d551!2m2!1d105.7829278!2d21.0580398?hl=vi&shorturl=1"
    },
    {
      Index: 11,
      Label: "63 Nguyễn Thiện Thuật, Quận 3, TP HCM",
      Link: "https://www.google.com/maps/place/Laptop88/@10.7669776,106.6788273,17z/data=!3m1!4b1!4m5!3m4!1s0x31752f4ffeb1fdd9:0xdb9023749504b4ee!8m2!3d10.7669776!4d106.681016?shorturl=1"
    }
  ],

  DateFormat: "DD/MM/YYYY",

  ListActionSetting: [
    {
      Role: 1,
      Value: [
        {
          name: "Thông tin tài khoản",
          comand: "infoOnClick",
          icon: "icon-user"
        },
        {
          name: "Đổi mật khẩu",
          comand: "changePasswordOnClick",
          icon: "icon-change-password"
        },
        {
          name: "Lịch sử mua hàng",
          comand: "historyShoppingOnClick",
          icon: "icon-history-shopping"
        },
        {
          name: "Danh sách đơn hàng",
          comand: "listOrderOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Đăng xuất",
          comand: "logout",
          icon: "icon-logout"
        }
      ]
    },
    {
      Role: 2,
      Value: [
        {
          name: "Thông tin tài khoản",
          comand: "infoOnClick",
          icon: "icon-user"
        },
        {
          name: "Đổi mật khẩu",
          comand: "changePasswordOnClick",
          icon: "icon-change-password"
        },
        {
          name: "Quản lý đơn hàng ",
          comand: "manageOrderOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý nhập hàng ",
          comand: "manageImportOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Đăng xuất",
          comand: "logout",
          icon: "icon-logout"
        }
      ]
    },
    {
      Role: 3,
      Value: [
        {
          name: "Thông tin tài khoản",
          comand: "infoOnClick",
          icon: "icon-user"
        },
        {
          name: "Đổi mật khẩu",
          comand: "changePasswordOnClick",
          icon: "icon-change-password"
        },
        {
          name: "Quản lý sản phẩm",
          comand: "manageProductOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý đơn hàng",
          comand: "manageOrderOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý nhập hàng",
          comand: "manageImportOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý quà tặng",
          comand: "manageGiftOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Đăng xuất",
          comand: "logout",
          icon: "icon-logout"
        }
      ]
    },
    {
      Role: 4,
      Value: [
        {
          name: "Thông tin tài khoản",
          comand: "infoOnClick",
          icon: "icon-user"
        },
        {
          name: "Đổi mật khẩu",
          comand: "changePasswordOnClick",
          icon: "icon-change-password"
        },
        {
          name: "Quản lý tài khoản",
          comand: "manageAccountOnClick",
          icon: "icon-history-shopping"
        },
        {
          name: "Quản lý sản phẩm",
          comand: "manageProductOnClick",
          icon: "icon-history-shopping"
        },
        {
          name: "Quản lý đơn hàng",
          comand: "manageOrderOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý nhập hàng ",
          comand: "manageImportOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý quà tặng",
          comand: "manageGiftOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý khuyến mãi",
          comand: "managePromotionOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Quản lý tin tức",
          comand: "manageNewsOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Báo cáo, thống kê",
          comand: "statisticOnClick",
          icon: "icon-shopping-list"
        },
        {
          name: "Đăng xuất",
          comand: "logout",
          icon: "icon-logout"
        }
      ]
    },
  ],

  ListMenu: [
    {
      icon: "icon-product",
      content: "Thông tin cá nhân",
      link: "/info-user",
      role: [1, 2, 3, 4]
    },
    {
      icon: "icon-product",
      content: "Đổi mật khẩu",
      link: "/change-password",
      role: [1, 2, 3, 4]
    },
    {
      icon: "icon-product",
      content: "Lịch sử mua hàng",
      link: "/history-shopping",
      role: [1]
    },
    {
      icon: "icon-product",
      content: "Quản lý tài khoản",
      link: "/account",
      role: [4]
    },
    {
      icon: "icon-product",
      content: "Quản lý sản phẩm",
      link: "/product",
      role: [2, 3, 4]
    },
    {
      icon: "icon-order",
      content: "Quản lý đơn hàng",
      link: "/order",
      role: [2, 3, 4]
    },
    {
      icon: "icon-order",
      content: "Quản lý nhập hàng",
      link: "/import",
      role: [2, 3, 4]
    },
    {
      icon: "icon-order",
      content: "Quản lý quà tặng",
      link: "/gift",
      role: [2, 3, 4]
    },
    {
      icon: "icon-order",
      content: "Quản lý khuyến mãi",
      link: "/promotion",
      role: [2, 3, 4]
    },
    {
      icon: "icon-news",
      content: "Quản lý tin tức",
      link: "/overview",
      role: [2, 3, 4]
    },
    {
      icon: "icon-product",
      content: "Báo cáo, thống kê",
      link: "/statistic",
      role: [3, 4]
    },
  ],

  Criteria: [
    {
      Label: "Thương hiệu",
      Value: "trademark",
      Key: "name"
    },  
    {
      Label: "CPU",
      Value: "chip",
      Key: "id"
    },
    {
      Label: "RAM",
      Value: "memory",
      Key: "id"
    },
    {
      Label: "Dung lượng ổ cứng",
      Value: "storage",
      Key: "id"
    },
    {
      Label: "Màn hình",
      Value: "display",
      Key: "id"
    },
    {
      Label: "Card đồ họa",
      Value: "cardType",
      Key: "name"
    },
    {
      Label: "Dòng máy",
      Value: "demand",
      Key: "id"
    },
  ],
};
export default Const;