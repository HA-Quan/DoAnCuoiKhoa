using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Models.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        //ApiReponse GetAllAccount();
        //Account GetAccountById(Guid accountID);
        //void CreateAccount(Account account);
        //ApiReponse GetAccountLogin(LoginModel loginModel);
        PagingData<Account> GetByFilter(byte role, bool? status, string? keyword, byte? sort, int pageSize, int pageNumber);
        List<string> CheckUsernameAndEmail(string username, string email);
        bool CheckExist(Account account);
        //void UpdateAccount(Account account);
        int UpdateMultiple(List<Guid> listID, bool status);

    }
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        //public ApiReponse GetAllAccount()
        //{
        //    try
        //    {
        //        List<Account> accountList =FindAll().ToList();
        //        return new ApiReponse()
        //        {
        //            Success = true,
        //            Data = accountList
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new ApiReponse()
        //        {
        //            Success = false,
        //            Data = HandleError.GenerateErrorResultDatabase()
        //        };

        //    }
        //}
        //public Account GetAccountById(Guid accountID)
        //{
        //    Account? result = (Account) FindByCondition(a => a.AccountID == accountID);
        //    return result;
        //}
        //public void CreateAccount(Account account)
        //{
        //    Create(account);
        //    _repositoryContext.SaveChanges();
        //}

        //public ApiReponse GetAccountLogin(LoginModel loginModel)
        //{
        //    Account? result = (Account) FindByCondition(a => a.Username == loginModel.Username);
        //    if (result != null && BCrypt.Net.BCrypt.Verify(result.Password, loginModel.Password))
        //        return new ApiReponse()
        //        {
        //            Success = true,
        //            Data = result
        //        };
        //    else
        //        return new ApiReponse()
        //        {
        //            Success = false,
        //            Data = new ErrorResult()
        //            {
        //                ErrorCode = EnumType.ErrorCode.LoginFail,
        //                UserMsg = new List<string>() { Resource.UserMsgLoginFail },
        //                DevMsg = new List<string>() { Resource.DevMsgLoginFail },
        //                MoreInfo = Resource.MoreInfo
        //            }
        //        };
        //}

        public PagingData<Account> GetByFilter(byte role, bool? status, string? keyword, byte? sort, int pageSize, int pageNumber)
        {
            var respone = new PagingData<Account>();
            var result = new List<Account>();
            var listAccount = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && 
            (p.FullName.Contains(keyword) || p.Username.Contains(keyword))).ToList();
            if(status != null)
            {
                listAccount = listAccount.Where(a=> a.Status == status).ToList();
            }
            if(role != (byte)EnumType.Role.All)
            {
                listAccount = listAccount.Where(a => a.Role == role).ToList();
            }

            listAccount = FilterSort(listAccount, sort);
            var maxResult = listAccount.Count();

            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            respone.Data = listAccount.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return respone;
        }

        public List<Account> FilterSort(List<Account> accounts, byte? sort)
        {
            switch (sort)
            {
                case (byte)EnumType.SortAcount.UsernameAsc:
                    return accounts.OrderBy(s => s.Username).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.UsernameDesc:
                    return accounts.OrderByDescending(s => s.Username).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.FullNameAsc:
                    return accounts.OrderBy(s => s.FullName).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.FullNameDesc:
                    return accounts.OrderByDescending(s => s.FullName).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.RoleAsc:
                    return accounts.OrderBy(s => s.Role).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.RoleDesc:
                    return accounts.OrderByDescending(s => s.Role).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.StatusAsc:
                    return accounts.OrderBy(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();

                case (byte)EnumType.SortAcount.StatusDesc:
                    return accounts.OrderByDescending(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();

            }
            return accounts.OrderByDescending(s => s.ModifiedDate).ToList();
        }
    

        public List<string> CheckUsernameAndEmail(string username, string email)
        {
            var result = new List<string>();
            if (FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.Username == username).FirstOrDefault() != null)
                result.Add("Username đã tồn tại!");
            if (FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.Email == email).FirstOrDefault() != null)
                result.Add("Email đã tồn tại!");
            return result;
        }

        public bool CheckExist(Account account)
        {
            return _repositoryContext.Accounts.Any(p => p.DelFalg == EnumType.DeleteFlag.Using &&
            p.AccountID == account.AccountID && p.Username == account.Username);
        }
        
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var listAccount = new List<Account>();
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
                    account.Status = status;
                    listAccount.Add(account);
                }
            }
            if (check)
            {
                UpdateMultiple(listAccount);
                Save();
                return listAccount.Count;
            }
            else
            {
                return 0;
            }
        }

    }
}
