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
            var response = new Response<List<ParentMessage>>();
            var parentId = _dBContext.children.Find(model.UserId)?.ParentId;
            var messages = _dBContext.ChildMessages.Where(w => w.ParentId == parentId).ToList();
            response.Data = messages;
            response.StatusCode = ResponseCode.success;
            response.Message = "all message";
            return Ok(response);
        }
    }
}
