using System.Globalization;

namespace LCManagerPartner.Implementation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Request;
    using Response;
    using Models;
    using Serilog;

    public class BonusService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        public BonusesNotForPurchasesResponse GetBonusesNotForPurchases(BonusesNotForPurchasesRequest request)
        {
            BonusesNotForPurchasesResponse response = new BonusesNotForPurchasesResponse();
            try
            {
                var bonuses = new List<Bonus>();
                _cnn.Open();
                var cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CardBonusesTypePaging";

                if (request.Operator > 0) cmd.Parameters.AddWithValue("@operator", request.Operator);
                if (request.Partner > 0) cmd.Parameters.AddWithValue("@partner", request.Partner);
                if (request.Pos > 0) cmd.Parameters.AddWithValue("@pos", request.Pos);
                if (request.Card > 0) cmd.Parameters.AddWithValue("@card", request.Card);
                if (request.Page == 0) request.Page++;
                cmd.Parameters.AddWithValue("@start", request.Page);
                cmd.Parameters.AddWithValue("@length", request.Page + request.PageSize);

                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add("@total_rows", SqlDbType.Int);
                cmd.Parameters["@total_rows"].Direction = ParameterDirection.Output;

                //Фильтр по дате
                if (!string.IsNullOrEmpty(request.Date))
                {
                    if (DateTime.TryParseExact(request.Date, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        cmd.Parameters.AddWithValue("@f_date", date);
                    }
                }

                //Фильтр по названию
                if (!string.IsNullOrEmpty(request.Name)) cmd.Parameters.AddWithValue("@f_name", request.Name);
                //Фильтр по начислению
                try { if (!string.IsNullOrEmpty(request.AddedMore)) cmd.Parameters.AddWithValue("@f_added_more", Convert.ToInt32(request.AddedMore)); } catch { }
                try { if (!string.IsNullOrEmpty(request.AddedLess)) cmd.Parameters.AddWithValue("@f_added_less", Convert.ToInt32(request.AddedLess)); } catch { }
                //Фильтр по списанию
                try { if (!string.IsNullOrEmpty(request.RedeemedMore)) cmd.Parameters.AddWithValue("@f_redeemed_more", Convert.ToInt32(request.RedeemedMore)); } catch { }
                try { if (!string.IsNullOrEmpty(request.RedeemedLess)) cmd.Parameters.AddWithValue("@f_redeemed_less", Convert.ToInt32(request.RedeemedLess)); } catch { }
                //Фильтр по сгорело
                try { if (!string.IsNullOrEmpty(request.BurnMore)) cmd.Parameters.AddWithValue("@f_burn_more", Convert.ToInt32(request.BurnMore)); } catch { }
                try { if (!string.IsNullOrEmpty(request.BurnLess)) cmd.Parameters.AddWithValue("@f_burn_less", Convert.ToInt32(request.BurnLess)); } catch { }

                SqlDataReader readerBonuses = cmd.ExecuteReader();
                while (readerBonuses.Read())
                {
                    Bonus bonus = new Bonus();
                    try
                    {
                        if (!readerBonuses.IsDBNull(0)) bonus.BonusSource = readerBonuses.GetString(0);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus source {0}", request.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(1)) bonus.BonusDate = readerBonuses.GetDateTime(1);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus date {0}", request.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(2)) bonus.BonusAdded = readerBonuses.GetDecimal(2);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus added {0}", request.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(3)) bonus.BonusRedeemed = readerBonuses.GetDecimal(3);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus redeemed {0}", request.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(4)) bonus.BonusBurn = readerBonuses.GetDecimal(4);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus burn {0}", request.Card);
                    }
                    try
                    {
                        if (!readerBonuses.IsDBNull(5)) bonus.BonusCard= readerBonuses.GetInt64(5);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ServerOperatorClientsManager getting bonus burn {0}", request.Card);
                    }
            bonuses.Add(bonus);
                }
                readerBonuses.Close();
                response.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                response.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                response.RecordTotal = Convert.ToInt32(cmd.Parameters["@total_rows"].Value);
                response.RecordFilterd = response.RecordTotal;

                response.Bonuses = bonuses;
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