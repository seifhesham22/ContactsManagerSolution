﻿using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage ="Email should be in a proper email format")]
        [Remote(action: "IsEmailAlreadyRegistered" , controller: "Account" , ErrorMessage ="Email is already in use")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone can't be blank")]
        [Phone(ErrorMessage ="Phone number should be in a proper format")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Confirm password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage = "Password and Confirm password don't match")]
        public string? ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;

   }
}
