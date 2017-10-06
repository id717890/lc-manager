using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LC_Manager.Models
{
    public class UserModel
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Required]
        public string Phone { get; set; }
    }

    public class VerificationCodeModel
    {
        [Required]
        public string Code { get; set; }
        public long Phone { get; set; }
    }

    public class NewPasswordModel
    {
        [Required(ErrorMessage = "Требуется пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Требуется подтвердить пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение должны совпадать")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterModel
    {
        public string Phone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string FriendPhone { get; set; }

        public string Email { get; set; }

        public string Card { get; set; }

        public string BirthDate { get; set; }

        public string Code { get; set; }

        public string Gender { get; set; }
    }
}