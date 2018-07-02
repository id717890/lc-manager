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