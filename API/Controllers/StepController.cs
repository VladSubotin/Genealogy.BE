using Application.Interfaces.IServices;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly IStepService stepService;

        public StepController(IStepService stepService)
        {
            this.stepService = stepService;
        }

        //[Authorize]
        [HttpPost("/addStep")]
        public IActionResult Post([FromBody] StepToAddDTO step)
        {
            try
            {
                stepService.Create(step);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updateStep")]
        public IActionResult Put([FromBody] StepToUpdateDTO step)
        {
            try
            {
                stepService.Update(step);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/replaceStep")]
        public IActionResult Replace([FromBody] StepToReplaceDTO step)
        {
            try
            {
                stepService.Replace(step);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removeStep")]
        public IActionResult Delete(string id)
        {
            try
            {
                stepService.Delete(Guid.Parse(id));
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
    }
}
