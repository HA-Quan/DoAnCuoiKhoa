﻿using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Service;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : BasesController<Promotion>
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService) : base(promotionService)
        {
            _promotionService = promotionService;
        }
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _promotionService.GetAll();
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
        /// API Lấy thông tin  theo ID khuyến mãi
        /// </summary>
        /// <param name="id">ID khuyến mãi muốn lấy thông tin</param>
        /// <returns>Thông tin khuyến mãi với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _promotionService.GetById(id);
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
        /// API Lấy thông tin theo Code khuyến mãi
        /// </summary>
        /// <param name="code">Code khuyến mãi muốn lấy thông tin</param>
        /// <returns>Thông tin khuyến mãi với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet("byCode")]
        public IActionResult GetRecordByCode([FromQuery] string code)
        {
            try
            {
                var result = _promotionService.GetByCode(code);
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
        /// API Thêm mới một khuyến mãi
        /// </summary>
        /// <param name="promotion">khuyến mãi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] Promotion promotion)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _promotionService.Save(promotion, IdEmpty);

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
        /// Sửa thông tin một khuyến mãi
        /// </summary>
        /// <param name="promotion">Thông tin khuyến mãi cần sửa</param>
        /// <param name="id">ID của khuyến mãi cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromBody] Promotion promotion, [FromRoute] Guid id)
        {
            try
            {
                var result = _promotionService.Save(promotion, id);

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
        /// Sửa nhiều khuyến mãi
        /// </summary>
        /// <param name="listID">Danh sách ID các khuyến mãi cần sửa</param>
        /// <returns>Số lượng khuyến mãi vừa sửa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut("multiple")]
        public IActionResult UpdateMultipleRecord([FromBody] List<Guid> listID, bool status)
        {
            try
            {
                var result = _promotionService.UpdateMultiple(listID, status);
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
        /// Xóa một khuyến mãi
        /// </summary>
        /// <param name="promotionID">ID của khuyến mãi cần xóa</param>
        /// <returns>Số lượng khuyến mãi vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{promotionID}")]
        public IActionResult DeleteRecord([FromRoute] Guid promotionID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { promotionID };
                var result = _promotionService.Delete(listID);
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
        /// Xóa nhiều khuyến mãi
        /// </summary>
        /// <param name="listID">Danh sách ID các khuyến mãi cần xóa</param>
        /// <returns>Số lượng khuyến mãi vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _promotionService.Delete(listID);
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
