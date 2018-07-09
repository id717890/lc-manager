using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using LCManager.Infrastructure.Data;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
                cmd.CommandText = "BookkeepingPaging";
                cmd.CommandTimeout = 300;

                if (request.Operator > 0) cmd.Parameters.AddWithValue("@operator", request.Operator);
                if (request.Partner > 0) cmd.Parameters.AddWithValue("@partner", request.Partner);
                if (request.Pos > 0) cmd.Parameters.AddWithValue("@pos", request.Pos);
                if (request.Page == 0) request.Page++;

                cmd.Parameters.AddWithValue("@start", request.Page);

                //Если start = -1 и length = -1 это значит выгрузить все без пагинации
                if (request.Page != -1) cmd.Parameters.AddWithValue("@length", request.Page + request.PageSize);
                else cmd.Parameters.AddWithValue("@length", request.PageSize);

                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add("@total_rows", SqlDbType.Int);
                cmd.Parameters["@total_rows"].Direction = ParameterDirection.Output;

                //Фильтр по дате (Верхний фильтр с диапазоном)
                if (!string.IsNullOrEmpty(request.DateStart))
                {
                    if (DateTime.TryParseExact(request.DateStart, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        cmd.Parameters.AddWithValue("@f_date_start", date.ToString("yyyy-MM-dd"));
                    }
                }

                //Фильтр по дате (Верхний фильтр с диапазоном)
                if (!string.IsNullOrEmpty(request.DateEnd))
                {
                    if (DateTime.TryParseExact(request.DateEnd, new[] { "dd.MM.yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        cmd.Parameters.AddWithValue("@f_date_end", date.ToString("yyyy-MM-dd"));
                    }
                }

                //Фильтр по названию
                if (!string.IsNullOrEmpty(request.Name)) cmd.Parameters.AddWithValue("@f_name", request.Name);
                //Фильтр по Покупкам
                if (!string.IsNullOrEmpty(request.PurchasesMore)) cmd.Parameters.AddWithValue("@f_buy_more", Convert.ToInt64(request.PurchasesMore));
                if (!string.IsNullOrEmpty(request.PurchasesLess)) cmd.Parameters.AddWithValue("@f_buy_less", Convert.ToInt64(request.PurchasesLess));
                //Фильтр по Начислено
                if (!string.IsNullOrEmpty(request.AddedMore)) cmd.Parameters.AddWithValue("@f_added_more", Convert.ToInt64(request.AddedMore));
                if (!string.IsNullOrEmpty(request.AddedLess)) cmd.Parameters.AddWithValue("@f_added_less", Convert.ToInt64(request.AddedLess));
                //Фильтр по Списано
                if (!string.IsNullOrEmpty(request.RedeemedMore)) cmd.Parameters.AddWithValue("@f_redeemed_more", Convert.ToInt64(request.RedeemedMore));
                if (!string.IsNullOrEmpty(request.RedeemedLess)) cmd.Parameters.AddWithValue("@f_redeemed_less", Convert.ToInt64(request.RedeemedLess));
                //Фильтр по Клиентам
                if (!string.IsNullOrEmpty(request.ClientsMore)) cmd.Parameters.AddWithValue("@f_clients_more", Convert.ToInt64(request.ClientsMore));
                if (!string.IsNullOrEmpty(request.ClientsLess)) cmd.Parameters.AddWithValue("@f_clients_less", Convert.ToInt64(request.ClientsLess));

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Bookkeeping bookkeeping = new Bookkeeping();
                    try
                    {
                        if (!reader.IsDBNull(0)) bookkeeping.Id = reader.GetInt16(0);
                        if (!reader.IsDBNull(1)) bookkeeping.Caption = reader.GetString(1);
                        if (!reader.IsDBNull(2)) bookkeeping.Gain = reader.GetDecimal(2);
                        if (!reader.IsDBNull(3)) bookkeeping.Added = reader.GetDecimal(3);
                        if (!reader.IsDBNull(4)) bookkeeping.Redeemed = reader.GetDecimal(4);
                        if (!reader.IsDBNull(5)) bookkeeping.Clients = reader.GetInt32(5);

                        if (!reader.IsDBNull(6)) bookkeeping.GainMonth1 = reader.GetInt64(6);
                        if (!reader.IsDBNull(7)) bookkeeping.AddedMonth1 = reader.GetInt64(7);
                        if (!reader.IsDBNull(8)) bookkeeping.RedeemedMonth1 = reader.GetInt64(8);
                        if (!reader.IsDBNull(9)) bookkeeping.ClientsMonth1 = reader.GetInt64(9);
                        if (!reader.IsDBNull(10)) bookkeeping.GainMonth2 = reader.GetInt64(10);
                        if (!reader.IsDBNull(11)) bookkeeping.AddedMonth2 = reader.GetInt64(11);
                        if (!reader.IsDBNull(12)) bookkeeping.RedeemedMonth2 = reader.GetInt64(12);
                        if (!reader.IsDBNull(13)) bookkeeping.ClientsMonth2 = reader.GetInt64(13);
                        if (!reader.IsDBNull(14)) bookkeeping.GainMonth3 = reader.GetInt64(14);
                        if (!reader.IsDBNull(15)) bookkeeping.AddedMonth3 = reader.GetInt64(15);
                        if (!reader.IsDBNull(16)) bookkeeping.RedeemedMonth3 = reader.GetInt64(16);
                        if (!reader.IsDBNull(17)) bookkeeping.ClientsMonth3 = reader.GetInt64(17);
                        if (!reader.IsDBNull(18)) bookkeeping.GainMonth4 = reader.GetInt64(18);
                        if (!reader.IsDBNull(19)) bookkeeping.AddedMonth4 = reader.GetInt64(19);
                        if (!reader.IsDBNull(20)) bookkeeping.RedeemedMonth4 = reader.GetInt64(20);
                        if (!reader.IsDBNull(21)) bookkeeping.ClientsMonth4 = reader.GetInt64(21);
                        if (!reader.IsDBNull(22)) bookkeeping.GainMonth5 = reader.GetInt64(22);
                        if (!reader.IsDBNull(23)) bookkeeping.AddedMonth5 = reader.GetInt64(23);
                        if (!reader.IsDBNull(24)) bookkeeping.RedeemedMonth5 = reader.GetInt64(24);
                        if (!reader.IsDBNull(25)) bookkeeping.ClientsMonth5 = reader.GetInt64(25);
                        if (!reader.IsDBNull(26)) bookkeeping.GainMonth6 = reader.GetInt64(26);
                        if (!reader.IsDBNull(27)) bookkeeping.AddedMonth6 = reader.GetInt64(27);
                        if (!reader.IsDBNull(28)) bookkeeping.RedeemedMonth6 = reader.GetInt64(28);
                        if (!reader.IsDBNull(29)) bookkeeping.ClientsMonth6 = reader.GetInt64(29);
                        if (!reader.IsDBNull(30)) bookkeeping.GainMonth7 = reader.GetInt64(30);
                        if (!reader.IsDBNull(31)) bookkeeping.AddedMonth7 = reader.GetInt64(31);
                        if (!reader.IsDBNull(32)) bookkeeping.RedeemedMonth7 = reader.GetInt64(32);
                        if (!reader.IsDBNull(33)) bookkeeping.ClientsMonth7 = reader.GetInt64(33);
                        if (!reader.IsDBNull(34)) bookkeeping.GainMonth8 = reader.GetInt64(34);
                        if (!reader.IsDBNull(35)) bookkeeping.AddedMonth8 = reader.GetInt64(35);
                        if (!reader.IsDBNull(36)) bookkeeping.RedeemedMonth8 = reader.GetInt64(36);
                        if (!reader.IsDBNull(37)) bookkeeping.ClientsMonth8 = reader.GetInt64(37);
                        if (!reader.IsDBNull(38)) bookkeeping.GainMonth9 = reader.GetInt64(38);
                        if (!reader.IsDBNull(39)) bookkeeping.AddedMonth9 = reader.GetInt64(39);
                        if (!reader.IsDBNull(40)) bookkeeping.RedeemedMonth9 = reader.GetInt64(40);
                        if (!reader.IsDBNull(41)) bookkeeping.ClientsMonth9 = reader.GetInt64(41);
                        if (!reader.IsDBNull(42)) bookkeeping.GainMonth10 = reader.GetInt64(42);
                        if (!reader.IsDBNull(43)) bookkeeping.AddedMonth10 = reader.GetInt64(43);
                        if (!reader.IsDBNull(44)) bookkeeping.RedeemedMonth10 = reader.GetInt64(44);
                        if (!reader.IsDBNull(45)) bookkeeping.ClientsMonth10 = reader.GetInt64(45);
                        if (!reader.IsDBNull(46)) bookkeeping.GainMonth11 = reader.GetInt64(46);
                        if (!reader.IsDBNull(47)) bookkeeping.AddedMonth11 = reader.GetInt64(47);
                        if (!reader.IsDBNull(48)) bookkeeping.RedeemedMonth11 = reader.GetInt64(48);
                        if (!reader.IsDBNull(49)) bookkeeping.ClientsMonth11 = reader.GetInt64(49);
                        if (!reader.IsDBNull(50)) bookkeeping.GainMonth12 = reader.GetInt64(50);
                        if (!reader.IsDBNull(51)) bookkeeping.AddedMonth12 = reader.GetInt64(51);
                        if (!reader.IsDBNull(52)) bookkeeping.RedeemedMonth12 = reader.GetInt64(52);
                        if (!reader.IsDBNull(53)) bookkeeping.ClientsMonth12 = reader.GetInt64(53);


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
                response.RecordTotal = Convert.ToInt32(cmd.Parameters["@total_rows"].Value);
                response.RecordFilterd = response.RecordTotal;

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

        /// <summary>
        /// Отчет по сверке
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ReportResponse BookkeepingReport(BookkeepingRequest request)
        {
            var response = new ReportResponse();
            try
            {
                var data = GetAllBookkeeping(request);
                using (var package = new ExcelPackage())
                {
                    var workbook = package.Workbook;
                    var workSheetName = $"с {request.DateStart} по {request.DateEnd}";
                    var worksheet = workbook.Worksheets.Add(workSheetName);
                    worksheet.View.FreezePanes(2, 1);
                    worksheet.Cells["A1"].Value = "Партнер";
                    worksheet.Cells["A1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["B1"].Value = "Выручка";
                    worksheet.Cells["B1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["C1"].Value = "Начислено";
                    worksheet.Cells["C1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D1"].Value = "Списано";
                    worksheet.Cells["D1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E1"].Value = "Клиентов";
                    worksheet.Cells["E1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["A1:E1"].AutoFilter = true;
                    worksheet.Cells["A1:E1"].Style.WrapText = true;
                    worksheet.Cells["A1:E1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    var color = ColorTranslator.FromHtml("#0070C0");
                    worksheet.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(color);
                    worksheet.Cells["A1:E1"].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                    worksheet.Cells["A1:E1"].Style.Font.Size = 11;
                    for (var i = 1; i < 6; i++)
                    {
                        worksheet.Column(i).Width = 20;
                    }

                    var row = 1;
                    var cellNum = string.Empty;
                    foreach (var item in data.Bookkeepings)
                    {
                        row++;

                        cellNum = row.ToString();
                        worksheet.Cells["A" + cellNum].Value = item.Caption;
                        worksheet.Cells["A" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                        worksheet.Cells["B" + cellNum].Value = item.Gain;
                        worksheet.Cells["B" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["B" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["C" + cellNum].Value = item.Added;
                        worksheet.Cells["C" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["C" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["D" + cellNum].Value = item.Redeemed;
                        worksheet.Cells["D" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["D" + cellNum].Style.Numberformat.Format = "0";

                        worksheet.Cells["E" + cellNum].Value = item.Clients;
                        worksheet.Cells["E" + cellNum].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                        worksheet.Cells["E" + cellNum].Style.Numberformat.Format = "0";
                    }
                    worksheet.Cells["A1:E" + cellNum].Style.Font.Size = 10;
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