using AdminSite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Service;
using System.Security.Claims;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BasesController<Account>
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService) : base(accountService)
        {
            _accountService = accountService;
        }
        /// <summary>
        /// API Lấy danh sách toàn bộ tài khoản chưa bị xóa
        /// </summary>
        /// <returns>Danh sách toàn bộ tài khoản chưa bị xóa trong bảng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _accountService.GetAllAccount();
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        [HttpGet("filter")]
        public IActionResult GetByFilter(
            [FromQuery] byte role,
            [FromQuery] bool? status,
            [FromQuery] string? keyword,
            [FromQuery] byte? sort,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var result = _accountService.GetByFilter(role, status, keyword, sort, pageSize, pageNumber);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// API Lấy thông tin tài khoản theo ID tài khoản
        /// </summary>
        /// <param name="id">ID tài khoản muốn lấy thông tin</param>
        /// <returns>Thông tin tài khoản với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAccountById(Guid id)
        {
            try
            {
                var result = _accountService.GetAccountById(id);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// API Thêm mới một tài khoản
        /// </summary>
        /// <param name="account">Tài khoản cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertAccount([FromBody] Account account)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _accountService.SaveAccount(account, IdEmpty);

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }

                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// Sửa thông tin một tài khoản
        /// </summary>
        /// <param name="account">Thông tin tài khoản cần sửa</param>
        /// <param name="id">ID của tài khoản cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromBody] Account account, [FromRoute] Guid id)
        {
            try
            {
                var result = _accountService.SaveAccount(account, id);

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }

                return StatusCode(StatusCodes.Status400BadRequest, result.Data);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// Sửa nhiều tài khoản
        /// </summary>
        /// <param name="listID">Danh sách ID các tài khoản cần sửa</param>
        /// <returns>Số lượng tài khoản vừa sửa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut("multiple")]
        public IActionResult UpdateMultipleAccount([FromBody] List<Guid> listID, bool status)
        {
            try
            {
                var result = _accountService.UpdateMultiple(listID, status);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// Xóa một tài khoản
        /// </summary>
        /// <param name="accountID">ID của tài khoản cần xóa</param>
        /// <returns>Số lượng tài khoản vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [Authorize(Policy = "IsStaff")]
        [HttpDelete("{accountID}")]
        public IActionResult DeleteProduct([FromRoute] Guid accountID)
        {
            //var user = HttpContext.User.Identity as ClaimsIdentity;
            //var tesst = user.HasClaim(c => c.Type == ClaimTypes.Role);
            try
            {
                List<Guid> listID = new List<Guid>() { accountID };
                var result = _accountService.DeleteAccount(listID);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);

                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }



        /// <summary>
        /// Xóa nhiều tài khoản
        /// </summary>
        /// <param name="listID">Danh sách ID các tài khoản cần xóa</param>
        /// <returns>Số lượng tài khoản vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleProduct([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _accountService.DeleteAccount(listID);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// API đăng ký tài khoản mới
        /// </summary>
        /// <param name="singUpModel">Tài khoản cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost("singUp")]
        public IActionResult SingUp([FromBody] SingUpModel singUpModel)
        {
            try
            {
                var result = _accountService.AddNewMember(singUpModel);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }


        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var acc = _accountService.GetAccountLogin(loginModel);
                if (!acc.Success)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, acc.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, acc.Data);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }

        }

        /// <summary>
        /// check đăng nhập
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("checkLogin")]
        public IActionResult GetUser()
        {
            try
            {
                var jwt = Request.Headers.Authorization.ToString().Split(' ');
                var token = jwt[1];
                var result = _accountService.GetInfoByToken(token);
                if (result.Success)
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        [HttpPost("renewToken")]
        public IActionResult RenewToken(TokenModel tokenModel)
        {
            try
            {
                var result = _accountService.RenewToken(tokenModel);
                if (result.Success)
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
            
        }

        /// <summary>
        /// API Cập nhật ảnh đại diện
        /// </summary>
        /// <returns>Thông tin tài khoản vừa cập nhật avatar. Nếu cập nhật thất bại thì return về null</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost("update-avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] UploadAvatarModel av)
        {
            try
            {
                var result = await _accountService.UpdateAvatar(av.AccountID, av.Image);

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

    }

}
