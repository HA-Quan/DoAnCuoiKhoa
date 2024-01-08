using AdminSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Service;
using System.Collections.Generic;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BasesController<Product>
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) : base(productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// API Lấy thông tin sản phẩm theo ID sản phẩm
        /// </summary>
        /// <param name="id">ID sản phẩm muốn lấy thông tin</param>
        /// <returns>Thông tin sản phẩm với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        //[HttpGet("getByFilter")]
        //public IActionResult GetProductByFilter([FromBody] FilterModel filter, [FromBody] SortModel sort,
        //    [FromQuery] int pageSize = 5, [FromQuery] int pageNumber = 1)
        //{
        //    try
        //    {
        //        //var result = _productService.GetById(id);
        //        var a = filter; var b = sort;
        //        var c = pageNumber; var d = pageSize;
        //        var result = true;
        //        if (result != null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, result);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateErrorResultNotFoundByID());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
        //    }
        //}
        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _productService.GetAll();
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
            [FromQuery] bool byManager,
            [FromQuery] Guid categoryID,
            [FromQuery] Guid chipID,
            [FromQuery] Guid memoryID,
            [FromQuery] Guid storageID,
            [FromQuery] bool? cardType,
            [FromQuery] Guid displayID,
            [FromQuery] string? trademark,
            [FromQuery] string? keyword,
            [FromQuery] int? sort,
            [FromQuery] byte status = (byte)EnumType.StatusProduct.All,
            [FromQuery] byte demand = (byte)EnumType.DemandType.All,
            [FromQuery] byte priceRange = (byte)EnumType.SortByPrice.All,
            [FromQuery] int pageSize = 5,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var result = _productService.GetByFilter(byManager, categoryID, chipID, memoryID,
                    storageID, cardType, displayID, trademark, keyword, sort, status, demand, priceRange, pageSize, pageNumber);
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

        [HttpGet("by-demand")]
        public IActionResult GetProductByDemand(
            [FromQuery] byte demand,
            [FromQuery] int number = 5)
        {
            try
            {
                var result = _productService.GetProductByDemand(number, demand);
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

        [HttpGet("topProduct")]
        public IActionResult GetTopProduct(
            [FromQuery] byte byType,
            [FromQuery] int number = 3)
        {
            try
            {
                var result = _productService.GetTopProduct(number, byType);
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
        /// API Lấy thông tin sản phẩm theo ID sản phẩm
        /// </summary>
        /// <param name="id">ID sản phẩm muốn lấy thông tin</param>
        /// <returns>Thông tin sản phẩm với ID đã cho. Nếu thêm thất bại thì return về lỗi</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRecordById(Guid id)
        {
            try
            {
                var result = _productService.GetById(id, true);
                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
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

        [HttpGet]
        [Route("productDetail{id}")]
        public IActionResult GetProductDetailById(Guid id) {
            try {
                var result = _productService.GetById(id, false);
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
        /// API Thêm mới một sản phẩm
        /// </summary>
        /// <returns>ID của bản ghi vừa thêm. Nếu thêm thất bại thì return về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPost]
        public async Task<IActionResult> InsertRecord([FromForm] Product product, [FromForm] IFormCollection? listImages,
            [FromForm] IFormFile? mainImage, [FromForm] List<Gift> listGift)
        {
            try
            {
                //var product = new ProductModel();
                //product.Product = JsonConvert.DeserializeObject<Product>(Request.Form["product"]);
                //product.ListGifts = JsonConvert.DeserializeObject<List<Gift>>(Request.Form["listGifts"]);
                //product.ListImages = Request.Form.Files;

                var productModel = new ProductModel();
                productModel.Product = product;
                productModel.ListImages = listImages;
                productModel.MainImage = mainImage;
                productModel.ListGifts = listGift;
                var IdEmpty = Guid.Empty;
                var result = await _productService.Save(productModel, IdEmpty);

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
        /// Sửa thông tin một sản phẩm
        /// </summary>
        /// <param name="product">Thông tin sản phẩm cần sửa</param>
        /// <param name="id">ID của sản phẩm cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa. Nếu sửa thất bại thì trả về Guid rỗng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRecord([FromForm] Product product, [FromForm] IFormCollection? listImages,
            [FromForm] IFormFile? mainImage, [FromForm] List<Gift> listGift, [FromRoute] Guid id)
        {
            try
            {
                var productModel = new ProductModel();
                productModel.Product = product;
                productModel.ListImages = listImages;
                productModel.MainImage = mainImage;
                productModel.ListGifts = listGift;
                var result = await _productService.Save(productModel, id);

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
        /// Sửa nhiều sản phẩm
        /// </summary>
        /// <param name="listID">Danh sách ID các sản phẩm cần sửa</param>
        /// <returns>Số lượng sản phẩm vừa sửa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpPut("multiple")]
        public IActionResult UpdateMultipleRecord([FromBody] List<Guid> listID, byte status)
        {
            try
            {
                var result = _productService.UpdateMultiple(listID, status);
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
        /// Xóa một sản phẩm
        /// </summary>
        /// <param name="productID">ID của sản phẩm cần xóa</param>
        /// <returns>Số lượng sản phẩm vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("{productID}")]
        public IActionResult DeleteRecord([FromRoute] Guid productID)
        {
            try
            {
                List<Guid> listID = new List<Guid>() { productID };
                var result = _productService.Delete(listID);
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
        /// Xóa nhiều sản phẩm
        /// </summary>
        /// <param name="listID">Danh sách ID các sản phẩm cần xóa</param>
        /// <returns>Số lượng sản phẩm vừa xóa</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpDelete("multiple")]
        public IActionResult DeleteMultipleRecord([FromBody] List<Guid> listID)
        {
            try
            {
                var result = _productService.Delete(listID);
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

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromForm] Product product, [FromForm] IFormCollection? listImages, 
            [FromForm] IFormFile? mainImage, [FromForm] List<Gift> gift)
        {
            try
            {
                var test = product;
                return Ok();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }

    }
}
