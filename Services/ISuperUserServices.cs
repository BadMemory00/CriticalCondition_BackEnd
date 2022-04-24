using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriticalConditionBackend.Services
{
    public interface ISuperUserServices
    {
        public Task<bool> RegisterAsync(SuperUserRegister model);
        public Task<string> LoginAsync(SuperUserLogin model);
        public Task<SuperUserResponse> GetSuperUserDataAsync(string token);
        public Task<SubUser> GenerateSubUserAsync(string token, SubUserGeneration model);
        public Task<List<SubUser>> GetAllSubUsersAsync(string token);
        public Task<List<EditsLogResponse>> GetEditsLogAsync(string token);
        public Task<List<DeviceSuperUserSmallResponse>> GetAllDevicesSmallCardAsync(string token);
        public Task<List<DeviceSuperUserFullResponse>> GetAllDevicesFullCardAsync(string token, bool IsArchived);

    }
}
