using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMSRepository.Dto
{
    public class AssetPhotoDto
    {
        public string Url { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public int LibraryAssetId { get; set; }

        public AssetPhotoDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}