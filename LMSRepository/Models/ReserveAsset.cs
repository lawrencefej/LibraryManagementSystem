using System;

namespace LMSRepository.Interfaces.Models
{
    public class ReserveAsset
    {
        public int Id { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public int LibraryCardId { get; set; }
        public int LibraryAssetId { get; set; }
        public DateTime Reserved { get; set; }
        public DateTime Until { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? DateCheckedOut { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
    }
}