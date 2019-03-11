using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int categoryId);

        Task<IEnumerable<Category>> GetAll();
    }
}