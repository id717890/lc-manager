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

    public class ChequeMaxSumRedeemRequest
    {
        public Int16 Operator { get; set; }

        public Int64 Phone { get; set; }

        public decimal ChequeSum { get; set; }
    }

    public class ChequeMaxSumRedeemResponse
    {
        public decimal MaxSum { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class Item
    {
        public int Position { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidByBonus { get; set; }
    }

    public class ChequeAddRequest
    {
        public long Card { get; set; }
        public DateTime ChequeTime { get; set; }
        public long Phone { get; set; }
        public int Partner { get; set; }
        public string POS { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidByBonus { get; set; }
        public string Number { get; set; }
        public List<Item> ItemData { get; set; }
    }

    public class ChequeAddResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
    }

    public class RedeemRequest
    {
        public long Card { get; set; }
        public long Phone { get; set; }
        public int Partner { get; set; }
        public decimal Bonus { get; set; }
    }

    public class RedeemResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
    }

    public class TerminalChequeAddResult
    {
        public string Amount { get; set; }
        public string BonusAdd { get; set; }
        public string BonusRedeem { get; set; }
        public string Cash { get; set; }
    }

    public class Cheque
    {
        public Int32 Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string OperationType { get; set; }
        public decimal Summ { get; set; }
        public decimal SummDiscount { get; set; }
        public decimal Bonus { get; set; }
        public decimal PaidByBonus { get; set; }
        public decimal Discount { get; set; }
        public string Partner { get; set; }
        public string Shop { get; set; }
        public Int64 CardNumber { get; set; }
        public string PosName { get; set; }
        public Cheque() { }
        public Cheque(Int32 id, string number, DateTime date, string operationtype, decimal summ, decimal summdiscount, decimal bonus, decimal paidbybonus, decimal discount, string partner, string shop, Int64 cardnumber)
        {
            Id = id;
            Number = number;
            OperationType = operationtype;
            Summ = summ;
            SummDiscount = summdiscount;
            Bonus = bonus;
            PaidByBonus = paidbybonus;
            Discount = discount;
            Partner = partner;
            Shop = shop;
            CardNumber = cardnumber;
        }
    }

    public class GetChequesByCardRequest
    {
        public Int64 CardNumber { get; set; }
    }

    public class GetChequesByCardResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Cheque> ChequeData { get; set; }
        public GetChequesByCardResponse()
        {
            ChequeData = new List<Cheque>();
        }
    }

    public class RefundRequest
    {
        public Int64 Card { get; set; }
        public DateTime ChequeTime { get; set; }
        public Int64 Phone { get; set; }
        public Int16 Partner { get; set; }
        public string Pos { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidByBonus { get; set; }
        public string Number { get; set; }
        public int PurchaseId { get; set; }
        public string PurchaseNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PurchasePos { get; set; }
        public string PurchaseTerminal { get; set; }
    }

    public class RefundResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Added { get; set; }
        public decimal Balance { get; set; }
        public decimal Redeemed { get; set; }
        public decimal Amount { get; set; }
    }
}