using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriticalConditionBackend.Services
{
    public interface ISubUserServices
    {
        public Task<string> LoginAsync(SubUserLoginRequest model);
        public Task<Device> AddDeviceAsync(string token, DeviceCreationRequest model);
        public Task<DeviceSubUserFullResponse> GetDeviceByIdAsync(string token, Guid DeviceId);
        public Task<List<DeviceSubUserSmallResponse>> GetAllDevicesAsync(string token);
        public Task<bool> CheckIfOperatorAsync(string token);
        public Task<bool> DeviceQuickEditAsync(string token, DeviceQuickEditRequest deviceQuickEditRequest);
        public Task<bool> DeviceEditAsync(string token, DeviceEditRequest deviceEditRequest);
        public Task<bool> DeviceArchiveAsync(string token, DeviceArchiveAndUnarchiveAndDeleteRequest deviceArchiveRequest);
    }
}
