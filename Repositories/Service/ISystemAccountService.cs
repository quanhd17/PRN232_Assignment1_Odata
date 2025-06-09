using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllSystemAccountsAsync();
        Task<SystemAccount?> GetSystemAccountByIdAsync(short id);
        Task AddSystemAccountAsync(SystemAccount systemAccount);
        Task UpdateSystemAccountAsync(SystemAccount systemAccount);
        Task DeleteSystemAccountAsync(short id);
        Task<SystemAccount?> Login(string email, string password);
    }
}
