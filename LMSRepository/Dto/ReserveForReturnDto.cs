using System;

namespace LMSLibrary.Dto
{
    public class ReserveForReturnDto
    {
        public int Id { get; set; }
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public string Title { get; set; }
        public DateTime Reserved { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime? DateCheckedOut { get; set; }
        public string Status { get; set; }
    }
}
