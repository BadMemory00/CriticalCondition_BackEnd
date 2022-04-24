using CriticalConditionBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CriticalConditionBackend.Services;
using System;
using CriticalConditionBackend.Models.DTOs;

namespace CriticalConditionBackend.Controllers
{
    [Authorize(Roles = CriticalConditionUserRoles.Technician + "," + CriticalConditionUserRoles.Operator)]
    [ApiController]
    [Route("[controller]")]
    public class SubUserController : Controller
    {
        private readonly ISubUserServices _subUserServices;
        public SubUserController(ISubUserServices subUserServices)
        {
            _subUserServices = subUserServices;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] SubUserLogin model)
        {
            var token = await _subUserServices.LoginAsync(model);

            return Ok(token);
        }

        [Authorize(Roles = CriticalConditionUserRoles.Operator)]
        [HttpPost]
        [Route("adddevice")]
        public async Task<IActionResult> AddDevice([FromHeader] string Authorization, [FromBody] DeviceCreation model)
        {
            await _subUserServices.AddDeviceAsync(Authorization, model);

            return CreatedAtAction(nameof(GetAllDevices), new Response() { Message = "devices created successfully", Status = "201"});
        }

        [HttpGet]
        [Route("viewdevices")]
        public async Task<IActionResult> GetAllDevices([FromHeader] string Authorization)
        {
            var devices = await _subUserServices.GetAllDevicesAsync(Authorization);

            return Ok(devices);
        }

        [HttpGet]
        [Route("viewdevices/{deviceId}")]
        public async Task<IActionResult> ViewDeviceById([FromHeader] string Authorization, [FromRoute] Guid deviceId)
        {
            var device = await _subUserServices.GetDeviceByIdAsync(Authorization, deviceId);

            return Ok(device);
        }
    }
}
