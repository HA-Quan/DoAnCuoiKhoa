using AdminSite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Entities;
using Services.Service;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BasesController<Category>
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) : base(categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _categoryService.GetAllCategory();
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
        /// API Lấy thông tin danh mục sản phẩm theo ID danh mục sản phẩm
        /// </summary>
        /// <param name="id">ID danh mục sản phẩm muốn lấy thông tin</param>
        /// <returns>Thông tin danh mục sản phẩm với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _categoryService.GetById(id);
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
        /// API Thêm mới một danh mục sản phẩm
        /// </summary>
        /// <param name="category">danh mục sản phẩm cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] Category category)
        {
            try
            {
                var IdEmpty = Guid.Empty;
                var result = _categoryService.Save(category, IdEmpty);

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
        /// Sửa thông tin một danh mục sản phẩm
        /// </summary>
        /// <param name="category">Thông tin danh mục sản phẩm cần sửa</param>
        /// <param name="id">ID của danh mục sản phẩm cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateRecord([FromBody] Category category, [FromRoute] Guid id)
        {
            try
            {
                var result = _categoryService.Save(category, id);

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
        /// Sửa nhiều danh mục sản phẩm
        /// </summary>
        /// <param name="listID">Danh sách ID các danh mục sản phẩm cần sửa</param>
        /// <returns>Số lượng danh mục sản phẩm vừa sửa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut("multiple")]
        public IActionResult UpdateMultipleRecord([FromBody] List<Guid> listID, bool status)
        {
            try
            {
                var result = _categoryService.UpdateMultiple(listID, status);
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
        /// Xóa một danh mục sản phẩm
        /// </summary>
        /// <param name="categoryID">ID của danh mục sản phẩm cần xóa</param>
        /// <returns>Số lượng danh mục sản phẩm vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{categoryID}")]
        public IActionResult DeleteRecord([FromRoute] Guid categoryID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { categoryID };
                var result = _categoryService.Delete(listID);
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
        /// Xóa nhiều danh mục sản phẩm
        /// </summary>
        /// <param name="listID">Danh sách ID các danh mục sản phẩm cần xóa</param>
        /// <returns>Số lượng danh mục sản phẩm vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _categoryService.Delete(listID);
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
    }
}
