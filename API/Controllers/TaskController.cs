using Application.Interfaces.IServices;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        //[Authorize]
        [HttpPost("/addTask")]
        public IActionResult Post([FromBody] TaskToCreateDTO task)
        {
            try
            {
                taskService.Create(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updateTask")]
        public IActionResult Put([FromBody] TaskToUpdateDTO task)
        {
            try
            {
                taskService.Update(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removeTask")]
        public IActionResult Delete(string id)
        {
            try
            {
                taskService.Delete(Guid.Parse(id));
                return Ok();
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpPost("/getTasks")]
        public IActionResult GetTasks(TaskOptionsDTO taskOptions)
        {
            try
            {
                var tasks = taskService.getAll(taskOptions);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        [HttpGet("/getTask")]
        public IActionResult GetTask(string id)
        {
            try
            {
                var task = taskService.get(Guid.Parse(id));
                return Ok(task);
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }
    }
}
