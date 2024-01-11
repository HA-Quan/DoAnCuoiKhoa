using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
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
        /// API Lấy thông tin đánh giá theo ID
        /// </summary>
        /// <param name="id">ID đánh giá muốn lấy thông tin</param>
        /// <returns>Thông tin đánh giá với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("getByProductID{id}")]
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

        [HttpGet("byStatus")]
        public IActionResult GetRateByStatus(
            [FromQuery] byte status =(byte) EnumType.StatusRate.NotReply,
            [FromQuery] int pageSize = 5,
            [FromQuery] int pageNumber = 1) {
            try {
                var result = _rateService.GetByFilter(status, pageSize, pageNumber);
                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                } else {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        [HttpGet("countRateNotReply")]
        public IActionResult GetCountRateNotReply() {
            try {
                var result = _rateService.GetCountRateNotReply();
                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                } else {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// API Lấy thông tin đánh giá theo ID
        /// </summary>
        /// <param name="id">ID đánh giá muốn lấy thông tin</param>
        /// <returns>Thông tin đánh giá với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id) {
            try {
                var result = _rateService.GetById(id);
                if (result.Success) {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                } else {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateErrorResultNotFoundByID());
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        /// <summary>
        /// API Thêm mới đánh giá
        /// </summary>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public async Task<IActionResult> InsertRecord([FromForm] Rate rate, [FromForm] IFormCollection? listImages) {
            try {
                var rateModel = new RateModel();
                rateModel.Rate = rate;
                rateModel.ListImages = listImages;
                var IdEmpty = Guid.Empty;
                var result = await _rateService.Save(rateModel, IdEmpty);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

        [HttpPost("replyRate")]
        public IActionResult ReplyComment([FromBody] ReplyModel replyModel ) {
            try {
                var result = _rateService.ReplyComment(replyModel);
                if (result.Success) {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }
                return StatusCode(StatusCodes.Status400BadRequest, result.Data);
            } catch (Exception ex) {
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
        public async Task<IActionResult> UpdateRecord([FromBody] RateModel rate, [FromRoute] Guid id) {
            try {
                var result = await _rateService.Save(rate, id);

                if (result.Success) {
                    return StatusCode(StatusCodes.Status201Created, result.Data);
                }

                return StatusCode(StatusCodes.Status400BadRequest, result.Data);

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

    }
}
