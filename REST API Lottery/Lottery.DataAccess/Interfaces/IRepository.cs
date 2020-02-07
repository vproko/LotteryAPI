using System;
using System.Collections.Generic;
using System.Text;

namespace Lottery.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(Guid id);
        int Add(TEntity entity);
        int Update(TEntity entity);
        int Delete(Guid id);
    }
}
