using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.Data
{
    public interface ILibraryCardRepository
    {
        //void Add(LibraryCard newCard);
        Task<List<LibraryCard>> GetAllLibraryCards();
        Task<LibraryCard> GetCard(int id);
        Task<LibraryCard> GetMemberCard(int id);
    }
}
