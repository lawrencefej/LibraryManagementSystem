using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetAuthorDto
    {
        public int Id { get; set; }

        [Required]
        public int LibrayAssetId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public byte Order { get; set; }
    }
}
