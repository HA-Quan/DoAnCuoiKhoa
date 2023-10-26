using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Service;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : BasesController<Storage>
    {
        private readonly IStorageService _storageService;
        public StorageController(IStorageService storageService) : base(storageService)
        {
            _storageService = storageService;
        }
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _storageService.GetAll();
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
        /// API Lấy thông tin ổ cứng theo ID ổ cứng
        /// </summary>
        /// <param name="id">ID ổ cứng muốn lấy thông tin</param>
        /// <returns>Thông tin ổ cứng với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _storageService.GetById(id);
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
        /// API Thêm mới một ổ cứng
        /// </summary>
        /// <param name="storage">ổ cứng cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] Storage storage)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _storageService.Save(storage, IdEmpty);

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
        /// Sửa thông tin một ổ cứng
        /// </summary>
        /// <param name="storage">Thông tin ổ cứng cần sửa</param>
        /// <param name="id">ID của ổ cứng cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromBody] Storage storage, [FromRoute] Guid id)
        {
            try
            {
                var result = _storageService.Save(storage, id);

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
        /// Sửa nhiều ổ cứng
        /// </summary>
        /// <param name="listID">Danh sách ID các ổ cứng cần sửa</param>
        /// <returns>Số lượng ổ cứng vừa sửa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut("multiple")]
        public IActionResult UpdateMultipleRecord([FromBody] List<Guid> listID, bool status)
        {
            try
            {
                var result = _storageService.UpdateMultiple(listID, status);
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
        /// Xóa một ổ cứng
        /// </summary>
        /// <param name="storageID">ID của ổ cứng cần xóa</param>
        /// <returns>Số lượng ổ cứng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{chipID}")]
        public IActionResult DeleteRecord([FromRoute] Guid storageID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { storageID };
                var result = _storageService.Delete(listID);
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
        /// Xóa nhiều ổ cứng
        /// </summary>
        /// <param name="listID">Danh sách ID các ổ cứng cần xóa</param>
        /// <returns>Số lượng ổ cứng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _storageService.Delete(listID);
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
    }
}
