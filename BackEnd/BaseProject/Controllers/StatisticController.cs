using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Models.Enum;
using Services.Service;

namespace BaseProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService) 
        {
            _statisticService = statisticService;
        }
        [HttpGet("top")]
        public IActionResult GetTop(
            [FromQuery] DateTime? timeStart,
            [FromQuery] DateTime? timeEnd,
            [FromQuery] int number = 5,
            [FromQuery] byte typeGet = (byte)EnumType.ByType.Product)
        {
            try
            {
                var result = _statisticService.GetTop(timeStart, timeEnd, number, typeGet);
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
