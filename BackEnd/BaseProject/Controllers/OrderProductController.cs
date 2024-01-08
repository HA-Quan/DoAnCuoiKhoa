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
    public class OrderProductController : BasesController<OrderProduct>
    {
        private readonly IOrderProductService _orderProductService;
        public OrderProductController(IOrderProductService orderProductService) : base(orderProductService)
        {
            _orderProductService = orderProductService;
        }
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _orderProductService.GetAll();
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
        [HttpGet]
        [Route("getByAccountID{id}")]
        public IActionResult GetByAccountID([FromRoute] Guid id, [FromQuery] bool isDelivered)
        {
            try
            {
                var result = _orderProductService.GetByAccountID(id, isDelivered);
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
        /// API Lấy thông tin đơn hàng theo ID đơn hàng
        /// </summary>
        /// <param name="id">ID đơn hàng muốn lấy thông tin</param>
        /// <returns>Thông tin đơn hàng với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _orderProductService.GetById(id);
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
        /// API Lấy thông tin đơn hàng theo ID đơn hàng
        /// </summary>
        /// <param name="id">ID đơn hàng muốn lấy thông tin</param>
        /// <returns>Thông tin đơn hàng với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("orderDetail{id}")]
        public IActionResult GetOrderDetailByOrderID(Guid id)
        {
            try
            {
                var result = _orderProductService.GetOrderDetailByOrderID(id);
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
            [FromQuery] DateTime? timeStart,
            [FromQuery] DateTime? timeEnd,
            [FromQuery] string? keyword,
            [FromQuery] int? sort,
            [FromQuery] bool? deliveryMethod,
            [FromQuery] bool? paymentMethod,
            [FromQuery] bool? statusPayment,
            [FromQuery] byte status = (byte) EnumType.StatusOrder.All,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var result = _orderProductService.GetByFilter(timeStart, timeEnd, keyword, sort, 
                    deliveryMethod, paymentMethod, statusPayment, status, pageSize, pageNumber);
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
        /// API Thêm mới một đơn hàng
        /// </summary>
        /// <param name="orderProduct">đơn hàng cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] OrderModel orderModel)
        {
            try
            {
                var result = _orderProductService.Save(orderModel);

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
        /// Sửa thông tin một đơn hàng
        /// </summary>
        /// <param name="orderModel">Thông tin đơn hàng cần sửa</param>
        /// <param name="id">ID của đơn hàng cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromRoute] Guid id, [FromBody] OrderModel orderModel)
        {
            try
            {
                var result = _orderProductService.Update(id, orderModel);

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
        /// Sửa trạng thái một đơn hàng
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng cần sửa</param>
        /// <param name="id">ID của đơn hàng cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("updateStatus{id}")]
        public IActionResult UpdateStatus([FromRoute] Guid id, [FromQuery] byte status)
        {
            try
            {
                var result = _orderProductService.UpdateStatus(id, status);

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
        /// Xóa một đơn hàng
        /// </summary>
        /// <param name="ordProductID">ID của đơn hàng cần xóa</param>
        /// <returns>Số lượng đơn hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{ordProductID}")]
        public IActionResult DeleteRecord([FromRoute] Guid ordProductID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { ordProductID };
                var result = _orderProductService.Delete(listID);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Xóa nhiều đơn hàng
        /// </summary>
        /// <param name="listID">Danh sách ID các đơn hàng cần xóa</param>
        /// <returns>Số lượng đơn hàng vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _orderProductService.Delete(listID);
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
