using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CreateTransaction();
        Task Commit();
        Task Rollback();
        Task SaveChange();

    }
}
