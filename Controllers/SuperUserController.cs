using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using CriticalConditionBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CriticalConditionBackend.Controllers
{
    [Authorize(Roles = CriticalConditionUserRoles.SuperUser)]
    [ApiController]
    [Route("[controller]")]
    public class SuperUserController : Controller
    {
        private readonly ISuperUserServices _superUserServices;

        public SuperUserController(ISuperUserServices superUserServices)
        {
            _superUserServices = superUserServices;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]SuperUserRegister model)
        {
            await _superUserServices.RegisterAsync(model);

            return CreatedAtAction(nameof(Login), new Response { Message = "Super-User Created Successfully", Status = "201" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SuperUserLogin model)
        {
            var token = await _superUserServices.LoginAsync(model);

            return Ok(token);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUserData([FromHeader] string Authorization)
        {
            var superUser = await _superUserServices.GetSuperUserDataAsync(Authorization);

            return Ok(superUser);
        }

        [HttpPost]
        [Route("generateuser")]
        public async Task<IActionResult> GenerateSubUser([FromHeader] string Authorization, [FromBody] SubUserGeneration model)
        {
            var subUser = await _superUserServices.GenerateSubUserAsync(Authorization, model);

            return CreatedAtAction(nameof(GetAllSubUsers), new Response() { Message = "Sub-User Created Successfully", Status = "201"});
        }

        [HttpGet]
        [Route("subusers")]
        public async Task<IActionResult> GetAllSubUsers([FromHeader]string Authorization)
        {
            var subUsers = await _superUserServices.GetAllSubUsersAsync(Authorization);

            return Ok(subUsers);
        }

        [HttpGet]
        [Route("editslog")]
        public async Task<IActionResult> GetEditsLog([FromHeader] string Authorization)
        {
            var editsLog = await _superUserServices.GetEditsLogAsync(Authorization);

            return Ok(editsLog);
        }

        [HttpGet]
        [Route("smalldevices")]
        public async Task<IActionResult> GetAllDevicesSmallCard([FromHeader] string Authorization)
        {
            var devices = await _superUserServices.GetAllDevicesSmallCardAsync(Authorization);

            return Ok(devices);
        }

        [HttpGet]
        [Route("devices")]
        public async Task<IActionResult> GetAllDevicesFullCard([FromHeader] string Authorization, [FromQuery(Name = "IsArchived")] bool IsArchived)
        {
            var devices = await _superUserServices.GetAllDevicesFullCardAsync(Authorization, IsArchived);

            return Ok(devices);
        }

        [HttpPost]
        [Route("devices/unarchive")]
        public async Task<IActionResult> UnArchiveDevice([FromHeader] string Authorization, [FromBody] DeviceArchiveAndUnarchiveAndDeleteRequest deviceUnarchiveRequest)
        {
            await _superUserServices.UnArchiveDeviceAsync(Authorization, deviceUnarchiveRequest);

            return Ok("Device Unarchived!");
        }

        [HttpPost]
        [Route("devices/delete")]
        public async Task<IActionResult> DeleteDevice([FromHeader] string Authorization, [FromBody] DeviceArchiveAndUnarchiveAndDeleteRequest deviceDeleteRequest)
        {
            await _superUserServices.DeleteDeviceAsync(Authorization, deviceDeleteRequest);

            return Ok("Device Deleted!");
        }

        [HttpPost]
        [Route("subusers/delete")]
        public async Task<IActionResult> DeleteSubUser([FromHeader] string Authorization, [FromBody] SubUserDeleteRequest subUserDeleteRequest)
        {
            await _superUserServices.DeleteSubUserAsync(Authorization, subUserDeleteRequest);

            return Ok("SubUser Deleted!");
        }
        [HttpPost]
        [Route("subusers/edit")]
        public async Task<IActionResult> EditSubUser([FromHeader] string Authorization, [FromBody] SubUserEditRequest subUserEditRequest)
        {
            await _superUserServices.EditSubUserAsync(Authorization, subUserEditRequest);

            return Ok("SubUser Edited!");
        }
    }
}
