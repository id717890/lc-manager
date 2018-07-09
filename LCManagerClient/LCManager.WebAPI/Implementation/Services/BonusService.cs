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
                //Фильтр по телефону
                if (!string.IsNullOrEmpty(request.Phone)) cmd.Parameters.AddWithValue("@f_phone", request.Phone);
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
                        if (!readerBonuses.IsDBNull(1)) bonus.BonusDate = readerBonuses.GetDateTime(1);
                        if (!readerBonuses.IsDBNull(2)) bonus.BonusAdded = readerBonuses.GetDecimal(2);
                        if (!readerBonuses.IsDBNull(3)) bonus.BonusRedeemed = readerBonuses.GetDecimal(3);
                        if (!readerBonuses.IsDBNull(4)) bonus.BonusBurn = readerBonuses.GetDecimal(4);
                        if (!readerBonuses.IsDBNull(5)) bonus.BonusCard= readerBonuses.GetInt64(5);
                        if (!readerBonuses.IsDBNull(6)) bonus.Phone= readerBonuses.GetInt64(6).ToString();
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
                    worksheet.Cells["C1"].Value = "Телефон";
                    worksheet.Cells["C1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D1"].Value = "Основание";
                    worksheet.Cells["D1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E1"].Value = "Начислено";
                    worksheet.Cells["E1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["F1"].Value = "Списано";
                    worksheet.Cells["F1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G1"].Value = "Сгорело";
                    worksheet.Cells["G1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["A1:G1"].AutoFilter = true;
                    worksheet.Cells["A1:G1"].Style.WrapText = true;
                    worksheet.Cells["A1:G1"].Style.VerticalAlignment =
                        OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:G1"].Style.HorizontalAlignment =
                        OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    var color = System.Drawing.ColorTranslator.FromHtml("#0070C0");
                    worksheet.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(color);
                    worksheet.Cells["A1:G1"].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                    worksheet.Cells["A1:G1"].Style.Font.Size = 11;
                    for (var i = 1; i < 8; i++)
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

                        worksheet.Cells["C" + cellNum].Value = bonuse.Phone;
                        worksheet.Cells["C" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["C" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["D" + cellNum].Value = "Механика ПЛ";
                        worksheet.Cells["D" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                        worksheet.Cells["E" + cellNum].Value = bonuse.BonusAdded;
                        worksheet.Cells["E" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["E" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["F" + cellNum].Value = bonuse.BonusRedeemed;
                        worksheet.Cells["F" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["F" + cellNum].Style.Numberformat.Format = "0";


                        worksheet.Cells["G" + cellNum].Value = bonuse.BonusBurn;
                        worksheet.Cells["G" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["G" + cellNum].Style.Numberformat.Format = "0";
                    }
                    worksheet.Cells["A1:G" + cellNum].Style.Font.Size = 10;
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