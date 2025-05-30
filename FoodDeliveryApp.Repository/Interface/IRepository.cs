using FoodDeliveryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(Guid? Id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);

    }
}
