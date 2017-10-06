using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LC_Manager.Models
{
    public class GetConfirmCodeRequest
    {
        public long Phone { get; set; }
        public string Code { get; set; }
    }

    public class GetConfirmCodeResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class GetSendVerificationCodeRequest
    {
        public long Phone { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
    }

    public class GetSendVerificationCodeResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class GetRegistrationUserRequest
    {
        public long Phone { get; set; }
        public long Card { get; set; }
        public Int16 PartnerID { get; set; }
        public string PosCode { get; set; }
        public bool AgreePersonalData { get; set; }
        public Int16 Operator { get; set; }
        public Int64? FriendPhone { get; set; }
    }

    public class GetRegistrationUserResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Int64 Phone { get; set; }
        public Int64 Card { get; set; }
        public int Client { get; set; }
        public Int16 Pos { get; set; }
    }

    public class Client
    {
        public int id { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public int gender { get; set; }
        public DateTime birthdate { get; set; }
        public string address { get; set; }
        public bool haschildren { get; set; }
        public string description { get; set; }
        public Int64 phone { get; set; }
        public string email { get; set; }
        public bool allowsms { get; set; }
        public bool allowemail { get; set; }
        public decimal balance { get; set; }
        public bool? allowpush { get; set; }
        public decimal lasturchaseamount { get; set; }
        public DateTime lastpurchasedate { get; set; }
        public Client() { }
    }

    public class ChangeClientRequest
    {
        public Client ClientData { get; set; }
        public Int16 Operator { get; set; }
    }

    public class ChangeClientResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class GetClientInfoRequest
    {
        public Int16 Operator { get; set; }
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
    }

    public class GetClientInfoResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public decimal LastPurchaseAmount { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowEmail { get; set; }
        public decimal FullBalance { get; set; }
        public string Condition { get; set; }
        public Int64 Id { get; set; }
        public int Gender { get; set; }
        public bool PhoneValidated { get; set; }
        public bool EmailValidated { get; set; }
        public DateTime RegDate { get; set; }
    }
}