using Application.Interfaces.IServices;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly INameService nameService;
        private readonly IImageService imageService;
        private readonly IRelationService relationService;
        private readonly IBiographicalFactService factService;
        private readonly IVersionService versionService;
        private readonly Application.Interfaces.IServices.IAuthenticationService authenticationService;

        public PersonController(IPersonService personService, INameService nameService, IImageService imageService,
            IRelationService relationService, IBiographicalFactService factService, IVersionService versionService,
            Application.Interfaces.IServices.IAuthenticationService authenticationService)
        {
            this.personService = personService;
            this.nameService = nameService;
            this.imageService = imageService;
            this.relationService = relationService;
            this.factService = factService;
            this.versionService = versionService;
            this.authenticationService = authenticationService;
        }

        //[Authorize]
        [HttpPost("/addPerson")]
        public IActionResult Post([FromBody] PersonToCreateDTO person)
        {
            try
            {
                return Ok(personService.Create(person));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updatePerson")]
        public IActionResult Put([FromBody] PersonProfileDTO person)
        {
            try
            {
                personService.Update(person);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removePerson")]
        public IActionResult Delete(string id)
        {
            try
            {
                personService.Delete(Guid.Parse(id));
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

        [HttpPost("/getPersons")]
        public IActionResult GetPersons(PersonOptionsDTO personOptions)
        {
            try
            {
                var persons = personService.getAll(personOptions);
                return Ok(persons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        [HttpGet("/getPerson")]
        public IActionResult GetPerson(string id)
        {
            string userLogin = String.Empty;
            var authHeader = HttpContext?.Request?.Headers?["Authorization"].FirstOrDefault();
            var token = authHeader?.Length > 7 == true ? authHeader.Substring("Bearer ".Length).Trim() : null;
            userLogin = token == null ? String.Empty : authenticationService.GetUserDataFromToken(token);
            try
            {
                var person = personService.get(Guid.Parse(id), userLogin);
                return Ok(person);
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        //[Authorize]
        [HttpGet("/getDescendingTree")]
        public IActionResult getDescendingHierarchicTreeNode(string id)
        {
            try
            {
                var person = personService.getHierarchicTreeNode(Guid.Parse(id), -1);
                return Ok(person);
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        //[Authorize]
        [HttpGet("/getAscendingTree")]
        public IActionResult getAscendingHierarchicTreeNode(string id)
        {
            try
            {
                var person = personService.getHierarchicTreeNode(Guid.Parse(id), 1);
                return Ok(person);
            }
            catch (FormatException)
            {
                return BadRequest($"Invalid GUID format: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        //[Authorize]
        [HttpPost("/addPersonImage")]
        public IActionResult PostImage([FromBody] ImageToCreateDTO imageToCreate)
        {
            try
            {
                imageService.Create(imageToCreate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removePersonImage")]
        public IActionResult DeleteImage(string id)
        {
            try
            {
                imageService.Delete(Guid.Parse(id));
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

        //[Authorize]
        [HttpPost("/addPersonName")]
        public IActionResult PostName([FromBody] NameToCreateDTO name)
        {
            try
            {
                nameService.Create(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("/updatePersonName")]
        public IActionResult PutName([FromBody] NameToUpdateDTO name)
        {
            try
            {
                nameService.Update(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removePersonName")]
        public IActionResult DeleteName(string id)
        {
            try
            {
                nameService.Delete(Guid.Parse(id));
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

        //[Authorize]
        [HttpPost("/addPersonRelation")]
        public IActionResult PostRelation([FromBody] RelationToCreateDTO relation)
        {
            try
            {
                relationService.Create(relation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removePersonRelation")]
        public IActionResult DeleteRelation(string id)
        {
            try
            {
                relationService.Delete(Guid.Parse(id));
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

        //[Authorize]
        [HttpPost("/addPersonFact")]
        public IActionResult PostFact([FromBody] FactToCreateDTO fact)
        {
            try
            {
                factService.Create(fact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Exception = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("/removePersonFact")]
        public IActionResult DeleteFact(string id)
        {
            try
            {
                factService.Delete(Guid.Parse(id));
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

        //[Authorize]
        [HttpPost("/addPersonFactVersion")]
        public IActionResult PostFactVersion([FromBody] VersionToCreateDTO version)
        {
            try
            {
                versionService.Create(version);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        //[Authorize]
        [HttpPut("/updatePersonFactVersion")]
        public IActionResult PutVersion([FromBody] VersionToUpdateDTO version)
        {
            try
            {
                versionService.Update(version);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
            }
        }

        //[Authorize]
        [HttpDelete("/removePersonFactVersion")]
        public IActionResult DeleteVersion(string id)
        {
            try
            {
                versionService.Delete(Guid.Parse(id));
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



        //[Authorize]
        //[HttpGet("/testValidation")]
        //public IActionResult Validate(string id)
        //{
        //    try
        //    {
        //        return Ok(personService.verifyFactDates(Guid.Parse(id)));
        //    }
        //    catch (FormatException)
        //    {
        //        return BadRequest($"Invalid GUID format: {id}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message, StackTrace = ex.StackTrace, Sourse = ex.Source });
        //    }
        //}
    }
}
