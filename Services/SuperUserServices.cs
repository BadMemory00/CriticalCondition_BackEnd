using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CriticalConditionBackend.CriticalConditionExceptions;
using CriticalConditionBackend.Data;
using CriticalConditionBackend.Models;
using CriticalConditionBackend.Models.DTOs;
using CriticalConditionBackend.Utillities;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace CriticalConditionBackend.Services
{
    public class SuperUserServices : ISuperUserServices
    {
        private readonly IConfiguration _configuration;
        private readonly CriticalConditionDbContext _dbContext;

        public SuperUserServices(IConfiguration configuration, CriticalConditionDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<bool> RegisterAsync(SuperUserRegister model)
        {
            var existedSuperUser = await _dbContext.SuperUsers.FindAsync(model.Email);
            if (existedSuperUser is not null)
                throw new LogicalException(CriticalConditionExceptionsEnum.SUPER_USER_ALREADY_EXISTS, 409);

            SuperUser superUser = FillSuperUserData(model);

            await _dbContext.SuperUsers.AddAsync(superUser);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<string> LoginAsync(SuperUserLogin model)
        {
            var user = await _dbContext.SuperUsers.FindAsync(model.Email);

            if (user == null || BC.Verify(model.Password, user.PassWord) is false)
                throw new LogicalException(CriticalConditionExceptionsEnum.SUPER_USER_EMAIL_OR_PASSWORD_ARE_WRONG, StatusCodes.Status401Unauthorized);

            List<Claim> authClaims = AddSuperUserClaims(user);

            return TokenUtilities.CreateToken(_configuration, authClaims);
        }

        public async Task<SuperUserResponse> GetSuperUserDataAsync(string token)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            var superUser = await _dbContext.SuperUsers.FindAsync(superUserEmail);

            if (superUser is null)
                throw new LogicalException(CriticalConditionExceptionsEnum.SUPER_USER_DOES_NOT_EXIST, StatusCodes.Status404NotFound);

            SuperUserResponse superUserResponse = FillSuperUserResponseData(superUser);

            return superUserResponse;
        }


        public async Task<SubUser> GenerateSubUserAsync(string token, SubUserGeneration model)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            SubUser subUser = new()
            {
                Code = GenerateAndEncryptSubUserCode(),
                UserName = model.UserName,
                Role = model.Role,
                SuperUserEmail = superUserEmail
            };

            await _dbContext.SubUsers.AddAsync(subUser);
            await _dbContext.SaveChangesAsync();

            return subUser;
        } 

        public async Task<List<SubUser>> GetAllSubUsersAsync(string token)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            var subUsers = await _dbContext.SubUsers.Where(x => x.SuperUserEmail.Equals(superUserEmail)).ToListAsync();

            subUsers.ForEach(x => x.Code = EncryptionUtilities.DecryptSubUserCode(x.Code, _configuration));

            return subUsers;
        }

        public async Task<List<EditsLogResponse>> GetEditsLogAsync(string token)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            var editsLog = await _dbContext.EditsLogs
                .Where(x => x.SuperUserEmail.Equals(superUserEmail))
                .Select(x => new EditsLogResponse
                {
                    SubUserUserName = x.SubUserUserName,
                    Action = x.Action,
                    DeviceName = x.DeviceName,
                }).ToListAsync();

            return editsLog;
        }

        public async Task<List<DeviceSuperUserSmallResponse>> GetAllDevicesSmallCardAsync(string token)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            var devices = await _dbContext.Devices
                .Where(x => x.SuperUserEmail == superUserEmail && x.IsArchived == false)
                .OrderByDescending(x => x.FMEARiskScore)
                .Select(x => new DeviceSuperUserSmallResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Model = x.Model,
                    NumberOfFailures = x.NumberOfFailures,
                    IsIoT = x.IsIoT,
                    FMEARiskScore = x.FMEARiskScore,
                    SecurityRiskScore = x.SecurityRiskScore
                }).ToListAsync();

            return devices;
        }

        public async Task<List<DeviceSuperUserFullResponse>> GetAllDevicesFullCardAsync(string token, bool IsArchived)
        {
            string superUserEmail = ValidateTokenAndReturnSuperUserEmail(token);

            var devices = await _dbContext.Devices
                .Where(x => x.SuperUserEmail == superUserEmail && x.IsArchived == IsArchived)
                .Select(x => new DeviceSuperUserFullResponse
                {
                    Id = x.Id,
                    Name= x.Name,
                    Model= x.Model,
                    TypeOfService = x.TypeOfService,
                    UtilizationRate = DeviceUtilities.UtilizationRateString(x),
                    Unavailability = DeviceUtilities.UnavailabilityString(x),
                    Safety = x.Safety,
                    Importance = DeviceUtilities.ImportanceString(x),
                    FinancialScore = DeviceUtilities.FinancialScoreString(x),
                    Detection = x.Detection,
                    AgeFactor = DeviceUtilities.AgeFactorString(x),
                    FMEARiskScore = x.FMEARiskScore,
                    IsIoT = x.IsIoT,
                    SecurityRiskScore = x.SecurityRiskScore
                }).ToListAsync();

            return devices;
        }
        private static SuperUserResponse FillSuperUserResponseData(SuperUser model) => new()
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            HospitalName = model.HospitalName,
            HospitalSpeciality = model.HospitalSpeciality,
            HospitalCountry = model.HospitalCountry,
            HospitalState = model.HospitalState,
            HospitalCity = model.HospitalCity,
            HospitalStreet = model.HospitalStreet,
            IsSubscriber = model.IsSubscriber,
            IsDataShareProgramMember = model.IsDataShareProgramMember,
            ExcludedDevicesNumber = model.ExcludedDevicesNumber
        };
        private static SuperUser FillSuperUserData(SuperUserRegister model) => new()
        {
            Email = model.Email,
            PassWord = BC.HashPassword(model.Password),
            PhoneNumber = model.PhoneNumber,
            HospitalName = model.HospitalName,
            HospitalSpeciality = model.HospitalSpeciality,
            HospitalCountry = model.HospitalCountry,
            HospitalState = model.HospitalState,
            HospitalCity = model.HospitalCity,
            HospitalStreet = model.HospitalStreet,
            IsSubscriber = false,
            IsDataShareProgramMember = false,
            ExcludedDevicesNumber = 0
        };
        private static List<Claim> AddSuperUserClaims(SuperUser user) => new()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, CriticalConditionUserRoles.SuperUser),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        private string ValidateTokenAndReturnSuperUserEmail(string token)
        {
            var tokenDetails = TokenUtilities.ValidateAndReturnToken(token, _configuration);

            var superUserEmail = tokenDetails.Payload.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").ToString();

            return superUserEmail;
        }

        private string GenerateAndEncryptSubUserCode()
        {
            string code = GenerateSubUserCode(8);

            return EncryptionUtilities.EncryptSubUserCode(code, _configuration);
        }

        private readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private string GenerateSubUserCode(int size)
        {
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
