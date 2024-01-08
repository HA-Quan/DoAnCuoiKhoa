using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Service;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportProductController : BasesController<ImportProduct>
    {
        private readonly IImportProductService _importProductService;
        public ImportProductController(IImportProductService importProductService) : base(importProductService)
        {
            _importProductService = importProductService;
        }
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _importProductService.GetAll();
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
        /// API Lấy thông tin nhập hàng theo ID nhập hàng
        /// </summary>
        /// <param name="id">ID nhập hàng muốn lấy thông tin</param>
        /// <returns>Thông tin nhập hàng với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _importProductService.GetById(id);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateErrorResultNotFoundByID());
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
            [FromQuery] DateTime? timeStart,
            [FromQuery] DateTime? timeEnd,
            [FromQuery] Guid supplier,
            [FromQuery] int? sort,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var result = _importProductService.GetByFilter(timeStart, timeEnd, supplier, sort, pageSize, pageNumber);
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
        /// API Thêm mới một nhập hàng
        /// </summary>
        /// <param name="importModel">nhập hàng cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] ImportModel importModel)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _importProductService.Save(importModel, IdEmpty);

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
        /// Sửa thông tin một nhập hàng
        /// </summary>
        /// <param name="importProduct">Thông tin nhập hàng cần sửa</param>
        /// <param name="id">ID của nhập hàng cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromBody] ImportModel importModel, [FromRoute] Guid id)
        {
            try
            {
                var result = _importProductService.Save(importModel, id);

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
        /// Xóa một nhập hàng
        /// </summary>
        /// <param name="importProductID">ID của nhập hàng cần xóa</param>
        /// <returns>Số lượng nhập hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{importProductID}")]
        public IActionResult DeleteRecord([FromRoute] Guid importProductID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { importProductID };
                var result = _importProductService.Delete(listID);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Xóa nhiều nhập hàng
        /// </summary>
        /// <param name="listID">Danh sách ID các nhập hàng cần xóa</param>
        /// <returns>Số lượng nhập hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _importProductService.Delete(listID);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
