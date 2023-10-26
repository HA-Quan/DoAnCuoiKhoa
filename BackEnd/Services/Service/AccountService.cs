using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Models.Resource;
using Services.Repository;
using Services.Service.Base;
using Services.Service.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface IAccountService : IServiceBase<Account>
    {
        ApiReponse GetAllAccount();
        ApiReponse GetByFilter(byte role, bool? status, string? keyword, byte? sort, int pageSize, int pageNumber);
        ApiReponse GetAccountLogin(LoginModel loginModel);
        ApiReponse GetAccountById(Guid accountID);
        ApiReponse GetInfoByToken(string token);
        ApiReponse AddNewMember(SingUpModel singUpModel);
        ApiReponse SaveAccount(Account account, Guid accountID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse DeleteAccount(List<Guid> listID);
        Task<ApiReponse> UpdateAvatar(Guid accountID, IFormFile img);
        TokenModel GenerateToken(Account account, DateTime time);
        ApiReponse RenewToken(TokenModel tokenModel);
        List<string> ValidateInput(SingUpModel singUpModel);
    }

    public class AccountService : BaseService<Account>, IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly RepositoryContext _repositoryContext;

        public AccountService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, 
            IOrderProductRepository orderProductRepository ,IAccountRepository accountRepository, 
            RepositoryContext repositoryContext, IProductRepository productRepository) : base(accountRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _repositoryContext = repositoryContext;
            _refreshTokenRepository = refreshTokenRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
        }
        public ApiReponse GetAllAccount()
        {
            try
            {
                List<Account> accountList = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using 
                && a.Role != (byte) EnumType.Role.Management).ToList();
                return new ApiReponse()
                {
                    Success = true,
                    Data = accountList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }
        public ApiReponse GetByFilter(byte role, bool? status, string? keyword, byte? sort, int pageSize, int pageNumber)
        {
            try
            {
                if (keyword == null)
                {
                    keyword = string.Empty;
                }
                else
                {
                    keyword = keyword.Trim().ToLower();
                }
                return new ApiReponse()
                {
                    Success = true,
                    Data = _accountRepository.GetByFilter(role, status, keyword, sort, pageSize, pageNumber)
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }
        public ApiReponse GetAccountById(Guid accountID)
        {
            try
            {
                var result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.AccountID == accountID).FirstOrDefault();
                if (result != null)
                {
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                else
                {
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultNotFoundByID()
                    };
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }
        public ApiReponse GetInfoByToken(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyByte = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
                var validateTokenParam = new TokenValidationParameters
                {
                    // Tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),

                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };

                var tokenInVerification = jwtTokenHandler.ValidateToken(token, validateTokenParam,
                                                                      out var validateToken);
                var accountID = new Guid(tokenInVerification.Claims.FirstOrDefault(x => x.Type == "Id").Value);
                var result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.AccountID == accountID).FirstOrDefault();
                if (result != null)
                {
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                else
                {
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultNotFoundByID()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }
        public async Task<ApiReponse> UpdateAvatar(Guid accountID, IFormFile img)
        {
            ApiReponse result = new ApiReponse();
                try
                {
                    var account = FindByCondition(a => a.AccountID == accountID).FirstOrDefault();
                    if (account.Avatar != Resource.AvatarDefault)
                    {
                        await ImageService.DeleteImage(account.Avatar, _configuration);
                    }
                    if (await ImageService.AddImage(img, _configuration) != null)
                    {
                        account.Avatar = await ImageService.AddImage(img, _configuration);
                        Update(account);
                        _accountRepository.Save();
                        result.Success = true;
                        result.Data = account;
                    }
                    else
                    {
                        result.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result.Success = false;
                    result.Data = HandleError.GenerateErrorResultException();
                }
            
            return result;
        }
    
        public ApiReponse SaveAccount(Account account, Guid accountID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (accountID != Guid.Empty)
                {
                    if (_accountRepository.CheckExist(account) && account.AccountID == accountID)
                    {
                        if(!FindByCondition(a => a.AccountID  == accountID && a.Password == account.Password).Any())
                        {
                            account.Password = HashPasswordService.HashPassword(account.Password);
                        }
                        Update(account);
                        result.Success = true;
                        result.Data = account;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    account.AccountID = Guid.NewGuid();
                    account.Password = HashPasswordService.HashPassword(account.Password);
                    account.Username = account.Username.ToLower();
                    Create(account);
                    result.Success = true;
                    result.Data = account;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Success = false;
                result.Data = HandleError.GenerateErrorResultException();
            }
            return result;
        }
        public ApiReponse UpdateMultiple(List<Guid> listID, bool status)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _accountRepository.UpdateMultiple(listID, status);
                    if (result == listID.Count)
                    {
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = listID
                        };
                    }
                    else 
                    {
                        transaction.Rollback();
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = HandleError.GenerateErrorUpdateFail()
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultException()
                    };
                }

            }
        }
        public ApiReponse DeleteAccount(List<Guid> listID)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var listAccount = new List<Account>();  
                    bool check = true;
                    foreach (var accountID in listID)
                    {
                        Account? account = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.AccountID == accountID).FirstOrDefault();
                        if (account == null)
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            _refreshTokenRepository.DeleteRefreshTokenByAccountID(accountID);
                            //_productRepository.DeleteByAccountID(accountID);
                            //_importProductRepository.DeleteByAccountID(accountID);
                            if (_orderProductRepository.DeleteByAccountID(accountID) != 0)
                            {
                                account.DelFalg = EnumType.DeleteFlag.Deleted;
                                listAccount.Add(account);
                            }
                            else
                            {
                                check = false;
                            }
                        }
                    }
                    if (check)
                    {
                        _accountRepository.UpdateMultiple(listAccount);
                        _accountRepository.Save();
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = listID.Count()
                        };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = HandleError.GenerateErrorDeleteFail()
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultException()
                    };
                }
            }
        }
        public ApiReponse GetAccountLogin(LoginModel loginModel)
        {
            try
            {
                Account? result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.Username == loginModel.Username).FirstOrDefault();
                if (result != null && HashPasswordService.Verify(result.Password, loginModel.Password))
                {
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = GenerateToken(result, DateTime.MinValue)
                    };
                }

                else
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = new ErrorResult()
                        {
                            ErrorCode = EnumType.ErrorCode.LoginFail,
                            UserMsg = new List<string>() { Resource.UserMsgLoginFail },
                            DevMsg = new List<string>() { Resource.DevMsgLoginFail },
                            MoreInfo = Resource.MoreInfo
                        }
                    };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }
        public ApiReponse AddNewMember(SingUpModel singUpModel)
        {
            try
            {
                var liseErrorValidate = ValidateInput(singUpModel);
                if (liseErrorValidate.Count() == 0)
                {
                    Account acc = new Account()
                    {
                        AccountID = Guid.NewGuid(),
                        Username = singUpModel.UserName.ToLower(),
                        Password = HashPasswordService.HashPassword(singUpModel.Password),
                        Avatar = _configuration["MinIO:AvatarDefault"],
                        Email = singUpModel.Email,
                        FullName = singUpModel.FullName,
                    };
                    acc.CreatedBy = acc.AccountID;
                    Create(acc);
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = GenerateToken(acc, DateTime.MinValue)
                    };


                }
                else
                {
                    var errors = HandleError.GenerateErrorResultValidate();
                    errors.UserMsg.AddRange(liseErrorValidate);
                    errors.DevMsg.AddRange(liseErrorValidate);
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = errors
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }

        }

        public TokenModel GenerateToken(Account account, DateTime time)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyByte = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Id", account.AccountID.ToString()),
                    new Claim("User", account.Username),
                    new Claim(ClaimTypes.Name, account.FullName),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("Roles", ConvertRole(account.Role)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),
                                                                 SecurityAlgorithms.HmacSha512Signature)
                };
                var token = jwtTokenHandler.CreateToken(tokenDescription);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenModel = new RefreshToken()
                {
                    RefreshTokenID = Guid.NewGuid(),
                    CreatedBy = account.AccountID,
                    JwtID = new Guid(token.Id),
                    Token = refreshToken,
                    IsUsed = false,
                    IsRevoked = false,
                    IssuedAt = DateTime.UtcNow,
                    ExpiredAt = time == DateTime.MinValue ? DateTime.UtcNow.AddMinutes(60) : time,
                };
                _refreshTokenRepository.Create(refreshTokenModel);
                _refreshTokenRepository.Save();
                return new TokenModel()
                {
                    AccessToken = jwtTokenHandler.WriteToken(token),
                    RefreshToken = refreshToken,
                    InfoUser = account,
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new TokenModel();
            }
        }

        public ApiReponse RenewToken(TokenModel tokenModel)
        {
            try
            {
                var validateMessage = ValidateRefreshToken(tokenModel);
                if (validateMessage.Count() == 0)
                {
                    var refreshTokenCurrent = _refreshTokenRepository.GetRefreshToken(tokenModel.RefreshToken);
                    refreshTokenCurrent.IsUsed = true;
                    refreshTokenCurrent.IsRevoked = true;
                    _refreshTokenRepository.Update(refreshTokenCurrent);

                    var acc = GetAccountById(refreshTokenCurrent.CreatedBy);
                    if (acc.Success)
                    {
                        var newToken = GenerateToken((Account)acc.Data, refreshTokenCurrent.ExpiredAt);
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = newToken,
                        };
                    }
                    else
                    {
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = new ErrorResult()
                            {
                                ErrorCode = EnumType.ErrorCode.NotFoundByID,
                                UserMsg = new List<string>() { Resource.UserMsgNotFoundRecordByID },
                                DevMsg = new List<string>() { Resource.DevMsgNotFoundRecordByID },
                                MoreInfo = Resource.MoreInfo
                            }
                        };
                    }

                }
                else
                {
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = new ErrorResult()
                        {
                            ErrorCode = EnumType.ErrorCode.RenewTokenFail,
                            UserMsg = new List<string>(validateMessage),
                            DevMsg = new List<string>() { Resource.DevMsgRenewTokenFail },
                            MoreInfo = Resource.MoreInfo
                        }
                    };
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }

        }

        public List<string> ValidateRefreshToken(TokenModel tokenModel)
        {
            var listErrors = new List<string>();
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
            var validateTokenParam = new TokenValidationParameters
            {
                // Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, validateTokenParam,
                                                                      out var validateToken);

                //check 2: Check Alg AccessToken 
                if (validateToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                                                                     StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                        listErrors.Add("Invalid Alg Token");
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x =>
                                                                                x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                    listErrors.Add("Access token not expired!");


                //check 4: Check refreshtoken exist in DB
                var checkExist = _refreshTokenRepository.GetRefreshToken(tokenModel.RefreshToken);
                if (checkExist == null)
                    listErrors.Add("Refresh token does not exist!");

                //check 5: check refreshToken is used/revoked?
                if (checkExist.IsUsed)
                    listErrors.Add("Refresh token has been used!");
                if (checkExist.IsRevoked)
                    listErrors.Add("Refresh token has been revoked!");

                if(checkExist.ExpiredAt < DateTime.UtcNow)
                {
                    listErrors.Add("Refresh token expired!");
                }

                //check 6: AccessToken id == JwtId in RefreshToken

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (checkExist.JwtID.ToString() != jti)
                    listErrors.Add("Token doesn't match!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                listErrors.Add("Something went wrong");
            }
            return listErrors;

        }

        public DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

        }

        public string GenerateRefreshToken()
        {
            var random = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public List<string> ValidateInput(SingUpModel singUpModel)
        {
            List<string> result = new List<string>();
            //if (singUpModel.UserName.IsNullOrEmpty())
            //    result.Add("Username not empty!");
            //if (singUpModel.Password.IsNullOrEmpty())
            //    result.Add("Password not empty!");
            //if (singUpModel.FullName.IsNullOrEmpty())
            //    result.Add("FullName not empty!");
            //if (singUpModel.Email.IsNullOrEmpty())
            //    result.Add("Email not empty!");
            if (singUpModel.UserName.Length == 0)
                result.Add("Username not empty!");
            if (singUpModel.Password.Length == 0)
                result.Add("Password not empty!");
            if (singUpModel.FullName.Length == 0)
                result.Add("FullName not empty!");
            if (singUpModel.Email.Length == 0)
                result.Add("Email not empty!");
            string strRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(strRegex);
            if (!regex.IsMatch(singUpModel.Email))
                result.Add("Email is not in correct format");

            result.AddRange(_accountRepository.CheckUsernameAndEmail(singUpModel.UserName, singUpModel.Email));
            return result;
        }
        public string ConvertRole(byte role)
        {
            if (role == (byte) EnumType.Role.Staff) return "Staff";
            if (role == (byte) EnumType.Role.Management) return "Management";
            return "Member";
        }

    }
}
