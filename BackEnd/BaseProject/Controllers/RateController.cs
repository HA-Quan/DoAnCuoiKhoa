using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Service;
using Services.Service.Base;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : BasesController<Rate>
    {
        private readonly IRateService _rateService;
        public RateController(IRateService rateService) : base(rateService)
        {
            _rateService = rateService;
        }
        /// <summary>
        /// API Lấy thông tin nhập hàng theo ID nhập hàng
        /// </summary>
        /// <param name="id">ID nhập hàng muốn lấy thông tin</param>
        /// <returns>Thông tin nhập hàng với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordByProductId(Guid id)
        {
            try
            {
                var result = _rateService.GetByProductID(id);
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

        /// <summary>
        /// API Thêm mới một nhập hàng
        /// </summary>
        /// <param name="importProduct">nhập hàng cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] Rate rate)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _rateService.Save(rate, IdEmpty);

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
        //[HttpPut]
        //[Route("{id}")]
        //public IActionResult UpdateRecord([FromBody] ImportProduct importProduct, [FromRoute] Guid id)
        //{
        //    try
        //    {
        //        var result = _importProductService.Save(importProduct, id);

        //        if (result.Success)
        //        {
        //            return StatusCode(StatusCodes.Status201Created, result.Data);
        //        }

        //        return StatusCode(StatusCodes.Status400BadRequest, result.Data);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
        //    }
        //}

        /// <summary>
        /// Xóa một nhập hàng
        /// </summary>
        /// <param name="importProductID">ID của nhập hàng cần xóa</param>
        /// <returns>Số lượng nhập hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        //[HttpDelete("{importProductID}")]
        //public IActionResult DeleteRecord([FromRoute] Guid importProductID)
        //{
        //    try
        //    {
        //        List<Guid> listID = new List<Guid>() { importProductID };
        //        var result = _importProductService.Delete(listID);
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}



        /// <summary>
        /// Xóa nhiều nhập hàng
        /// </summary>
        /// <param name="listID">Danh sách ID các nhập hàng cần xóa</param>
        /// <returns>Số lượng nhập hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        //[HttpDelete("multiple")]
        //public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        //{
        //    try
        //    {
        //        var result = _importProductService.Delete(listID);
        //        return StatusCode(StatusCodes.Status200OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}
