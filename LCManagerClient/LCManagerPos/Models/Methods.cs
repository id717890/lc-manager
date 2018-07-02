using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LCManagerPos.Models
{
    public class BalanceGetRequest
    {
        public long Card { get; set; }
        public long Phone { get; set; }
        public long PartnerID { get; set; }
        public Int16 Operator { get; set; }
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
            if(request.Operator > 0)
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
        public long Card { get; set; }
        public long Phone { get; set; }
        public int Partner { get; set; }
        public decimal Bonus { get; set; }
        public Int16 Operator { get; set; }
    }

    public class RedeemResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public decimal Bonus { get; set; }
        public decimal Balance { get; set; }
        public int BonusId { get; set; }
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
            cmd.Parameters.Add("@bonusid", SqlDbType.Int);
            cmd.Parameters["@bonusid"].Direction = ParameterDirection.Output;
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
            try
            {
                returnValue.BonusId = Convert.ToInt32(cmd.Parameters["@bonusid"].Value);
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
            if(request.ChequeTime < new DateTime(1753, 1, 1))
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
            catch(Exception ex)
            {
                returnValue.ErrorCode = 25;
                returnValue.Message = ex.Message;
                Log.Error("LCManagerPos ChequeAdd {Message}", ex.Message);
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
        public Int16 Operator { get; set; }
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
            cmd.Parameters.AddWithValue("@purchasedate", request.PurchaseDate);
            cmd.Parameters.AddWithValue("@purchasepos", request.PurchasePos);
            cmd.Parameters.AddWithValue("@purchaseterminal", request.PurchaseTerminal);
            try
            { 
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
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

    public class CancelLastChequeRequest
    {
        public Int16 Partner { get; set; }
        public Int64 Card { get; set; }
        public DateTime? ChequeTime { get; set; }
        public Int64 Phone { get; set; }
        public string Pos { get; set; }
        public string Number { get; set; }
        public string Terminal { get; set; }
        public Int16 Operator { get; set; }
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
        public bool ClientSetPassword { get; set; }
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
            cmd.Parameters.AddWithValue("@clientsetpassword", request.ClientSetPassword);
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
            cmd.Parameters.AddWithValue("@clientsetpassword", request.ClientSetPassword);
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
}