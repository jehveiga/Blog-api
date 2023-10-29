﻿using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string Password { get; set; }
    }
}
