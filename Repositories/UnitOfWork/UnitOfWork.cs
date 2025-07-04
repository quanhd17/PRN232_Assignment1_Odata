﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BusinessLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FunewsManagementContext _context;
        private IDbContextTransaction? _transaction;
        public UnitOfWork(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task CreateTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            if (_transaction == null)
            {
                throw new Exception("");
            }
            await _transaction.CommitAsync();
        }

        public async Task Rollback()
        {
            if (_transaction == null)
            {
                throw new Exception("");
            }
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

    }
}
