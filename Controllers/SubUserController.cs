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
        public async Task<IActionResult> Login([FromBody] SubUserLoginRequest model)
        {
            var token = await _subUserServices.LoginAsync(model);

            return Ok(token);
        }

        [Authorize(Roles = CriticalConditionUserRoles.Operator)]
        [HttpPost]
        [Route("device/add")]
        public async Task<IActionResult> AddDevice([FromHeader] string Authorization, [FromBody] DeviceCreationRequest model)
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

        [HttpGet]
        [Route("isoperator")]
        public async Task<IActionResult> CheckIfOperator([FromHeader] string Authorization)
        {
            var isOP = await _subUserServices.CheckIfOperatorAsync(Authorization);

            return Ok(isOP);
        }

        [HttpPost]
        [Route("device/quickedit")]
        public async Task<IActionResult> DeviceQuickEdit([FromHeader] string Authorization, DeviceQuickEditRequest deviceQuickEditRequest)
        {
            await _subUserServices.DeviceQuickEditAsync(Authorization, deviceQuickEditRequest);

            return Ok("Device Quick Eddited!");
        }

        [HttpPost]
        [Route("device/edit")]
        public async Task<IActionResult> DeviceEdit([FromHeader] string Authorization, DeviceEditRequest deviceEditRequest)
        {
            await _subUserServices.DeviceEditAsync(Authorization, deviceEditRequest);

            return Ok("Device Eddited!");
        }

        [HttpPost]
        [Route("device/archive")]
        public async Task<IActionResult> DeviceArchive([FromHeader] string Authorization, DeviceArchiveAndUnarchiveAndDeleteRequest deviceArchiveRequest)
        {
            await _subUserServices.DeviceArchiveAsync(Authorization, deviceArchiveRequest);

            return Ok("Device Archived!");
        }
    }
}
