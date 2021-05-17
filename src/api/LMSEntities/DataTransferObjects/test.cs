using System;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class test
    {
        public int UserId { get; set; }
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public DateTime Reserved { get; set; }
        public DateTime Until { get; set; }
        // public Status Status { get; set; }
        public string AssetStatus { get; set; }
        public int CurrentReserveCount { get; set; }
        public decimal Fees { get; set; }
    }
}
