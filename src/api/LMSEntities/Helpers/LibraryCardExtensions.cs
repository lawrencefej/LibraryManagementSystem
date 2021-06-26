using System;
using LMSEntities.Models;

namespace LMSEntities.Helpers
{
    public static class LibraryCardExtensions
    {
        public static string GenerateCardNumber(this LibraryCard card)
        {
            string date = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            return $"{card.LastName.Substring(0, 1).ToUpper()}-{date.Substring(0, 4)}-{date.Substring(4, 4)}-{date.Substring(8, 6)}";

        }
    }
}
