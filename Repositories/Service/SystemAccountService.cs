using BusinessLogic.UnitOfWork;
using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<SystemAccount, short> _systemAccountRepository;
        private readonly IGenericRepository<NewsArticle, int> _newsArticleRepository;
        private readonly IConfiguration _config;

        public SystemAccountService(IUnitOfWork unitOfWork, IGenericRepository<SystemAccount, short> systemAccountRepository, IGenericRepository<NewsArticle, int> newsArticleRepository, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _systemAccountRepository = systemAccountRepository;
            _newsArticleRepository = newsArticleRepository;
            _config = config;
        }

        // Get all system accounts
        public async Task<IEnumerable<SystemAccount>> GetAllSystemAccountsAsync()
        {
            return await _systemAccountRepository.FindAll().ToListAsync();
        }

        // Get system account by ID
        public async Task<SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _systemAccountRepository.FindById(id, "AccountId");
        }

        // Add a new system account
        public async Task AddSystemAccountAsync(SystemAccount systemAccount)
        {
            if (systemAccount == null)
                throw new ArgumentNullException(nameof(systemAccount));

            _systemAccountRepository.Create(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Update an existing system account
        public async Task UpdateSystemAccountAsync(SystemAccount systemAccount)
        {
            if (systemAccount == null)
                throw new ArgumentNullException(nameof(systemAccount));

            _systemAccountRepository.Update(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Delete a system account by ID
        public async Task DeleteSystemAccountAsync(short id)
        {
            var systemAccount = await _systemAccountRepository.FindById(id, "AccountId");
            if (systemAccount == null)
                throw new ArgumentException("System account not found");
            var newsArticles = await _newsArticleRepository.FindAll(x => x.CreatedById == id).ToListAsync();
            if (newsArticles.Any())
                throw new InvalidOperationException("Cannot delete system account with existing news articles.");
            _systemAccountRepository.Delete(systemAccount);
            await _unitOfWork.SaveChange(); // Save changes using UnitOfWork
        }

        // Login
        public async Task<(SystemAccount? user, string? token)> Login(string email, string password)
        {
            var user = await _systemAccountRepository
                .FindAll()
                .Where(x => x.AccountEmail.Equals(email) && x.AccountPassword.Equals(password)).FirstOrDefaultAsync();

            if (user == null) return (null, null);

            string token = GenerateToken(user);
            return (user, token);
        }

        private string GenerateToken(SystemAccount user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
        new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
        new Claim(ClaimTypes.Name, user.AccountName ?? ""),
        new Claim(ClaimTypes.Role, user.AccountRole == 1 ? "Staff" :
                                   user.AccountRole == 2 ? "Lecturer" : "Admin")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
