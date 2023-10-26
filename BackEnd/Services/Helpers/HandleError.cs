using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Models.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    /// <summary>
    /// Class static gồm các hàm xử lý lỗi khi gọi API
    /// </summary>
    public static class HandleError
    {
        /// <summary>
        /// Sinh ra lỗi khi dữ liệu đầu vào k hợp lệ
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorResultValidate()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.Validate,
                UserMsg = new List<string>() {Resource.UserMsgValidate},
                DevMsg = new List<string>() {Resource.DevMsgValidate },
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi gặp exception
        /// </summary>
        /// <returns>Dối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorResultException()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.Exception,
                UserMsg = new List<string>() { Resource.UserMsg },
                DevMsg = new List<string>() { Resource.DevMsgExeption },
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi trùng data
        /// </summary>
        /// <returns>Trả về đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorResultDuplicate()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.Validate,
                UserMsg = new List<string>() { Resource.UserMsgDuplicate },
                DevMsg = new List<string>() { Resource.DevMsgDuplicate },
                MoreInfo = Resource.MoreInfo
            }; 
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi gặp lỗi ở cơ sở dữ liệu
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorResultDatabase()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.Database,
                UserMsg = new List<string>() { Resource.UserMsg },
                DevMsg = new List<string>() { Resource.DevMsgDatabase},
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi không tìm thấy bản ghi ứng với ID đầu vào
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorResultNotFoundByID()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.NotFoundByID,
                UserMsg = new List<string>() { Resource.UserMsgNotFoundRecordByID },
                DevMsg = new List<string>() { Resource.DevMsgNotFoundRecordByID },
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi cập nhật thất bại
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorUpdateFail()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.NotFoundByID,
                UserMsg = new List<string>() { Resource.UserMsgUpdateFail },
                DevMsg = new List<string>() { Resource.DevMsgUpdateFail },
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi xóa thất bại
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorDeleteFail()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.NotFoundByID,
                UserMsg = new List<string>() { Resource.UserMsgDeleteFail },
                DevMsg = new List<string>() { Resource.DevMsgDeleteFail },
                MoreInfo = Resource.MoreInfo
            };
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi thêm mới thất bại
        /// </summary>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: HAQUAN(14/09/2023)
        public static ErrorResult GenerateErrorAddFail()
        {
            return new ErrorResult()
            {
                ErrorCode = EnumType.ErrorCode.NotFoundByID,
                UserMsg = new List<string>() { Resource.UserMsgAddFail },
                DevMsg = new List<string>() { Resource.DevMsgAddFail },
                MoreInfo = Resource.MoreInfo
            };
        }
    }
}
