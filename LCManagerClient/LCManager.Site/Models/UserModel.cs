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
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Required]
        public string Login { get; set; }
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
        [Required]
        public string Phone { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string FriendPhone { get; set; }

        public string Email { get; set; }

        public string Card { get; set; }

        public string BirthDate { get; set; }

        [Required]
        public string Code { get; set; }

        public string Gender { get; set; }

        public string Promocode { get; set; }
    }

    public class TerminalRedeemSumModel
    {
        public string Card { get; set; }

        public string Sum { get; set; }
    }

    public class TerminalChequeModel
    {
        public long Card { get; set; }

        public string Amount { get; set; }

        public string PaidByBonus { get; set; }

        public string MaxRedeem { get; set; }
    }

    public class TerminalRefundModel
    {
        public long Card { get; set; }

        public string ChequeDate { get; set; }

        public string ChequeNum { get; set; }

        public string ChequeSum { get; set; }
    }
}