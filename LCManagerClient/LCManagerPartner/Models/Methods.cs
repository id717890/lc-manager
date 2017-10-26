using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Models
{
    public class BalanceGetRequest
    {
        public long Card { get; set; }
        public long Phone { get; set; }
        public long PartnerID { get; set; }
    }

    public class BalanceGetResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Balance { get; set; }
        public long Card { get; set; }
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
            cmd.Parameters.AddWithValue("@chequetime", request.ChequeTime);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@pos", request.POS);
            cmd.Parameters.AddWithValue("@amount", request.Amount);
            cmd.Parameters.AddWithValue("@paidbybonus", request.PaidByBonus);
            cmd.Parameters.AddWithValue("@number", request.Number);

            if (request.ItemData != null && request.ItemData.Count > 0)
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("Position", typeof(int));
                    table.Columns.Add("Code", typeof(string));
                    table.Columns.Add("Price", typeof(decimal));
                    table.Columns.Add("Quantity", typeof(decimal));
                    table.Columns.Add("Amount", typeof(decimal));
                    table.Columns.Add("paidbybonus", typeof(decimal));

                    foreach (var item in request.ItemData)
                    {
                        DataRow row = table.NewRow();
                        row["Position"] = item.Position;
                        row["Code"] = item.Code;
                        row["Price"] = item.Price;
                        row["Quantity"] = item.Quantity;
                        row["Amount"] = item.Amount;
                        row["paidbybonus"] = item.PaidByBonus;
                        table.Rows.Add(row);
                    }
                    var items = new SqlParameter("@chequeitems", SqlDbType.Structured);
                    items.TypeName = "dbo.ChequeItems";
                    items.Value = table;
                    cmd.Parameters.Add(items);
                }
            }

            cmd.Parameters.Add("@cheque", SqlDbType.Int);
            cmd.Parameters["@cheque"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@added", SqlDbType.Decimal);
            cmd.Parameters["@added"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
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
                returnValue.Bonus = Convert.ToDecimal(cmd.Parameters["@added"].Value);
            }
            catch { }
            cnn.Close();
            return returnValue;
        }
    }

    public class Pos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Boolean ShowOnSite { get; set; }
        public Boolean GivesCard { get; set; }
        public Pos() { }
        public Pos(int id, string code, string region, string city, string address, string phone, bool showonsite, bool givescard)
        {
            Id = id;
            Name = code;
            Region = region;
            City = city;
            Address = address;
            Phone = phone;
            ShowOnSite = showonsite;
            GivesCard = givescard;
        }
    }

    public class GetPosesRequest
    {
        public int PartnerID { get; set; }
    }

    public class GetPosesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
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

    public class GetChequesRequest
    {
        public Int16 Operator { get; set; }
        public int PartnerId { get; set; }
        public Int16 Pos { get; set; }
        public Int16 Page { get; set; }
        public Int16 PageSize { get; set; }
    }

    public class GetChequesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int PageCount { get; set; }
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
            GetChequesResponse returnValue = new GetChequesResponse();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Cheques";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.PartnerId);
            cmd.Parameters.AddWithValue("@pos", request.Pos);
            cmd.Parameters.AddWithValue("@page", request.Page);
            cmd.Parameters.AddWithValue("@pagesize", request.PageSize);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("@pagecount", SqlDbType.Int);
            cmd.Parameters["@pagecount"].Direction = ParameterDirection.Output;
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
                returnValue.ChequeData.Add(cheque);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.PageCount = Convert.ToInt32(cmd.Parameters["@pagecount"].Value);
            con.Close();
            return returnValue;
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
        public long Phone { get; set; }
        public string Code { get; set; }
    }

    public class GetConfirmCodeResponse
    {
        public int ErrorCode { get; set; }
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
        public long Phone { get; set; }
        public long Card { get; set; }
        public int PartnerID { get; set; }
        public string PosCode { get; set; }
        public string AgreePersonalData { get; set; }
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
            cmd.ExecuteNonQuery();
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
        public long Phone { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
    }

    public class GetSendVerificationCodeResponse
    {
        public int ErrorCode { get; set; }
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
        public Int64 Login { get; set; }
        public string Password { get; set; }
        public string IdFB { get; set; }
        public string IdOK { get; set; }
        public string IdVK { get; set; }
    }

    public class ClientLoginResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int ClientID { get; set; }
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
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int client;
                if (reader.IsDBNull(0)) client = 0; else client = reader.GetInt32(0);
                returnValue.ClientID = client;
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class SendEmailCodeRequest
    {
        public string Email { get; set; }
    }

    public class SendEmailCodeResponse
    {
        public int ErrorCode { get; set; }
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
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class ValidateEmailResponse
    {
        public int ErrorCode { get; set; }
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
        public int ClientID { get; set; }
        public Int64 Phone { get; set; }
    }

    public class AddPhoneResponse
    {
        public int ErrorCode { get; set; }
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
        public Int64 Phone { get; set; }
    }

    public class DeletePhoneResponse
    {
        public int ErrorCode { get; set; }
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
        public int ErrorCode { get; set; }
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
            cmd.Parameters.Add("@added", SqlDbType.Decimal);
            cmd.Parameters["@added"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            if (request.PurchaseId > 0)
            {
                cmd.Parameters.AddWithValue("@purchaseid", request.PurchaseId);
            }
            cmd.Parameters.AddWithValue("@purchasenumber", request.PurchaseNumber);
            if (request.PurchaseDate.HasValue)
            {
                cmd.Parameters.AddWithValue("@purchasedate", request.PurchaseDate);
            }
            cmd.Parameters.AddWithValue("@purchasepos", request.PurchasePos);
            cmd.Parameters.AddWithValue("@purchaseterminal", request.PurchaseTerminal);
            cmd.ExecuteNonQuery();
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
        public Int16 Operator { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }

    public class OperatorStatisticsResponse
    {
        public Int32 Clients { get; set; }
        public Int32 Clients_all { get; set; }
        public Int32 Purchases { get; set; }
        public decimal PurchaseSum { get; set; }
        public Int32 Refunds { get; set; }
        public decimal RefundSum { get; set; }
        public decimal SpentSum { get; set; }
        public decimal Charged { get; set; }
        public decimal Redeemed { get; set; }
        public decimal ChargeRefund { get; set; }
        public decimal RedeemRefund { get; set; }
        public decimal Balance { get; set; }
        public decimal Paysum { get; set; }
        public int ErrorCode { get; set; }
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
            cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refunds", SqlDbType.Decimal);
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
            cmd.Parameters.Add("@paysum", SqlDbType.Decimal);
            cmd.Parameters["@paysum"].Direction = ParameterDirection.Output;

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
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public bool large { get; set; }
        public string partnerlogo { get; set; }
        public string description { get; set; }
        public string condition { get; set; }
        public string tagline { get; set; }

        public bool isPopular { get; set; }
        public bool isNew { get; set; }
        public List<int> categoryId { get; set; }
        public int partnerId { get; set; }
        public string internetShop { get; set; }

        public bool isFav { get; set; }
        public string share_url { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
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
        public int PartnerID { get; set; }
        public Int16 Operator { get; set; }
    }

    public class GetCampaignsResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
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
        public int ErrorCode { get; set; }
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
        public Int32 Client { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public bool? AllowSms { get; set; }
        public bool? AllowEmail { get; set; }
        public DateTime? Birthdate { get; set; }
        public Int64? Phone { get; set; }
        public string Email { get; set; }
        public Int16 Operator { get; set; }
        public int Gender { get; set; }
    }

    public class SetClientUpdateResponse
    {
        public int ErrorCode { get; set; }
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
        public Int16 Partner { get; set; }
        public Int64 Card { get; set; }
        public DateTime? ChequeTime { get; set; }
        public Int64 Phone { get; set; }
        public string Pos { get; set; }
        public string Number { get; set; }
        public string Terminal { get; set; }
    }

    public class CancelLastChequeResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Int64 Card { get; set; }
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
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
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
    }

    public class ClientCreateResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Int64 Phone { get; set; }
        public Int64 Card { get; set; }
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

    public class GetClientRequest
    {
        public int ClientID { get; set; }
        public bool LastPurchase { get; set; }
    }

    public class GetClientResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
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
        public byte[] ExcelFile { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
    }

    public class ClientImportResponse
    {
        public int Imported { get; set; }
        public int Successful { get; set; }
        public int Unsuccessful { get; set; }
        public byte[] ExcelFile { get; set; }
        public int ErrorCode { get; set; }
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
        public Int64 Active { get; set; }
        public Int64 Merged { get; set; }
    }

    public class MergeResponse
    {
        public int ErrorCode { get; set; }
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
        public Int16 Pos { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }

    public class PosStatisticsResponse
    {
        public Int32 Clients { get; set; }
        public Int32 Clients_all { get; set; }
        public Int32 Purchases { get; set; }
        public decimal PurchaseSum { get; set; }
        public Int32 Refunds { get; set; }
        public decimal RefundSum { get; set; }
        public decimal SpentSum { get; set; }
        public decimal Charged { get; set; }
        public decimal Redeemed { get; set; }
        public decimal ChargeRefund { get; set; }
        public decimal RedeemRefund { get; set; }
        public decimal Balance { get; set; }
        public decimal Paysum { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int PurchasesClient { get; set; }
        public decimal PurchaseSumClient { get; set; }
        public int RefundsClient { get; set; }
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
            cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refunds", SqlDbType.Decimal);
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
            cmd.Parameters.Add("@paysum", SqlDbType.Decimal);
            cmd.Parameters["@paysum"].Direction = ParameterDirection.Output;

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
        public decimal Amount { get; set; }
        public decimal BonusAdded { get; set; }
        public decimal BonusRedeemed { get; set; }
        public int ChequeQty { get; set; }
        public int ChequeQtyWithoutRefund { get; set; }
        public int? MonthWeekNum { get; set; }
        public DateTime ChequeDate { get; set; }
    }

    public class ChequeAggregationRequest
    {
        public Int16 Operator { get; set; }
        public Int16? Partner { get; set; }
        public string Pos { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Int16 Layout { get; set; }
    }

    public class ChequeAggregationResponse
    {
        public List<ChequeAggregation> ChequeInfo { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
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
        public int ClientQty { get; set; }
        public decimal Amount { get; set; }
        public decimal BonusAdded { get; set; }
        public decimal BonusRedeemed { get; set; }
        public int ChequeQty { get; set; }
        public int ChequeQtyWithoutRefund { get; set; }
        public int? MonthWeekNum { get; set; }
        public DateTime RegDate { get; set; }
    }

    public class ClientAggregationRequest
    {
        public Int16 Operator { get; set; }
        public Int16? Partner { get; set; }
        public string Pos { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Int16 Layout { get; set; }
    }

    public class ClientAggregationResponse
    {
        public List<ClientAggregation> ClientInfo { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
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
        public string BonusSource { get; set; }
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
        public Int64 Card { get; set; }
        public int Client { get; set; }
        public Int16 Operator { get; set; }
        public int Partner { get; set; }
        public Int16 Pos { get; set; }
        public Int16 Page { get; set; }
        public Int16 PageSize { get; set; }
    }

    public class GetChequesBonusesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public long Phone { get; set; }
    }

    public class OperatorClientRequest
    {
        public Int16 Operator { get; set; }
    }

    public class OperatorClientResponse
    {
        public List<OperatorClient> Clients { get; set; }
        public int ErrorCode { get; set; }
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
        public byte[] ExcelFile { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
    }

    public class BuysImportResult
    {
        public Int64 Card { get; set; }
        public Int64 Phone { get; set; }
        public decimal Amount { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class BuysImportResponse
    {
        public byte[] ExcelFile { get; set; }
        public int ErrorCode { get; set; }
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
                cmd.Parameters.Add("@added", SqlDbType.Decimal);
                cmd.Parameters["@added"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@balance", SqlDbType.Decimal);
                cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
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
        public Int32 Id { get; set; }
        public decimal Bonus { get; set; }
        public string BonusType { get; set; }
        public DateTime BonusTime { get; set; }
    }

    public class CardBonusesRequest
    {
        public Int16 Operator { get; set; }
        public Int64 Card { get; set; }
    }

    public class CardBonusesResponse
    {
        public List<CardBonus> CardBonuses { get; set; }
        public int ErrorCode { get; set; }
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
        public byte[] ExcelFile { get; set; }
        public string FastBonusName { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
    }

    public class FastBonusCreateResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
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
        public byte[] ExcelFile { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Operator { get; set; }
        public string Level { get; set; }
    }

    public class ClientUpdateLevelResponse
    {
        public int ErrorCode { get; set; }
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
        public long Phone { get; set; }

        public string Password { get; set; }
    }

    public class ManagerLoginResponse
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public string PosCode { get; set; }

        public string RoleName { get; set; }

        public string PermissionCode { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }
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
            cmd.Parameters.AddWithValue("@phone", request.Phone);
            cmd.Parameters.AddWithValue("@password", request.Password);
            cmd.Parameters.Add("@operator", SqlDbType.SmallInt);
            cmd.Parameters["@operator"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@partner", SqlDbType.SmallInt);
            cmd.Parameters["@partner"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@posCode", SqlDbType.NVarChar, 10);
            cmd.Parameters["@posCode"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@rolename", SqlDbType.NVarChar, 50);
            cmd.Parameters["@rolename"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@permissioncode", SqlDbType.NVarChar, 20);
            cmd.Parameters["@permissioncode"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            if (!DBNull.Value.Equals(cmd.Parameters["@operator"].Value))
            {
                returnValue.Operator = Convert.ToInt16(cmd.Parameters["@operator"].Value);
            }
            if (!DBNull.Value.Equals(cmd.Parameters["@partner"].Value))
            {
                returnValue.Partner = Convert.ToInt16(cmd.Parameters["@partner"].Value);
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
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);

            cnn.Close();
            return returnValue;
        }
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

    public class ServerChequeMaxSumRedeem
    {
        public ChequeMaxSumRedeemResponse ProcessRequest(SqlConnection cnn, ChequeMaxSumRedeemRequest request)
        {
            var returnValue = new ChequeMaxSumRedeemResponse();
            cnn.Open();
            returnValue.MaxSum = request.ChequeSum / 10;
            cnn.Close();
            return returnValue;
        }
    }
}