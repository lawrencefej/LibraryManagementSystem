using System;

namespace LMSRepository.Interfaces.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public bool IsReturned { get; set; }
        public DateTime? DateReturned { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
    }
}