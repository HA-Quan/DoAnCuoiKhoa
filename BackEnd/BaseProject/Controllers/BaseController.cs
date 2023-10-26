using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Service.Base;

namespace AdminSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        private readonly IServiceBase<T> _serviceBase;

        public BasesController(IServiceBase<T> serviceBase)
        {
            _serviceBase = serviceBase;
        }
        /// <summary>
        /// API Lấy danh sách toàn bộ bản ghi trong một bảng
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi trong bảng</returns>
        /// Author: HAQUAN (15/09/2023)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllRecords()
        {
            try
            {
                var records = _serviceBase.FindAll();
                return StatusCode(StatusCodes.Status200OK, records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateErrorResultException());
            }
        }
    }
}
