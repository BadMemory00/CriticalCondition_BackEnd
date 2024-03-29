﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CriticalConditionBackend.CriticalConditionExceptions;
using CriticalConditionBackend.Data;
using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using CriticalConditionBackend.Utillities;
using CriticalConditionBackend.Validations;
using Microsoft.EntityFrameworkCore;

namespace CriticalConditionBackend.Services
{
    public class SubUserServices : ISubUserServices
    {
        private readonly IConfiguration _configuration;
        private readonly CriticalConditionDbContext _dbContext;

        public SubUserServices(IConfiguration configuration, CriticalConditionDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        private const string SUBUSERCODECLAIM = "SubUserCode";

        public async Task<string> LoginAsync(SubUserLoginRequest model)
        {
            var hashedCode = EncryptionUtilities.EncryptSubUserCode(model.Code, _configuration);

            var user = await _dbContext.SubUsers.FindAsync(hashedCode);

            if (user is null)
                throw new LogicalException(CriticalConditionExceptionsEnum.SUB_USER_DOES_NOT_EXIST, StatusCodes.Status401Unauthorized);

            List<Claim> authClaims = AddSubUserClaimsAsync(user);

            return TokenUtilities.CreateToken(_configuration, authClaims);
        }

        public async Task<Device> AddDeviceAsync(string token, DeviceCreationRequest model)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            Device device = FillDeviceData(model, subUser.Code, subUser.SuperUserEmail);
            device.FMEARiskScore = DeviceUtilities.CalculateFMEARiskScore(device);

            EditsLog editsLog = AddActionToEditsLog(subUser, device, "Created");

            await _dbContext.Devices.AddAsync(device);
            await _dbContext.EditsLogs.AddAsync(editsLog);

            await _dbContext.SaveChangesAsync();

            return device;
        }

        public async Task<DeviceSubUserFullResponse> GetDeviceByIdAsync(string token, Guid DeviceId)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            var device = await _dbContext.Devices.FindAsync(DeviceId);

            if (device is null)
                throw new LogicalException(CriticalConditionExceptionsEnum.DEVICE_WAS_NOT_FOUND, 404);

            if(!device.SuperUserEmail.Equals(subUser.SuperUserEmail))
                throw new LogicalException(CriticalConditionExceptionsEnum.NOT_AUTHORIZED_TO_VIEW_DEVICE, 401);

            var deviceToReturn = FillDeviceFullResponseData(device);

            return deviceToReturn;
        }

        public async Task<List<DeviceSubUserSmallResponse>> GetAllDevicesAsync(string token)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            var devices = await _dbContext.Devices
                .Where(x => x.SuperUserEmail == subUser.SuperUserEmail && x.IsArchived == false)
                .Select(x => new DeviceSubUserSmallResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Model = x.Model,
                    Area = x.Area,
                    NumberOfFailures = x.NumberOfFailures,
                    DownTime = x.DownTime,
                    IsIoT = x.IsIoT
                }).ToListAsync();

            return devices;
        }

        public async Task<bool> CheckIfOperatorAsync(string token)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            if (subUser.Role == CriticalConditionUserRoles.Operator)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeviceQuickEditAsync(string token, DeviceQuickEditRequest deviceQuickEditRequest)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            var device = await _dbContext.Devices.FindAsync(deviceQuickEditRequest.DeviceId);
            if (device == null)
                throw new LogicalException(CriticalConditionExceptionsEnum.DEVICE_WAS_NOT_FOUND, 404);
            if(device.SuperUserEmail != subUser.SuperUserEmail)
                throw new LogicalException(CriticalConditionExceptionsEnum.NOT_AUTHORIZED_TO_EDIT_DEVICE, 401);

            device.NumberOfFailures = deviceQuickEditRequest.NumberOfFailures;
            device.DownTime = deviceQuickEditRequest.DownTime;
            device.FMEARiskScore = DeviceUtilities.CalculateFMEARiskScore(device);

            EditsLog editsLog = AddActionToEditsLog(subUser, device, "Edited");
            await _dbContext.EditsLogs.AddAsync(editsLog);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeviceEditAsync(string token, DeviceEditRequest deviceEditRequest)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            var device = await _dbContext.Devices.FindAsync(deviceEditRequest.DeviceId);
            if (device == null)
                throw new LogicalException(CriticalConditionExceptionsEnum.DEVICE_WAS_NOT_FOUND, 404);
            if (device.SuperUserEmail != subUser.SuperUserEmail)
                throw new LogicalException(CriticalConditionExceptionsEnum.NOT_AUTHORIZED_TO_EDIT_DEVICE, 401);
            
            ApplyEditedDeviceData(deviceEditRequest, device);
            device.FMEARiskScore = DeviceUtilities.CalculateFMEARiskScore(device);

            EditsLog editsLog = AddActionToEditsLog(subUser, device, "Edited");
            await _dbContext.EditsLogs.AddAsync(editsLog);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeviceArchiveAsync(string token, DeviceArchiveAndUnarchiveAndDeleteRequest deviceArchiveRequest)
        {
            var subUser = await ValidateTokenAndReturnSubUser(token);

            var device = await _dbContext.Devices.FindAsync(deviceArchiveRequest.DeviceId);
            if (device == null)
                throw new LogicalException(CriticalConditionExceptionsEnum.DEVICE_WAS_NOT_FOUND, 404);
            if (device.SuperUserEmail != subUser.SuperUserEmail)
                throw new LogicalException(CriticalConditionExceptionsEnum.NOT_AUTHORIZED_TO_EDIT_DEVICE, 401);

            if (device.IsArchived)
                throw new LogicalException(CriticalConditionExceptionsEnum.DEVICE_ALREADY_ARCHIVED, 400);
                
            device.IsArchived = true;

            EditsLog editsLog = AddActionToEditsLog(subUser, device, "Archived");
            await _dbContext.EditsLogs.AddAsync(editsLog);

            await _dbContext.SaveChangesAsync();

            return true;
        }



        private static List<Claim> AddSubUserClaimsAsync(SubUser user) => new()
        {
            new Claim(SUBUSERCODECLAIM, user.Code),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        private async Task<SubUser> ValidateTokenAndReturnSubUser(string token)
        {
            var tokenDetails = TokenUtilities.ValidateAndReturnToken(token, _configuration);

            var subUserCode = tokenDetails.Payload.GetValueOrDefault(SUBUSERCODECLAIM).ToString();

            var subUser = await _dbContext.SubUsers.FindAsync(subUserCode);

            if (subUser is null)
                throw new LogicalException(CriticalConditionExceptionsEnum.SUB_USER_DOES_NOT_EXIST, 401);

            return subUser;
        }

        private static Device FillDeviceData(DeviceCreationRequest model, string subUserCode, string SuperUserEmail) => new()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Brand = model.Brand,
            Model = model.Model,
            TypeOfService = model.TypeOfService,
            PurchaseDate = model.PurchaseDate,
            NumberOfWorkingDays = model.NumberOfWorkingDays,
            NumberOfFailures = model.NumberOfFailures,
            DownTime = model.DownTime,
            Safety = model.Safety,
            Function = model.Function,
            Area = model.Area,
            ServiceCost = model.ServiceCost,
            OperationCost = model.OperationCost,
            PurchasingCost = model.PurchasingCost,
            Detection = model.Detection,
            IsExcludedFromDSP = false,
            IsArchived = false,
            IsIoT = false,
            AddedBy = subUserCode,
            LastEditBy = subUserCode,
            AddedAt = DateTime.Now,
            LastEditDate = DateTime.Now,
            SuperUserEmail = SuperUserEmail,
        };

        private static EditsLog AddActionToEditsLog(SubUser subUser, Device device, [StringRange(AllowableValues = new string[] { "Created", "Edited", "Archived" })] string action) => new()
        {
            Id = Guid.NewGuid(),
            Action = action,
            ActionDate = DateTime.Now,
            DeviceName = device.Name,
            DeviceId = device.Id,
            SubUserUserName = subUser.UserName,
            SubUserCode = subUser.Code,
            SuperUserEmail = subUser.SuperUserEmail
        };

        private static DeviceSubUserFullResponse FillDeviceFullResponseData(Device device) => new()
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            Model = device.Model,
            TypeOfService = device.TypeOfService,
            PurchaseDate = device.PurchaseDate,
            NumberOfWorkingDays = device.NumberOfWorkingDays,
            NumberOfFailures = device.NumberOfFailures,
            DownTime = device.DownTime,
            Safety = device.Safety,
            Function = DeviceUtilities.FunctionString(device),
            Area = device.Area,
            ServiceCost = device.ServiceCost,
            OperationCost = device.OperationCost,
            PurchasingCost = device.PurchasingCost,
            Detection = device.Detection,
            IsIoT = device.IsIoT,
            FMEARiskScore = device.FMEARiskScore
        };
        private static void ApplyEditedDeviceData(DeviceEditRequest deviceEditRequest, Device device)
        {
            device.Name = deviceEditRequest.Name;
            device.Brand = deviceEditRequest.Brand;
            device.Model = deviceEditRequest.Model;
            device.TypeOfService = deviceEditRequest.TypeOfService;
            device.PurchaseDate = deviceEditRequest.PurchaseDate;
            device.NumberOfWorkingDays = deviceEditRequest.NumberOfWorkingDays;
            device.NumberOfFailures = deviceEditRequest.NumberOfFailures;
            device.DownTime = deviceEditRequest.DownTime;
            device.Safety = deviceEditRequest.Safety;
            device.Function = deviceEditRequest.Function;
            device.Area = deviceEditRequest.Area;
            device.ServiceCost = deviceEditRequest.ServiceCost;
            device.OperationCost = deviceEditRequest.OperationCost;
            device.PurchasingCost = deviceEditRequest.PurchasingCost;
            device.Detection = deviceEditRequest.Detection;
        }
    }
}
