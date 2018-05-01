using OfficeOpenXml;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using LCManagerPartner.Implementation.Data;
using LCManagerPartner.Implementation.Request;
using LCManagerPartner.Implementation.Response;

namespace LCManagerPartner.Models
{

    public class BalanceGetRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public long PartnerID { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class BalanceGetResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }

    }

    public class ServerBalanceGetResponse
    {
        public BalanceGetResponse ProcessRequest(SqlConnection cnn, BalanceGetRequest request)
        {
            BalanceGetResponse returnValue = new BalanceGetResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "BalanceGet";
            cmd.Parameters.Add("@card", SqlDbType.BigInt);
            cmd.Parameters["@card"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@card"].Value = null;
            cmd.Parameters.Add("@phone", SqlDbType.BigInt);
            cmd.Parameters["@phone"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@phone"].Value = null;
            if (request.Card > 0)
            {
                cmd.Parameters["@card"].Value = request.Card;
            }
            if (request.Phone > 0)
            {
                cmd.Parameters["@phone"].Value = request.Phone;
            }
            if (request.PartnerID == 0) cmd.Parameters.AddWithValue("@partner", null); else cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            if (request.Operator > 0)
            {
                cmd.Parameters.AddWithValue("@operator", request.Operator);
            }
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            if (returnValue.ErrorCode == 0)
            {
                try
                {
                    returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
                }
                catch { }
                try
                {
                    returnValue.Card = Convert.ToInt64(cmd.Parameters["@card"].Value);
                }
                catch { }
                try
                {
                    returnValue.Phone = Convert.ToInt64(cmd.Parameters["@phone"].Value);
                }
                catch { }
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class RedeemRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int Partner { get; set; }
        /// <summary>
        /// количестов бонусных баллов для списания
        /// </summary>
        public decimal Bonus { get; set; }
    }

    public class RedeemResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// количестов бонусных баллов для списания
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
    }

    public class ServerRedeemResponse
    {
        public RedeemResponse ProcessRequest(SqlConnection cnn, RedeemRequest request)
        {
            RedeemResponse returnValue = new RedeemResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Redeem";
            if (request.Card == 0) cmd.Parameters.AddWithValue("@card", null); else cmd.Parameters.AddWithValue("@card", request.Card);
            if (request.Phone == 0) cmd.Parameters.AddWithValue("@phone", null); else cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@bonus", request.Bonus);
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }
            try
            {
                returnValue.Bonus = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
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
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// дата и время чека
        /// </summary>
        public DateTime ChequeTime { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int Partner { get; set; }
        /// <summary>
        /// код Торговой точки Партнера
        /// </summary>
        public string POS { get; set; }
        /// <summary>
        /// сумма чека
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// оплачено бонусными баллами
        /// </summary>
        public decimal PaidByBonus { get; set; }
        /// <summary>
        /// сумма возврата
        /// </summary>
        public decimal Redeemed { get; set; }
        /// <summary>
        ///  номер чека
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// список позиций чека
        /// </summary>
        public List<Item> ItemData { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NoWrite { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BonusId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NoAdd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NoRedeem { get; set; }
    }

    public class ChequeAddResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public decimal Added { get; set; }
        public decimal Bonus { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
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

    public class ServerChequeAddResponse
    {
        public ChequeAddResponse ProcessRequest(SqlConnection cnn, ChequeAddRequest request)
        {
            ChequeAddResponse returnValue = new ChequeAddResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChequeAdd";

            if (request.Card == 0) cmd.Parameters.AddWithValue("@card", null); else cmd.Parameters.AddWithValue("@card", request.Card);
            if (request.Phone == 0) cmd.Parameters.AddWithValue("@phone", null); else cmd.Parameters.AddWithValue("@phone", request.Phone);
            if (request.ChequeTime < new DateTime(1753, 1, 1))
            {
                request.ChequeTime = DateTime.Now;
            }
            cmd.Parameters.AddWithValue("@chequetime", request.ChequeTime);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@pos", request.POS);
            cmd.Parameters.AddWithValue("@amount", request.Amount);
            cmd.Parameters.AddWithValue("@paidbybonus", request.PaidByBonus);
            cmd.Parameters.AddWithValue("@number", request.Number);
            cmd.Parameters.AddWithValue("@nowrite", request.NoWrite);
            cmd.Parameters.AddWithValue("@noadd", request.NoAdd);
            cmd.Parameters.AddWithValue("@noredeem", request.NoRedeem);
            if (request.BonusId > 0)
            {
                cmd.Parameters.AddWithValue("@bonusid", request.BonusId);
            }

            if (request.ItemData != null && request.ItemData.Count > 0)
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("id", typeof(int));
                    table.Columns.Add("position", typeof(byte));
                    table.Columns.Add("code", typeof(string));
                    table.Columns.Add("price", typeof(decimal));
                    table.Columns.Add("quantity", typeof(decimal));
                    table.Columns.Add("amount", typeof(decimal));
                    table.Columns.Add("paidbybonus", typeof(decimal));
                    table.Columns.Add("minprice", typeof(decimal));
                    table.Columns.Add("noadd", typeof(bool));
                    table.Columns.Add("noredeem", typeof(bool));
                    table.Columns.Add("maxredeem", typeof(decimal));
                    table.Columns.Add("added", typeof(decimal));
                    table.Columns.Add("redeemed", typeof(decimal));

                    foreach (var item in request.ItemData)
                    {
                        DataRow row = table.NewRow();
                        row["id"] = item.Id;
                        row["position"] = item.Position;
                        row["code"] = item.Code;
                        row["price"] = item.Price;
                        row["quantity"] = item.Quantity;
                        row["amount"] = item.Amount;
                        row["paidbybonus"] = item.PaidByBonus;
                        row["minprice"] = item.MinPrice;
                        row["noadd"] = item.NoAdd;
                        row["noredeem"] = item.NoRedeem;
                        row["maxredeem"] = item.MaxRedeem;
                        row["added"] = item.Added;
                        row["redeemed"] = item.Redeemed;
                        table.Rows.Add(row);
                    }
                    var items = new SqlParameter("@chequeitems", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ChequeItems",
                        Value = table
                    };
                    cmd.Parameters.Add(items);
                }
            }

            cmd.Parameters.Add("@cheque", SqlDbType.Int);
            cmd.Parameters["@cheque"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@added", SqlDbType.Decimal);
            cmd.Parameters["@added"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            cmd.Parameters["@redeemed"].Value = request.Redeemed;
            cmd.Parameters["@redeemed"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add("@maxredeem", SqlDbType.Decimal);
            cmd.Parameters["@maxredeem"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@fullbalance", SqlDbType.Decimal);
            cmd.Parameters["@fullbalance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item();
                    if (!reader.IsDBNull(0)) item.Position = reader.GetByte(0);
                    if (!reader.IsDBNull(1)) item.Code = reader.GetString(1);
                    if (!reader.IsDBNull(2)) item.Price = reader.GetDecimal(2);
                    if (!reader.IsDBNull(3)) item.Quantity = reader.GetDecimal(3);
                    if (!reader.IsDBNull(4)) item.Amount = reader.GetDecimal(4);
                    if (!reader.IsDBNull(5)) item.MaxRedeem = reader.GetDecimal(5);
                    if (!reader.IsDBNull(6)) item.Redeemed = reader.GetDecimal(6);
                    if (!reader.IsDBNull(7)) item.Added = reader.GetDecimal(7);
                    returnValue.ItemData.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                returnValue.ErrorCode = 25;
                returnValue.Message = ex.Message;
                Log.Error("LCManagerPartner ChequeAdd {Message}", ex.Message);
                return returnValue;
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }
            try
            {
                returnValue.Bonus = Convert.ToDecimal(cmd.Parameters["@added"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.MaxRedeem = Convert.ToDecimal(cmd.Parameters["@maxredeem"].Value);
            }
            catch { }
            try
            {
                returnValue.FullBalance = Convert.ToDecimal(cmd.Parameters["@fullbalance"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value);
            }
            catch { }
            try
            {
                returnValue.Added = Convert.ToDecimal(cmd.Parameters["@added"].Value);
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    

    public class GetPosesRequest
    {
        /// <summary>
        /// ID партнера
        /// </summary>
        [Required]
        public int PartnerID { get; set; }
    }

    public class GetPosesResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Список ТТ
        /// </summary>
        public List<Pos> PosData { get; set; }
        public GetPosesResponse()
        {
            PosData = new List<Pos>();
        }
    }

    public class ServerGetPosesResponse
    {
        public GetPosesResponse ProcessRequest(SqlConnection cnn, GetPosesRequest request)
        {
            GetPosesResponse returnValue = new GetPosesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Poses";
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Pos pos = new Pos();
                pos.Id = reader.GetInt16(0);
                if (!reader.IsDBNull(1)) pos.Name = reader.GetString(1);
                if (!reader.IsDBNull(3)) pos.Region = reader.GetString(3);
                if (!reader.IsDBNull(4)) pos.City = reader.GetString(4);
                if (!reader.IsDBNull(5)) pos.Address = reader.GetString(5);
                if (!reader.IsDBNull(6)) pos.Phone = reader.GetString(6);
                if (!reader.IsDBNull(8)) pos.ShowOnSite = reader.GetBoolean(8);
                if (!reader.IsDBNull(9)) pos.GivesCard = reader.GetBoolean(9);
                returnValue.PosData.Add(pos);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ChequeItem
    {
        /// <summary>
        /// код товара
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// цена за еденицу
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// количество товара
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// сумма чека
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// начислено бонусов
        /// </summary>
        public decimal AddedBonus { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal RedeemedBonus { get; set; }
    }

    public class Cheque
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// номер
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// тип операции
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Summ { get; set; }
        /// <summary>
        /// сумма скидки
        /// </summary>
        public decimal SummDiscount { get; set; }
        /// <summary>
        /// бонусов с чека
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// оплачено бонусами
        /// </summary>
        public decimal PaidByBonus { get; set; }
        /// <summary>
        /// скидка
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// магазин? (интернет\обычный?)
        /// </summary>
        public string Shop { get; set; }
        /// <summary>
        /// номер карты участника программы лояльности
        /// </summary>
        public Int64 CardNumber { get; set; }
        /// <summary>
        /// наименование торговой точки
        /// </summary>
        public string PosName { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
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

    public class GetChequesRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int PartnerId { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public Int16 Pos { get; set; }

        #region Параметр передаваемые от jqyery плагина DataTable для фильтрации и пагинации
        /// <summary>
        /// Дата покупки
        /// </summary>
        public string DateBuy { get; set; }
        
        /// <summary>
        /// Точка продаж
        /// </summary>
        public string PosStr { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Операция (покупка, продажа)
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// № чека
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Сумма покупки
        /// </summary>
        public string Sum { get; set; }

        /// <summary>
        /// Начисленно
        /// </summary>
        public string Added { get; set; }

        /// <summary>
        /// Списано
        /// </summary>
        public string Redeemed { get; set; }

        /// <summary>
        /// Позиция с которой отображать выборку
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Сколько объектов отображать
        /// </summary>
        public Int16 PageSize { get; set; }

        /// <summary>
        /// Показывать продажи с
        /// </summary>
        public string DateStart { get; set; }

        /// <summary>
        /// Показывать продажи по
        /// </summary>
        public string DateEnd { get; set; }
        #endregion
    }

    public class GetChequesResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// количество страниц
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// детализация по чекам
        /// </summary>

        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }

        public List<Cheque> ChequeData { get; set; }
        public GetChequesResponse()
        {
            ChequeData = new List<Cheque>();
        }
    }

    public class ServerGetChequesResponse
    {
        public GetChequesResponse ProcessRequest(SqlConnection con, GetChequesRequest request)
        {
            //неизменный текст запроса
    //        var sql = @"
    //        SELECT 
	   //         c.id, 
	   //         c.number, 
	   //         c.chequetime, 
	   //         c.refund, 
	   //         ABS(c.amount) AS amount, 
	   //         c.discount, 
	   //         p.name AS partner, 
	   //         pos.code AS pos, 
	   //         c.card, 
	   //         b1.summa AS added, 
	   //         -b2.summa AS redeemed, 
	   //         pos.name AS posname,
	   //         co.phone AS clientPhone,
	   //         ROW_NUMBER() OVER ( ORDER BY c.proctime DESC ) AS RowNum
    //        FROM 
	   //         cheque as c ";

    //        var sqlCount = @"SELECT COUNT(*) FROM  cheque as c ";

    //        //Формируем блок WHERE в зависимости от фильтрации в таблице на клиенте
    //        var whereStr = string.Empty;

    //        //Фильтр по дате (Верхний фильтр с диапазоном)
    //        if (!string.IsNullOrEmpty(request.DateStart))
    //        {
    //            if (DateTime.TryParseExact(request.DateStart, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
    //            {
    //                whereStr = whereStr + "AND YEAR(c.proctime)>=" + date.Year + " AND MONTH(c.proctime)>=" + date.Month + " AND DAY(c.proctime)>=" + date.Day + " ";
    //            }
    //        }

    //        //Фильтр по дате (Верхний фильтр с диапазоном)
    //        if (!string.IsNullOrEmpty(request.DateEnd))
    //        {
    //            if (DateTime.TryParseExact(request.DateEnd, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
    //            {
    //                whereStr = whereStr + "AND YEAR(c.proctime)<=" + date.Year + " AND MONTH(c.proctime)<=" + date.Month + " AND DAY(c.proctime)<" + date.Day + " ";
    //            }
    //        }

    //        //Фильтр по дате покупки
    //        if (!string.IsNullOrEmpty(request.DateBuy))
    //        {
    //            if (DateTime.TryParseExact(request.DateBuy, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
    //            {
    //                whereStr = whereStr + "AND YEAR(c.proctime)=" + date.Year+" AND MONTH(c.proctime)="+date.Month+ " AND DAY(c.proctime)=" + date.Day+" ";
    //            }
    //        }

    //        //Фильтр по точке продаж
    //        if (!string.IsNullOrEmpty(request.PosStr))
    //        {
    //            whereStr = whereStr + " AND pos.name LIKE '%" + request.PosStr + "%' ";
    //        }

    //        //Фильтр по операции
    //        if (!string.IsNullOrEmpty(request.Operation))
    //        {
    //            if (request.Operation.ToLower().Contains("возврат"))
    //                whereStr = whereStr + " AND c.refund=1 ";
    //            if (request.Operation.ToLower().Contains("покупка"))
    //                whereStr = whereStr + " AND c.refund!=1 ";
    //        }

    //        //Фильтр по №чека
    //        if (!string.IsNullOrEmpty(request.Number))
    //        {
    //            whereStr = whereStr + " AND (c.number LIKE '%" + request.Number + "%'OR c.number LIKE '%" + request.Number + "' OR c.number LIKE '" + request.Number + "%' OR c.number ='" + request.Number+ "') ";
    //        }

    //        //Фильтр по клиенту
    //        if (!string.IsNullOrEmpty(request.Phone))
    //        {
    //            whereStr = whereStr + " AND (co.phone LIKE '%" + request.Phone + "%' OR co.phone LIKE '%" + request.Phone+ "' OR co.phone LIKE '" + request.Phone+ "%' OR co.phone ='"+request.Phone+"') ";
    //        }

    //        //Фильтр по сумме
    //        if (!string.IsNullOrEmpty(request.Sum))
    //        {
    //            var values = request.Sum.Split('-');
    //            whereStr = whereStr + " AND ABS(c.amount)>="+values[0]+ " AND ABS(c.amount)<"+values[1]+" ";
    //        }

    //        //Фильтр по начислениям
    //        if (!string.IsNullOrEmpty(request.Added))
    //        {
    //            var values = request.Added.Split('-');
    //            whereStr = whereStr + " AND b1.summa>=" + values[0] + " AND b1.summa<" + values[1] + " ";
    //        }

    //        //Фильтр по списаниям
    //        if (!string.IsNullOrEmpty(request.Redeemed))
    //        {
    //            var values = request.Redeemed.Split('-');
    //            whereStr = whereStr + " AND ABS(b2.summa)>=" + values[0] + " AND ABS(b2.summa)<" + values[1] + " ";
    //        }

    //        if (!string.IsNullOrEmpty(whereStr)) whereStr = whereStr + "AND 1=1";

    //        //Формируем блок JOIN в зависимости от пришедших парамтеров
    //        var joinStr = string.Empty;
    //        if (request.Operator != 0 && request.PartnerId == 0 && request.Pos == 0)
    //        {
    //            joinStr = @"
    //                    LEFT JOIN partner as p ON c.partner = p.id 
		  //              LEFT JOIN pos ON c.pos = pos.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus > 0 group by cheque) b1 on b1.cheque=c.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus < 0 group by cheque) b2 on b2.cheque=c.id
		  //              left join card cd on cd.number = c.card
		  //              inner join clientoperator co on cd.client = co.client AND cd.operator = co.operator
	   //             WHERE 
    //                    c.partner IN(SELECT id FROM partner WHERE operator = @operator) AND(c.cancelled IS NULL OR c.cancelled = 0) ";
                
    //        }
    //        else if (request.Operator != 0 && request.PartnerId != 0 && request.Pos == 0)
    //        {
    //            joinStr = @"
    //                    LEFT JOIN partner as p ON c.partner = p.id 
		  //              LEFT JOIN pos ON c.pos = pos.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus > 0 group by cheque) b1 on b1.cheque=c.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus < 0 group by cheque) b2 on b2.cheque=c.id
		  //              left join card cd on cd.number = c.card
		  //              inner join clientoperator co on cd.client = co.client AND cd.operator = co.operator
	   //             WHERE 
		  //              c.partner = @partner AND (c.cancelled IS NULL OR c.cancelled = 0)";
    //        }
    //        else if (request.Operator != 0 && request.PartnerId != 0 && request.Pos != 0)
    //        {
    //            joinStr = @"
    //                    LEFT JOIN partner as p ON c.partner = p.id 
		  //              LEFT JOIN pos ON c.pos = pos.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus > 0 group by cheque) b1 on b1.cheque=c.id
		  //              left join (select cheque, sum(bonus) as summa from bonus where bonus < 0 group by cheque) b2 on b2.cheque=c.id
		  //              left join card cd on cd.number = c.card
		  //              inner join clientoperator co on cd.client = co.client AND cd.operator = co.operator
	   //             WHERE 
    //                    c.pos = @pos AND (c.cancelled IS NULL OR c.cancelled = 0) ";
    //        }

    //        sql = sql + joinStr;
    //        sqlCount = sqlCount + joinStr;

    //        sql = sql + whereStr;
    //        sqlCount = sqlCount + whereStr;

    //        sql = @"SELECT  * FROM (" + sql + @") AS RowConstrainedResult
				//WHERE   RowNum >= @start
				//	AND RowNum < @length
				//ORDER BY RowNum";

    //        sql = sql.Replace("@partner", request.PartnerId.ToString());
    //        sql = sql.Replace("@operator", request.Operator.ToString());
    //        sql = sql.Replace("@pos", request.Pos.ToString());
    //        sqlCount = sqlCount.Replace("@partner", request.PartnerId.ToString());
    //        sqlCount = sqlCount.Replace("@operator", request.Operator.ToString());
    //        sqlCount = sqlCount.Replace("@pos", request.Pos.ToString());

    //        if (request.Page == 0) request.Page++;
    //        sql = sql.Replace("@start", request.Page.ToString());
    //        sql = sql.Replace("@length", (request.PageSize + request.Page).ToString());

            GetChequesResponse returnValue = new GetChequesResponse();
            //con.Open();
            //SqlCommand cmd = con.CreateCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = sqlCount;
            //returnValue.RecordTotal = (Int32) cmd.ExecuteScalar();
            //returnValue.RecordFilterd = returnValue.RecordTotal;
            //cmd.CommandText = sql;

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChequesPaging";
            if (request.Operator > 0) cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.PartnerId > 0) cmd.Parameters.AddWithValue("@partner", request.PartnerId);
            if (request.Pos > 0) cmd.Parameters.AddWithValue("@pos", request.Pos);
            if (request.Page == 0) request.Page++;
            cmd.Parameters.AddWithValue("@start", request.Page);
            cmd.Parameters.AddWithValue("@length", request.Page + request.PageSize);
            cmd.Parameters.AddWithValue("@f_pos", request.PosStr);
            cmd.Parameters.AddWithValue("@f_phone", request.PosStr);
            cmd.Parameters.AddWithValue("@f_cheque", request.Number);
            
            if (!string.IsNullOrEmpty(request.DateBuy))
            {
                if (DateTime.TryParseExact(request.DateBuy, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                {
                    cmd.Parameters.AddWithValue("@f_date_buy", date);
                }
            }
            //Фильтр по дате (Верхний фильтр с диапазоном)
            if (!string.IsNullOrEmpty(request.DateStart))
            {
                if (DateTime.TryParseExact(request.DateStart, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                {
                    cmd.Parameters.AddWithValue("@f_date_start", date);
                }
            }

            //Фильтр по дате (Верхний фильтр с диапазоном)
            if (!string.IsNullOrEmpty(request.DateEnd))
            {
                if (DateTime.TryParseExact(request.DateEnd, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                {
                    cmd.Parameters.AddWithValue("@f_date_end", date);
                }
            }

            //Фильтр по операции
            if (!string.IsNullOrEmpty(request.Operation))
            {
                if (request.Operation.ToLower().Contains("возврат")) cmd.Parameters.AddWithValue("@f_operation", 1);
                else if (request.Operation.ToLower().Contains("покупка")) cmd.Parameters.AddWithValue("@f_operation", 0);
            }

            //Фильтр по сумме
            if (!string.IsNullOrEmpty(request.Sum))
            {
                var values = request.Sum.Split('-');
                cmd.Parameters.AddWithValue("@f_sum_more", Convert.ToInt32(values[0]));
                cmd.Parameters.AddWithValue("@f_sum_less", Convert.ToInt32(values[1]));
            }

            //Фильтр по начислениям
            if (!string.IsNullOrEmpty(request.Added))
            {
                var values = request.Added.Split('-');
                cmd.Parameters.AddWithValue("@f_added_more", Convert.ToInt32(values[0]));
                cmd.Parameters.AddWithValue("@f_added_less", Convert.ToInt32(values[1]));
            }

            //Фильтр по списаниям
            if (!string.IsNullOrEmpty(request.Redeemed))
            {
                var values = request.Redeemed.Split('-');
                cmd.Parameters.AddWithValue("@f_redeemed_more", Convert.ToInt32(values[0]));
                cmd.Parameters.AddWithValue("@f_redeemed_less", Convert.ToInt32(values[1]));
            }



            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@total_rows", SqlDbType.Int);
            cmd.Parameters["@total_rows"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            //cmd.Parameters.Add("@pagecount", SqlDbType.Int);
            //cmd.Parameters["@pagecount"].Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@total", SqlDbType.Int);
            //cmd.Parameters["@total"].Direction = ParameterDirection.Output;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Cheque cheque = new Cheque();
                cheque.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) cheque.Number = reader.GetString(1);
                if (!reader.IsDBNull(2)) cheque.Date = reader.GetDateTime(2);
                cheque.OperationType = "Покупка";
                if (!reader.IsDBNull(3)) if (reader.GetBoolean(3) == true) cheque.OperationType = "Возврат";
                if (!reader.IsDBNull(4)) cheque.Summ = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) cheque.Discount = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) cheque.Partner = reader.GetString(6);
                if (!reader.IsDBNull(7)) cheque.Shop = reader.GetString(7);
                if (!reader.IsDBNull(8)) cheque.CardNumber = reader.GetInt64(8);
                if (!reader.IsDBNull(9)) cheque.Bonus = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) cheque.PaidByBonus = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) cheque.PosName = reader.GetString(11);
                if (!reader.IsDBNull(12)) cheque.Phone = reader.GetInt64(12);
                returnValue.ChequeData.Add(cheque);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.RecordTotal = Convert.ToInt32(cmd.Parameters["@total_rows"].Value);
            returnValue.RecordFilterd = returnValue.RecordTotal;
            try
            {
                foreach (var cheque in returnValue.ChequeData)
                {
                    SqlCommand cmdItems = con.CreateCommand();
                    cmdItems.CommandType = CommandType.StoredProcedure;
                    cmdItems.CommandText = "ChequeItems";
                    cmdItems.Parameters.AddWithValue("@cheque", cheque.Id);
                    using (var readerItems = cmdItems.ExecuteReader())
                    {
                        while (readerItems.Read())
                        {
                            ChequeItem item = new ChequeItem();
                            if (!readerItems.IsDBNull(0)) item.Code = readerItems.GetString(0);
                            if (!readerItems.IsDBNull(1)) item.Price = readerItems.GetDecimal(1);
                            if (!readerItems.IsDBNull(2)) item.Qty = readerItems.GetDecimal(2);
                            if (!readerItems.IsDBNull(3)) item.Amount = readerItems.GetDecimal(3);
                            if (!readerItems.IsDBNull(4)) item.RedeemedBonus = readerItems.GetDecimal(4);
                            if (!readerItems.IsDBNull(5)) item.AddedBonus = readerItems.GetDecimal(5);
                            cheque.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
            }
            con.Close();
            return returnValue;
        }
    }

    public class GetChequesByCardRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 CardNumber { get; set; }
        /// <summary>
        /// код Торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
    }

    public class GetChequesByCardResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// информация по чеку
        /// </summary>
        public List<Cheque> ChequeData { get; set; }
        public GetChequesByCardResponse()
        {
            ChequeData = new List<Cheque>();
        }
    }

    public class ServerGetChequesByCardResponse
    {
        public GetChequesByCardResponse ProcessRequest(SqlConnection con, GetChequesByCardRequest request)
        {
            GetChequesByCardResponse returnValue = new GetChequesByCardResponse();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Cheques";
            cmd.Parameters.AddWithValue("@card", request.CardNumber);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Cheque cheque = new Cheque();
                cheque.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) cheque.Number = reader.GetString(1);
                if (!reader.IsDBNull(2)) cheque.Date = reader.GetDateTime(2);
                cheque.OperationType = "Покупка";
                if (!reader.IsDBNull(3)) if (reader.GetBoolean(3) == true) cheque.OperationType = "Возврат";
                if (!reader.IsDBNull(4)) cheque.Summ = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) cheque.Discount = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) cheque.Partner = reader.GetString(6);
                if (!reader.IsDBNull(11)) cheque.Shop = reader.GetString(11);
                if (!reader.IsDBNull(8)) cheque.CardNumber = reader.GetInt64(8);
                if (!reader.IsDBNull(9)) cheque.Bonus = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) cheque.PaidByBonus = reader.GetDecimal(10);
                returnValue.ChequeData.Add(cheque);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            con.Close();
            return returnValue;
        }
    }

    public class GetConfirmCodeRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// код подтверждения
        /// </summary>
        public string Code { get; set; }
    }

    public class GetConfirmCodeResponse
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

    public class ServerGetConfirmCodeResponse
    {
        public GetConfirmCodeResponse ProcessRequest(SqlConnection cnn, GetConfirmCodeRequest request)
        {
            GetConfirmCodeResponse returnValue = new GetConfirmCodeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientConfirm";
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@code", request.Code);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class SetClientPasswordRequest
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

    public class SetClientPasswordResponse
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

    public class ServerSetClientPasswordResponse
    {
        public SetClientPasswordResponse ProcessRequest(SqlConnection cnn, SetClientPasswordRequest request)
        {
            SetClientPasswordResponse returnValue = new SetClientPasswordResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientSetPassword";
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@code", request.Code);
            cmd.Parameters.AddWithValue("@password", request.Password);

            cmd.Parameters.AddWithValue("@client", request.ClientID);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class GetRegistrationUserRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int PartnerID { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string PosCode { get; set; }
        /// <summary>
        /// согласие на обработку персональных данных
        /// </summary>
        public string AgreePersonalData { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// телефон друга/подруги для механики “Приведи друга”
        /// </summary>
        public Int64? FriendPhone { get; set; }
        /// <summary>
        /// задать пароль?
        /// </summary>
        public bool ClientSetPassword { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// промокод
        /// </summary>
        public string Promocode { get; set; }
    }

    public class GetRegistrationUserResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// ID участника
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// ID торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
    }

    public class ServerGetRegistrationUserResponse
    {
        public GetRegistrationUserResponse ProcessRequest(SqlConnection cnn, GetRegistrationUserRequest request)
        {
            GetRegistrationUserResponse returnValue = new GetRegistrationUserResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAdd";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters["@phone"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters["@card"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("@client", SqlDbType.Int);
            cmd.Parameters["@client"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@posCode", request.PosCode);
            cmd.Parameters.Add("@pos", SqlDbType.SmallInt);
            cmd.Parameters["@pos"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@agreepersonaldata", request.AgreePersonalData);
            if (request.FriendPhone.HasValue)
            {
                cmd.Parameters.AddWithValue("@friend", request.FriendPhone.Value);
            }
            cmd.Parameters.AddWithValue("@clientsetpassword", request.ClientSetPassword);
            cmd.Parameters.AddWithValue("@email", request.Email);
            if(!string.IsNullOrEmpty(request.Promocode))
            {
                cmd.Parameters.AddWithValue("@promocode", request.Promocode);
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ServerGetRegistrationUserResponse");
                cnn.Close();
                returnValue.ErrorCode = 500;
                returnValue.Message = ex.Message;
                return returnValue;
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try
            {
                returnValue.Client = Convert.ToInt32(cmd.Parameters["@client"].Value);
            }
            catch { }
            try
            {
                returnValue.Phone = Convert.ToInt64(cmd.Parameters["@phone"].Value);
            }
            catch { }
            try
            {
                returnValue.Card = Convert.ToInt64(cmd.Parameters["@card"].Value);
            }
            catch { }
            try
            {
                returnValue.Pos = Convert.ToInt16(cmd.Parameters["@pos"].Value);
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    public class GetSendVerificationCodeRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class GetSendVerificationCodeResponse
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

    public class ServerGetSendVerificationCodeResponse
    {
        public GetSendVerificationCodeResponse ProcessRequest(SqlConnection cnn, GetSendVerificationCodeRequest request)
        {
            GetSendVerificationCodeResponse returnValue = new GetSendVerificationCodeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientSendCode";
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            if (request.Operator > 0)
            {
                cmd.Parameters.AddWithValue("@operator", request.Operator);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientLoginRequest
    {
        /// <summary>
        /// номер телефона или номер карты
        /// </summary>
        public Int64 Login { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// токен (временный код) Участника в социальной сети “Facebook”
        /// </summary>
        public string IdFB { get; set; }
        /// <summary>
        /// токен (временный код) Участника в “Одноклассники”
        /// </summary>
        public string IdOK { get; set; }
        /// <summary>
        /// токен (временный код) Участника в “ВКонтакте”
        /// </summary>
        public string IdVK { get; set; }
        /// <summary>
        /// Идентификатор оператора, у которого авторизовывается клиент
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class ClientLoginResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// ID клиента
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ClientLoginResponse ()?
        /// </summary>
        public ClientLoginResponse() { }
    }

    public class ServerClientLoginResponse
    {
        public ClientLoginResponse ProcessRequest(SqlConnection cnn, ClientLoginRequest request)
        {
            ClientLoginResponse returnValue = new ClientLoginResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientLogin";
            cmd.Parameters.AddWithValue("@login", request.Login);
            cmd.Parameters.AddWithValue("@password", request.Password);
            cmd.Parameters.AddWithValue("@idfb", request.IdFB);
            cmd.Parameters.AddWithValue("@idok", request.IdOK);
            cmd.Parameters.AddWithValue("@idvk", request.IdVK);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("@client", SqlDbType.Int);
            cmd.Parameters["@client"].Direction = ParameterDirection.Output;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int client;
                if (reader.IsDBNull(0)) client = 0; else client = reader.GetInt32(0);
                returnValue.ClientID = client;
            }
            reader.Close();
            if (!DBNull.Value.Equals(cmd.Parameters["@client"].Value))
            {
                returnValue.ClientID = Convert.ToInt16(cmd.Parameters["@client"].Value);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class SendEmailCodeRequest
    {
        /// <summary>
        /// адрес электронной почты участника программы лояльности
        /// </summary>
        public string Email { get; set; }
    }

    public class SendEmailCodeResponse
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

    public class ServerSendEmailCodeResponse
    {
        public SendEmailCodeResponse ProcessRequest(SqlConnection cnn, SendEmailCodeRequest request)
        {
            SendEmailCodeResponse returnValue = new SendEmailCodeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientSendEmailCode";
            cmd.Parameters.AddWithValue("@email", request.Email);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ValidateEmailRequest
    {
        /// <summary>
        /// электронная почта
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// полученный в сообщении проверочный код
        /// </summary>
        public string Code { get; set; }
    }

    public class ValidateEmailResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public int ClientID { get; set; }
    }

    public class ServerValidateEmailResponse
    {
        public ValidateEmailResponse ProcessRequest(SqlConnection cnn, ValidateEmailRequest request)
        {
            ValidateEmailResponse returnValue = new ValidateEmailResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientEmailConfirm";
            cmd.Parameters.AddWithValue("@email", request.Email);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("@client", SqlDbType.Int);
            cmd.Parameters["@client"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try { returnValue.ClientID = Convert.ToInt32(cmd.Parameters["@client"].Value); }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    public class AddPhoneRequest
    {
        /// <summary>
        /// идентификатор Участника
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
    }

    public class AddPhoneResponse
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

    public class ServerAddPhoneResponse
    {
        public AddPhoneResponse ProcessRequest(SqlConnection cnn, AddPhoneRequest request)
        {
            AddPhoneResponse returnValue = new AddPhoneResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddPhone";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class DeletePhoneRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
    }

    public class DeletePhoneResponse
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

    public class ServerDeletePhoneResponse
    {
        public DeletePhoneResponse ProcessRequest(SqlConnection cnn, DeletePhoneRequest request)
        {
            DeletePhoneResponse returnValue = new DeletePhoneResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PhoneDelete";
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class PartnerFullInfo
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public Int64 Phone { get; set; }
        public Int64 Card { get; set; }
        public DateTime ChequeTime { get; set; }
        public string Pos { get; set; }
        public string Terminal { get; set; }
        public decimal Amount { get; set; }
        public decimal Bonus { get; set; }
    }

    public class PartnerFullInfoRequest
    {
        public Int16 Partner { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }

    public class PartnerFullInfoResponse
    {
        //public List<PartnerFullInfo> PartnerFullInfoData { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        //public int ClientQty { get; set; }
        public Int32 Clients { get; set; }
        public Int32 Clients_all { get; set; }
        public int Purchases { get; set; }
        public decimal PurchaseSum { get; set; }
        public int Refunds { get; set; }
        public decimal RefundSum { get; set; }
        public decimal SpentSum { get; set; }
        public decimal Charged { get; set; }
        public decimal Redeemed { get; set; }
        public decimal ChargeRefund { get; set; }
        public decimal RedeemRefund { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        public int PurchasesClient { get; set; }
        public decimal PurchaseSumClient { get; set; }
        public int RefundsClient { get; set; }
        public decimal RefundSumClient { get; set; }
        //public PartnerFullInfoResponse()
        //{
        //    PartnerFullInfoData = new List<PartnerFullInfo>();
        //}
    }

    public class ServerPartnerFullInfoResponse
    {
        public PartnerFullInfoResponse ProcessRequest(SqlConnection cnn, PartnerFullInfoRequest request)
        {
            PartnerFullInfoResponse returnValue = new PartnerFullInfoResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PartnerStatistics";
            cmd.Parameters.AddWithValue("@partner", request.Partner);

            //cmd.Parameters.Add("@clientQty", SqlDbType.Int);
            //cmd.Parameters["@clientQty"].Direction = ParameterDirection.Output;
            if (request.Start_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@start_date", request.Start_date);
            }
            if (request.End_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@end_date", request.End_date);
            }

            cmd.Parameters.Add("@clients", SqlDbType.Int);
            cmd.Parameters["@clients"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clients_all", SqlDbType.Int);
            cmd.Parameters["@clients_all"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchases", SqlDbType.Int);
            cmd.Parameters["@purchases"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refunds", SqlDbType.Int);
            cmd.Parameters["@refunds"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refundsum", SqlDbType.Decimal);
            cmd.Parameters["@refundsum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@spentsum", SqlDbType.Decimal);
            cmd.Parameters["@spentsum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@charged", SqlDbType.Decimal);
            cmd.Parameters["@charged"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@chargerefund", SqlDbType.Decimal);
            cmd.Parameters["@chargerefund"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemrefund", SqlDbType.Decimal);
            cmd.Parameters["@redeemrefund"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@purchasesClient", SqlDbType.Int);
            cmd.Parameters["@purchasesClient"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchasesumClient", SqlDbType.Decimal);
            cmd.Parameters["@purchasesumClient"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refundsClient", SqlDbType.Int);
            cmd.Parameters["@refundsClient"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refundsumClient", SqlDbType.Decimal);
            cmd.Parameters["@refundsumClient"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            //try
            //{
            //    returnValue.ClientQty = Convert.ToInt32(cmd.Parameters["@clientQty"].Value);
            //}
            //catch { }
            try
            {
                returnValue.Clients = Convert.ToInt32(cmd.Parameters["@clients"].Value);
            }
            catch { }
            try
            {
                returnValue.Clients_all = Convert.ToInt32(cmd.Parameters["@clients_all"].Value);
            }
            catch { }
            try
            {
                returnValue.Purchases = Convert.ToInt32(cmd.Parameters["@purchases"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value);
            }
            catch { }
            try
            {
                returnValue.Refunds = Convert.ToInt32(cmd.Parameters["@refunds"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSum = Convert.ToDecimal(cmd.Parameters["@refundsum"].Value);
            }
            catch { }
            try
            {
                returnValue.SpentSum = Convert.ToDecimal(cmd.Parameters["@spentsum"].Value);
            }
            catch { }
            try
            {
                returnValue.Charged = Convert.ToDecimal(cmd.Parameters["@charged"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.ChargeRefund = Convert.ToDecimal(cmd.Parameters["@chargerefund"].Value);
            }
            catch { }
            try
            {
                returnValue.RedeemRefund = Convert.ToDecimal(cmd.Parameters["@redeemrefund"].Value);
            }
            catch { }
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchasesClient = Convert.ToInt32(cmd.Parameters["@purchasesClient"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSumClient = Convert.ToDecimal(cmd.Parameters["@purchasesumClient"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundsClient = Convert.ToInt32(cmd.Parameters["@refundsClient"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSumClient = Convert.ToDecimal(cmd.Parameters["@refundsumClient"].Value);
            }
            catch { }

            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class RefundRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// время чека
        /// </summary>
        public DateTime ChequeTime { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// оплачено бонусами
        /// </summary>
        public decimal PaidByBonus { get; set; }
        /// <summary>
        /// номер чека
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// ID покупки
        /// </summary>
        public int PurchaseId { get; set; }
        /// <summary>
        /// номер покупки?
        /// </summary>
        public string PurchaseNumber { get; set; }
        /// <summary>
        /// дата покупки
        /// </summary>
        public DateTime? PurchaseDate { get; set; }
        /// <summary>
        /// торговая точка покупки
        /// </summary>
        public string PurchasePos { get; set; }
        /// <summary>
        /// торговый терминал покупки
        /// </summary>
        public string PurchaseTerminal { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class RefundResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public decimal Added { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        public decimal Redeemed { get; set; }
        public decimal Amount { get; set; }
    }

    public class ServerRefundResponse
    {
        public RefundResponse ProcessRequest(SqlConnection cnn, RefundRequest request)
        {
            RefundResponse returnValue = new RefundResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Refund";
            if (request.Card > 0)
            {
                cmd.Parameters.AddWithValue("@card", request.Card);
            }
            cmd.Parameters.AddWithValue("@chequetime", request.ChequeTime);
            if (request.Phone > 0)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone);
            }
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            cmd.Parameters.AddWithValue("@pos", request.Pos);
            cmd.Parameters.AddWithValue("@amount", request.Amount);
            cmd.Parameters["@amount"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@paidbybonus", request.PaidByBonus);
            cmd.Parameters.AddWithValue("@number", request.Number);
            SqlParameter added = new SqlParameter("@added", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(added);
            SqlParameter redeemed = new SqlParameter("@redeemed", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemed);
            SqlParameter balance = new SqlParameter("@balance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(balance);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            if (request.PurchaseId > 0)
            {
                cmd.Parameters.AddWithValue("@purchaseid", request.PurchaseId);
            }
            cmd.Parameters.AddWithValue("@purchasenumber", request.PurchaseNumber);
            cmd.Parameters.AddWithValue("@purchasedate", request.PurchaseDate);
            cmd.Parameters.AddWithValue("@purchasepos", request.PurchasePos);
            cmd.Parameters.AddWithValue("@purchaseterminal", request.PurchaseTerminal);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                returnValue.ErrorCode = 25;
                returnValue.Message = ex.Message;
                Log.Error("LCManagerPos Refund {Message}", ex.Message);
                return returnValue;
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try
            {
                returnValue.Added = Convert.ToDecimal(cmd.Parameters["@added"].Value);
            }
            catch { }
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.Amount = Convert.ToDecimal(cmd.Parameters["@amount"].Value);
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    public class OperatorStatisticsRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// дата начала периода
        /// </summary>
        public DateTime Start_date { get; set; }
        /// <summary>
        /// дата окончания периода
        /// </summary>
        public DateTime End_date { get; set; }
    }

    public class OperatorStatisticsResponse
    {
        /// <summary>
        /// новые клиенты?
        /// </summary>
        public Int32 Clients { get; set; }
        /// <summary>
        /// всего клиентов?
        /// </summary>
        public Int32 Clients_all { get; set; }
        /// <summary>
        /// всего покупок
        /// </summary>
        public Int32 Purchases { get; set; }
        /// <summary>
        /// сумма покупок
        /// </summary>
        public decimal PurchaseSum { get; set; }
        /// <summary>
        /// всего возвратов
        /// </summary>
        public Int32 Refunds { get; set; }
        /// <summary>
        /// сумма возвратов
        /// </summary>
        public decimal RefundSum { get; set; }
        /// <summary>
        /// всего потрачено
        /// </summary>
        public decimal SpentSum { get; set; }
        /// <summary>
        /// начислено бонусов
        /// </summary>
        public decimal Charged { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal Redeemed { get; set; }
        /// <summary>
        /// начислено с возвратов?
        /// </summary>
        public decimal ChargeRefund { get; set; }
        /// <summary>
        /// списано с возвратов?
        /// </summary>
        public decimal RedeemRefund { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// сумма к оплате?
        /// </summary>
        public decimal Paysum { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerOperatorStatisticsResponse
    {
        public OperatorStatisticsResponse ProcessRequest(SqlConnection cnn, OperatorStatisticsRequest request)
        {
            OperatorStatisticsResponse returnValue = new OperatorStatisticsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorStatistics";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Start_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@start_date", request.Start_date);
            }
            if (request.End_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@end_date", request.End_date);
            }

            cmd.Parameters.Add("@clients", SqlDbType.Int);
            cmd.Parameters["@clients"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clients_all", SqlDbType.Int);
            cmd.Parameters["@clients_all"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchases", SqlDbType.Int);
            cmd.Parameters["@purchases"].Direction = ParameterDirection.Output;
            SqlParameter purchasesum = new SqlParameter("@purchasesum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(purchasesum);
            SqlParameter refunds = new SqlParameter("@refunds", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(refunds);
            SqlParameter refundsum = new SqlParameter("@refundsum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(refundsum);
            SqlParameter spentsum = new SqlParameter("@spentsum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(spentsum);
            SqlParameter charged = new SqlParameter("@charged", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(charged);
            SqlParameter redeemed = new SqlParameter("@redeemed", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemed);
            SqlParameter chargerefund = new SqlParameter("@chargerefund", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(chargerefund);
            SqlParameter redeemrefund = new SqlParameter("@redeemrefund", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemrefund);
            SqlParameter balance = new SqlParameter("@balance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(balance);
            SqlParameter paysum = new SqlParameter("@paysum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(paysum);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            try
            {
                returnValue.Clients = Convert.ToInt32(cmd.Parameters["@clients"].Value);
            }
            catch { }
            try
            {
                returnValue.Clients_all = Convert.ToInt32(cmd.Parameters["@clients_all"].Value);
            }
            catch { }
            try
            {
                returnValue.Purchases = Convert.ToInt32(cmd.Parameters["@purchases"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value);
            }
            catch { }

            try
            {
                returnValue.Refunds = Convert.ToInt32(cmd.Parameters["@refunds"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSum = Convert.ToDecimal(cmd.Parameters["@refundsum"].Value);
            }
            catch { }
            try
            {
                returnValue.SpentSum = Convert.ToDecimal(cmd.Parameters["@spentsum"].Value);
            }
            catch { }
            try
            {
                returnValue.Charged = Convert.ToDecimal(cmd.Parameters["@charged"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.ChargeRefund = Convert.ToDecimal(cmd.Parameters["@chargerefund"].Value);
            }
            catch { }
            try
            {
                returnValue.RedeemRefund = Convert.ToDecimal(cmd.Parameters["@redeemrefund"].Value);
            }
            catch { }
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }

            try
            {
                returnValue.Paysum = Convert.ToDecimal(cmd.Parameters["@paysum"].Value);
            }
            catch { }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class Campaign
    {
        /// <summary>
        /// ID акции
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// наименование акции
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ссылка на логотип
        /// </summary>
        public string logo { get; set; }
        /// <summary>
        /// Большое Лого?
        /// </summary>
        public bool large { get; set; }
        /// <summary>
        /// ссылка на логотип партнера
        /// </summary>
        public string partnerlogo { get; set; }
        /// <summary>
        /// описание акции
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// состояние акции
        /// </summary>
        public string condition { get; set; }
        /// <summary>
        /// список тегов
        /// </summary>
        public string tagline { get; set; }
        /// <summary>
        /// Популярна?
        /// </summary>
        public bool isPopular { get; set; }
        /// <summary>
        /// Новая акция?
        /// </summary>
        public bool isNew { get; set; }
        /// <summary>
        /// список категорий
        /// </summary>
        public List<int> categoryId { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int partnerId { get; set; }
        /// <summary>
        /// Адрес интернет магазина
        /// </summary>
        public string internetShop { get; set; }
        /// <summary>
        /// Любимая акция?
        /// </summary>
        public bool isFav { get; set; }
        /// <summary>
        /// общая ссылка
        /// </summary>
        public string share_url { get; set; }
        /// <summary>
        /// дата начала
        /// </summary>
        public DateTime startDate { get; set; }
        /// <summary>
        /// дата окончания
        /// </summary>
        public DateTime endDate { get; set; }
        /// <summary>
        /// Активная?
        /// </summary>
        public bool active { get; set; }

        public Campaign() { }
        public Campaign(int Id, string Name, string Logo, bool Large, string PartnerLogo, string Description, string Condition, string Tagline, bool IsPopular, bool IsNew, int PartnerId, string InternetShop)
        {
            id = Id;
            name = Name;
            logo = Logo;
            large = Large;
            partnerlogo = PartnerLogo;
            description = Description;
            condition = Condition;
            tagline = Tagline;
            isPopular = IsPopular;
            isNew = IsNew;
            partnerId = PartnerId;
            internetShop = InternetShop;
        }
    }

    public class GetCampaignsRequest
    {
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int PartnerID { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class GetCampaignsResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Список акций
        /// </summary>
        public List<Campaign> CampaignData { get; set; }
        public GetCampaignsResponse()
        {
            CampaignData = new List<Campaign>();
        }
    }

    public class ServerGetCampaignsResponse
    {
        public GetCampaignsResponse ProcessRequest(SqlConnection cnn, GetCampaignsRequest request)
        {
            GetCampaignsResponse returnValue = new GetCampaignsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Campaigns";
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Campaign campaign = new Campaign();
                campaign.id = reader.GetInt16(0);
                if (reader.IsDBNull(1)) campaign.logo = null; else campaign.logo = reader.GetString(1);
                if (reader.IsDBNull(2)) campaign.large = false; else campaign.large = reader.GetBoolean(2);
                if (reader.IsDBNull(3)) campaign.partnerlogo = null; else campaign.partnerlogo = reader.GetString(3);
                if (reader.IsDBNull(4)) campaign.condition = null; else campaign.condition = reader.GetString(4);
                if (reader.IsDBNull(5)) campaign.tagline = null; else campaign.tagline = reader.GetString(5);
                if (reader.IsDBNull(6)) campaign.isNew = false; else campaign.isNew = reader.GetBoolean(6);
                if (reader.IsDBNull(7)) campaign.isPopular = false; else campaign.isPopular = reader.GetBoolean(7);
                if (!reader.IsDBNull(8)) campaign.partnerId = reader.GetInt16(8);
                if (!reader.IsDBNull(10)) campaign.name = reader.GetString(10);
                if (!reader.IsDBNull(11)) campaign.startDate = reader.GetDateTime(11);
                if (!reader.IsDBNull(12)) campaign.endDate = reader.GetDateTime(12);
                if (!reader.IsDBNull(13)) campaign.description = reader.GetString(13);
                if (!reader.IsDBNull(14)) campaign.active = reader.GetBoolean(14);

                campaign.isFav = false;
                campaign.share_url = "";
                returnValue.CampaignData.Add(campaign);
            }
            reader.Close();
            foreach (var c in returnValue.CampaignData)
            {
                c.categoryId = new List<int>();
                SqlCommand cmdCategory = cnn.CreateCommand();
                cmdCategory.CommandType = CommandType.StoredProcedure;
                cmdCategory.CommandText = "PartnerCategoryGet";
                cmdCategory.Parameters.AddWithValue("@partner", c.partnerId);
                cmdCategory.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmdCategory.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmdCategory.Parameters.Add("@result", SqlDbType.Int);
                cmdCategory.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                SqlDataReader readerCategory = cmdCategory.ExecuteReader();
                while (readerCategory.Read())
                {
                    if (!readerCategory.IsDBNull(0)) c.categoryId.Add(readerCategory.GetInt16(0));
                }
                readerCategory.Close();
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class GetClientInfoRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
    }

    public class GetClientInfoResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
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

    public class ServerGetClientInfoResponse
    {
        public GetClientInfoResponse ProcessRequest(SqlConnection cnn, GetClientInfoRequest request)
        {
            GetClientInfoResponse returnValue = new GetClientInfoResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientInfo";
            if (request.Phone > 0)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone);
            }
            if (request.Card > 0)
            {
                cmd.Parameters.AddWithValue("@card", request.Card);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0)) returnValue.Card = reader.GetInt64(0);
                    if (!reader.IsDBNull(1)) returnValue.Phone = reader.GetInt64(1);
                    if (!reader.IsDBNull(2)) returnValue.Surname = reader.GetString(2);
                    if (!reader.IsDBNull(3)) returnValue.Name = reader.GetString(3);
                    if (!reader.IsDBNull(4)) returnValue.Patronymic = reader.GetString(4);
                    if (!reader.IsDBNull(5)) returnValue.Email = reader.GetString(5);
                    if (!reader.IsDBNull(6)) returnValue.Birthdate = reader.GetDateTime(6);
                    if (!reader.IsDBNull(7)) returnValue.LastPurchaseDate = reader.GetDateTime(7);
                    if (!reader.IsDBNull(8)) returnValue.LastPurchaseAmount = reader.GetDecimal(8);
                    if (!reader.IsDBNull(9)) returnValue.AllowSms = reader.GetBoolean(9);
                    if (!reader.IsDBNull(10)) returnValue.AllowEmail = reader.GetBoolean(10);
                    if (!reader.IsDBNull(11)) returnValue.FullBalance = reader.GetDecimal(11);
                    if (!reader.IsDBNull(12)) returnValue.Condition = reader.GetString(12);
                    if (!reader.IsDBNull(13)) returnValue.Id = reader.GetInt32(13);
                    if (!reader.IsDBNull(14))
                    {
                        if (reader.GetBoolean(14) == false)
                        {
                            returnValue.Gender = -1;
                        }
                        else
                        {
                            returnValue.Gender = 1;
                        }
                    }
                    if (!reader.IsDBNull(15)) returnValue.PhoneValidated = reader.GetBoolean(15);
                    if (!reader.IsDBNull(16)) returnValue.EmailValidated = reader.GetBoolean(16);
                    if (!reader.IsDBNull(17)) returnValue.RegDate = reader.GetDateTime(17);
                }

                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientInfo
    {
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
    }

    public class GetClientInfoArrayResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public List<ClientInfo> Clients { get; set; }
        public GetClientInfoArrayResponse()
        {
            Clients = new List<ClientInfo>();
        }
    }

    public class ServerGetClientInfoArrayResponse
    {
        public GetClientInfoArrayResponse ProcessRequest(SqlConnection cnn, GetClientInfoRequest request)
        {
            GetClientInfoArrayResponse returnValue = new GetClientInfoArrayResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientInfoArray";
            if (request.Phone > 0)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone);
            }
            if (request.Card > 0)
            {
                cmd.Parameters.AddWithValue("@card", request.Card);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    ClientInfo client = new ClientInfo();
                    if (!reader.IsDBNull(0)) client.Card = reader.GetInt64(0);
                    if (!reader.IsDBNull(1)) client.Phone = reader.GetInt64(1);
                    if (!reader.IsDBNull(2)) client.Surname = reader.GetString(2);
                    if (!reader.IsDBNull(3)) client.Name = reader.GetString(3);
                    if (!reader.IsDBNull(4)) client.Patronymic = reader.GetString(4);
                    if (!reader.IsDBNull(5)) client.Email = reader.GetString(5);
                    if (!reader.IsDBNull(6)) client.Birthdate = reader.GetDateTime(6);
                    if (!reader.IsDBNull(7)) client.LastPurchaseDate = reader.GetDateTime(7);
                    if (!reader.IsDBNull(8)) client.LastPurchaseAmount = reader.GetDecimal(8);
                    if (!reader.IsDBNull(9)) client.AllowSms = reader.GetBoolean(9);
                    if (!reader.IsDBNull(10)) client.AllowEmail = reader.GetBoolean(10);
                    if (!reader.IsDBNull(11)) client.FullBalance = reader.GetDecimal(11);
                    if (!reader.IsDBNull(12)) client.Condition = reader.GetString(12);
                    if (!reader.IsDBNull(13)) client.Id = reader.GetInt32(13);
                    if (!reader.IsDBNull(14))
                    {
                        if (reader.GetBoolean(14) == false)
                        {
                            client.Gender = -1;
                        }
                        else
                        {
                            client.Gender = 1;
                        }
                    }
                    returnValue.Clients.Add(client);
                }
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class SetClientUpdateRequest
    {
        /// <summary>
        /// идентификатор Участника программы лояльности
        /// </summary>
        public Int32 Client { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// получать уведомления по SMS
        /// </summary>
        public bool? AllowSms { get; set; }
        /// <summary>
        /// получать уведомления по E-mail
        /// </summary>
        public bool? AllowEmail { get; set; }
        /// <summary>
        /// дата рождения
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64? Phone { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// пол (1 - муж., -1 - жен., 0 - не определен)
        /// </summary>
        public int Gender { get; set; }
    }

    public class SetClientUpdateResponse
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

    public class ServerSetClientUpdate
    {
        public SetClientUpdateResponse ProcessRequest(SqlConnection cnn, SetClientUpdateRequest request)
        {
            SetClientUpdateResponse returnValue = new SetClientUpdateResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientUpdate";
            cmd.Parameters.AddWithValue("@client", request.Client);
            if (!string.IsNullOrEmpty(request.Name))
            {
                cmd.Parameters.AddWithValue("@name", request.Name);
            }
            if (!string.IsNullOrEmpty(request.Surname))
            {
                cmd.Parameters.AddWithValue("@surname", request.Surname);
            }
            if (!string.IsNullOrEmpty(request.Patronymic))
            {
                cmd.Parameters.AddWithValue("@patronymic", request.Patronymic);
            }
            if (request.AllowSms.HasValue)
            {
                cmd.Parameters.AddWithValue("@allowsms", request.AllowSms);
            }
            if (request.AllowEmail.HasValue)
            {
                cmd.Parameters.AddWithValue("@allowemail", request.AllowEmail);
            }
            if (request.Birthdate.HasValue)
            {
                cmd.Parameters.AddWithValue("@birthdate", request.Birthdate);
            }
            if (request.Phone.HasValue)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone);
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                cmd.Parameters.AddWithValue("@email", request.Email);
            }
            if (request.Gender == -1)
            {
                cmd.Parameters.AddWithValue("@gender", 0);
            }
            else if (request.Gender == 1)
            {
                cmd.Parameters.AddWithValue("@gender", 1);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();

            return returnValue;
        }
    }

    public class CancelLastChequeRequest
    {
        /// <summary>
        /// идентификатор Партнера программы лоялности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// время чека
        /// </summary>
        public DateTime? ChequeTime { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// Код торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// номер чека
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// код терминала Торговой точки
        /// </summary>
        public string Terminal { get; set; }
    }

    public class CancelLastChequeResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public Int64 Card { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
    }

    public class ServerCancelLastCheque
    {
        public CancelLastChequeResponse ProcessRequest(SqlConnection cnn, CancelLastChequeRequest request)
        {
            CancelLastChequeResponse returnValue = new CancelLastChequeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Reverse";
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.Add("@card", SqlDbType.BigInt);
            if (request.Card > 0)
            {
                cmd.Parameters["@card"].Value = request.Card;
            }
            cmd.Parameters["@card"].Direction = ParameterDirection.InputOutput;
            if (request.ChequeTime.HasValue)
            {
                cmd.Parameters.AddWithValue("@chequetime", request.ChequeTime);
            }
            if (request.Phone > 0)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone);
            }
            if (!string.IsNullOrEmpty(request.Pos))
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            if (!string.IsNullOrEmpty(request.Number))
            {
                cmd.Parameters.AddWithValue("@number", request.Number);
            }
            if (!string.IsNullOrEmpty(request.Terminal))
            {
                cmd.Parameters.AddWithValue("@terminal", request.Terminal);
            }
            SqlParameter balance = new SqlParameter("@balance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(balance);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            if (!DBNull.Value.Equals(cmd.Parameters["@balance"].Value))
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@card"].Value))
            {
                returnValue.Card = Convert.ToInt64(cmd.Parameters["@card"].Value);
            }
            cnn.Close();

            return returnValue;
        }
    }

    public class ClientCreateRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльноси
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// дата рождения
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// получать уведомления по SMS
        /// </summary>
        public bool AllowSms { get; set; }
        /// <summary>
        /// получать уведомления по E-mail
        /// </summary>
        public bool AllowEmail { get; set; }
        /// <summary>
        /// пол (1 - муж., -1 - жен., 0 - не определен)
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// согласие на обработку персональных данных
        /// </summary>
        public bool AgreePersonalData { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string PosCode { get; set; }
        /// <summary>
        /// телефон друга/подруги для механики “Приведи друга”
        /// </summary>
        public Int64? FriendPhone { get; set; }
        /// <summary>
        /// пароль клиента
        /// </summary>
        public bool ClientSetPassword { get; set; }
        /// <summary>
        /// промокод
        /// </summary>
        public string Promocode { get; set; }
    }

    public class ClientCreateResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// идентификатор участника программы лояльности
        /// </summary>
        public int Client { get; set; }
    }

    public class ServerClientCreate
    {
        public ClientCreateResponse ProcessRequest(SqlConnection cnn, ClientCreateRequest request)
        {
            ClientCreateResponse returnValue = new ClientCreateResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientCreate";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters["@card"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters["@phone"].Direction = ParameterDirection.InputOutput;
            if (!string.IsNullOrEmpty(request.Name))
            {
                cmd.Parameters.AddWithValue("@name", request.Name);
            }
            if (!string.IsNullOrEmpty(request.Surname))
            {
                cmd.Parameters.AddWithValue("@surname", request.Surname);
            }
            if (!string.IsNullOrEmpty(request.Patronymic))
            {
                cmd.Parameters.AddWithValue("@patronymic", request.Patronymic);
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                cmd.Parameters.AddWithValue("@email", request.Email);
            }
            if (request.Birthdate.HasValue)
            {
                cmd.Parameters.AddWithValue("@birthdate", request.Birthdate);
            }
            if (request.Gender == -1)
            {
                cmd.Parameters.AddWithValue("@gender", 0);
            }
            else if (request.Gender == 1)
            {
                cmd.Parameters.AddWithValue("@gender", 1);
            }
            cmd.Parameters.AddWithValue("@allowsms", request.AllowSms);
            cmd.Parameters.AddWithValue("@allowemail", request.AllowEmail);
            cmd.Parameters.AddWithValue("@agreepersonaldata", request.AgreePersonalData);
            cmd.Parameters.AddWithValue("@poscode", request.PosCode);
            if (request.FriendPhone.HasValue)
            {
                cmd.Parameters.AddWithValue("@friend", request.FriendPhone.Value);
            }
            cmd.Parameters.AddWithValue("@clientsetpassword", request.ClientSetPassword);
            if (!string.IsNullOrEmpty(request.Promocode))
            {
                cmd.Parameters.AddWithValue("@promocode", request.Promocode);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@client", SqlDbType.Int);
            cmd.Parameters["@client"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            try
            {
                if (cmd.Parameters["@client"].Value != null)
                {
                    returnValue.Client = Convert.ToInt32(cmd.Parameters["@client"].Value);
                }
            }
            catch { }
            try
            {
                if (cmd.Parameters["@phone"].Value != null)
                {
                    returnValue.Phone = Convert.ToInt64(cmd.Parameters["@phone"].Value);
                }
            }
            catch { }
            try
            {
                if (cmd.Parameters["@card"].Value != null)
                {
                    returnValue.Card = Convert.ToInt64(cmd.Parameters["@card"].Value);
                }
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    public class Client
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string firstname { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string middlename { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string lastname { get; set; }
        /// <summary>
        /// Полу
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? birthdate { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// Есть дети?
        /// </summary>
        public bool haschildren { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public Int64 phone { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// получать уведомления по SMS
        /// </summary>
        public bool allowsms { get; set; }
        /// <summary>
        /// получать уведомления по E-mail
        /// </summary>
        public bool allowemail { get; set; }
        /// <summary>
        /// активный баланс бонусных баллов
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// получать push-уведомления
        /// </summary>
        public bool? allowpush { get; set; }
        /// <summary>
        /// сумма последней покупки
        /// </summary>
        public decimal lasturchaseamount { get; set; }
        /// <summary>
        /// дата последней покупки
        /// </summary>
        public DateTime lastpurchasedate { get; set; }
        public Client() { }
    }

    public class GetClientRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// Последняя покупка?
        /// </summary>
        public bool LastPurchase { get; set; }
    }

    public class GetClientResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Информация об участнике программы лояльности
        /// </summary>
        public Client ClientData { get; set; }
        public GetClientResponse()
        {
            ClientData = new Client();
        }
    }

    public class ServerGetClientResponse
    {
        public GetClientResponse ProcessRequest(SqlConnection cnn, GetClientRequest request)
        {
            GetClientResponse returnValue = new GetClientResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientGet";
            cmd.Parameters.AddWithValue("@id", request.ClientID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            returnValue.ClientData = null;
            if (returnValue.ErrorCode == 0)
            {
                System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Client client = new Client();
                    if (!reader.IsDBNull(0)) client.firstname = reader.GetString(0);
                    if (!reader.IsDBNull(1)) client.middlename = reader.GetString(1);
                    if (!reader.IsDBNull(2)) client.lastname = reader.GetString(2);
                    if (!reader.IsDBNull(3)) if (reader.GetBoolean(3) == false) client.gender = -1; else client.gender = 1;
                    if (!reader.IsDBNull(4)) client.birthdate = reader.GetDateTime(4);
                    if (!reader.IsDBNull(5)) client.haschildren = reader.GetBoolean(5);
                    if (!reader.IsDBNull(6)) client.description = reader.GetString(6);
                    if (!reader.IsDBNull(7)) client.phone = reader.GetInt64(7);
                    if (!reader.IsDBNull(8)) client.email = reader.GetString(8);
                    if (!reader.IsDBNull(9)) client.allowsms = reader.GetBoolean(9);
                    if (!reader.IsDBNull(10)) client.allowemail = reader.GetBoolean(10);
                    if (!reader.IsDBNull(11)) client.balance = reader.GetDecimal(11);
                    if (!reader.IsDBNull(12)) client.allowpush = reader.GetBoolean(12);
                    returnValue.ClientData = client;
                }
                reader.Close();
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientImport
    {
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? Gender { get; set; }
        public string Address { get; set; }
        public bool? HasChildren { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowEmail { get; set; }
        public DateTime RegDate { get; set; }
        public decimal TotalPurchase { get; set; }
        public string ErrorMessage { get; set; }
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

    public class ServerClientImportResponse
    {
        public ClientImportResponse ProcessRequest(SqlConnection cnn, ClientImportRequest request)
        {
            ClientImportResponse returnValue = new ClientImportResponse();
            string path = @"c:\ImportClient\";
            string fileName = "ImportClient" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".xlsx";
            string filePath = path + fileName;
            try
            {
                System.IO.File.WriteAllBytes(filePath, request.ExcelFile);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
                return returnValue;
            }
            List<ClientImport> clients = new List<ClientImport>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    ClientImport client = new ClientImport();
                    try
                    {
                        if (worksheet.Cells[i, 1].Value != null)
                        {
                            client.Card = Convert.ToInt64(worksheet.Cells[i, 1].Value);
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (worksheet.Cells[i, 2].Value != null)
                        {
                            string phone = worksheet.Cells[i, 2].Value.ToString();
                            if (phone.Length == 11)
                            {
                                phone = phone.Substring(1);
                            }
                            client.Phone = Convert.ToInt64(phone);
                        }
                        else
                        {
                            client.ErrorMessage = "Не заполнен номер телефона";
                        }
                    }
                    catch
                    {
                        client.ErrorMessage = "Не заполнен номер телефона";
                    }
                    try
                    {
                        if (worksheet.Cells[i, 3].Value != null)
                        {
                            client.Email = worksheet.Cells[i, 3].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 4].Value != null)
                        {
                            client.Surname = worksheet.Cells[i, 4].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 5].Value != null)
                        {
                            client.Name = worksheet.Cells[i, 5].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 6].Value != null)
                        {
                            client.Patronymic = worksheet.Cells[i, 6].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 7].Value != null)
                        {
                            client.BirthDate = Convert.ToDateTime(worksheet.Cells[i, 7].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 8].Value != null)
                        {
                            string gender = Convert.ToString(worksheet.Cells[i, 8].Value);
                            if (gender.ToLower().Equals("женский"))
                            {
                                client.Gender = false;
                            }
                            else if (gender.ToLower().Equals("мужской"))
                            {
                                client.Gender = true;
                            }
                        }

                        if (worksheet.Cells[i, 8].Value != null)
                        {
                            client.Gender = Convert.ToBoolean(worksheet.Cells[i, 8].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 9].Value != null)
                        {
                            client.Address = worksheet.Cells[i, 9].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 10].Value != null)
                        {
                            client.HasChildren = Convert.ToBoolean(worksheet.Cells[i, 10].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 11].Value != null)
                        {
                            client.AllowSms = Convert.ToBoolean(worksheet.Cells[i, 11].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 12].Value != null)
                        {
                            client.AllowEmail = Convert.ToBoolean(worksheet.Cells[i, 12].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 13].Value != null)
                        {
                            client.RegDate = Convert.ToDateTime(worksheet.Cells[i, 13].Value);
                        }
                        else
                        {
                            client.RegDate = DateTime.Now;
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 14].Value != null)
                        {
                            client.TotalPurchase = Convert.ToDecimal(worksheet.Cells[i, 14].Value);
                        }
                    }
                    catch { }
                    clients.Add(client);
                }
            }

            //using (var table = new DataTable())
            //{
            //    table.Columns.Add("card", typeof(Int64));
            //    table.Columns.Add("phone", typeof(Int64));
            //    table.Columns.Add("surname", typeof(string));
            //    table.Columns.Add("name", typeof(string));
            //    table.Columns.Add("patronymic", typeof(string));

            //    DataColumn column;
            //    column = new DataColumn("birthdate", typeof(DateTime));
            //    column.AllowDBNull = true;

            //    //table.Columns.Add("birthdate", typeof(DateTime));
            //    table.Columns.Add(column);

            //    table.Columns.Add("gender", typeof(bool));
            //    table.Columns.Add("address", typeof(string));
            //    table.Columns.Add("haschildren", typeof(bool));
            //    table.Columns.Add("email", typeof(string));
            //    table.Columns.Add("allowsms", typeof(bool));
            //    table.Columns.Add("allowemail", typeof(bool));
            //    table.Columns.Add("partner", typeof(Int16));
            //    table.Columns.Add("operator", typeof(Int16));
            //    table.Columns.Add("regdate", typeof(DateTime));
            //    table.Columns.Add("totalpurchase", typeof(decimal));
            //    table.Columns.Add("rownum", typeof(int));
            //    int i = 0;
            //    foreach (var c in clients)
            //    {                    
            //        DataRow row = table.NewRow();
            //        row["card"] = c.Card;
            //        row["phone"] = c.Phone;
            //        row["surname"] = c.Surname;
            //        row["name"] = c.Name;
            //        row["patronymic"] = c.Patronymic;
            //        row["birthdate"] = c.BirthDate.HasValue ? (object)c.BirthDate.Value : DBNull.Value;
            //        row["gender"] = c.Gender;
            //        row["address"] = c.Address;
            //        row["haschildren"] = c.HasChildren.HasValue ? (object)c.HasChildren.Value : DBNull.Value;
            //        row["email"] = c.Email;
            //        row["allowsms"] = c.AllowSms;
            //        row["allowemail"] = c.AllowEmail;
            //        row["partner"] = request.Partner;
            //        row["operator"] = request.Operator;
            //        row["regdate"] = c.RegDate;
            //        row["totalpurchase"] = c.TotalPurchase;
            //        row["rownum"] = i;
            //        table.Rows.Add(row);
            //        i++;
            //    }
            //    cnn.Open();
            //    SqlCommand cmd = cnn.CreateCommand();
            //    cmd = cnn.CreateCommand();
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "ClientImport";
            //    var clientsImport = new SqlParameter("@clients", SqlDbType.Structured);
            //    clientsImport.TypeName = "dbo.Clients";
            //    clientsImport.Value = table;
            //    cmd.Parameters.Add(clientsImport);
            //    cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            //    cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        //c.ErrorMessage = ex.Message;
            //        returnValue.Message = ex.Message;
            //    }
            //    //if (string.IsNullOrEmpty(c.ErrorMessage))
            //    //{
            //    //    c.ErrorMessage = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            //    //}
            //    returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            //    cnn.Close();
            //}

            foreach (var c in clients)
            {
                cnn.Open();
                SqlCommand cmd = cnn.CreateCommand();
                cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ClientImport";
                if (c.Card > 0)
                {
                    cmd.Parameters.AddWithValue("@card", c.Card);
                }
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@surname", c.Surname);
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@patronymic", c.Patronymic);
                cmd.Parameters.AddWithValue("@birthdate", c.BirthDate);
                cmd.Parameters.AddWithValue("@gender", c.Gender);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.Parameters.AddWithValue("@haschildren", c.HasChildren);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.Parameters.AddWithValue("@allowsms", c.AllowSms);
                cmd.Parameters.AddWithValue("@allowemail", c.AllowEmail);
                cmd.Parameters.AddWithValue("@partner", request.Partner);
                cmd.Parameters.AddWithValue("@operator", request.Operator);
                cmd.Parameters.AddWithValue("@regdate", c.RegDate);
                cmd.Parameters.AddWithValue("@totalpurchase", c.TotalPurchase);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    c.ErrorMessage = ex.Message;
                }
                if (string.IsNullOrEmpty(c.ErrorMessage))
                {
                    c.ErrorMessage = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                }
                cnn.Close();
            }
            returnValue.Imported = clients.Count;
            returnValue.Successful = clients.Where(c => string.IsNullOrEmpty(c.ErrorMessage)).Count();
            returnValue.Unsuccessful = clients.Where(c => !string.IsNullOrEmpty(c.ErrorMessage)).Count();
            if (returnValue.Unsuccessful > 0)
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].Value = "Номер телефона";
                    worksheet.Cells["B1"].Value = "Номер карты";
                    worksheet.Cells["C1"].Value = "Ошибка";

                    worksheet.Cells["A1:C1"].Style.WrapText = true;
                    worksheet.Cells["A1:C1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    for (int i = 1; i <= 2; i++)
                    {
                        worksheet.Column(i).Width = 20;
                    }
                    List<ClientImport> clientsError = new List<ClientImport>(clients.Where(c => c.ErrorMessage != null));
                    for (int i = 0; i < clientsError.Count; i++)
                    {
                        worksheet.Cells["A" + (i + 2).ToString()].Value = clientsError[i].Phone;
                        worksheet.Cells["A" + (i + 2).ToString()].Style.Numberformat.Format = "#";
                        worksheet.Cells["B" + (i + 2).ToString()].Value = clientsError[i].Card;
                        worksheet.Cells["B" + (i + 2).ToString()].Style.Numberformat.Format = "#";

                        worksheet.Cells["C" + (i + 2).ToString()].Value = clientsError[i].ErrorMessage;

                    }
                    returnValue.ExcelFile = package.GetAsByteArray();
                }
            }

            //cmd = cnn.CreateCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "ClientImport";
            //try
            //{
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    returnValue.Message = ex.Message;
            //    return returnValue;
            //}

            //cmd = cnn.CreateCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "ClientImportErrors";
            //SqlDataReader reader = cmd.ExecuteReader();
            //List<string> errors = new List<string>();
            //while (reader.Read())
            //{
            //    if (!reader.IsDBNull(0)) errors.Add(reader[0].ToString());
            //}
            return returnValue;
        }
    }

    public class MergeRequest
    {
        /// <summary>
        /// Номер актуальной карты
        /// </summary>
        public Int64 Active { get; set; }
        /// <summary>
        /// Номер устаревшей карты
        /// </summary>
        public Int64 Merged { get; set; }
    }

    public class MergeResponse
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

    public class ServerMergeResponse
    {
        public MergeResponse ProcessRequest(SqlConnection cnn, MergeRequest request)
        {
            MergeResponse returnValue = new MergeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[Merge]";
            cmd.Parameters.AddWithValue("@active", request.Active);
            cmd.Parameters.AddWithValue("@merged", request.Merged);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class PosStatisticsRequest
    {
        /// <summary>
        /// код Торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// начало периода
        /// </summary>
        public DateTime Start_date { get; set; }
        /// <summary>
        /// окончание периода
        /// </summary>
        public DateTime End_date { get; set; }
    }

    public class PosStatisticsResponse
    {
        /// <summary>
        /// новых клиентов?
        /// </summary>
        public Int32 Clients { get; set; }
        /// <summary>
        /// всего клиентов?
        /// </summary>
        public Int32 Clients_all { get; set; }
        /// <summary>
        /// всего покупок
        /// </summary>
        public Int32 Purchases { get; set; }
        /// <summary>
        /// сумма покупок
        /// </summary>
        public decimal PurchaseSum { get; set; }
        /// <summary>
        /// всего возратов
        /// </summary>
        public Int32 Refunds { get; set; }
        /// <summary>
        /// сумма возратов
        /// </summary>
        public decimal RefundSum { get; set; }
        /// <summary>
        /// всего потрачено клиентам
        /// </summary>
        public decimal SpentSum { get; set; }
        /// <summary>
        /// всего начислено бонусов
        /// </summary>
        public decimal Charged { get; set; }
        /// <summary>
        /// всего списано бонусов
        /// </summary>
        public decimal Redeemed { get; set; }
        /// <summary>
        /// начислено возвратов?
        /// </summary>
        public decimal ChargeRefund { get; set; }
        /// <summary>
        /// списано возвратов?
        /// </summary>
        public decimal RedeemRefund { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// сумма к оплате
        /// </summary>
        public decimal Paysum { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Покупок клиентом
        /// </summary>
        public int PurchasesClient { get; set; }
        /// <summary>
        /// Сумма покупок клиента
        /// </summary>
        public decimal PurchaseSumClient { get; set; }
        /// <summary>
        /// Возратов у клиента
        /// </summary>
        public int RefundsClient { get; set; }
        /// <summary>
        /// Сумма возвратов у клиента
        /// </summary>
        public decimal RefundSumClient { get; set; }
    }

    public class ServerPosStatisticsResponse
    {
        public PosStatisticsResponse ProcessRequest(SqlConnection cnn, PosStatisticsRequest request)
        {
            PosStatisticsResponse returnValue = new PosStatisticsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PosStatistics";
            cmd.Parameters.AddWithValue("@pos", request.Pos);
            if (request.Start_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@start_date", request.Start_date);
            }
            if (request.End_date > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@end_date", request.End_date);
            }

            cmd.Parameters.Add("@clients", SqlDbType.Int);
            cmd.Parameters["@clients"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clients_all", SqlDbType.Int);
            cmd.Parameters["@clients_all"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchases", SqlDbType.Int);
            cmd.Parameters["@purchases"].Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            //cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            SqlParameter purchasesum = new SqlParameter("@purchasesum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(purchasesum);

            //cmd.Parameters.Add("@refunds", SqlDbType.Decimal);
            //cmd.Parameters["@refunds"].Direction = ParameterDirection.Output;
            SqlParameter refunds = new SqlParameter("@refunds", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(refunds);

            //cmd.Parameters.Add("@refundsum", SqlDbType.Decimal);
            //cmd.Parameters["@refundsum"].Direction = ParameterDirection.Output;
            SqlParameter refundsum = new SqlParameter("@refundsum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(refundsum);

            //cmd.Parameters.Add("@spentsum", SqlDbType.Decimal);
            //cmd.Parameters["@spentsum"].Direction = ParameterDirection.Output;
            SqlParameter spentsum = new SqlParameter("@spentsum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(spentsum);

            //cmd.Parameters.Add("@charged", SqlDbType.Decimal);
            //cmd.Parameters["@charged"].Direction = ParameterDirection.Output;
            SqlParameter charged = new SqlParameter("@charged", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(charged);

            //cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            //cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
            SqlParameter redeemed = new SqlParameter("@redeemed", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemed);

            //cmd.Parameters.Add("@chargerefund", SqlDbType.Decimal);
            //cmd.Parameters["@chargerefund"].Direction = ParameterDirection.Output;
            SqlParameter chargerefund = new SqlParameter("@chargerefund", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(chargerefund);

            //cmd.Parameters.Add("@redeemrefund", SqlDbType.Decimal);
            //cmd.Parameters["@redeemrefund"].Direction = ParameterDirection.Output;
            SqlParameter redeemrefund = new SqlParameter("@redeemrefund", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemrefund);

            //cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            //cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            SqlParameter balance = new SqlParameter("@balance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(balance);

            //cmd.Parameters.Add("@paysum", SqlDbType.Decimal);
            //cmd.Parameters["@paysum"].Direction = ParameterDirection.Output;
            SqlParameter paysum = new SqlParameter("@paysum", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(paysum);

            cmd.Parameters.Add("@purchasesClient", SqlDbType.Int);
            cmd.Parameters["@purchasesClient"].Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@purchasesumClient", SqlDbType.Decimal);
            //cmd.Parameters["@purchasesumClient"].Direction = ParameterDirection.Output;
            SqlParameter purchasesumClient = new SqlParameter("@purchasesumClient", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(purchasesumClient);

            cmd.Parameters.Add("@refundsClient", SqlDbType.Int);
            cmd.Parameters["@refundsClient"].Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@refundsumClient", SqlDbType.Decimal);
            //cmd.Parameters["@refundsumClient"].Direction = ParameterDirection.Output;
            SqlParameter refundsumClient = new SqlParameter("@refundsumClient", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(refundsumClient);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            try
            {
                returnValue.Clients = Convert.ToInt32(cmd.Parameters["@clients"].Value);
            }
            catch { }
            try
            {
                returnValue.Clients_all = Convert.ToInt32(cmd.Parameters["@clients_all"].Value);
            }
            catch { }
            try
            {
                returnValue.Purchases = Convert.ToInt32(cmd.Parameters["@purchases"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value);
            }
            catch { }

            try
            {
                returnValue.Refunds = Convert.ToInt32(cmd.Parameters["@refunds"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSum = Convert.ToDecimal(cmd.Parameters["@refundsum"].Value);
            }
            catch { }
            try
            {
                returnValue.SpentSum = Convert.ToDecimal(cmd.Parameters["@spentsum"].Value);
            }
            catch { }
            try
            {
                returnValue.Charged = Convert.ToDecimal(cmd.Parameters["@charged"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.ChargeRefund = Convert.ToDecimal(cmd.Parameters["@chargerefund"].Value);
            }
            catch { }
            try
            {
                returnValue.RedeemRefund = Convert.ToDecimal(cmd.Parameters["@redeemrefund"].Value);
            }
            catch { }
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }

            try
            {
                returnValue.Paysum = Convert.ToDecimal(cmd.Parameters["@paysum"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchasesClient = Convert.ToInt32(cmd.Parameters["@purchasesClient"].Value);
            }
            catch { }
            try
            {
                returnValue.PurchaseSumClient = Convert.ToDecimal(cmd.Parameters["@purchasesumClient"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundsClient = Convert.ToInt32(cmd.Parameters["@refundsClient"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSumClient = Convert.ToDecimal(cmd.Parameters["@refundsumClient"].Value);
            }
            catch { }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ChequeAggregation
    {
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// начислено бонусов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// количество чеков
        /// </summary>
        public int ChequeQty { get; set; }
        /// <summary>
        /// количество чеков без возвратов
        /// </summary>
        public int ChequeQtyWithoutRefund { get; set; }
        /// <summary>
        /// период агрегирования
        /// </summary>
        public int? MonthWeekNum { get; set; }
        /// <summary>
        /// дата чека
        /// </summary>
        public DateTime ChequeDate { get; set; }
    }

    public class ChequeAggregationRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльноси
        /// </summary>
        public Int16? Partner { get; set; }
        /// <summary>
        /// код Торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTime? From { get; set; }
        /// <summary>
        /// окончание периода
        /// </summary>
        public DateTime? To { get; set; }
        /// <summary>
        /// форма агрегирования
        /// </summary>
        public Int16 Layout { get; set; }
    }

    public class ChequeAggregationResponse
    {
        public List<ChequeAggregation> ChequeInfo { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// статистика по чекам
        /// </summary>
        public ChequeAggregationResponse()
        {
            ChequeInfo = new List<ChequeAggregation>();
        }
    }

    public class ServerGetChequeAggregation
    {
        public ChequeAggregationResponse ProcessRequest(SqlConnection cnn, ChequeAggregationRequest request)
        {
            ChequeAggregationResponse returnValue = new ChequeAggregationResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChequeAggregation";
            if (request.Operator > 0)
            {
                cmd.Parameters.AddWithValue("@operator", request.Operator);
            }
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (!string.IsNullOrEmpty(request.Pos))
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            if (request.From.HasValue)
            {
                cmd.Parameters.AddWithValue("@from", request.From.Value);
            }
            if (request.To.HasValue)
            {
                cmd.Parameters.AddWithValue("@to", request.To.Value);
            }
            cmd.Parameters.AddWithValue("@layout", request.Layout);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ChequeAggregation cheque = new ChequeAggregation();
                if (!reader.IsDBNull(0))
                {
                    if (request.Layout == 2)
                    {
                        cheque.ChequeDate = reader.GetDateTime(0);
                    }
                    else
                    {
                        cheque.MonthWeekNum = reader.GetInt32(0);
                    }
                }
                if (!reader.IsDBNull(1))
                {
                    cheque.Amount = reader.GetDecimal(1);
                }
                if (!reader.IsDBNull(2))
                {
                    cheque.BonusAdded = reader.GetDecimal(2);
                }
                if (!reader.IsDBNull(3))
                {
                    cheque.BonusRedeemed = reader.GetDecimal(3);
                }
                if (!reader.IsDBNull(4))
                {
                    cheque.ChequeQty = reader.GetInt32(4);
                }
                if (!reader.IsDBNull(5))
                {
                    cheque.ChequeQtyWithoutRefund = reader.GetInt32(5);
                }
                returnValue.ChequeInfo.Add(cheque);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientAggregation
    {
        /// <summary>
        /// количество клиентов
        /// </summary>
        public int ClientQty { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// начислено бонусов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// количество чеков
        /// </summary>
        public int ChequeQty { get; set; }
        /// <summary>
        /// количество чеков без возратов
        /// </summary>
        public int ChequeQtyWithoutRefund { get; set; }
        /// <summary>
        /// форма агрегирования
        /// </summary>
        public int? MonthWeekNum { get; set; }
        /// <summary>
        /// дата регистрации
        /// </summary>
        public DateTime RegDate { get; set; }
    }

    public class ClientAggregationRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16? Partner { get; set; }
        /// <summary>
        /// код Торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// начало периода
        /// </summary>
        public DateTime? From { get; set; }
        /// <summary>
        /// окончание периода
        /// </summary>
        public DateTime? To { get; set; }
        /// <summary>
        /// форма агрегирования
        /// </summary>
        public Int16 Layout { get; set; }
    }

    public class ClientAggregationResponse
    {
        /// <summary>
        /// статистика по клиентам
        /// </summary>
        public List<ClientAggregation> ClientInfo { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public ClientAggregationResponse()
        {
            ClientInfo = new List<ClientAggregation>();
        }
    }

    public class ServerClientAggregationResponse
    {
        public ClientAggregationResponse ProcessRequest(SqlConnection cnn, ClientAggregationRequest request)
        {
            ClientAggregationResponse returnValue = new ClientAggregationResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAggregation";
            if (request.Operator > 0)
            {
                cmd.Parameters.AddWithValue("@operator", request.Operator);
            }
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (!string.IsNullOrEmpty(request.Pos))
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            if (request.From.HasValue)
            {
                cmd.Parameters.AddWithValue("@from", request.From.Value);
            }
            if (request.To.HasValue)
            {
                cmd.Parameters.AddWithValue("@to", request.To.Value);
            }
            cmd.Parameters.AddWithValue("@layout", request.Layout);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientAggregation client = new ClientAggregation();

                if (!reader.IsDBNull(0))
                {
                    if (request.Layout == 2)
                    {
                        client.RegDate = reader.GetDateTime(0);
                    }
                    else
                    {
                        client.MonthWeekNum = reader.GetInt32(0);
                    }
                }
                if (!reader.IsDBNull(1))
                {
                    client.ClientQty = reader.GetInt32(1);
                }
                if (!reader.IsDBNull(2))
                {
                    client.Amount = reader.GetDecimal(2);
                }
                if (!reader.IsDBNull(3))
                {
                    client.BonusAdded = reader.GetDecimal(3);
                }
                if (!reader.IsDBNull(4))
                {
                    client.BonusRedeemed = reader.GetDecimal(4);
                }
                if (!reader.IsDBNull(5))
                {
                    client.ChequeQty = reader.GetInt32(5);
                }
                if (!reader.IsDBNull(6))
                {
                    client.ChequeQtyWithoutRefund = reader.GetInt32(6);
                }
                returnValue.ClientInfo.Add(client);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class BonusAddRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64? Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64? Phone { get; set; }
        /// <summary>
        /// начисляемые бонусы
        /// </summary>
        public decimal Bonus { get; set; }
    }

    public class BonusAddResponse
    {
        public decimal Bonus { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerBonusAdd
    {
        public BonusAddResponse ProcessRequest(SqlConnection cnn, BonusAddRequest request)
        {
            BonusAddResponse returnValue = new BonusAddResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorBonus";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Card.HasValue)
            {
                cmd.Parameters.AddWithValue("@card", request.Card.Value);
            }
            if (request.Phone.HasValue)
            {
                cmd.Parameters.AddWithValue("@phone", request.Phone.Value);
            }
            cmd.Parameters.AddWithValue("@bonus", request.Bonus);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);

            returnValue.Bonus = request.Bonus;
            cnn.Close();
            return returnValue;
        }
    }

    public class ChequeBonus
    {
        /// <summary>
        /// ID чека
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// номер чека
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// тип операции
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Summ { get; set; }
        /// <summary>
        /// сумма скидки
        /// </summary>
        public decimal SummDiscount { get; set; }
        /// <summary>
        /// бонусы
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// оплачено бонусами
        /// </summary>
        public decimal PaidByBonus { get; set; }
        /// <summary>
        /// скидка
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльноси
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// магазин?
        /// </summary>
        public string Shop { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 CardNumber { get; set; }
        /// <summary>
        /// наименование Торговой точки
        /// </summary>
        public string PosName { get; set; }
        /// <summary>
        /// источник бонусов?
        /// </summary>
        public string BonusSource { get; set; }
        /// <summary>
        /// начислено боунсов?
        /// </summary>
        public ChequeBonus()
        {
            Number = "";
            Partner = "";
            Shop = "";
            PosName = "";
        }
        public ChequeBonus(Int32 id, string number, DateTime date, string operationtype, decimal summ, decimal summdiscount, decimal bonus, decimal paidbybonus, decimal discount, string partner, string shop, Int64 cardnumber, string bonussource)
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
            BonusSource = bonussource;
        }
    }

    public class GetChequesBonusesRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// идентификатор участника программы лояльности
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int Partner { get; set; }
        /// <summary>
        /// код Торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// страницы
        /// </summary>
        public Int16 Page { get; set; }
        /// <summary>
        /// количество строк на странице
        /// </summary>
        public Int16 PageSize { get; set; }
    }

    public class GetChequesBonusesResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// всего страницы
        /// </summary>
        public int PageCount { get; set; }
        public List<ChequeBonus> ChequeBonusData { get; set; }
        public GetChequesBonusesResponse()
        {
            ChequeBonusData = new List<ChequeBonus>();
        }
    }

    public class ServerGetChequesBonusesResponse
    {
        public GetChequesBonusesResponse ProcessRequest(SqlConnection con, GetChequesBonusesRequest request)
        {
            GetChequesBonusesResponse returnValue = new GetChequesBonusesResponse();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChequesBonuses";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }

            cmd.Parameters.AddWithValue("@page", request.Page);
            cmd.Parameters.AddWithValue("@pagesize", request.PageSize);
            if (request.Card > 0)
            {
                cmd.Parameters.AddWithValue("@card", request.Card);
            }
            if (request.Client > 0)
            {
                cmd.Parameters.AddWithValue("@client", request.Client);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("@pagecount", SqlDbType.Int);
            cmd.Parameters["@pagecount"].Direction = ParameterDirection.Output;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ChequeBonus chequeBonus = new ChequeBonus();
                if (!reader.IsDBNull(0)) chequeBonus.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) chequeBonus.Number = reader.GetString(1);
                if (!reader.IsDBNull(2)) chequeBonus.Date = reader.GetDateTime(2);
                if (!reader.IsDBNull(3))
                {
                    if (reader.GetBoolean(3) == true)
                    {
                        chequeBonus.OperationType = "Возврат";
                    }
                    else
                    {
                        chequeBonus.OperationType = "Покупка";
                    }
                }
                else
                {
                    if (!reader.IsDBNull(12)) chequeBonus.OperationType = reader.GetString(12);
                }
                if (!reader.IsDBNull(4)) chequeBonus.Summ = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) chequeBonus.Discount = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) chequeBonus.Partner = reader.GetString(6);
                if (!reader.IsDBNull(7)) chequeBonus.Shop = reader.GetString(7);
                if (!reader.IsDBNull(8)) chequeBonus.CardNumber = reader.GetInt64(8);
                if (!reader.IsDBNull(9)) chequeBonus.Bonus = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) chequeBonus.PaidByBonus = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) chequeBonus.PosName = reader.GetString(11);
                if (!reader.IsDBNull(12)) chequeBonus.BonusSource = reader.GetString(12);
                returnValue.ChequeBonusData.Add(chequeBonus);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.PageCount = Convert.ToInt32(cmd.Parameters["@pagecount"].Value);
            con.Close();
            return returnValue;
        }
    }

    public class OperatorClient
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }

    }

    public class OperatorClientRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class OperatorClientResponse
    {
        public List<OperatorClient> Clients { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public OperatorClientResponse()
        {
            Clients = new List<OperatorClient>();
        }
    }

    public class ServerOperatorClient
    {
        public OperatorClientResponse ProcessRequest(SqlConnection cnn, OperatorClientRequest request)
        {
            OperatorClientResponse returnValue = new OperatorClientResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorClients";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OperatorClient client = new OperatorClient();
                if (!reader.IsDBNull(0)) client.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) client.Name = reader.GetString(1);
                if (!reader.IsDBNull(2)) client.Surname = reader.GetString(2);
                if (!reader.IsDBNull(3)) client.Patronymic = reader.GetString(3);
                if (!reader.IsDBNull(4)) client.Phone = reader.GetInt64(4);
                returnValue.Clients.Add(client);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class BuysImport
    {
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public decimal Amount { get; set; }
    }

    public class BuysImportRequest
    {
        /// <summary>
        /// файл базы
        /// </summary>
        public byte[] ExcelFile { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльноси
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class BuysImportResult
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// начислено бонусов?
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class BuysImportResponse
    {
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

    public class ServerBuysImport
    {
        public BuysImportResponse ProcessRequest(SqlConnection cnn, BuysImportRequest request)
        {
            BuysImportResponse returnValue = new BuysImportResponse();
            string path = @"c:\ImportBuys\";
            string fileName = "ImportBuys" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".xlsx";
            string filePath = path + fileName;
            try
            {
                System.IO.File.WriteAllBytes(filePath, request.ExcelFile);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
                return returnValue;
            }
            List<BuysImport> buys = new List<BuysImport>();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    BuysImport buy = new BuysImport();
                    try
                    {
                        if (worksheet.Cells[i, 1].Value != null)
                        {
                            buy.Card = Convert.ToInt64(worksheet.Cells[i, 1].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 2].Value != null)
                        {
                            string phone = worksheet.Cells[i, 2].Value.ToString();
                            if (phone.Length == 11)
                            {
                                phone = phone.Substring(1);
                            }
                            buy.Phone = Convert.ToInt64(phone);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 3].Value != null)
                        {
                            buy.Amount = Convert.ToDecimal(worksheet.Cells[i, 3].Value);
                        }
                    }
                    catch { }
                    if ((buy.Card > 0 || buy.Phone > 0) && buy.Amount > 0)
                    {
                        buys.Add(buy);
                    }
                }
            }

            List<BuysImportResult> result = new List<BuysImportResult>();

            foreach (var b in buys)
            {
                BuysImportResult import = new BuysImportResult();
                import.Amount = b.Amount;
                import.Card = b.Card;
                import.Phone = b.Phone;
                cnn.Open();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ChequeAdd";

                if (b.Card == 0) cmd.Parameters.AddWithValue("@card", null); else cmd.Parameters.AddWithValue("@card", b.Card);
                if (b.Phone == 0) cmd.Parameters.AddWithValue("@phone", null); else cmd.Parameters.AddWithValue("@phone", b.Phone);
                cmd.Parameters.AddWithValue("@chequetime", DateTime.Now);
                cmd.Parameters.AddWithValue("@partner", request.Partner);
                cmd.Parameters.AddWithValue("@amount", b.Amount);
                cmd.Parameters.AddWithValue("@number", DateTime.Now.ToString("HHMMss"));

                cmd.Parameters.Add("@cheque", SqlDbType.Int);
                cmd.Parameters["@cheque"].Direction = ParameterDirection.Output;
                SqlParameter added = new SqlParameter("@added", SqlDbType.Decimal)
                {
                    Precision = 19,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(added);
                SqlParameter balance = new SqlParameter("@balance", SqlDbType.Decimal)
                {
                    Precision = 19,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(balance);

                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                import.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                import.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                try
                {
                    import.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
                }
                catch { }
                try
                {
                    import.Bonus = Convert.ToDecimal(cmd.Parameters["@added"].Value);
                }
                catch { }
                cmd.Dispose();
                if (request.Operator == 1)
                {
                    SqlCommand cmdLevels = cnn.CreateCommand();
                    cmdLevels.CommandType = CommandType.StoredProcedure;
                    cmdLevels.CommandText = "MRCLevelsCard";
                    if (b.Card == 0) cmdLevels.Parameters.AddWithValue("@card", null); else cmdLevels.Parameters.AddWithValue("@card", b.Card);
                    if (b.Phone == 0) cmdLevels.Parameters.AddWithValue("@phone", null); else cmdLevels.Parameters.AddWithValue("@phone", b.Phone);
                    cmdLevels.Parameters.AddWithValue("@partner", request.Partner);
                    cmdLevels.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                    cmdLevels.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                    cmdLevels.Parameters.Add("@result", SqlDbType.Int);
                    cmdLevels.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                    cmdLevels.ExecuteNonQuery();
                    import.Message = Convert.ToString(cmdLevels.Parameters["@errormessage"].Value);
                    cmdLevels.Dispose();
                }
                cnn.Close();
                result.Add(import);
            }
            using (ExcelPackage package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "Номер карты";
                worksheet.Cells["B1"].Value = "Номер телефона";
                worksheet.Cells["C1"].Value = "Сумма покупки";
                worksheet.Cells["D1"].Value = "Начислено";
                worksheet.Cells["E1"].Value = "Баланс";
                worksheet.Cells["A1:E1"].Style.WrapText = true;
                worksheet.Cells["A1:E1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A1:E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                for (int i = 1; i <= 5; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0; i < result.Count; i++)
                {
                    worksheet.Cells["A" + (i + 2).ToString()].Value = result[i].Card;
                    worksheet.Cells["A" + (i + 2).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["B" + (i + 2).ToString()].Value = result[i].Phone;
                    worksheet.Cells["B" + (i + 2).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["C" + (i + 2).ToString()].Value = result[i].Amount;
                    worksheet.Cells["C" + (i + 2).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["D" + (i + 2).ToString()].Value = result[i].Bonus;
                    worksheet.Cells["D" + (i + 2).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["E" + (i + 2).ToString()].Value = result[i].Balance;
                    worksheet.Cells["E" + (i + 2).ToString()].Style.Numberformat.Format = "0.00";
                }
                returnValue.ExcelFile = package.GetAsByteArray();
            }
            string fileNameResult = "ImportBuysResult" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".xlsx";
            string filePathResult = path + fileNameResult;
            try
            {
                System.IO.File.WriteAllBytes(filePathResult, returnValue.ExcelFile);
            }
            catch { }
            return returnValue;
        }
    }

    public class CardBonus
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// размер начисления
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// тип бонуса
        /// </summary>
        public string BonusType { get; set; }
        /// <summary>
        /// время начисления
        /// </summary>
        public DateTime BonusTime { get; set; }
    }

    public class CardBonusesRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
    }

    public class CardBonusesResponse
    {
        public List<CardBonus> CardBonuses { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public CardBonusesResponse()
        {
            CardBonuses = new List<CardBonus>();
        }
    }

    public class ServerCardBonuses
    {
        public CardBonusesResponse ProcessRequest(SqlConnection cnn, CardBonusesRequest request)
        {
            CardBonusesResponse returnValue = new CardBonusesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardBonuses";
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CardBonus bonus = new CardBonus();
                if (!reader.IsDBNull(0)) bonus.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) bonus.Bonus = reader.GetDecimal(1);
                if (!reader.IsDBNull(2)) bonus.BonusType = reader.GetString(2);
                if (!reader.IsDBNull(3)) bonus.BonusTime = reader.GetDateTime(3);
                returnValue.CardBonuses.Add(bonus);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            reader.Close();
            cnn.Close();
            return returnValue;
        }
    }

    public class FastBonusCreate
    {
        public Int64 Phone { get; set; }
        public string Name { get; set; }
        public Int64 Card { get; set; }
    }

    public class FastBonusCreateRequest
    {
        /// <summary>
        /// файл базы
        /// </summary>
        public byte[] ExcelFile { get; set; }
        /// <summary>
        /// наименование 
        /// </summary>
        public string FastBonusName { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class FastBonusCreateResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Код торговой точки?
        /// </summary>
        public Int16 MarketList { get; set; }
    }

    public class ServerFastBonusCreateResponse
    {
        public FastBonusCreateResponse ProcessRequest(SqlConnection cnn, FastBonusCreateRequest request)
        {
            FastBonusCreateResponse returnValue = new FastBonusCreateResponse();
            string path = @"c:\FastBonus\";
            string fileName = "FastBonus" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".xlsx";
            string filePath = path + fileName;
            try
            {
                System.IO.File.WriteAllBytes(filePath, request.ExcelFile);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
                return returnValue;
            }
            List<FastBonusCreate> fastBonus = new List<FastBonusCreate>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                for (int i = 2, n = worksheet.Dimension.End.Row; i <= n; i++)
                {
                    FastBonusCreate bonus = new FastBonusCreate();
                    try
                    {
                        if (worksheet.Cells[i, 1].Value != null)
                        {
                            string phone = worksheet.Cells[i, 1].Value.ToString();
                            if (phone.Length == 11)
                            {
                                phone = phone.Substring(1);
                            }
                            bonus.Phone = Convert.ToInt64(phone);
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (worksheet.Cells[i, 2].Value != null)
                        {
                            bonus.Name = worksheet.Cells[i, 2].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 3].Value != null)
                        {
                            bonus.Card = Convert.ToInt64(worksheet.Cells[i, 3].Value);
                        }
                    }
                    catch { }
                    fastBonus.Add(bonus);
                }
            }

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardListCreate";
            cnn.Open();
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@name", request.FastBonusName);
            cmd.Parameters.AddWithValue("@create", true);
            cmd.Parameters.Add("@marketlist", SqlDbType.SmallInt);
            cmd.Parameters["@marketlist"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            using (var table = new DataTable())
            {
                table.Columns.Add("Phone", typeof(Int64));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Card", typeof(Int64));

                foreach (var bonus in fastBonus)
                {
                    DataRow row = table.NewRow();
                    row["Phone"] = bonus.Phone;
                    row["Name"] = bonus.Name;
                    row["Card"] = bonus.Card;
                    table.Rows.Add(row);
                }
                var bonuses = new SqlParameter("@cardlist", SqlDbType.Structured);
                bonuses.TypeName = "dbo.FastBonusCardList";
                bonuses.Value = table;
                cmd.Parameters.Add(bonuses);
            }
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.MarketList = Convert.ToInt16(cmd.Parameters["@marketlist"].Value);
            cnn.Close();

            return returnValue;
        }
    }

    public class ClientUpdateLevel
    {
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool? Gender { get; set; }
        public string Address { get; set; }
        public bool? HasChildren { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowEmail { get; set; }
        public DateTime RegDate { get; set; }
        public decimal TotalPurchase { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ClientUpdateLevelRequest
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
        /// <summary>
        /// уровень
        /// </summary>
        public string Level { get; set; }
    }

    public class ClientUpdateLevelResponse
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

    public class ServerClientUpdateLevel
    {
        public ClientUpdateLevelResponse ProcessRequest(SqlConnection cnn, ClientUpdateLevelRequest request)
        {
            var returnValue = new ClientUpdateLevelResponse();
            string path = @"c:\ImportClient\";
            string fileName = "ImportClient" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".xlsx";
            string filePath = path + fileName;
            try
            {
                System.IO.File.WriteAllBytes(filePath, request.ExcelFile);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
                return returnValue;
            }

            List<ClientUpdateLevel> clients = new List<ClientUpdateLevel>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    ClientUpdateLevel client = new ClientUpdateLevel();
                    try
                    {
                        if (worksheet.Cells[i, 1].Value != null)
                        {
                            client.Card = Convert.ToInt64(worksheet.Cells[i, 1].Value);
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (worksheet.Cells[i, 2].Value != null)
                        {
                            string phone = worksheet.Cells[i, 2].Value.ToString();
                            if (phone.Length == 11)
                            {
                                phone = phone.Substring(1);
                            }
                            client.Phone = Convert.ToInt64(phone);
                        }
                        else
                        {
                            client.ErrorMessage = "Не заполнен номер телефона";
                        }
                    }
                    catch
                    {
                        client.ErrorMessage = "Не заполнен номер телефона";
                    }
                    try
                    {
                        if (worksheet.Cells[i, 3].Value != null)
                        {
                            client.Email = worksheet.Cells[i, 3].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 4].Value != null)
                        {
                            client.Surname = worksheet.Cells[i, 4].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 5].Value != null)
                        {
                            client.Name = worksheet.Cells[i, 5].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 6].Value != null)
                        {
                            client.Patronymic = worksheet.Cells[i, 6].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 7].Value != null)
                        {
                            client.BirthDate = Convert.ToDateTime(worksheet.Cells[i, 7].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 8].Value != null)
                        {
                            string gender = Convert.ToString(worksheet.Cells[i, 8].Value);
                            if (gender.ToLower().Equals("женский"))
                            {
                                client.Gender = false;
                            }
                            else if (gender.ToLower().Equals("мужской"))
                            {
                                client.Gender = true;
                            }
                        }

                        if (worksheet.Cells[i, 8].Value != null)
                        {
                            client.Gender = Convert.ToBoolean(worksheet.Cells[i, 8].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 9].Value != null)
                        {
                            client.Address = worksheet.Cells[i, 9].Value.ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 10].Value != null)
                        {
                            client.HasChildren = Convert.ToBoolean(worksheet.Cells[i, 10].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 11].Value != null)
                        {
                            client.AllowSms = Convert.ToBoolean(worksheet.Cells[i, 11].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 12].Value != null)
                        {
                            client.AllowEmail = Convert.ToBoolean(worksheet.Cells[i, 12].Value);
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 13].Value != null)
                        {
                            client.RegDate = Convert.ToDateTime(worksheet.Cells[i, 13].Value);
                        }
                        else
                        {
                            client.RegDate = DateTime.Now;
                        }
                    }
                    catch { }
                    try
                    {
                        if (worksheet.Cells[i, 14].Value != null)
                        {
                            client.TotalPurchase = Convert.ToDecimal(worksheet.Cells[i, 14].Value);
                        }
                    }
                    catch { }
                    clients.Add(client);
                }
            }

            foreach (var c in clients)
            {
                cnn.Open();
                SqlCommand cmd = cnn.CreateCommand();
                cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ClientUpdateLevel";
                if (c.Card > 0)
                {
                    cmd.Parameters.AddWithValue("@card", c.Card);
                }
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@partner", request.Partner);
                cmd.Parameters.AddWithValue("@operator", request.Operator);
                cmd.Parameters.AddWithValue("@level", request.Level);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    c.ErrorMessage = ex.Message;
                }
                if (string.IsNullOrEmpty(c.ErrorMessage))
                {
                    c.ErrorMessage = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                }
                cnn.Close();
            }

            return returnValue;
        }
    }

    public class ManagerLoginRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }
    }

    public class ManagerLoginResponse
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// ID торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// Код торговой точки
        /// </summary>
        public string PosCode { get; set; }
        /// <summary>
        /// роль?
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// разрешения
        /// </summary>
        public string PermissionCode { get; set; }
        /// <summary>
        /// Идентификатор партнёра по умолчанию, проставляется в случаях регистрации участника оператором
        /// </summary>
        public Int16 DefaultPartner { get; set; }
        /// <summary>
        /// Идентификатор торговой точки по умолчанию, проставляется в случаях регистрации участника оператором или партнёром
        /// </summary>
        public Int16 DefaultPos { get; set; }
        /// <summary>
        /// Код торговой точки по умолчанию, проставляется в случаях регистрации участника оператором или партнёром
        /// </summary>
        public string DefaultPosCode { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Список ролей менеджера
        /// </summary>
        public List<string> Roles { get; set; }

        public ManagerLoginResponse()
        {
            Roles = new List<string>();
        }
    }

    public class ServerManagerLogin
    {
        public ManagerLoginResponse ProcessRequest(SqlConnection cnn, ManagerLoginRequest request)
        {
            var returnValue = new ManagerLoginResponse();
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ManagerLogin";
            cmd.Parameters.AddWithValue("@login", request.Login);
            cmd.Parameters.AddWithValue("@password", request.Password);
            cmd.Parameters.Add("@operator", SqlDbType.SmallInt);
            cmd.Parameters["@operator"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@partner", SqlDbType.SmallInt);
            cmd.Parameters["@partner"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@pos", SqlDbType.SmallInt);
            cmd.Parameters["@pos"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@posCode", SqlDbType.NVarChar, 10);
            cmd.Parameters["@posCode"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@rolename", SqlDbType.NVarChar, 50);
            cmd.Parameters["@rolename"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@permissioncode", SqlDbType.NVarChar, 20);
            cmd.Parameters["@permissioncode"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@defaultpartner", SqlDbType.SmallInt);
            cmd.Parameters["@defaultpartner"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@defaultpos", SqlDbType.SmallInt);
            cmd.Parameters["@defaultpos"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@defaultposcode", SqlDbType.NVarChar, 10);
            cmd.Parameters["@defaultposcode"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            //cmd.ExecuteNonQuery();
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                try
                {
                    if (!reader.IsDBNull(0)) returnValue.Roles.Add(reader.GetString(0));
                }
                catch(Exception ex)
                {
                    Log.Error(ex, "LCManagerAPI");
                }
            }
            reader.Close();

            if (!DBNull.Value.Equals(cmd.Parameters["@operator"].Value))
            {
                returnValue.Operator = Convert.ToInt16(cmd.Parameters["@operator"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@partner"].Value))
            {
                returnValue.Partner = Convert.ToInt16(cmd.Parameters["@partner"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@pos"].Value))
            {
                returnValue.Pos = Convert.ToInt16(cmd.Parameters["@pos"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@posCode"].Value))
            {
                returnValue.PosCode = Convert.ToString(cmd.Parameters["@posCode"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@rolename"].Value))
            {
                returnValue.RoleName = Convert.ToString(cmd.Parameters["@rolename"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@permissioncode"].Value))
            {
                returnValue.PermissionCode = Convert.ToString(cmd.Parameters["@permissioncode"].Value);
            }
            if(!string.IsNullOrWhiteSpace(returnValue.RoleName) && !returnValue.Roles.Contains(returnValue.RoleName))
            {
                returnValue.Roles.Add(returnValue.RoleName);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@defaultpartner"].Value))
            {
                returnValue.Partner = Convert.ToInt16(cmd.Parameters["@defaultpartner"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@defaultpos"].Value))
            {
                returnValue.Pos = Convert.ToInt16(cmd.Parameters["@defaultpos"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@defaultposcode"].Value))
            {
                returnValue.PosCode = Convert.ToString(cmd.Parameters["@defaultposcode"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@rolename"].Value))
            {
                returnValue.RoleName = Convert.ToString(cmd.Parameters["@rolename"].Value);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);

            cnn.Close();
            return returnValue;
        }
    }

    public class ChangeClientRequest
    {
        /// <summary>
        /// информация о клиенте
        /// </summary>
        public Client ClientData { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class ChangeClientResponse
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

    public class ServerChangeClientResponse
    {
        public ChangeClientResponse ProcessRequest(SqlConnection cnn, ChangeClientRequest request)
        {
            ChangeClientResponse returnValue = new ChangeClientResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientChange";
            cmd.Parameters.AddWithValue("@client", request.ClientData.id);
            cmd.Parameters.AddWithValue("@password", request.ClientData.password);
            cmd.Parameters.AddWithValue("@surname", request.ClientData.lastname);
            cmd.Parameters.AddWithValue("@name", request.ClientData.firstname);
            cmd.Parameters.AddWithValue("@patronymic", request.ClientData.middlename);
            if (request.ClientData.gender == 1) cmd.Parameters.AddWithValue("@gender", true); else if (request.ClientData.gender == -1) cmd.Parameters.AddWithValue("@gender", false); else cmd.Parameters.AddWithValue("@gender", null);
            if (request.ClientData.birthdate > Convert.ToDateTime("1753-01-01"))
            {
                cmd.Parameters.AddWithValue("@birthdate", request.ClientData.birthdate);
            }
            cmd.Parameters.AddWithValue("@address", request.ClientData.address);
            cmd.Parameters.AddWithValue("@haschildren", request.ClientData.haschildren);
            cmd.Parameters.AddWithValue("@description", request.ClientData.description);
            cmd.Parameters.AddWithValue("@allowsms", request.ClientData.allowsms);
            cmd.Parameters.AddWithValue("@allowemail", request.ClientData.allowemail);
            cmd.Parameters.AddWithValue("@allowpush", request.ClientData.allowpush);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            if (request.Operator > 0)
            {
                cmd.Parameters.AddWithValue("@operator", request.Operator);
            }
            cmd.Parameters.AddWithValue("@email", request.ClientData.email);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ChequeMaxSumRedeemRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// сумма чека
        /// </summary>
        public decimal ChequeSum { get; set; }
    }

    public class ChequeMaxSumRedeemResponse
    {
        /// <summary>
        /// максимальная сумма списания
        /// </summary>
        public decimal MaxSum { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerChequeMaxSumRedeem
    {
        public ChequeMaxSumRedeemResponse ProcessRequest(SqlConnection cnn, ChequeMaxSumRedeemRequest request)
        {
            var returnValue = new ChequeMaxSumRedeemResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChequeAdd";
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters.AddWithValue("@chequetime", DateTime.Now);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@amount", request.ChequeSum);
            cmd.Parameters.AddWithValue("@nowrite", true);

            //cmd.Parameters.Add("@maxredeem", SqlDbType.Decimal);
            //cmd.Parameters["@maxredeem"].Direction = ParameterDirection.Output;
            SqlParameter maxRedeem = new SqlParameter("@maxredeem", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(maxRedeem);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.MaxSum = Convert.ToDecimal(cmd.Parameters["@maxredeem"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class CardBuysByMonth
    {
        /// <summary>
        /// зачисленно бонусов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// средний чек
        /// </summary>
        public decimal AvgCheque { get; set; }
        /// <summary>
        /// всего потрачено
        /// </summary>
        public decimal ChequeSum { get; set; }
        /// <summary>
        /// месяц
        /// </summary>
        public int MonthNum { get; set; }
    }

    public class Bonus
    {
        /// <summary>
        /// источник бонусов
        /// </summary>
        public string BonusSource { get; set; }
        /// <summary>
        /// дата зачисления
        /// </summary>
        public DateTime? BonusDate { get; set; }
        /// <summary>
        /// начислено баллов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано баллов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// сгоревшие баллы
        /// </summary>
        public decimal BonusBurn { get; set; }
    }

    public class OperatorClientsManagerBuys
    {
        /// <summary>
        /// ID участника
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// дата рождения
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// пол
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// тип клиента
        /// </summary>
        public string ClientType { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// уровень
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// баланс бонусных баллов
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// количество покупок
        /// </summary>
        public int BuyQty { get; set; }
        /// <summary>
        /// сумма покупок
        /// </summary>
        public decimal BuySum { get; set; }
        /// <summary>
        /// дата последней покупки
        /// </summary>
        public DateTime LastBuyDate { get; set; }
        /// <summary>
        /// сумма последней покупки
        /// </summary>
        public decimal LastBuyAmount { get; set; }
        /// <summary>
        /// количество списаний бонусных баллов
        /// </summary>
        public int BonusRedeemQty { get; set; }
        /// <summary>
        /// сумма потраченных бонусов
        /// </summary>
        public decimal BonusRedeemSum { get; set; }
        /// <summary>
        /// дата зачисления "приветственных" баллов
        /// </summary>
        public DateTime WelcomeBonusDate { get; set; }
        /// <summary>
        /// сумма "приветственых" баллов
        /// </summary>
        public decimal WelcomeBonus { get; set; }
        /// <summary>
        /// дата зачисления "промо" баллов
        /// </summary>
        public DateTime PromoBonusDate { get; set; }
        /// <summary>
        /// сумма "промо" баллов
        /// </summary>
        public decimal PromoBonus { get; set; }
        /// <summary>
        /// дата зачисления баллов "оператора"?
        /// </summary>
        public DateTime OperatorBonusDate { get; set; }
        /// <summary>
        /// сумма зачисления баллов "оператора"?
        /// </summary>
        public decimal OperatorBonus { get; set; }
        /// <summary>
        /// дата зачисления "дружеских" баллов
        /// </summary>
        public DateTime FriendBonusDate { get; set; }
        /// <summary>
        /// сумма зачисления "дружеских" баллов
        /// </summary>
        public decimal FriendBonus { get; set; }
        /// <summary>
        /// дата зачисления "поздравительных" баллов
        /// </summary>
        public DateTime BirthdayBonusDate { get; set; }
        /// <summary>
        /// сумма зачисления "поздравительных" баллов
        /// </summary>
        public decimal BirthdayBonus { get; set; }
        /// <summary>
        /// регистрация в торговой точке?
        /// </summary>
        public string PosRegister { get; set; }
        /// <summary>
        /// дата регистрации
        /// </summary>
        public DateTime DateRegister { get; set; }
        /// <summary>
        /// количество возвратов
        /// </summary>
        public int RefundQty { get; set; }
        /// <summary>
        /// сумма возвратов
        /// </summary>
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
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// начало периода
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// конец периода
        /// </summary>
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

        public Int16 Page { get; set; }
        public Int16 PageSize { get; set; }
    }

    public class OperatorClientsManagerResponse
    {
        public List<OperatorClientsManagerBuys> OperatorClients { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }

        public OperatorClientsManagerResponse()
        {
            OperatorClients = new List<OperatorClientsManagerBuys>();
        }
    }

    public class ServerOperatorClientsManager
    {
        public OperatorClientsManagerResponse ProcessRequest(SqlConnection cnn, OperatorClientsManagerRequest request)
        {
            //неизменный текст запроса
            var sql = @"
            SELECT
				cr.client,
				COALESCE(cr.surname + ' ', '') + COALESCE(cr.name + ' ', '') + COALESCE(cr.patronymic + ' ', '') AS fullname,
				cr.phone,
				COALESCE(cr.email, N'Отсутствует') AS email,
				cr.birthdate,
				CASE
					WHEN cr.gender = 1 THEN N'Мужской'
					WHEN cr.gender = 0 THEN N'Женский'
					ELSE N'Не указан'
				END AS gender,
				COALESCE(ce.name, N'Клиент') AS client_type,
				cd.number,
				COALESCE(ll.condition,(SELECT CAST(CAST(MIN(interest) AS INT) AS NVARCHAR(5)) + N' %' FROM chequerule WHERE operator = @operator), N'Отсутствует') AS level,
				cd.fullbalance,
				(SELECT COUNT(id) FROM cheque WHERE card = cd.number AND (refund = 0 OR refund IS NULL) AND (cancelled IS NULL OR cancelled = 0)) AS buyqty,
				(SELECT SUM(amount) FROM cheque WHERE card = cd.number AND (refund = 0 OR refund IS NULL) AND (cancelled IS NULL OR cancelled = 0)) AS buysum,
				(SELECT proctime FROM cheque WHERE card = cd.number AND proctime = (SELECT MAX(proctime) FROM cheque WHERE card = cd.number) AND (cancelled IS NULL OR cancelled = 0) AND id NOT IN (SELECT cheque FROM cheque WHERE refund = 1)) AS lastBuyDate,
				(SELECT amount FROM cheque WHERE card = cd.number AND proctime = (SELECT MAX(proctime) FROM cheque WHERE card = cd.number) AND (cancelled IS NULL OR cancelled = 0) AND id NOT IN (SELECT cheque FROM cheque WHERE refund = 1)) AS lastBuyAmount,
				(SELECT 
					COUNT(bs.id) 
				FROM 
					bonus AS bs 
					INNER JOIN cheque AS ce ON bs.cheque = ce.id 
				WHERE 
					bs.bonus < 0 
					AND bs.card = cd.number
					AND (ce.cancelled IS NULL OR ce.cancelled = 0)
					AND (ce.refund = 0 OR ce.refund IS NULL)
					AND ce.id NOT IN (SELECT cheque FROM cheque WHERE refund = 1)
				) AS bonusredeemqty,
				(SELECT 
					COALESCE(SUM(ABS(bonus)), 0) 
				FROM 
					bonus AS bs
					INNER JOIN cheque AS ce ON bs.cheque = ce.id
				WHERE 
					bs.bonus < 0 
					AND bs.card = cd.number
					AND (ce.cancelled IS NULL OR ce.cancelled = 0)
					AND (ce.refund = 0 OR ce.refund IS NULL)
					AND ce.id NOT IN (SELECT cheque FROM cheque WHERE refund = 1)
				) AS bonusredeemsum,
				(SELECT MAX(proctime) FROM bonus WHERE source = 4 AND card = cd.number) AS welcomeBonusDate,
				(SELECT SUM(bonus) FROM bonus WHERE source = 4 AND card = cd.number AND proctime = (SELECT MAX(proctime) FROM bonus WHERE source = 4 AND card = cd.number)) AS welcomeBonus,
				(SELECT MAX(proctime) FROM bonus WHERE source = 2 AND card = cd.number) AS promoBonusDate,
				(SELECT SUM(bonus) FROM bonus WHERE source = 2 AND card = cd.number AND proctime = (SELECT MAX(proctime) FROM bonus WHERE source = 2 AND card = cd.number)) AS promoBonus,
				(SELECT MAX(proctime) FROM bonus WHERE source = 3 AND card = cd.number) AS operatorBonusDate,
				(SELECT SUM(bonus) FROM bonus WHERE source = 3 AND card = cd.number AND proctime = (SELECT MAX(proctime) FROM bonus WHERE source = 3 AND card = cd.number)) AS operatorBonus,
				(SELECT MAX(proctime) FROM bonus WHERE source = 5 AND card = cd.number) AS friendBonusDate,
				(SELECT SUM(bonus) FROM bonus WHERE source = 5 AND card = cd.number AND proctime = (SELECT MAX(proctime) FROM bonus WHERE source = 5 AND card = cd.number)) AS friendBonus,
				(SELECT MAX(proctime) FROM bonus WHERE source = 6 AND card = cd.number) AS birthdayBonusDate,
				(SELECT SUM(bonus) FROM bonus WHERE source = 6 AND card = cd.number AND proctime = (SELECT MAX(proctime) FROM bonus WHERE source = 5 AND card = cd.number)) AS birthdayBonus,
				CASE
					WHEN cr.appdevice IS NOT NULL AND cr.pos IS NULL THEN N'Мобилка'
					WHEN cr.appdevice IS NULL AND cr.pos IS NULL THEN N'Сайт'
					WHEN cr.pos IS NOT NULL THEN (SELECT name FROM pos WHERE id = cr.pos)
				END AS posRegistrator,
				cr.regdate,
				(SELECT COUNT(id) FROM cheque WHERE card = cd.number AND refund = 1) AS refundQty,
				(SELECT COALESCE(SUM(ABS(amount)), 0) FROM cheque WHERE card = cd.number AND refund = 1) AS refund,
                ROW_NUMBER() OVER ( ORDER BY cr.regdate DESC ) AS RowNum
			FROM
				clientoperator AS cr
				INNER JOIN card AS cd ON cr.client = cd.client AND cr.operator = cd.operator
				LEFT JOIN cardtype AS ce ON ce.id = cd.type
				LEFT JOIN level AS ll ON cd.level = ll.id ";

            var sqlCount = @"
            SELECT COUNT(*) 
            FROM  
                clientoperator as cr 
				INNER JOIN card AS cd ON cr.client = cd.client AND cr.operator = cd.operator
				LEFT JOIN cardtype AS ce ON ce.id = cd.type
				LEFT JOIN level AS ll ON cd.level = ll.id ";

            var joinStr = string.Empty;

            //Формируем блок WHERE в зависимости от фильтрации в таблице на клиенте
            var whereStr = string.Empty;
            if (request.Operator != 0 && request.Partner == 0 && string.IsNullOrEmpty(request.Pos))
            {
                whereStr = @"
                WHERE
                    cr.operator = @operator ";
            }
            else if (request.Operator != 0 && request.Partner != 0 && string.IsNullOrEmpty(request.Pos))
            {
                whereStr = @"
                WHERE
                    cr.operator = @operator
                    AND cr.pos IN (SELECT id FROM pos WHERE partner = @partner) ";
            }
            else
            if (request.Operator != 0 && request.Partner != 0 && !string.IsNullOrEmpty(request.Pos))
            {
                joinStr = joinStr + "INNER JOIN pos AS ps ON cr.pos = ps.id ";
                whereStr = @"
                WHERE
                    cr.operator = @operator
                    AND ps.code = @pos ";
            }

            //Фильтр по ФИО
            if (!string.IsNullOrEmpty(request.Name))
            {
                whereStr = whereStr + @" 
                    AND (
                        COALESCE(cr.surname +' ', '') +COALESCE(cr.name + ' ', '') + COALESCE(cr.patronymic + ' ', '') like '%" + request.Name + @"' 
                        OR COALESCE(cr.surname +' ', '') +COALESCE(cr.name + ' ', '') + COALESCE(cr.patronymic + ' ', '') like '%" + request.Name + @"%' 
                        OR COALESCE(cr.surname +' ', '') +COALESCE(cr.name + ' ', '') + COALESCE(cr.patronymic + ' ', '') like '" + request.Name + @"%' 
                        OR COALESCE(cr.surname +' ', '') +COALESCE(cr.name + ' ', '') + COALESCE(cr.patronymic + ' ', '') = '" + request.Name + @"') ";
            }
            
            //Фильтр по телефону
            if (!string.IsNullOrEmpty(request.Phone))
            {
                whereStr = whereStr + " AND (cr.phone LIKE '%" + request.Phone + "%' OR cr.phone LIKE '%" + request.Phone + "' OR cr.phone LIKE '" + request.Phone + "%' OR cr.phone ='" + request.Phone + "') ";
            }

            //Фильтр по телефону
            if (!string.IsNullOrEmpty(request.Email))
            {
                whereStr = whereStr + " AND (cr.email LIKE '%" + request.Email + "%' OR cr.email LIKE '%" + request.Email + "' OR cr.email LIKE '" + request.Email + "%' OR cr.email ='" + request.Email + "') ";
            }

            //Фильтр по дате рождения
            if (!string.IsNullOrEmpty(request.Birthdate))
            {
                if (DateTime.TryParseExact(request.Birthdate, new []{"dd.MM.yyyy"}, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                {
                    whereStr = whereStr + "AND YEAR(cr.birthdate)=" + date.Year + " AND MONTH(cr.birthdate)=" + date.Month + " AND DAY(cr.birthdate)=" + date.Day + " ";
                }
            }

            //Фильтр по полу
            if (!string.IsNullOrEmpty(request.Sex))
            {
                if (request.Sex.ToLower().Contains("мужской")) whereStr = whereStr + " AND cr.gender=1 ";
                else if (request.Sex.ToLower().Contains("женский")) whereStr = whereStr + " AND cr.gender=0 ";
                else whereStr = whereStr + " AND cr.gender is NULL ";
            }

            //Фильтр по типу клиента
            if (!string.IsNullOrEmpty(request.ClientType))
            {
                whereStr = whereStr + " AND COALESCE(ce.name, N'Клиент')='" + request.ClientType.Substring(1, request.ClientType.Length - 2) + "' ";
            }

            //Фильтр по номеру карты
            if (!string.IsNullOrEmpty(request.Number))
            {
                whereStr = whereStr + " AND (cd.number LIKE '%" + request.Number + "%'OR cd.number LIKE '%" + request.Number + "' OR cd.number LIKE '" + request.Number + "%' OR cd.number ='" + request.Number + "') ";
            }

            //Фильтр по уровню
            if (!string.IsNullOrEmpty(request.Level))
            {
                whereStr = whereStr + " AND COALESCE(ll.condition,(SELECT CAST(CAST(MIN(interest) AS INT) AS NVARCHAR(5)) + N' %' FROM chequerule WHERE operator = @operator), N'Отсутствует') = '"+ request.Level.Substring(1, request.Level.Length - 2) + "' ";
            }

            //Фильтр по балансу
            if (!string.IsNullOrEmpty(request.Balance))
            {
                var values = request.Balance.Split('-');
                whereStr = whereStr + " AND cd.fullbalance>=" + values[0] + " AND cd.fullbalance<" + values[1] + " ";
            }

            sql = sql + joinStr + whereStr;
            sqlCount = sqlCount + joinStr + whereStr;

            sql = @"SELECT  * FROM (" + sql + @") AS RowConstrainedResult
				WHERE   RowNum >= @start
					AND RowNum < @length
				ORDER BY RowNum";

            sql = sql.Replace("@partner", request.Partner.ToString());
            sql = sql.Replace("@operator", request.Operator.ToString());
            sql = sql.Replace("@pos", "'" + request.Pos.ToString() + "'");
            sqlCount = sqlCount.Replace("@partner", request.Partner.ToString());
            sqlCount = sqlCount.Replace("@operator", request.Operator.ToString());
            sqlCount = sqlCount.Replace("@pos", "'"+request.Pos.ToString()+"'");

            if (request.Page == 0) request.Page++;
            sql = sql.Replace("@start", request.Page.ToString());
            sql = sql.Replace("@length", (request.PageSize + request.Page).ToString());

            OperatorClientsManagerResponse returnValue = new OperatorClientsManagerResponse();
            returnValue.ErrorCode = 0;
            returnValue.Message = string.Empty;

            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlCount;
            try
            {
                returnValue.RecordTotal = (Int32)cmd.ExecuteScalar();
                returnValue.RecordFilterd = returnValue.RecordTotal;
            }
            catch (Exception)
            {
            }

            cmd.CommandText = sql;

            //var returnValue = new OperatorClientsManagerResponse();
            //cnn.Open();
            //SqlCommand cmd = cnn.CreateCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "Clients";

            //cmd.Parameters.AddWithValue("@operator", request.Operator);
            //if (request.Partner > 0)
            //{
            //    cmd.Parameters.AddWithValue("@partner", request.Partner);
            //}
            //if (!string.IsNullOrEmpty(request.Pos))
            //{
            //    cmd.Parameters.AddWithValue("@pos", request.Pos);
            //}
            //cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            //cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("@result", SqlDbType.Int);
            //cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OperatorClientsManagerBuys clientBuys = new OperatorClientsManagerBuys();
                    if (!reader.IsDBNull(0)) clientBuys.Id = reader.GetInt32(0);
                    if (!reader.IsDBNull(1)) clientBuys.Name = reader.GetString(1);
                    if (!reader.IsDBNull(2)) clientBuys.Phone = reader.GetInt64(2);
                    if (!reader.IsDBNull(3)) clientBuys.Email = reader.GetString(3);
                    if (!reader.IsDBNull(4)) clientBuys.BirthDate = reader.GetDateTime(4);
                    if (!reader.IsDBNull(5)) clientBuys.Gender = reader.GetString(5);
                    if (!reader.IsDBNull(6)) clientBuys.ClientType = reader.GetString(6);
                    if (!reader.IsDBNull(7)) clientBuys.Card = reader.GetInt64(7);
                    if (!reader.IsDBNull(8)) clientBuys.Level = reader.GetString(8);
                    if (!reader.IsDBNull(9)) clientBuys.Balance = reader.GetDecimal(9);
                    if (!reader.IsDBNull(10)) clientBuys.BuyQty = reader.GetInt32(10);
                    if (!reader.IsDBNull(11)) clientBuys.BuySum = reader.GetDecimal(11);
                    if (!reader.IsDBNull(12)) clientBuys.LastBuyDate = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13)) clientBuys.LastBuyAmount = reader.GetDecimal(13);
                    if (!reader.IsDBNull(14)) clientBuys.BonusRedeemQty = reader.GetInt32(14);
                    if (!reader.IsDBNull(15)) clientBuys.BonusRedeemSum = reader.GetDecimal(15);
                    if (!reader.IsDBNull(16)) clientBuys.WelcomeBonusDate = reader.GetDateTime(16);
                    if (!reader.IsDBNull(17)) clientBuys.WelcomeBonus = reader.GetDecimal(17);
                    if (!reader.IsDBNull(18)) clientBuys.PromoBonusDate = reader.GetDateTime(18);
                    if (!reader.IsDBNull(19)) clientBuys.PromoBonus = reader.GetDecimal(19);
                    if (!reader.IsDBNull(20)) clientBuys.OperatorBonusDate = reader.GetDateTime(20);
                    if (!reader.IsDBNull(21)) clientBuys.OperatorBonus = reader.GetDecimal(21);
                    if (!reader.IsDBNull(22)) clientBuys.FriendBonusDate = reader.GetDateTime(22);
                    if (!reader.IsDBNull(23)) clientBuys.FriendBonus = reader.GetDecimal(23);
                    if (!reader.IsDBNull(24)) clientBuys.BirthdayBonusDate = reader.GetDateTime(24);
                    if (!reader.IsDBNull(25)) clientBuys.BirthdayBonus = reader.GetDecimal(25);
                    if (!reader.IsDBNull(26)) clientBuys.PosRegister = reader.GetString(26);
                    if (!reader.IsDBNull(27)) clientBuys.DateRegister = reader.GetDateTime(27);
                    if (!reader.IsDBNull(28)) clientBuys.RefundQty = reader.GetInt32(28);
                    if (!reader.IsDBNull(29)) clientBuys.Refund = reader.GetDecimal(29);
                    returnValue.OperatorClients.Add(clientBuys);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                returnValue.ErrorCode = 10;
                returnValue.Message = e.Message;
            }
            
            foreach (var c in returnValue.OperatorClients)
            {
                var cmdCards = cnn.CreateCommand();
                cmdCards.CommandType = CommandType.StoredProcedure;
                cmdCards.CommandText = "CardBonusesByMonth";
                cmdCards.Parameters.AddWithValue("@card", c.Card);
                cmdCards.Parameters.AddWithValue("@from", request.From);
                cmdCards.Parameters.AddWithValue("@to", request.To);
                try
                {
                    var readerClients = cmdCards.ExecuteReader();
                    while (readerClients.Read())
                    {
                        CardBuysByMonth buys = new CardBuysByMonth();
                        if (!readerClients.IsDBNull(0)) buys.BonusAdded = readerClients.GetDecimal(0);
                        if (!readerClients.IsDBNull(1)) buys.BonusRedeemed = readerClients.GetDecimal(1);
                        if (!readerClients.IsDBNull(2)) buys.AvgCheque = readerClients.GetDecimal(2);
                        if (!readerClients.IsDBNull(3)) buys.ChequeSum = readerClients.GetDecimal(3);
                        if (!readerClients.IsDBNull(4)) buys.MonthNum = readerClients.GetInt32(4);
                        c.CardBuys.Add(buys);
                    }
                    readerClients.Close();
                }
                catch (Exception e)
                {
                    returnValue.ErrorCode = 10;
                    returnValue.Message = e.Message;
                }
            }

            foreach (var c in returnValue.OperatorClients)
            {
                var cmdCheques = cnn.CreateCommand();
                cmdCheques.CommandType = CommandType.StoredProcedure;
                cmdCheques.CommandText = "Cheques";
                cmdCheques.Parameters.AddWithValue("@card", c.Card);
                cmdCheques.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmdCheques.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmdCheques.Parameters.Add("@result", SqlDbType.Int);
                cmdCheques.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                System.Data.SqlClient.SqlDataReader readerCheques = cmdCheques.ExecuteReader();
                while (readerCheques.Read())
                {
                    Cheque cheque = new Cheque();
                    cheque.Id = readerCheques.GetInt32(0);
                    if (!readerCheques.IsDBNull(1)) cheque.Number = readerCheques.GetString(1);
                    if (!readerCheques.IsDBNull(2)) cheque.Date = readerCheques.GetDateTime(2);
                    cheque.OperationType = "Покупка";
                    if (!readerCheques.IsDBNull(3)) if (readerCheques.GetBoolean(3) == true) cheque.OperationType = "Возврат";
                    if (!readerCheques.IsDBNull(4)) cheque.Summ = readerCheques.GetDecimal(4);
                    if (!readerCheques.IsDBNull(5)) cheque.Discount = readerCheques.GetDecimal(5);
                    if (!readerCheques.IsDBNull(6)) cheque.Partner = readerCheques.GetString(6);
                    if (!readerCheques.IsDBNull(11)) cheque.PosName = readerCheques.GetString(11);
                    if (!readerCheques.IsDBNull(8)) cheque.CardNumber = readerCheques.GetInt64(8);
                    if (!readerCheques.IsDBNull(9)) cheque.Bonus = readerCheques.GetDecimal(9);
                    if (!readerCheques.IsDBNull(10)) cheque.PaidByBonus = readerCheques.GetDecimal(10);
                    c.ChequeData.Add(cheque);
                }
                readerCheques.Close();
            }
            foreach (var c in returnValue.OperatorClients)
            {
                var cmdBonuses = cnn.CreateCommand();
                cmdBonuses.CommandType = CommandType.StoredProcedure;
                cmdBonuses.CommandText = "CardBonusesType";
                cmdBonuses.Parameters.AddWithValue("@card", c.Card);
                cmdBonuses.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmdBonuses.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmdBonuses.Parameters.Add("@result", SqlDbType.Int);
                cmdBonuses.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                SqlDataReader readerBonuses = cmdBonuses.ExecuteReader();
                while (readerBonuses.Read())
                {
                    Bonus bonus = new Bonus();
                    try
                    {
                        if (!readerBonuses.IsDBNull(0)) bonus.BonusSource = readerBonuses.GetString(0);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus source {0}", c.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(1)) bonus.BonusDate = readerBonuses.GetDateTime(1);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus date {0}", c.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(2)) bonus.BonusAdded = readerBonuses.GetDecimal(2);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus added {0}", c.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(3)) bonus.BonusRedeemed = readerBonuses.GetDecimal(3);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus redeemed {0}", c.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(4)) bonus.BonusBurn = readerBonuses.GetDecimal(4);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus burn {0}", c.Card);
                    }
                    c.Bonuses.Add(bonus);
                }
                readerBonuses.Close();
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class SegmentationAgeRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
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
        /// <summary>
        /// моложе 25 лет
        /// </summary>
        public int LessThen25 { get; set; }
        /// <summary>
        /// 25-35 лет
        /// </summary>
        public int More25Less35 { get; set; }
        /// <summary>
        /// 35-45 лет
        /// </summary>
        public int More35Less45 { get; set; }
        /// <summary>
        /// старше 45
        /// </summary>
        public int More45 { get; set; }
        /// <summary>
        /// возраст неизвестен
        /// </summary>
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
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerSegmentationAge
    {
        public SegmentationAgeResponse ProcessRequest(SqlConnection cnn, SegmentationAgeRequest request)
        {
            SegmentationAgeResponse returnValue = new SegmentationAgeResponse();
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAnalyticClientSegmentationAge";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }

            cmd.Parameters.AddWithValue(@"beginDate", request.BeginDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue(@"endDate", request.EndDate.ToString("yyyy-MM-dd"));

            cmd.Parameters.Add("@less25", SqlDbType.Int);
            cmd.Parameters["@less25"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@more25less35", SqlDbType.Int);
            cmd.Parameters["@more25less35"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@more35less45", SqlDbType.Int);
            cmd.Parameters["@more35less45"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@more45", SqlDbType.Int);
            cmd.Parameters["@more45"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@unknown", SqlDbType.Int);
            cmd.Parameters["@unknown"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientQty", SqlDbType.Int);
            cmd.Parameters["@clientQty"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withBirthDate", SqlDbType.Int);
            cmd.Parameters["@withBirthDate"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withoutBirthDate", SqlDbType.Int);
            cmd.Parameters["@withoutBirthDate"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();

                returnValue.LessThen25 = Convert.ToInt32(cmd.Parameters["@less25"].Value);
                returnValue.More25Less35 = Convert.ToInt32(cmd.Parameters["@more25less35"].Value);
                returnValue.More35Less45 = Convert.ToInt32(cmd.Parameters["@more35less45"].Value);
                returnValue.More45 = Convert.ToInt32(cmd.Parameters["@more45"].Value);
                returnValue.Unknown = Convert.ToInt32(cmd.Parameters["@unknown"].Value);
                returnValue.ClientQty = Convert.ToInt32(cmd.Parameters["@clientQty"].Value);
                returnValue.WithBirthDate = Convert.ToInt32(cmd.Parameters["@withBirthDate"].Value);
                returnValue.WithoutBirthDate = Convert.ToInt32(cmd.Parameters["@withoutBirthDate"].Value);
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LCManagerPartner SegmentationAge {Message}", ex.Message);
                returnValue.ErrorCode = 500;
                returnValue.Message = ex.Message;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class ServerBonuses
    {
        public BonusesResponse ProcessRequest(SqlConnection cnn, BonusesRequest request)
        {
            BonusesResponse returnValue = new BonusesResponse();
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAnalyticClientBonuses";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }

            cmd.Parameters.AddWithValue(@"beginDate", request.BeginDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue(@"endDate", request.EndDate.ToString("yyyy-MM-dd"));

            SqlParameter addedBonus = new SqlParameter("@addedBonus", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(addedBonus);

            SqlParameter avgCharge = new SqlParameter("@avgCharge", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgCharge);

            SqlParameter redeemedBonus = new SqlParameter("@redeemedBonus", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemedBonus);

            SqlParameter avgRedeem = new SqlParameter("@avgRedeem", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgRedeem);

            SqlParameter avgBalance = new SqlParameter("@avgBalance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgBalance);

            SqlParameter avgDiscount = new SqlParameter("@avgDiscount", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgDiscount);
            cmd.Parameters.Add("@addedBonusCount", SqlDbType.Int);
            cmd.Parameters["@addedBonusCount"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemedBonusCount", SqlDbType.Int);
            cmd.Parameters["@redeemedBonusCount"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientCount", SqlDbType.Int);
            cmd.Parameters["@clientCount"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();
                returnValue.AddedBonus = Convert.ToDecimal(cmd.Parameters["@addedBonus"].Value);
                returnValue.AddedBonusCount = Convert.ToInt32(cmd.Parameters["@addedBonusCount"].Value);
                returnValue.AvgCharge = Convert.ToDecimal(cmd.Parameters["@avgCharge"].Value);
                returnValue.RedeemedBonus = Convert.ToDecimal(cmd.Parameters["@redeemedBonus"].Value);
                returnValue.RedeemedBonusCount = Convert.ToInt32(cmd.Parameters["@redeemedBonusCount"].Value);
                returnValue.AvgRedeem = Convert.ToDecimal(cmd.Parameters["@avgRedeem"].Value);
                returnValue.ClientCount = Convert.ToInt32(cmd.Parameters["@clientCount"].Value);
                returnValue.AvgBalance = Convert.ToDecimal(cmd.Parameters["@avgBalance"].Value);
                returnValue.AvgDiscount = Convert.ToDecimal(cmd.Parameters["@avgDiscount"].Value);

                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                
                
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LCManagerPartner SegmentationAge {Message}", ex.Message);
                returnValue.ErrorCode = 500;
                returnValue.Message = ex.Message;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientBaseStructureRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
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

    public class ClientBaseStructureResponse
    {
        /// <summary>
        /// мужчины
        /// </summary>
        public int MenQty { get; set; }
        /// <summary>
        /// женщины
        /// </summary>
        public int WomenQty { get; set; }
        /// <summary>
        /// пол не указан
        /// </summary>
        public int UnknownGender { get; set; }
        /// <summary>
        /// клиенты с покупками
        /// </summary>
        public int ClientsWithBuys { get; set; }
        /// <summary>
        /// клиенты без покупок
        /// </summary>
        public int ClientsWithoutBuys { get; set; }
        /// <summary>
        /// клиенты с 10 покупками
        /// </summary>
        public int ClientsWithTenBuys { get; set; }
        /// <summary>
        /// клиенты с 1 покупкой
        /// </summary>
        public int ClientsWitnOneBuys { get; set; }
        /// <summary>
        /// клиенты с указанным номером телефона?
        /// </summary>
        public int ClientsWithPhone { get; set; }
        /// <summary>
        /// клиенты с указанным адресом электронной почты
        /// </summary>
        public int ClientsWithEmail { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerClientBaseStructure
    {
        public ClientBaseStructureResponse ProcessRequest(SqlConnection cnn, ClientBaseStructureRequest request)
        {
            var returnValue = new ClientBaseStructureResponse();
            cnn.Open();

            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAnalyticClientBaseStructure";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }

            cmd.Parameters.AddWithValue(@"beginDate", request.BeginDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue(@"endDate", request.EndDate.ToString("yyyy-MM-dd"));

            cmd.Parameters.Add("@menQty", SqlDbType.Int);
            cmd.Parameters["@menQty"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@womenQty", SqlDbType.Int);
            cmd.Parameters["@womenQty"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@unknownGender", SqlDbType.Int);
            cmd.Parameters["@unknownGender"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithBuys", SqlDbType.Int);
            cmd.Parameters["@clientsWithBuys"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithoutBuys", SqlDbType.Int);
            cmd.Parameters["@clientsWithoutBuys"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithTenBuys", SqlDbType.Int);
            cmd.Parameters["@clientsWithTenBuys"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithOneBuys", SqlDbType.Int);
            cmd.Parameters["@clientsWithOneBuys"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithPhone", SqlDbType.Int);
            cmd.Parameters["@clientsWithPhone"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientsWithEmail", SqlDbType.Int);
            cmd.Parameters["@clientsWithEmail"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();

                returnValue.MenQty = cmd.Parameters["@menQty"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@menQty"].Value) :0;
                returnValue.WomenQty = cmd.Parameters["@womenQty"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@womenQty"].Value) : 0;
                returnValue.UnknownGender = cmd.Parameters["@unknownGender"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@unknownGender"].Value) : 0;
                returnValue.ClientsWithBuys = cmd.Parameters["@clientsWithBuys"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithBuys"].Value) : 0;
                returnValue.ClientsWithoutBuys = cmd.Parameters["@clientsWithoutBuys"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithoutBuys"].Value) : 0;
                returnValue.ClientsWithTenBuys = cmd.Parameters["@clientsWithTenBuys"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithTenBuys"].Value) : 0;
                returnValue.ClientsWitnOneBuys = cmd.Parameters["@clientsWithOneBuys"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithOneBuys"].Value) : 0;
                returnValue.ClientsWithPhone = cmd.Parameters["@clientsWithPhone"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithPhone"].Value) : 0;
                returnValue.ClientsWithEmail = cmd.Parameters["@clientsWithEmail"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@clientsWithEmail"].Value) : 0;
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LCManagerPartner ServerClientBaseStructure {Message}", ex.Message);
                returnValue.Message = ex.Message;
                returnValue.ErrorCode = 25;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientBaseActiveRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
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
        /// <summary>
        /// покупок у мужчин
        /// </summary>
        public decimal MenBuys { get; set; }
        /// <summary>
        /// покупок у женщин
        /// </summary>
        public decimal WomenBuys { get; set; }
        /// <summary>
        /// покупок у людей без определённого пола
        /// </summary>
        public decimal UnknownGenderBuys { get; set; }
        /// <summary>
        /// повторных покупок
        /// </summary>
        public decimal RepeatedBuys { get; set; }
        /// <summary>
        /// среднее количество покупок на клиента?
        /// </summary>
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
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerClientBaseActive
    {
        public ClientBaseActiveResponse ProcessRequest(SqlConnection cnn, ClientBaseActiveRequest request)
        {
            var returnValue = new ClientBaseActiveResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAnalyticClientBaseActive";
            cmd.Parameters.AddWithValue("@operator", request.Operator);

            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }

            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }

            cmd.Parameters.AddWithValue(@"beginDate", request.BeginDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue(@"endDate", request.EndDate.ToString("yyyy-MM-dd"));

            SqlParameter menBuys = new SqlParameter("@menBuys", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(menBuys);

            SqlParameter womenBuys = new SqlParameter("@womenBuys", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(womenBuys);

            SqlParameter unknownGender = new SqlParameter("@unknownGenderBuys", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(unknownGender);

            SqlParameter repeatedBuys = new SqlParameter("@repeatedBuys", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(repeatedBuys);

            SqlParameter buysOnClient = new SqlParameter("@buysOnClient", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(buysOnClient);

            cmd.Parameters.Add("@clientActiveQty", SqlDbType.Int);
            cmd.Parameters["@clientActiveQty"].Direction = ParameterDirection.Output;
            SqlParameter gain = new SqlParameter("@gain", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(gain);

            SqlParameter avgCheque = new SqlParameter("@avgCheque", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgCheque);

            cmd.Parameters.Add("@buysWeekdays", SqlDbType.Int);
            cmd.Parameters["@buysWeekdays"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@buysWeekOff", SqlDbType.Int);
            cmd.Parameters["@buysWeekOff"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();

                returnValue.MenBuys = Convert.ToDecimal(cmd.Parameters["@menBuys"].Value);
                returnValue.WomenBuys = Convert.ToDecimal(cmd.Parameters["@womenBuys"].Value);
                returnValue.UnknownGenderBuys = Convert.ToDecimal(cmd.Parameters["@unknownGenderBuys"].Value);
                returnValue.RepeatedBuys = Convert.ToDecimal(cmd.Parameters["@repeatedBuys"].Value);
                returnValue.BuysOnClient = Convert.ToDecimal(cmd.Parameters["@buysOnClient"].Value);
                returnValue.ClientActiveQty = Convert.ToInt32(cmd.Parameters["@clientActiveQty"].Value);
                returnValue.Gain = Convert.ToDecimal(cmd.Parameters["@gain"].Value);
                returnValue.AvgCheque = Convert.ToDecimal(cmd.Parameters["@avgCheque"].Value);
                returnValue.BuysWeekdays = Convert.ToDecimal(cmd.Parameters["@buysWeekdays"].Value);
                returnValue.BuysWeekOff = Convert.ToDecimal(cmd.Parameters["@buysWeekOff"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LCManagerPartner ServerClientBaseActive");
                returnValue.ErrorCode = 500;
                returnValue.Message = ex.Message;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientAnalyticMoneyRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
    }

    public class ClientAnalyticMoneyResponse
    {
        /// <summary>
        /// с указанной датой рождения
        /// </summary>
        public int WithBirthDate { get; set; }
        /// <summary>
        /// без указания даты рождения
        /// </summary>
        public int WithoutBirthDate { get; set; }
        /// <summary>
        /// с указанным номером телефона
        /// </summary>
        public int WithPhone { get; set; }
        /// <summary>
        /// с указанным адресом электронной почты
        /// </summary>
        public int WithEmail { get; set; }
        /// <summary>
        /// с 10 покупками
        /// </summary>
        public int MoreTenBuys { get; set; }
        /// <summary>
        /// с 1 покупкой
        /// </summary>
        public int WithOneBuy { get; set; }
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
        /// <summary>
        /// начислено бонусов
        /// </summary>
        public decimal AddedBonus { get; set; }
        /// <summary>
        /// среднее начисление бонусов
        /// </summary>
        public decimal AvgCharge { get; set; }
        /// <summary>
        /// списано бонусов
        /// </summary>
        public decimal RedeemedBonus { get; set; }
        /// <summary>
        /// среднее списание бонусов
        /// </summary>
        public decimal AvgRedeem { get; set; }
        /// <summary>
        /// средний баланс
        /// </summary>
        public decimal AvgBalance { get; set; }
        /// <summary>
        /// средняя скидка
        /// </summary>
        public decimal AvgDiscount { get; set; }
        /// <summary>
        /// количество клиентов
        /// </summary>
        public int ClientQty { get; set; }
        /// <summary>
        /// количество активных клиентов
        /// </summary>
        public int ClientActiveQty { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerClientAnalyticMoney
    {
        public ClientAnalyticMoneyResponse ProcessRequest(SqlConnection cnn, ClientAnalyticMoneyRequest request)
        {
            var returnValue = new ClientAnalyticMoneyResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAnalyticMoney";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            cmd.Parameters.Add("@withBirthDate", SqlDbType.Int);
            cmd.Parameters["@withBirthDate"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withoutBirthDate", SqlDbType.Int);
            cmd.Parameters["@withoutBirthDate"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withPhone", SqlDbType.Int);
            cmd.Parameters["@withPhone"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withEmail", SqlDbType.Int);
            cmd.Parameters["@withEmail"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@moreTenBuys", SqlDbType.Int);
            cmd.Parameters["@moreTenBuys"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@withOneBuy", SqlDbType.Int);
            cmd.Parameters["@withOneBuy"].Direction = ParameterDirection.Output;

            SqlParameter gain = new SqlParameter("@gain", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(gain);

            SqlParameter avgCheque = new SqlParameter("@avgCheque", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgCheque);

            cmd.Parameters.Add("@buysWeekdays", SqlDbType.Int);
            cmd.Parameters["@buysWeekdays"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@buysWeekOff", SqlDbType.Int);
            cmd.Parameters["@buysWeekOff"].Direction = ParameterDirection.Output;

            SqlParameter addedBonus = new SqlParameter("@addedBonus", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(addedBonus);

            SqlParameter avgCharge = new SqlParameter("@avgCharge", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgCharge);

            //cmd.Parameters.Add("@redeemedBonus", SqlDbType.Decimal);
            //cmd.Parameters["@redeemedBonus"].Direction = ParameterDirection.Output;
            SqlParameter redeemedBonus = new SqlParameter("@redeemedBonus", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(redeemedBonus);

            SqlParameter avgRedeem = new SqlParameter("@avgRedeem", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgRedeem);

            SqlParameter avgBalance = new SqlParameter("@avgBalance", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgBalance);

            SqlParameter avgDiscount = new SqlParameter("@avgDiscount", SqlDbType.Decimal)
            {
                Precision = 19,
                Scale = 2,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(avgDiscount);

            cmd.Parameters.Add("@clientQty", SqlDbType.Int);
            cmd.Parameters["@clientQty"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@clientActiveQty", SqlDbType.Int);
            cmd.Parameters["@clientActiveQty"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            try
            {
                cmd.ExecuteNonQuery();

                returnValue.WithBirthDate = Convert.ToInt32(cmd.Parameters["@withBirthDate"].Value);
                returnValue.WithoutBirthDate = Convert.ToInt32(cmd.Parameters["@withoutBirthDate"].Value);
                returnValue.WithPhone = Convert.ToInt32(cmd.Parameters["@withPhone"].Value);
                returnValue.WithEmail = Convert.ToInt32(cmd.Parameters["@withEmail"].Value);
                returnValue.MoreTenBuys = Convert.ToInt32(cmd.Parameters["@moreTenBuys"].Value);
                returnValue.WithOneBuy = Convert.ToInt32(cmd.Parameters["@withOneBuy"].Value);
                returnValue.Gain = Convert.ToDecimal(cmd.Parameters["@gain"].Value);
                returnValue.AvgCheque = Convert.ToDecimal(cmd.Parameters["@avgCheque"].Value);
                returnValue.BuysWeekdays = Convert.ToDecimal(cmd.Parameters["@buysWeekdays"].Value);
                returnValue.BuysWeekOff = Convert.ToDecimal(cmd.Parameters["@buysWeekOff"].Value);
                returnValue.AddedBonus = Convert.ToDecimal(cmd.Parameters["@addedBonus"].Value);
                returnValue.AvgCharge = Convert.ToDecimal(cmd.Parameters["@avgCharge"].Value);
                returnValue.RedeemedBonus = Convert.ToDecimal(cmd.Parameters["@redeemedBonus"].Value);
                returnValue.AvgRedeem = Convert.ToDecimal(cmd.Parameters["@avgRedeem"].Value);
                returnValue.AvgBalance = Convert.ToDecimal(cmd.Parameters["@avgBalance"].Value);
                returnValue.AvgDiscount = Convert.ToDecimal(cmd.Parameters["@avgDiscount"].Value);
                returnValue.ClientQty = Convert.ToInt32(cmd.Parameters["@clientQty"].Value);
                returnValue.ClientActiveQty = Convert.ToInt32(cmd.Parameters["@clientActiveQty"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LCManagerPartner ClientAnalyticMoney {Message}", ex.Message);
                returnValue.Message = ex.Message;
                returnValue.ErrorCode = 500;
            }
            cnn.Close();
            return returnValue;
        }
    }

    public class GainPeriod
    {
        /// <summary>
        /// месяц
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// средний чек
        /// </summary>
        public decimal AvgCheque { get; set; }
        /// <summary>
        /// получено
        /// </summary>
        public decimal Gain { get; set; }
    }

    public class GainOperatorPeriodRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// дата начала
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// дата конца
        /// </summary>
        public DateTime To { get; set; }
    }

    public class GainOperatorPeriodResponse
    {
        public List<GainPeriod> GainOperatorPeriod { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public GainOperatorPeriodResponse()
        {
            GainOperatorPeriod = new List<GainPeriod>();
        }
    }

    public class ServerGainOperatorPeriod
    {
        public GainOperatorPeriodResponse ProcessRequest(SqlConnection cnn, GainOperatorPeriodRequest request)
        {
            var returnValue = new GainOperatorPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GainOperatorPeriod";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }

            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                GainPeriod gain = new GainPeriod();
                if (!reader.IsDBNull(0)) gain.Gain = reader.GetDecimal(0);
                if (!reader.IsDBNull(1)) gain.AvgCheque = reader.GetDecimal(1);
                if (!reader.IsDBNull(2)) gain.Month = reader.GetInt32(2);
                returnValue.GainOperatorPeriod.Add(gain);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class RefundPeriod
    {
        /// <summary>
        /// месяц
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// сумма возратов
        /// </summary>
        public decimal RefundSum { get; set; }
    }

    public class RefundOperatorPeriodRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// дата начала
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// дата конца
        /// </summary>
        public DateTime To { get; set; }
    }

    public class RefundOperatorPeriodResponse
    {
        /// <summary>
        /// возратов за период
        /// </summary>
        public List<RefundPeriod> RefundOperatorPeriod { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        public RefundOperatorPeriodResponse()
        {
            RefundOperatorPeriod = new List<RefundPeriod>();
        }
    }

    public class ServerRefundOperatorPeriod
    {
        public RefundOperatorPeriodResponse ProcessRequest(SqlConnection cnn, RefundOperatorPeriodRequest request)
        {
            var returnValue = new RefundOperatorPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "RefundOperatorPeriod";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                RefundPeriod refund = new RefundPeriod();
                if (!reader.IsDBNull(0)) refund.Month = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) refund.RefundSum = reader.GetDecimal(1);
                returnValue.RefundOperatorPeriod.Add(refund);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientPeriod
    {
        /// <summary>
        /// месяц
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// количество клиентов
        /// </summary>
        public int ClientQty { get; set; }
    }

    public class ClientOperatorPeriodRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public Int16 Pos { get; set; }
        /// <summary>
        /// дата начала
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// дата конца
        /// </summary>
        public DateTime To { get; set; }
    }

    public class ClientOperatorPeriodResponse
    {
        /// <summary>
        /// новых клиентов за период?
        /// </summary>
        public List<ClientPeriod> ClientOperatorPeriod { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public ClientOperatorPeriodResponse()
        {
            ClientOperatorPeriod = new List<ClientPeriod>();
        }
    }

    public class ServerClientOperatorPeriod
    {
        public ClientOperatorPeriodResponse ProcessRequest(SqlConnection cnn, ClientOperatorPeriodRequest request)
        {
            var returnValue = new ClientOperatorPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientOperatorPeriod";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }

            if (request.Pos > 0)
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientPeriod client = new ClientPeriod();
                if (!reader.IsDBNull(0)) client.Month = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) client.ClientQty = reader.GetInt32(1);
                returnValue.ClientOperatorPeriod.Add(client);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);

            cnn.Close();

            return returnValue;
        }
    }

    public class ManagerSendCodeRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

    }

    public class ManagerSendCodeResponse
    {
        /// <summary>
        /// Номер телефона пользователя, на который отправлялся проверочный код
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerManagerSendCode
    {
        public ManagerSendCodeResponse ProcessRequest(SqlConnection cnn, ManagerSendCodeRequest request)
        {
            ManagerSendCodeResponse returnValue = new ManagerSendCodeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ManagerSendCode";
            cmd.Parameters.AddWithValue("@login", request.Login);
            cmd.Parameters.Add("@phone", SqlDbType.BigInt);
            cmd.Parameters["@phone"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            try
            {
                cmd.ExecuteNonQuery();
                returnValue.Phone = Convert.ToInt64(cmd.Parameters["@phone"].Value);
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ManagerSendCode");
            }
            cnn.Close();
            return returnValue;
        }
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

    public class ServerSetManagerPasswordResponse
    {
        public SetManagerPasswordResponse ProcessRequest(SqlConnection cnn, SetManagerPasswordRequest request)
        {
            SetManagerPasswordResponse returnValue = new SetManagerPasswordResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ManagerSetPassword";
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@code", request.Code);
            cmd.Parameters.AddWithValue("@password", request.Password);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class OperatorInfoRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class OperatorInfoResponse
    {
        /// <summary>
        /// имя оператора
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }

    public class ServerOperatorInfo
    {
        public OperatorInfoResponse ProcessRequest(SqlConnection cnn, OperatorInfoRequest request)
        {
            OperatorInfoResponse returnValue = new OperatorInfoResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorInfoGet";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@operatorname", SqlDbType.NVarChar, 20);
            cmd.Parameters["@operatorname"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            try
            {
                returnValue.OperatorName = Convert.ToString(cmd.Parameters["@operatorname"].Value);
            }
            catch { }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ActivateCardRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// код подтверждения
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

    public class ActivateCardResponse
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

    public class ServerActivateCard
    {
        public ActivateCardResponse ProcessRequest(SqlConnection cnn, ActivateCardRequest request)
        {
            var returnValue = new ActivateCardResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardActivate";
            cmd.Parameters.AddWithValue("@card", request.Card);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@code", request.Code);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class Good
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public short Id{ get; set; }

        /// <summary>
        /// код товара
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// наименование
        /// </summary>
        public string Name { get; set; }
    }
    
    public class ServerOperatorGoods
    {
        public OperatorGoodsResponse ProcessRequest(SqlConnection cnn, OperatorGoodsRequest request)
        {
            OperatorGoodsResponse returnValue = new OperatorGoodsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorGetGoods";
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Good good = new Good();
                if (!reader.IsDBNull(0)) good.Code = reader.GetString(0);
                if (!reader.IsDBNull(1)) good.Name = reader.GetString(1);
                if (!reader.IsDBNull(2)) good.Id = reader.GetInt16(2);
                returnValue.OperatorGoods.Add(good);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class VerificationPromocodeRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// промокод
        /// </summary>
        public string Promocode { get; set; }
    }

    public class VerificationPromocodeResponse
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

    public class ServerVerificationPromocode
    {
        public VerificationPromocodeResponse ProcessRequest(SqlConnection cnn, VerificationPromocodeRequest request)
        {
            VerificationPromocodeResponse returnValue = new VerificationPromocodeResponse();
            if (request.Promocode.Length != 6)
            {
                returnValue.ErrorCode = 1;
                returnValue.Message = "Промокод должен состоять из 6 цифр";
            }
            return returnValue;
        }
    }
}