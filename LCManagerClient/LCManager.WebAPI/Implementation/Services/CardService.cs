using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Implementation.Services
{
    /// <summary>
    /// Сервис для работы с картами ПЛ
    /// </summary>
    public class CardService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Получает базовую инфу по карте ПЛ (статус, тип, оператор, баланс, кол-во покупок, сумма покупок, кол-во начисленных бонусов, кол-во списанных бонусов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CardInfoResponse GetCard(CardStatisticsRequest request)
        {
            CardInfoResponse response = new CardInfoResponse();
            try
            {
                _cnn.Open();
                var cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CardGet";
                if (request.Card > 0) cmd.Parameters.AddWithValue("@number", request.Card);

                //cmd.Parameters.Add("@status", SqlDbType.NVarChar, 20);
                //cmd.Parameters["@status"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@virtual", SqlDbType.Bit, 100);
                //cmd.Parameters["@virtual"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@name", SqlDbType.NVarChar, 20);
                //cmd.Parameters["@name"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@fullbalance", SqlDbType.Decimal);
                //cmd.Parameters["@fullbalance"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@balance", SqlDbType.Decimal);
                //cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@purchases", SqlDbType.Int);
                //cmd.Parameters["@purchases"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
                //cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@charged", SqlDbType.Decimal);
                //cmd.Parameters["@charged"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
                //cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0)) response.Status = reader.GetString(0);
                    if (!reader.IsDBNull(1)) response.Type = reader.GetBoolean(1);
                    if (!reader.IsDBNull(2)) response.OperatorName = reader.GetString(2);
                    if (!reader.IsDBNull(3)) response.FullBalance = reader.GetDecimal(3);
                    if (!reader.IsDBNull(4)) response.Balance = reader.GetDecimal(4);
                    if (!reader.IsDBNull(5)) response.Purchases = reader.GetInt16(5);
                    if (!reader.IsDBNull(6)) response.PurchaseSum = reader.GetDecimal(6);
                    if (!reader.IsDBNull(7)) response.Charged = reader.GetDecimal(7);
                    if (!reader.IsDBNull(8)) response.Redeemed = reader.GetDecimal(8);
                    //if (!reader.IsDBNull(14))
                    //{
                    //    if (reader.GetBoolean(14) == false)
                    //    {
                    //        returnValue.Gender = -1;
                    //    }
                    //    else
                    //    {
                    //        returnValue.Gender = 1;
                    //    }
                    //}
                }



                //try { response.Status = Convert.ToString(cmd.Parameters["@status"].Value); } catch { }
                //try { response.OperatorName = Convert.ToString(cmd.Parameters["@name"].Value); } catch { }
                //try { response.Type = Convert.ToString(cmd.Parameters["@virtual"].Value); } catch { }
                //try { response.FullBalance = Convert.ToDecimal(cmd.Parameters["@fullbalance"].Value); } catch { }
                //try { response.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value); } catch { }
                //try { response.Purchases = Convert.ToInt32(cmd.Parameters["@purchases"].Value); } catch { }
                //try { response.PurchaseSum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value); } catch { }
                //try { response.Charged = Convert.ToDecimal(cmd.Parameters["@charged"].Value); } catch { }
                //try { response.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value); } catch { }
                response.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                response.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception e)
            {
                response.ErrorCode = 10;
                response.Message = e.Message;
            }
            finally
            {
                _cnn.Close();
            }
            return response;
        }

    }
}