const Resource = {
    PrefixImage: "http://127.0.0.1:9000/doantotnghiep/",
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
    
    // Nội dung lỗi
    Error: {
        UsernameExists: "Tên đăng nhập đã tồn tại",
        ProductNameExists: "Tên sản phẩm đã tồn tại",
        ProductName: "Tên sản phẩm không được để trống",
        Title: "Tiêu đề sản phẩm không được để trống",
        Category: "Danh mục sản phẩm không được để trống",
        CategoryNotFound: "Danh mục sản phẩm không đúng. Vui lòng chọn danh mục sản phẩm trong danh sách.",
        Chip: "Loại chip không được để trống",
        ChipNotFound: "Loại chip không đúng. Vui lòng chọn loại chip trong danh sách.",
        Memory: "Loại Ram không được để trống",
        MemoryNotFound: "Loại Ram không đúng. Vui lòng chọn loại ram trong danh sách.",
        Storage: "Ổ cứng không được để trống",
        StorageNotFound: "Ổ cứng không đúng. Vui lòng chọn ổ cứng trong danh sách.",
        Display: "Loại màn hình không được để trống",
        DisplayNotFound: "Loại màn hình không đúng. Vui lòng chọn loại màn hình trong danh sách.",
        ChipDetail: "Chi tiết Chip không được để trống",
        MemoryDetail: "Chi tiết Ram không được để trống",
        StorageDetail: "Chi tiết ổ cứng không được để trống",
        DisplayDetail: "Chi tiết màn hình không được để trống",
        CardDetail: "Chi tiết card màn hình không được để trống",
        Image: "Ảnh sản phẩm không được để trống",
        Quantity: "Số lượng sản phẩm không được để trống",
        Price: "Giá tiền sản phẩm không được để trống",
        Condition: "Không để trống tình trạng sản phẩm",
        AvatarProduct: "Không để trống avatar sản phẩm",
        FullName: "Không để trống họ tên người dùng",
        UserName: "Không để trống tên đăng nhập",
        Password: "Không để trống mật khẩu",
        PasswordNotSpace: "Mật khẩu không được chứ dấu cách",
        VerifyPassword: "Mật khẩu không khớp",
        Email: "Không để trống email",
        Phone: "Số điện thoại không đúng",
        PhoneNotEmpty: "Số điện thoại không được để trống",
        InvalidEmail: "Email không đúng định dạng",
        InvalidPassword: "Mật khẩu phải có ít nhất 8 ký tự, có ít nhất một chữ thường, một chữ hoa, một chữ số và một kí tự đặc biệt!",
        FullNameNotEmpty: "Họ tên không được để trống",
        PhoneFormat: "Số điện thoại không đúng định dạng",
        AddressNotEmpty: "Vui lòng chọn địa chỉ nhận hàng",
        PromotionNotFound: "Mã Voucher không tồn tại",
        PromotionOver: "Mã Voucher đã dùng hết",
        PromotionOverTime: "Mã Voucher đã hết hạn",
        Unconditional: "Đơn hàng của bạn không đủ điều kiện để áp dụng mã Voucher",
        DateStart:"Ngày bắt đầu không được lớn hơn ngày hiện tại",
        DateEnd:"Ngày kết thúc không được lớn hơn ngày hiện tại"
    },

    // Nội dung tin nhắn, thông báo
    Message: {
        NoData: "Không có dữ liệu",
        AddProductSucces: "Thêm sản phẩm thành công",
        UpdateProductSucces: "Cập nhật sản phẩm thành công",
        DeleteSucces: "Xóa thành công",
        UsingSucces: "Sử dụng thành công",
        StopUsingSucces: "Ngừng sử dụng thành công",
        HaveError: "Có lỗi xảy ra",
        AddAccountSucces: "Thêm tài khoản thành công",
        UpdateAccountSucces: "Cập tài khoản thành công",
        UpdateInfoSucces: "Cập thông tin thành công",
        UpdateAvatarSucces: "Cập ảnh đại diện thành công",
        ChangePasswordSucces: "Đổi mật khẩu thành công",
        AddProductToCartSucces: "Thêm sản phẩm vào giỏ hàng thành công",
        AddOrderSucces: "Thêm mới đơn hàng thành công",
        UpdateOrderSucces: "Cập nhật đơn hàng thành công",
    },

    // Nội dung tiêu đề form
    Title: {
        AddAccount: "Thêm tài khoản",
        EditAccount: "Sửa tài khoản",
        AddProduct: "Thêm sản phẩm",
        EditProduct: "Sửa sản phẩm",
        Delete: "Xóa sản phẩm",
        DeleteAccount: "Xóa tài khoản",
        AddOrder: "Thêm mới đơn hàng",
        EditOrder: "Sửa đơn hàng",
        DeleteOrder: "Xóa đơn hàng",
        Notification: "Thông báo",
        Error: "Lỗi",
        Management: "Manager Home",
        VerifyDelete: "Xác nhận xóa",
        OrderSucces: "Đặt hàng thành công"
    },

    All: "Tất cả",

    ChooseImage: "Choose image ...",

    // Trạng thái
    Status: {
        Using: "Sử dụng",
        StopUsing: "Ngừng sử dụng",
        All: "Tất cả"
    },

    Text: {
        DoneNote1: "Cảm ơn Quý khách hàng đã chọn mua hàng tại TopLaptop. Trong 15 phút, TopLaptop sẽ SMS hoặc gọi để xác nhận đơn hàng.",
        DoneNote2: "* Các đơn hàng từ 21h30 tối tới 8h sáng hôm sau. TopLaptop sẽ liên hệ với Quý khách trước 10h trưa cùng ngày",
        Product: "Sản phẩm",
        Product2: "sản phẩm",
        IsExist: "đã tồn tại trong danh sách. Xin vui lòng kiểm tra lại.",
        CanDeleteProduct: "Bạn có chắc chắn muốn xóa Sản phẩm ",
        CanDeleteAccount: "Bạn có chắc chắn muốn xóa Tài khoản",
        CanDeleteOrder: "Bạn có chắc chắn muốn xóa Đơn hàng",
        SelectedNo: "đã chọn không?",
        Delete: "Bạn có chắc chắn muốn xóa",
        Selected: "đã chọn?",
        Loading: "Đang tải",
        StopFilter: "Bỏ lọc",
        Total: "Tổng số",
        Record: "Bản ghi",
        RecordInPage: "Số bản ghi/trang",
        Account: "tài khoản",
        PriceOld: "Giá gốc",
        InfoWarranty:"Thông tin bảo hành",
        InfoGift:"Thông tin Khuyến mãi",
        AssuredShopping: "Yên tâm mua hàng",
        Description: "Đặc điểm nổi bật",
        Property:"Thông số kỹ thuật",
        SameProduct: "Sản phẩm liên quan",
        FilterPrice: "Chọn khoảng giá:",
        FilterCriteria: "Chọn theo tiêu chí:",
        SortBy: "Sắp xếp theo:",
        Contact: "Liên hệ",
        GoBack: "Trở về",
        Cart: "Giỏ hàng",
        OutCart: " khỏi giỏ hàng?",
        Order: "đơn hàng",
    },

    Label: {
        SelectQuantity: "Chọn số lượng: ",
        Product: "Danh sách sản phẩm",
        ProductName: "Tên sản phẩm",
        Quantity: "Số lượng",
        Category: "Danh mục sản phẩm",
        Title: "Tiêu đề",
        PriceOld: "Giá gốc",
        PriceSell: "Giá bán",
        Description: "Thông tin chi tiết sản phẩm",
        NumberSell: "Đã bán",
        FilterProduct: "Lọc sản phẩm",
        FilterAccount: "Lọc tài khoản",
        FilterOrder: "Lọc đơn hàng",
        Selected: "Đã chọn :",
        StopSelected: "Bỏ chọn",
        Status: "Trạng thái",
        Mode: "Chức năng",
        Avatar: "Ảnh hiển thị sản phẩm",
        ImageDetail: "Ảnh chi tiết sản phẩm",
        ChipType: "Loại Chip",
        ChipDetail: "Chi tiết Chip",
        MemoryType: "Loại Ram",
        MemoryDetail: "Chi tiết Ram",
        StorageType: "Loại ổ cứng",
        StorageDetail: "Chi tiết ổ cứng",
        DisplayType: "Loại màn hình",
        DisplayDetail: "Chi tiết màn hình",
        CardType: "Loại card màn hình",
        CardDetail: "Chi tiết card màn hình",
        Demand: "Nhu cầu sử dụng",
        Condition: "Tình trạng sản phẩm",
        AmountRam: "Số lượng Ram",
        AmountDisk: "Số lượng ổ cứng",
        Weight: "Khối lượng",
        Dimension: "Kích thước",
        Material: "Vật liệu",
        Color: "Màu sắc",
        OperatingSystem: "Hệ điều hành",
        Touchpad: "Bàn di chuột",
        Keyboard: "Bàn phím",
        Speakers: "Loa",
        Battery: "Pin",
        Camera: "Máy ảnh",
        ConnectivityNetwork: "Giao tiếp mạng",
        StandardPorts: "Cổng kết nối" ,
        Gift: "Quà tặng kèm",
        Account: "Danh sách tài khoản",
        Role: "Chức vụ",
        FullName: "Họ tên",
        Username: "Tên đăng nhập",
        PasswordOld: "Mật khẩu cũ",
        Password: "Mật khẩu",
        VerifyPassword: "Xác nhận mật khẩu",
        Email: "Email",
        Phone: "Số điện thoại",
        Address: "Địa chỉ",
        Chip: "CPU",
        Memory: "RAM",
        Storage:"Ổ cứng",
        Card:"Card đồ họa",
        Display:"Màn hình",
        TemporaryTotalMoney: "Tổng tiền tạm tính",
        DiscountMoney: "Số tiền được giảm",
        FinalMMoney: "Tổng tiền thanh toán",
        InfoGuest: "Thông tin khách hàng",
        SelectDeliveryMethod: "Chọn cách thức giao hàng",
        SelectPaymentMethod: 'Chọn phương thức thanh toán',
        SelectAddress: "Quý khách vui lòng lựa chọn địa điểm nhận hàng phù hợp",
        Receiver: "Người nhận",
        PaymentMethod: 'Phương thức thanh toán',
        Order: "Danh sách đơn hàng",
        DeliveryMethod: "Hình thức giao hàng",
        StatusOrder: "Trạng thái đơn hàng",
        PaymentStatus: "Trạng thái thanh toán",
        OrderBy: "Tài khoản đặt hàng",
        ReceiverName: "Tên người nhận",
        Total: "Tổng tiền",
        TimeStart: "Ngày bắt đầu",
        TimeEnd: "Ngày kết thúc",
        Note: "Yêu cầu khác",
        PromotionCode: "Mã Voucher",
        Info: "Thông tin tài khoản",
        ChangePassword: "Đổi mật khẩu"
    },

    LabelProduct: {
        Chip: "Bộ vi xử lý",
        Memory: "Bộ nhớ trong",
        AmountMemory: "Số khe cắm",
        Storage: "Ổ cứng",
        AmountStorage: "Số ổ cứng",
        Display: "Màn hình",
        Card: "Card đồ họa",
        Weight: "Trọng lượng",
        Dimension: "Kích thước",
        Color: "Màu sắc",
        Material: "Vật liệu",
        OperatingSystem: "Hệ điều hành",
        Touchpad: "Bàn di chuột",
        Keyboard: "Bàn phím",
        Speakers: "Âm thanh",
        Battery: "Pin",
        Camera: "Máy ảnh",
        ConnectivityNetwork: "Kết nốt mạng",
        StandardPorts: "Cổng kết nối"

    },

    ProductProperty: {
        ProductName: "productName", // tên sản phẩm
        ChipDetail: "chipDetail", // chi tiết chip
        MemoryDetail: "memoryDetail", // chi tiết ram
        StorageDetail: "storageDetail", // chi tiết ổ cứng
        DisplayDetail: "displayDetail", // chi tiết màn hình
        CardDetail: "cardDetail", // chi tiết card
        Description: "description", // thông tin chi tiết
        AmountRam: "amountRam", // số lượng ram
        AmountDisk: "amountDisk", // số ổ đĩa
        Weight: "weight", // cân nặng
        Dimension: "dimension", // kích thước
        Color: "color", // màu sắc
        Material: "material", // vật liệu
        OperatingSystem: "operatingSystem", // hệ điều hành
        Touchpad: "touchpad", // bàn di chuột
        Keyboard: "keyboard", // bàn phím 
        Speakers: "speakers", // âm thanh
        Battery: "battery", // pin
        Camera: "camera", // máy ảnh
        ConnectivityNetwork: "connectivityNetwork", // giao tiếp mạng
        StandardPorts: "standardPorts", // cổng kết nốt
    },
    
    InfoGuestProperty: {
        FullName: 'fullName',
        Phone: 'phone',
        Email: 'email',
        Address: 'address',
        Note: 'note',
        PromotionCode: 'promotionCode'
    },

    AccountProperty: {
        FullName: "fullName",
        Username: "username",
        Password: "password",
        VerifyPassword: "verifyPassword",
        Email: "email",
        Phone: "phone",
        Address: "address"
    },

    Placehoder: {
        ProductName: "Nhập tên sản phẩm",
        Title: "Nhập tiêu đề sản phẩm",
        Image: "Nhập link ảnh sản phẩm",
        ImageDetail: "Nhập chi tiết ảnh sản phẩm",
        Description: "Nhập thông tin chi tiết sản phẩm",
        Search: "Nhập tên sản phẩm...",
        ChipDetail: "Nhập chi tiết Chip",
        MemoryDetail: "Nhập chi tiết Ram",
        StorageDetail: "Nhập chi tiết ổ cứng",
        DisplayDetail: "Nhập chi tiết màn hình",
        CardDetail: "Nhập chi tiết Card màn hình",
        StandardPorts: "Nhập thông tin cổng kết nối",
        Category: "Chọn danh mục sản phẩm",
        Chip: "Chọn loại chip",
        Memory: "Chọn loại ram",
        Storage: "Chọn loại ổ cứng",
        Display: "Chọn loại màn hình",
        Condition: "Chọn tình trạng sản phẩm",
        DemandType: "Chọn nhu cầu sử dụng",
        Gift: "Chọn quà tặng kèm",
        SearchAccount :"Nhập tên, username tài khoản...",
        FullName: "Nhập họ tên người dùng",
        Username: "Nhập tên đăng nhập",
        Password: "Nhập mật khẩu",
        VerifyPassword: "Xác nhận mật khẩu",
        Email: "Nhập email",
        Phone: "Nhập số điện thoại",
        Address: "Nhập địa chỉ",
        InfoGuest: {
            FullName: "Họ và tên (Bắt buộc)",
            Phone: "Số điện thoại (Bắt buộc)",
            Email: "Email (Vui lòng điền email để nhận hóa đơn VAT)",
            Note: "Yêu cầu khác",
            Address: "Nhập địa chỉ",
            PromotionCode: "Nhập mã Voucher"
        },
        TimeStart: "Chọn ngày bắt đầu",
        TimeEnd: "Chọn ngày kết thúc"
    },

    Button: {
        Close: "Đóng",
        No: "Không",
        Cancel: "Hủy",
        Save: "Lưu",
        SaveAndNew: "Lưu và thêm mới",
        SaveChange: "Lưu thay đổi",
        Filter: "Bộ lọc",
        Apply: "Áp dụng",
        AddProduct: "Thêm sản phẩm",
        DeleteProduct: "Xóa sản phẩm",
        Delete: "Xóa",
        Import: "Import",
        Export: "Export",
        See: "Xem ảnh",
        UpdateAvatar: "Cập nhật ảnh đại diện",
        Choose:"Chọn ảnh",
        BuyNow: "Mua ngay",
        AddToCart: "Thêm vào giỏ hàng",
        MoreAll: "Xem tất cả",
        MoreConfigDetail: "Xem cấu hình chi tiết",
        OrderProduct: "Tiến hành đặt hàng",
        SelectMore: "Chọn thêm sản phẩm khác",
        ContinueShoping: "Tiếp tục mua hàng",
        AddNewOrder: "Thêm mới đơn hàng",
        UpdateInfo: "Sửa thông tin",
        UpdatePassword: "Cập nhật mật khẩu"
    },

    Warranty:[
        "✅Bảo hành chính hãng Lenovo Việt Nam 36 tháng",
        "✔️Tùy chọn:",
        "- Gói BẢO HÀNH VÀNG: gia tăng thêm 12 tháng bảo hành",
        "- Gói BẢO HÀNH KIM CƯƠNG: gia tăng thêm 24 tháng bảo hành",
        "✅Giá ở trên đã bao gồm 10% VAT",
        "✅ MIỄN PHÍ GIAO HÀNG TẬN NHÀ",
        "- Với đơn hàng < 4.000.000 đồng: Miễn phí giao hàng cho đơn hàng < 5km tính từ cửa hàng Laptop88 gần nhất",
        "- Với đơn hàng > 4.000.000 đồng: Miễn phí giao hàng (khách hàng chịu phí bảo hiểm hàng hóa nếu có)"
    ],

    AssuredShopping: [
        "- Hệ thống cửa hàng toàn quốc",
        "- Đại lý phân phối chính hãng",
        "- Giá luôn tốt nhất",
        "- Hỗ trợ trả góp lãi suất thấp",
        "- Bảo hành dài, hậu mãi chu đáo",
        "- Miễn phí giao hàng toàn quốc"
    ],

    Tooltip: {
        Delete: "Xóa",
        Edit: "Sửa",
        Close: "Đóng",
        Help: "Hướng dẫn",
    },
    CartEmpty: {
        Prefix: "Giỏ hàng có 0 sản phẩm, ",
        Content: "click vào đây",
        Suffix: " để quay lại trang mua hàng"
    }

};
export default Resource;
