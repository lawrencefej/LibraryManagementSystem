using LMSRepository.Models;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ILibraryRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<Status> GetStatus(string status);
    }
}