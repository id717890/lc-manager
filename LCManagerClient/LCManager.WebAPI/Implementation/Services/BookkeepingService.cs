using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LCManager.Infrastructure.Data;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using Serilog;

namespace LCManagerPartner.Implementation.Services
{
    /// <summary>
    /// Сервис для работы со Сверкой
    /// </summary>
    public class BookkeepingService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Получает всю сверку по партнерам или торговым точкам
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BookkeepingsResponse GetAllBookkeeping(BookkeepingRequest request)
        {
            BookkeepingsResponse response = new BookkeepingsResponse();
            try
            {
                var bookkeepings = new List<Bookkeeping>();
                _cnn.Open();
                var cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "TotalStatistics";

                if (request.Operator > 0) cmd.Parameters.AddWithValue("@operator", request.Operator);
                if (request.Partner > 0) cmd.Parameters.AddWithValue("@partner", request.Partner);
                if (request.Pos > 0) cmd.Parameters.AddWithValue("@pos", request.Pos);
                //if (request.Page == 0) request.Page++;

                //cmd.Parameters.AddWithValue("@start", request.Page);

                //Если start = -1 и length = -1 это значит выгрузить все без пагинации
                //if (request.Page != -1) cmd.Parameters.AddWithValue("@length", request.Page + request.PageSize);
                //else cmd.Parameters.AddWithValue("@length", request.PageSize);

                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                //cmd.Parameters.Add("@total_rows", SqlDbType.Int);
                //cmd.Parameters["@total_rows"].Direction = ParameterDirection.Output;

                ////Фильтр по дате (Верхний фильтр с диапазоном)
                //if (!string.IsNullOrEmpty(request.DateStart))
                //{
                //    if (DateTime.TryParseExact(request.DateStart, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                //    {
                //        cmd.Parameters.AddWithValue("@f_date_start", date.ToString("yyyy-MM-dd"));
                //    }
                //}

                ////Фильтр по дате (Верхний фильтр с диапазоном)
                //if (!string.IsNullOrEmpty(request.DateEnd))
                //{
                //    if (DateTime.TryParseExact(request.DateEnd, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                //    {
                //        cmd.Parameters.AddWithValue("@f_date_end", date.ToString("yyyy-MM-dd"));
                //    }
                //}

                ////Фильтр по названию
                //if (!string.IsNullOrEmpty(request.Name)) cmd.Parameters.AddWithValue("@f_name", request.Name);
                ////Фильтр по начислению
                //try { if (!string.IsNullOrEmpty(request.AddedMore)) cmd.Parameters.AddWithValue("@f_added_more", Convert.ToInt32(request.AddedMore)); } catch { }
                //try { if (!string.IsNullOrEmpty(request.AddedLess)) cmd.Parameters.AddWithValue("@f_added_less", Convert.ToInt32(request.AddedLess)); } catch { }
                ////Фильтр по списанию
                //try { if (!string.IsNullOrEmpty(request.RedeemedMore)) cmd.Parameters.AddWithValue("@f_redeemed_more", Convert.ToInt32(request.RedeemedMore)); } catch { }
                //try { if (!string.IsNullOrEmpty(request.RedeemedLess)) cmd.Parameters.AddWithValue("@f_redeemed_less", Convert.ToInt32(request.RedeemedLess)); } catch { }
                ////Фильтр по сгорело
                //try { if (!string.IsNullOrEmpty(request.BurnMore)) cmd.Parameters.AddWithValue("@f_burn_more", Convert.ToInt32(request.BurnMore)); } catch { }
                //try { if (!string.IsNullOrEmpty(request.BurnLess)) cmd.Parameters.AddWithValue("@f_burn_less", Convert.ToInt32(request.BurnLess)); } catch { }

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Bookkeeping bookkeeping = new Bookkeeping();
                    try
                    {
                        if (!reader.IsDBNull(0)) bookkeeping.Id = reader.GetInt16(0);
                        if (!reader.IsDBNull(1)) bookkeeping.Caption = reader.GetString(1);
                        if (!reader.IsDBNull(2)) bookkeeping.Purchases = reader.GetInt32(2);
                        if (!reader.IsDBNull(3)) bookkeeping.Added = reader.GetDecimal(3);
                        if (!reader.IsDBNull(4)) bookkeeping.Redeemed = reader.GetDecimal(4);
                        if (!reader.IsDBNull(5)) bookkeeping.Clients = reader.GetInt32(5);

                        if (!reader.IsDBNull(6)) bookkeeping.PurchasesMonth1 = reader.GetInt32(6);
                        if (!reader.IsDBNull(7)) bookkeeping.AddedMonth1 = reader.GetDecimal(7);
                        if (!reader.IsDBNull(8)) bookkeeping.RedeemedMonth1 = reader.GetDecimal(8);
                        if (!reader.IsDBNull(9)) bookkeeping.ClientsMonth1 = reader.GetInt32(9);
                        if (!reader.IsDBNull(10)) bookkeeping.PurchasesMonth2 = reader.GetInt32(10);
                        if (!reader.IsDBNull(11)) bookkeeping.AddedMonth2 = reader.GetDecimal(11);
                        if (!reader.IsDBNull(12)) bookkeeping.RedeemedMonth2 = reader.GetDecimal(12);
                        if (!reader.IsDBNull(13)) bookkeeping.ClientsMonth2 = reader.GetInt32(13);
                        if (!reader.IsDBNull(14)) bookkeeping.PurchasesMonth3 = reader.GetInt32(14);
                        if (!reader.IsDBNull(15)) bookkeeping.AddedMonth3 = reader.GetDecimal(15);
                        if (!reader.IsDBNull(16)) bookkeeping.RedeemedMonth3 = reader.GetDecimal(16);
                        if (!reader.IsDBNull(17)) bookkeeping.ClientsMonth3 = reader.GetInt32(17);
                        if (!reader.IsDBNull(18)) bookkeeping.PurchasesMonth4 = reader.GetInt32(18);
                        if (!reader.IsDBNull(19)) bookkeeping.AddedMonth4 = reader.GetDecimal(19);
                        if (!reader.IsDBNull(20)) bookkeeping.RedeemedMonth4 = reader.GetDecimal(20);
                        if (!reader.IsDBNull(21)) bookkeeping.ClientsMonth4 = reader.GetInt32(21);
                        if (!reader.IsDBNull(22)) bookkeeping.PurchasesMonth5 = reader.GetInt32(22);
                        if (!reader.IsDBNull(23)) bookkeeping.AddedMonth5 = reader.GetDecimal(23);
                        if (!reader.IsDBNull(24)) bookkeeping.RedeemedMonth5 = reader.GetDecimal(24);
                        if (!reader.IsDBNull(25)) bookkeeping.ClientsMonth5 = reader.GetInt32(25);
                        if (!reader.IsDBNull(26)) bookkeeping.PurchasesMonth6 = reader.GetInt32(26);
                        if (!reader.IsDBNull(27)) bookkeeping.AddedMonth6 = reader.GetDecimal(27);
                        if (!reader.IsDBNull(28)) bookkeeping.RedeemedMonth6 = reader.GetDecimal(28);
                        if (!reader.IsDBNull(29)) bookkeeping.ClientsMonth6 = reader.GetInt32(29);
                        if (!reader.IsDBNull(30)) bookkeeping.PurchasesMonth7 = reader.GetInt32(30);
                        if (!reader.IsDBNull(31)) bookkeeping.AddedMonth7 = reader.GetDecimal(31);
                        if (!reader.IsDBNull(32)) bookkeeping.RedeemedMonth7 = reader.GetDecimal(32);
                        if (!reader.IsDBNull(33)) bookkeeping.ClientsMonth7 = reader.GetInt32(33);
                        if (!reader.IsDBNull(34)) bookkeeping.PurchasesMonth8 = reader.GetInt32(34);
                        if (!reader.IsDBNull(35)) bookkeeping.AddedMonth8 = reader.GetDecimal(35);
                        if (!reader.IsDBNull(36)) bookkeeping.RedeemedMonth8 = reader.GetDecimal(36);
                        if (!reader.IsDBNull(37)) bookkeeping.ClientsMonth8 = reader.GetInt32(37);
                        if (!reader.IsDBNull(38)) bookkeeping.PurchasesMonth9 = reader.GetInt32(38);
                        if (!reader.IsDBNull(39)) bookkeeping.AddedMonth9 = reader.GetDecimal(39);
                        if (!reader.IsDBNull(40)) bookkeeping.RedeemedMonth9 = reader.GetDecimal(40);
                        if (!reader.IsDBNull(41)) bookkeeping.ClientsMonth9 = reader.GetInt32(41);
                        if (!reader.IsDBNull(42)) bookkeeping.PurchasesMonth10 = reader.GetInt32(42);
                        if (!reader.IsDBNull(43)) bookkeeping.AddedMonth10 = reader.GetDecimal(43);
                        if (!reader.IsDBNull(44)) bookkeeping.RedeemedMonth10 = reader.GetDecimal(44);
                        if (!reader.IsDBNull(45)) bookkeeping.ClientsMonth10 = reader.GetInt32(45);
                        if (!reader.IsDBNull(46)) bookkeeping.PurchasesMonth11 = reader.GetInt32(46);
                        if (!reader.IsDBNull(47)) bookkeeping.AddedMonth11 = reader.GetDecimal(47);
                        if (!reader.IsDBNull(48)) bookkeeping.RedeemedMonth11 = reader.GetDecimal(48);
                        if (!reader.IsDBNull(49)) bookkeeping.ClientsMonth11 = reader.GetInt32(49);
                        if (!reader.IsDBNull(50)) bookkeeping.PurchasesMonth12 = reader.GetInt32(50);
                        if (!reader.IsDBNull(51)) bookkeeping.AddedMonth12 = reader.GetDecimal(51);
                        if (!reader.IsDBNull(52)) bookkeeping.RedeemedMonth12 = reader.GetDecimal(52);
                        if (!reader.IsDBNull(53)) bookkeeping.ClientsMonth12 = reader.GetInt32(53);


                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "BookkeepingService read bookkeeping operator {0}, partner {1}, pos {2}", request.Operator, request.Partner, request.Pos);
                    }
                    bookkeepings.Add(bookkeeping);
                }
                reader.Close();
                response.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                response.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                //response.RecordTotal = Convert.ToInt32(cmd.Parameters["@total_rows"].Value);
                //response.RecordFilterd = response.RecordTotal;

                response.Bookkeepings = bookkeepings;
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