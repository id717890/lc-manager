using Newtonsoft.Json;
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
        public bool ClientSetPassword { get; set; }
        public string Email { get; set; }
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
        public DateTime? birthdate { get; set; }
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
        public string Promocode { get; set; }
    }

    public class ChequeMaxSumRedeemRequest
    {
        public Int64 Card { get; set; }

        public Int16 Partner { get; set; }

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
        public int Id { get; set; }
        public byte Position { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidByBonus { get; set; }
        public decimal MinPrice { get; set; }
        public bool NoAdd { get; set; }
        public bool NoRedeem { get; set; }
        public decimal MaxRedeem { get; set; }
        public decimal Redeemed { get; set; }
        public decimal Added { get; set; }
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
        public decimal Redeemed { get; set; }
        public string Number { get; set; }
        public List<Item> ItemData { get; set; }
        public Int16 Operator { get; set; }
        public bool NoWrite { get; set; }
        public int BonusId { get; set; }
        public bool NoAdd { get; set; }
        public bool NoRedeem { get; set; }
    }

    public class ChequeAddResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Added { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
        public decimal Redeemed { get; set; }
        public decimal MaxRedeem { get; set; }
        public decimal FullBalance { get; set; }
        public decimal PurchaseSum { get; set; }
        public List<Item> ItemData { get; set; }
        public ChequeAddResponse()
        {
            ItemData = new List<Item>();
        }
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
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ChequeItem
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal RedeemedBonus { get; set; }
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
        public long Phone { get; set; }
        public List<ChequeItem> Items { get; set; }
        public Cheque()
        {
            Items = new List<ChequeItem>();
        }
        public Cheque(Int32 id, string number, DateTime date, string operationtype, decimal summ, decimal summdiscount, decimal bonus, decimal paidbybonus, decimal discount, string partner, string shop, Int64 cardnumber, long phone)
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
            Phone = phone;
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
        public DateTime PurchaseDate { get; set; }
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

    public class ManagerLoginRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }

    public class ManagerLoginResponse
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        public string PosCode { get; set; }

        public string RoleName { get; set; }

        public string PermissionCode { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }
    }

    public class BonusAddRequest
    {
        public Int16 Operator { get; set; }
        public Int64? Card { get; set; }
        public Int64? Phone { get; set; }
        public decimal Bonus { get; set; }
    }

    public class BonusAddResponse
    {
        public decimal Bonus { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }

    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        [JsonProperty(".issued")]
        public string issued { get; set; }
        [JsonProperty(".expires")]
        public string expires { get; set; }
        public string refresh_token { get; set; }
    }

    public class SetClientPasswordRequest
    {
        public long Phone { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public int ClientID { get; set; }
    }

    public class SetClientPasswordResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
    
    public class GetChequesRequest
    {
        public Int16 Operator { get; set; }
        public int PartnerId { get; set; }
        public Int16 Pos { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }

        public string DateBuy { get; set; }
        public string PosStr { get; set; }
        public string Phone { get; set; }
        public string Operation { get; set; }
        public string Number { get; set; }
        public string Sum { get; set; }
        public string Added { get; set; }
        public string Redeemed { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
    }

    public class GetChequesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int PageCount { get; set; }
        public List<Cheque> ChequeData { get; set; }
        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }
        public GetChequesResponse()
        {
            ChequeData = new List<Cheque>();
        }
    }

    public class ChequeForSalePage
    {
        public int id { get; set; }

        public string date { get; set; }

        public string pos { get; set; }

        public long phone { get; set; }

        public string operation { get; set; }

        public string number { get; set; }

        public string summ { get; set; }

        public string added { get; set; }

        public string redeemed { get; set; }

        public string lorem { get; set; }
    }

    public class Cheques
    {
        public List<ChequeForSalePage> data { get; set; }
        public int recordsTotal { get; set; }
        public int draw { get; set; }
        public int recordsFiltered { get; set; }

        public Cheques()
        {
            data = new List<ChequeForSalePage>();
        }
    }

    public class CardBuysByMonth
    {
        public decimal BonusAdded { get; set; }

        public decimal BonusRedeemed { get; set; }

        public decimal AvgCheque { get; set; }

        public decimal ChequeSum { get; set; }

        public int MonthNum { get; set; }
    }

    public class Bonus
    {
        public string BonusSource { get; set; }

        public DateTime? BonusDate { get; set; }

        public decimal BonusAdded { get; set; }

        public decimal BonusRedeemed { get; set; }

        public decimal BonusBurn { get; set; }
    }

    public class OperatorClientsManagerBuys
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Phone { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string ClientType { get; set; }

        public long Card { get; set; }

        public string Level { get; set; }

        public decimal Balance { get; set; }

        public int BuyQty { get; set; }

        public decimal BuySum { get; set; }

        public DateTime LastBuyDate { get; set; }

        public decimal LastBuyAmount { get; set; }

        public int BonusRedeemQty { get; set; }

        public decimal BonusRedeemSum { get; set; }

        public DateTime WelcomeBonusDate { get; set; }

        public decimal WelcomeBonus { get; set; }

        public DateTime PromoBonusDate { get; set; }

        public decimal PromoBonus { get; set; }

        public DateTime OperatorBonusDate { get; set; }

        public decimal OperatorBonus { get; set; }

        public DateTime FriendBonusDate { get; set; }

        public decimal FriendBonus { get; set; }

        public DateTime BirthdayBonusDate { get; set; }

        public decimal BirthdayBonus { get; set; }

        public string PosRegister { get; set; }

        public DateTime DateRegister { get; set; }

        public int RefundQty { get; set; }

        public decimal Refund { get; set; }

        public List<CardBuysByMonth> CardBuys { get; set; }

        public List<Cheque> ChequeData { get; set; }

        public List<Bonus> Bonuses { get; set; }

        public OperatorClientsManagerBuys()
        {
            CardBuys = new List<CardBuysByMonth>();
            ChequeData = new List<Cheque>();
            Bonuses = new List<Bonus>();
        }
    }

    public class OperatorClientsManagerRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public string Pos { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthdate { get; set; }
        public string Sex { get; set; }
        public string ClientType { get; set; }
        public string Number { get; set; }
        public string Level { get; set; }
        public string Balance { get; set; }

        public long Page { get; set; }
        public long PageSize { get; set; }
    }

    public class OperatorClientsManagerResponse
    {
        public List<OperatorClientsManagerBuys> OperatorClients { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }

        public OperatorClientsManagerResponse()
        {
            OperatorClients = new List<OperatorClientsManagerBuys>();
        }
    }

    public class ClientsForClientsPage
    {
        public int id { get; set; }

        public string name { get; set; }

        public long phone { get; set; }

        public string email { get; set; }

        public string birthdate { get; set; }

        public string gender { get; set; }

        public string client_type { get; set; }

        public long card { get; set; }

        public string level { get; set; }

        public decimal balance { get; set; }

        public int buyCount { get; set; }

        public decimal buyAmount { get; set; }

        public string buyLastDate { get; set; }

        public decimal buyLastAmount { get; set; }

        public int writeOffCount { get; set; }

        public decimal writeOffAmount { get; set; }

        public string welcomeBonusDate { get; set; }

        public decimal welcomeBonusAmount { get; set; }

        public string promoBonusDate { get; set; }

        public decimal promoBonusAmount { get; set; }

        public string opperatorBonusDate { get; set; }

        public decimal operatorBonusAmount { get; set; }

        public string friendBonusDate { get; set; }

        public decimal friendBonusAmount { get; set; }

        public string birthdayBonusDate { get; set; }

        public decimal birthdayBonusAmount { get; set; }

        public string posRegister { get; set; }

        public string dateRegister { get; set; }

        public int refundQty { get; set; }

        public decimal refund { get; set; }

        public string diagram { get; set; }        
    }

    public class clientdata
    {
        public List<ClientsForClientsPage> data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public int draw { get; set; }

        public clientdata()
        {
            data = new List<ClientsForClientsPage>();
        }
    }

    public class BonusesRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        /// <summary>
        /// Начало периода для расчета аналитики
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Окончание периода для расчета аналитики
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    public class BonusesResponse
    {
        /// <summary>
        /// Начислено бонусов
        /// </summary>
        public decimal AddedBonus { get; set; }
        /// <summary>
        /// Кол-во начислени
        /// </summary>
        public decimal AddedBonusCount { get; set; }
        /// <summary>
        /// Среднее начисление
        /// </summary>
        public decimal AvgCharge { get; set; }
        /// <summary>
        /// Списано бонусов
        /// </summary>
        public decimal RedeemedBonus { get; set; }
        /// <summary>
        /// Кол-во списаний
        /// </summary>
        public decimal RedeemedBonusCount { get; set; }
        /// <summary>
        /// Среднее списание
        /// </summary>
        public decimal AvgRedeem { get; set; }
        /// <summary>
        /// Средний баланс
        /// </summary>
        public decimal AvgBalance { get; set; }
        /// <summary>
        /// Фактическая скидка
        /// </summary>
        public decimal AvgDiscount { get; set; }
        /// <summary>
        /// Кол-во клиентов
        /// </summary>
        public decimal ClientCount { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class SegmentationAgeRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        /// <summary>
        /// Начало периода для расчета аналитики
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Окончание периода для расчета аналитики
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    public class SegmentationAgeResponse
    {
        public int LessThen25 { get; set; }
        public int More25Less35 { get; set; }
        public int More35Less45 { get; set; }
        public int More45 { get; set; }
        public int Unknown { get; set; }
        /// <summary>
        /// количество клиентов
        /// </summary>
        public int ClientQty { get; set; }
        /// <summary>
        /// с указанной датой рождения
        /// </summary>
        public int WithBirthDate { get; set; }
        /// <summary>
        /// без указания даты рождения
        /// </summary>
        public int WithoutBirthDate { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class DiagramValue
    {
        public int value { get; set; }

        public string color { get; set; }
    }

    public class ClientBaseStructureRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class ClientBaseStructureResponse
    {
        public int MenQty { get; set; }
        public int WomenQty { get; set; }
        public int UnknownGender { get; set; }
        public int ClientsWithBuys { get; set; }
        public int ClientsWithoutBuys { get; set; }
        public int ClientsWithTenBuys { get; set; }
        public int ClientsWitnOneBuys { get; set; }
        public int ClientsWithPhone { get; set; }
        public int ClientsWithEmail { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ClientBaseActiveRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        /// <summary>
        /// Начало периода для расчета аналитики
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Окончание периода для расчета аналитики
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    public class ClientBaseActiveResponse
    {
        public decimal MenBuys { get; set; }
        public decimal WomenBuys { get; set; }
        public decimal UnknownGenderBuys { get; set; }
        public decimal RepeatedBuys { get; set; }
        public decimal BuysOnClient { get; set; }
        /// <summary>
        /// количество активных клиентов
        /// </summary>
        public int ClientActiveQty { get; set; }
        /// <summary>
        /// получено?
        /// </summary>
        public decimal Gain { get; set; }
        /// <summary>
        /// средний чек
        /// </summary>
        public decimal AvgCheque { get; set; }
        /// <summary>
        /// покупок в будни
        /// </summary>
        public decimal BuysWeekdays { get; set; }
        /// <summary>
        /// покупок в выходные
        /// </summary>
        public decimal BuysWeekOff { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class AnalitycMoneyDiagram
    {
        public decimal BuysAmount { get; set; }
    }

    public class ClientAnalyticMoneyRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }
    }

    public class ClientAnalyticMoneyResponse
    {
        public int WithBirthDate { get; set; }
        public int WithoutBirthDate { get; set; }
        public int WithPhone { get; set; }
        public int WithEmail { get; set; }
        public int MoreTenBuys { get; set; }
        public int WithOneBuy { get; set; }
        public decimal Gain { get; set; }
        public decimal AvgCheque { get; set; }
        public decimal BuysWeekdays { get; set; }
        public decimal BuysWeekOff { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal AvgCharge { get; set; }
        public decimal RedeemedBonus { get; set; }
        public decimal AvgRedeem { get; set; }
        public decimal AvgBalance { get; set; }
        public decimal AvgDiscount { get; set; }
        public int ClientQty { get; set; }
        public int ClientActiveQty { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ClientPeriod
    {
        public int Month { get; set; }
        public int ClientQty { get; set; }
    }

    public class ClientOperatorPeriodRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class ClientOperatorPeriodResponse
    {
        public List<ClientPeriod> ClientOperatorPeriod { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public ClientOperatorPeriodResponse()
        {
            ClientOperatorPeriod = new List<ClientPeriod>();
        }
    }

    public class RefundPeriod
    {
        public int Month { get; set; }
        public decimal RefundSum { get; set; }
    }

    public class RefundOperatorPeriodRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class RefundOperatorPeriodResponse
    {
        public List<RefundPeriod> RefundOperatorPeriod { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public RefundOperatorPeriodResponse()
        {
            RefundOperatorPeriod = new List<RefundPeriod>();
        }
    }

    public class GainPeriod
    {
        public int Month { get; set; }
        public decimal AvgCheque { get; set; }
        public decimal Gain { get; set; }
    }

    public class GainOperatorPeriodRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class GainOperatorPeriodResponse
    {
        public List<GainPeriod> GainOperatorPeriod { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public GainOperatorPeriodResponse()
        {
            GainOperatorPeriod = new List<GainPeriod>();
        }
    }

    public class ClientCreateRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowEmail { get; set; }
        public int Gender { get; set; }
        public bool AgreePersonalData { get; set; }
        public string PosCode { get; set; }
        public Int64? FriendPhone { get; set; }
        public bool ClientSetPassword { get; set; }
        public string Promocode { get; set; }
    }

    public class ClientCreateResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Int64 Phone { get; set; }
        public Int64 Card { get; set; }
        public int Client { get; set; }
    }

    public class ManagerSendCodeRequest
    {
        public string Login { get; set; }
    }

    public class ManagerSendCodeResponse
    {
        public long Phone { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class SetManagerPasswordRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// код подтверждения
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
    }

    public class SetManagerPasswordResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class OperatorInfoRequest
    {
        public Int16 Operator { get; set; }
    }

    public class OperatorInfoResponse
    {
        public string OperatorName { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ActivateCardRequest
    {
        public Int64 Phone { get; set; }
        public Int64 Card { get; set; }
        public string Code { get; set; }
        public Int16 Operator { get; set; }
    }

    public class ActivateCardResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class good
    {
        public string id { get; set; }
        public string chek { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }

    public class goodsdata
    {
        public List<good> data { get; set; }
        public goodsdata()
        {
            data = new List<good>();
        }
    }

    public class OperatorSalesRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        /// <summary>
        /// Фильтр по дате покупки
        /// </summary>
        public DateTime? DateBuy { get; set; }
        /// <summary>
        /// Фильтр по имени ТТ
        /// </summary>
        public string PosName { get; set; }
        /// <summary>
        /// Фильтр по телефону клиента
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Фильтр по типу операции
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// Фильтр по номеру чека
        /// </summary>
        public string Cheque { get; set; }
        /// <summary>
        /// Фильтр по сумме (нижняя граница)
        /// </summary>
        public int? SumMore { get; set; }
        /// <summary>
        /// Фильтр по сумме (верхняя граница)
        /// </summary>
        public int? SumLess { get; set; }
        /// <summary>
        /// Фильтр по начислению (нижняя граница)
        /// </summary>
        public int? ChargeMore { get; set; }
        /// <summary>
        /// Фильтр по начислению (верхняя граница)
        /// </summary>
        public int? ChargeLess { get; set; }
        /// <summary>
        /// Фильтр по списанию (нижняя граница)
        /// </summary>
        public int? RedeemMore { get; set; }
        /// <summary>
        /// Фильтр по списанию (верхняя граница)
        /// </summary>
        public int? RedeemLess { get; set; }
    }
    
    public class ReportOperatorClientRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        /// <summary>
        /// Фильтр по полю ФИО
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Фильтр по полю телефон
        /// </summary>
        public String Phone { get; set; }
        /// <summary>
        /// Фильтр по полю email
        /// </summary>
        public String Email { get; set; }
        /// <summary>
        /// Фильтр по полю дата рождения
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Фильтр по полю пол
        /// </summary>
        public Int16? Sex { get; set; }
        /// <summary>
        /// Фильтр по полю тип клиента
        /// </summary>
        public String Type { get; set; }
        /// <summary>
        /// Фильтр по полю номер карты
        /// </summary>
        public String Card { get; set; }
        /// <summary>
        /// Фильтр по полю уровень
        /// </summary>
        public String Level { get; set; }
        /// <summary>
        /// Фильтр по полю баланс
        /// </summary>
        public String Balance { get; set; }
    }

    public class ReportResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class PartnerClientRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class PosClientPeriodRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public string Pos { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class GetFaq
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class GetFaqRequest
    {
        /// <summary>
        /// Идентификатор оператора
        /// </summary>
        public Int16 Operator { get; set; }

        /// <summary>
        /// Флаг получения FAQ для LCManager
        /// </summary>
        public bool LCManager { get; set; }

        /// <summary>
        /// Флаг получения FAQ для сайта
        /// </summary>
        public bool Site { get; set; }

        /// <summary>
        /// Флаг получения FAQ для мобильного приложения
        /// </summary>
        public bool Mobile { get; set; }
    }

    public class GetFaqResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<GetFaq> FaqData { get; set; }
        public GetFaqResponse()
        {
            FaqData = new List<GetFaq>();
        }
    }

    public class Faq
    {
        public string id { get; set; }
        public string chek { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string lorem { get; set; }
    }

    public class faqdata
    {
        public List<Faq> data { get; set; }
        public faqdata()
        {
            data = new List<Faq>();
        }
    }

    public class ClientImportRequest
    {
        /// <summary>
        /// файл базы
        /// </summary>
        public byte[] ExcelFile { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class ClientImportResponse
    {
        /// <summary>
        /// импортировано
        /// </summary>
        public int Imported { get; set; }
        /// <summary>
        /// успешно
        /// </summary>
        public int Successful { get; set; }
        /// <summary>
        /// неудачно
        /// </summary>
        public int Unsuccessful { get; set; }
        /// <summary>
        /// файл базы
        /// </summary>
        public byte[] ExcelFile { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }
}