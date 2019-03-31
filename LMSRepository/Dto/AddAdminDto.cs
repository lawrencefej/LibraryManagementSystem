﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LMSRepository.Dto
{
    public class AddAdminDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime Created { get; set; }

        public AddAdminDto()
        {
            UserName = Email;
            Created = DateTime.Now;
        }
    }
}