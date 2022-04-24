using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriticalConditionBackend.Services
{
    public interface ISubUserServices
    {
        public Task<string> LoginAsync(SubUserLogin model);
        public Task<Device> AddDeviceAsync(string token, DeviceCreation model);
        public Task<DeviceSubUserFullResponse> GetDeviceByIdAsync(string token, Guid DeviceId);
        public Task<List<DeviceSubUserSmallResponse>> GetAllDevicesAsync(string token);

    }
}
