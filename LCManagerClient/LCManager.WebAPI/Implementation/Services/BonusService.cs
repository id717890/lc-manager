using System.Drawing;
using System.Globalization;
using System.Linq;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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
                if (!string.IsNullOrEmpty(request.CardStr)) cmd.Parameters.AddWithValue("@f_card", request.CardStr);
                if (request.Page == 0) request.Page++;

                cmd.Parameters.AddWithValue("@start", request.Page);

                //Если start = -1 и length = -1 это значит выгрузить все без пагинации
                if (request.Page !=-1) cmd.Parameters.AddWithValue("@length", request.Page + request.PageSize);
                else cmd.Parameters.AddWithValue("@length", request.PageSize);

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

                //Фильтр по дате (Верхний фильтр с диапазоном)
                if (!string.IsNullOrEmpty(request.DateStart))
                {
                    if (DateTime.TryParseExact(request.DateStart, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        cmd.Parameters.AddWithValue("@f_date_start", date.ToString("yyyy-MM-dd"));
                    }
                }

                //Фильтр по дате (Верхний фильтр с диапазоном)
                if (!string.IsNullOrEmpty(request.DateEnd))
                {
                    if (DateTime.TryParseExact(request.DateEnd, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        cmd.Parameters.AddWithValue("@f_date_end", date.ToString("yyyy-MM-dd"));
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

        /// <summary>
        /// Отчет по бонусам не за покупки
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public LCManager.Infrastructure.Response.ReportResponse BonusesNoChequeReport(BonusesNotForPurchasesRequest request)
        {
            var response = new LCManager.Infrastructure.Response.ReportResponse();
            try
            {
                var data = this.GetBonusesNotForPurchases(request);


                //ReportResponse returnValue = new ReportResponse();
                //cnn.Open();
                //SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "Reports.OperatorSales";
                //cmd.Parameters.AddWithValue("@operator", request.Operator);
                //cmd.Parameters.AddWithValue("@from", request.From);
                //cmd.Parameters.AddWithValue("@to", request.To);

                //if (request.Partner != 0) cmd.Parameters.AddWithValue("@partner", request.Partner);
                //if (request.Pos != 0) cmd.Parameters.AddWithValue("@pos", request.Pos);
                //cmd.Parameters.AddWithValue("@f_date", request.DateBuy);
                //cmd.Parameters.AddWithValue("@f_pos", request.PosName);
                //cmd.Parameters.AddWithValue("@f_phone", request.Phone);
                //cmd.Parameters.AddWithValue("@f_operation", request.Operation);
                //cmd.Parameters.AddWithValue("@f_cheque", request.Cheque);
                //cmd.Parameters.AddWithValue("@f_sum_more", request.SumMore);
                //cmd.Parameters.AddWithValue("@f_sum_less", request.SumLess);
                //cmd.Parameters.AddWithValue("@f_charge_more", request.ChargeMore);
                //cmd.Parameters.AddWithValue("@f_charge_less", request.ChargeLess);
                //cmd.Parameters.AddWithValue("@f_redeem_more", request.RedeemMore);
                //cmd.Parameters.AddWithValue("@f_redeem_less", request.RedeemLess);

                //cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
                //cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@result", SqlDbType.Int);
                //cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                //SqlDataReader reader = cmd.ExecuteReader();
                //List<OperatorSale> operatorSales = new List<OperatorSale>();
                //while (reader.Read())
                //{
                //    OperatorSale operatorSale = new OperatorSale();
                //    if (!reader.IsDBNull(0)) operatorSale.Brand = reader.GetString(0);
                //    if (!reader.IsDBNull(1)) operatorSale.Address = reader.GetString(1);
                //    if (!reader.IsDBNull(2)) operatorSale.ClientName = reader.GetString(2);
                //    if (!reader.IsDBNull(3)) operatorSale.Gender = reader.GetString(3);
                //    if (!reader.IsDBNull(4)) operatorSale.Phone = 80000000000 + reader.GetInt64(4);
                //    if (!reader.IsDBNull(5)) operatorSale.Email = reader.GetString(5);
                //    if (!reader.IsDBNull(6)) operatorSale.Card = reader.GetInt64(6);
                //    if (!reader.IsDBNull(7)) operatorSale.ClientType = reader.GetString(7);
                //    if (!reader.IsDBNull(8)) operatorSale.ChequeTime = reader.GetDateTime(8);
                //    if (!reader.IsDBNull(9)) operatorSale.OperationType = reader.GetString(9);
                //    if (!reader.IsDBNull(10)) operatorSale.Amount = reader.GetDecimal(10);
                //    if (!reader.IsDBNull(11)) operatorSale.AddedBonus = reader.GetDecimal(11);
                //    if (!reader.IsDBNull(12)) operatorSale.SubstractBonus = reader.GetDecimal(12);
                //    operatorSales.Add(operatorSale);
                //}
                //reader.Close();
                //cnn.Close();
                //string reportName = string.Format("Отчёт о продажах по программе лояльности с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));
                using (var package = new ExcelPackage())
                {
                    var workbook = package.Workbook;
                    string workSheetName = string.Format("с {0} по {1}", request.DateStart, request.DateEnd);
                    var worksheet = workbook.Worksheets.Add(workSheetName);
                    worksheet.View.FreezePanes(2, 1);
                    worksheet.Cells["A1"].Value = "Дата";
                    worksheet.Cells["A1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["B1"].Value = "Тип бонуса";
                    worksheet.Cells["B1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C1"].Value = "Начислено";
                    worksheet.Cells["C1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D1"].Value = "Списано";
                    worksheet.Cells["D1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E1"].Value = "Сгорело";
                    worksheet.Cells["E1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["F1"].Value = "Номер карты";
                    worksheet.Cells["F1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);



                    worksheet.Cells["A1:F1"].AutoFilter = true;
                    worksheet.Cells["A1:F1"].Style.WrapText = true;
                    worksheet.Cells["A1:F1"].Style.VerticalAlignment =
                        OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:F1"].Style.HorizontalAlignment =
                        OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    var color = System.Drawing.ColorTranslator.FromHtml("#0070C0");
                    worksheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(color);
                    worksheet.Cells["A1:F1"].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                    worksheet.Cells["A1:F1"].Style.Font.Size = 11;
                    for (var i = 1; i < 7; i++)
                    {
                        worksheet.Column(i).Width = 20;
                    }

                    var row = 1;
                    var cellNum = string.Empty;
                    foreach (var bonuse in data.Bonuses)
                    {
                        row++;

                        cellNum = row.ToString();
                        worksheet.Cells["A" + cellNum].Value = bonuse.BonusDate != null
                            ? Convert.ToDateTime(bonuse.BonusDate).ToString("dd.MM.yyyy")
                            : string.Empty;
                        worksheet.Cells["A" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                        worksheet.Cells["B" + cellNum].Value = bonuse.BonusSource;
                        worksheet.Cells["B" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                        worksheet.Cells["C" + cellNum].Value = bonuse.BonusAdded;
                        worksheet.Cells["C" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["C" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["D" + cellNum].Value = bonuse.BonusRedeemed;
                        worksheet.Cells["D" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["D" + cellNum].Style.Numberformat.Format = "0";


                        worksheet.Cells["E" + cellNum].Value = bonuse.BonusBurn;
                        worksheet.Cells["E" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["E" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["F" + cellNum].Value = bonuse.BonusCard.ToString();
                        worksheet.Cells["F" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["F" + cellNum].Style.Numberformat.Format = "0";

                    }
                    worksheet.Cells["A1:F" + cellNum].Style.Font.Size = 10;
                    response.Report = package.GetAsByteArray();
                }
                return response;
            }
            catch (Exception e)
            {
                response.ErrorCode = 10;
                response.Message = e.Message;
            }
            return response;

        }
    }
}