using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.DTOS;
using TechnoEgypt.Models;

namespace TechnoEgypt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly UserDbContext _dBContext;

        public MessageController(UserDbContext dBContext)
        {
            _dBContext = dBContext;
        }
        [HttpPost("GetUserMessage")]
        public IActionResult GetUserMessage(BaseDto model)
        {
            var response = new Response<List<ChildMessage>>();
            var messages = _dBContext.ChildMessages.Where(w=>w.ChildId==model.UserId).ToList();
            response.Data = messages;
            response.StatusCode = ResponseCode.success;
            response.Message = "all message";
            return Ok(response);
        }
    }
}
