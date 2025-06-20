﻿using BusinessLogic.UnitOfWork;
using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<SystemAccount, short> _systemAccountRepository;
        private readonly IGenericRepository<NewsArticle, int> _newsArticleRepository;

        public SystemAccountService(IUnitOfWork unitOfWork, IGenericRepository<SystemAccount, short> systemAccountRepository, IGenericRepository<NewsArticle, int> newsArticleRepository)
        {
            _unitOfWork = unitOfWork;
            _systemAccountRepository = systemAccountRepository;
            _newsArticleRepository = newsArticleRepository;
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

        // Check Login
        public async Task<SystemAccount> Login(string email, string password)
        {
            return await _systemAccountRepository.FindAll().Where(x => x.AccountEmail.Equals(email) && x.AccountPassword.Equals(password)).FirstOrDefaultAsync();
        }
    }
}
