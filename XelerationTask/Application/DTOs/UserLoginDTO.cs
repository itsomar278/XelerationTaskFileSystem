﻿using System.ComponentModel.DataAnnotations;

namespace XelerationTask.Application.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
