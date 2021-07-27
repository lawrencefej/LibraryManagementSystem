﻿using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class AuthorDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Description { get; set; }
    }
}
