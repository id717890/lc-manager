using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Models
{
    public class ClientBuys
    {
        public string Name { get; set; }
        public long Phone { get; set; }
        public long Card { get; set; }
        public string Email { get; set; }
        public string Level { get; set; }
        public decimal Balance { get; set; }
        public decimal SumAmount { get; set; }
        public decimal SumRefund { get; set; }
        public int QtyBuys { get; set; }
        public int QtyRefund { get; set; }
        public decimal SumAmountPeriod { get; set; }
        public decimal SumRefundPeriod { get; set; }
        public int QtyBuysPeriod { get; set; }
        public int QtyRefundPeriod { get; set; }
        public decimal AmountFirstBuy { get; set; }
        public decimal AmountLastBuy { get; set; }
        public DateTime? DateFirstBuy { get; set; }
        public DateTime? DateLastBuy { get; set; }
    }

    public class ClientBuysRequest
    {
        public Int16 Operator { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class ClientBuysResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerClientBuyResponse
    {
        public ClientBuysResponse ProcessRequest(SqlConnection cnn, ClientBuysRequest request)
        {
            ClientBuysResponse returnValue = new ClientBuysResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.ClientBuy";
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<ClientBuys> clients = new List<ClientBuys>();
            while (reader.Read())
            {
                ClientBuys client = new ClientBuys();
                if (!reader.IsDBNull(0)) client.Name = reader.GetString(0);
                if (!reader.IsDBNull(1)) client.Phone = 80000000000 + reader.GetInt64(1);
                if (!reader.IsDBNull(2)) client.Email = reader.GetString(2);
                if (!reader.IsDBNull(3)) client.Level = reader.GetString(3);
                if (!reader.IsDBNull(4)) client.Card = reader.GetInt64(4);
                if (!reader.IsDBNull(5)) client.Balance = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) client.SumAmount = reader.GetDecimal(6);
                if (!reader.IsDBNull(7)) client.SumRefund = reader.GetDecimal(7);
                if (!reader.IsDBNull(8)) client.QtyBuys = reader.GetInt32(8);
                if (!reader.IsDBNull(9)) client.QtyRefund = reader.GetInt32(9);
                if (!reader.IsDBNull(10)) client.SumAmountPeriod = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) client.SumRefundPeriod = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) client.QtyBuysPeriod = reader.GetInt32(12);
                if (!reader.IsDBNull(13)) client.QtyRefundPeriod = reader.GetInt32(13);
                if (!reader.IsDBNull(14)) client.AmountFirstBuy = reader.GetDecimal(14);
                if (!reader.IsDBNull(15)) client.AmountLastBuy = reader.GetDecimal(15);
                if (!reader.IsDBNull(16)) client.DateFirstBuy = reader.GetDateTime(16);
                if (!reader.IsDBNull(17)) client.DateLastBuy = reader.GetDateTime(17);
                clients.Add(client);
            }
            reader.Close();
            cnn.Close();

            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1"].Value = "Данные покупателя";
                worksheet.Cells["E1:F1"].Merge = true;
                worksheet.Cells["E1"].Value = "Процент и баланс на конец периода";
                worksheet.Cells["G1:J1"].Merge = true;
                worksheet.Cells["G1"].Value = "Покупки и возвраты за всё время";
                worksheet.Cells["K1:N1"].Merge = true;
                worksheet.Cells["K1"].Value = "Покупки и возвраты за период";
                worksheet.Cells["O1:R1"].Merge = true;
                worksheet.Cells["O1"].Value = "Дата и сумма первой и последней покупки за всё время";
                worksheet.Cells["A2"].Value = "ФИО";
                worksheet.Cells["B2"].Value = "Номер телефона";
                worksheet.Cells["C2"].Value = "Номер карты";
                worksheet.Cells["D2"].Value = "e-mail";
                worksheet.Cells["E2"].Value = "Процент премии на конец периода";
                worksheet.Cells["F2"].Value = "Баланс на конец периода";
                worksheet.Cells["G2"].Value = "Количество покупок за всё время";
                worksheet.Cells["H2"].Value = "Cумма покупок за всё время";
                worksheet.Cells["I2"].Value = "Количество возвратов за всё время";
                worksheet.Cells["J2"].Value = "Cумма возвратов за всё время";
                worksheet.Cells["K2"].Value = "Количество покупок за период";
                worksheet.Cells["L2"].Value = "Сумма покупок за период";
                worksheet.Cells["M2"].Value = "Количество возвратов за период";
                worksheet.Cells["N2"].Value = "Сумма возвратов за период";

                worksheet.Cells["O2"].Value = "Дата первой покупки за всё время";
                worksheet.Cells["P2"].Value = "Сумма первой покупки за всё время";
                worksheet.Cells["Q2"].Value = "Дата последней покупки за всё время";
                worksheet.Cells["R2"].Value = "Сумма последней покупки за всё время";
                worksheet.Cells["A1:R1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A1:R1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2:R2"].AutoFilter = true;
                worksheet.Cells["A2:R2"].Style.WrapText = true;
                worksheet.Cells["A2:R2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:R2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                for (int i = 1; i < 19; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0; i < clients.Count; i++)
                {
                    worksheet.Cells["A" + (i + 3).ToString()].Value = clients[i].Name;
                    worksheet.Cells["B" + (i + 3).ToString()].Value = clients[i].Phone;
                    worksheet.Cells["B" + (i + 3).ToString()].Style.Numberformat.Format = "#";
                    worksheet.Cells["C" + (i + 3).ToString()].Value = clients[i].Card;
                    worksheet.Cells["C" + (i + 3).ToString()].Style.Numberformat.Format = "#";
                    worksheet.Cells["D" + (i + 3).ToString()].Value = clients[i].Email;

                    worksheet.Cells["E" + (i + 3).ToString()].Value = clients[i].Level;
                    worksheet.Cells["F" + (i + 3).ToString()].Value = clients[i].Balance;

                    worksheet.Cells["G" + (i + 3).ToString()].Value = clients[i].QtyBuys;
                    worksheet.Cells["H" + (i + 3).ToString()].Value = clients[i].SumAmount;
                    worksheet.Cells["I" + (i + 3).ToString()].Value = clients[i].QtyRefund;
                    worksheet.Cells["J" + (i + 3).ToString()].Value = clients[i].SumRefund;

                    worksheet.Cells["K" + (i + 3).ToString()].Value = clients[i].QtyBuysPeriod;
                    worksheet.Cells["L" + (i + 3).ToString()].Value = clients[i].SumAmountPeriod;
                    worksheet.Cells["M" + (i + 3).ToString()].Value = clients[i].QtyRefundPeriod;
                    worksheet.Cells["N" + (i + 3).ToString()].Value = clients[i].SumRefundPeriod;

                    worksheet.Cells["O" + (i + 3).ToString()].Value = clients[i].DateFirstBuy.Value.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells["P" + (i + 3).ToString()].Value = clients[i].AmountFirstBuy;
                    worksheet.Cells["Q" + (i + 3).ToString()].Value = clients[i].DateLastBuy.Value.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells["R" + (i + 3).ToString()].Value = clients[i].AmountLastBuy;
                }
                returnValue.Report = package.GetAsByteArray();
            }

            return returnValue;
        }
    }

    public class Buy
    {
        public string ChequeNum { get; set; }
        public DateTime? ChequeTime { get; set; }
        public decimal ChequeAmount { get; set; }
        public string CardLevel { get; set; }
        public string OperationType { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
        public decimal ChangeBalance { get; set; }
        public string Name { get; set; }
        public Int64 Card { get; set; }
        public long Phone { get; set; }
        public string Email { get; set; }
    }

    public class BuysRequest
    {
        public Int16 Operator { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class BuysResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerBuyResponse
    {
        public BuysResponse ProcessRequest(SqlConnection cnn, BuysRequest request)
        {
            BuysResponse returnValue = new BuysResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.Buys";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            if (request.From.HasValue)
            {
                cmd.Parameters.AddWithValue("@from", request.From.Value);
            }
            if (request.To.HasValue)
            {
                cmd.Parameters.AddWithValue("@to", request.To.Value);
            }
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Buy> buys = new List<Buy>();
            while (reader.Read())
            {
                Buy buy = new Buy();
                if (!reader.IsDBNull(0)) buy.ChequeNum = reader.GetValue(0).ToString();
                if (!reader.IsDBNull(1)) buy.ChequeTime = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) buy.ChequeAmount = reader.GetDecimal(2);
                if (!reader.IsDBNull(3)) buy.CardLevel = reader.GetString(3);
                if (!reader.IsDBNull(4)) buy.OperationType = reader.GetString(4);
                if (!reader.IsDBNull(5)) buy.AddedBonus = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) buy.SubstractBonus = reader.GetDecimal(6);
                if (!reader.IsDBNull(7)) buy.ChangeBalance = reader.GetDecimal(7);
                if (!reader.IsDBNull(8)) buy.Name = reader.GetString(8);
                if (!reader.IsDBNull(9)) buy.Card = reader.GetInt64(9);
                if (!reader.IsDBNull(10)) buy.Phone = 80000000000 + reader.GetInt64(10);
                if (!reader.IsDBNull(11)) buy.Email = reader.GetString(11);
                buys.Add(buy);
            }
            reader.Close();
            cnn.Close();
            using (ExcelPackage package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = "ФИО участника";
                worksheet.Cells["B1"].Value = "Номер карты";
                worksheet.Cells["C1"].Value = "Телефон";
                worksheet.Cells["D1"].Value = "email";
                worksheet.Cells["E1"].Value = "Номер чека";
                worksheet.Cells["F1"].Value = "Дата и время чека";
                worksheet.Cells["G1"].Value = "Сумма чека";
                worksheet.Cells["H1"].Value = "Уровень скидки";
                worksheet.Cells["I1"].Value = "Тип операции";
                worksheet.Cells["J1"].Value = "Начислено бонусов";
                worksheet.Cells["K1"].Value = "Списано бонусов";
                worksheet.Cells["L1"].Value = "Бонусный баланс";

                worksheet.Cells["A1:L1"].AutoFilter = true;
                worksheet.Cells["A1:L1"].Style.WrapText = true;
                worksheet.Cells["A1:L1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A1:L1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                for (int i = 1; i <= 12; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0; i < buys.Count; i++)
                {
                    worksheet.Cells["A" + (i + 2).ToString()].Value = buys[i].Name;
                    worksheet.Cells["B" + (i + 2).ToString()].Value = buys[i].Card;
                    worksheet.Cells["B" + (i + 2).ToString()].Style.Numberformat.Format = "#";

                    worksheet.Cells["C" + (i + 2).ToString()].Value = buys[i].Phone;
                    worksheet.Cells["D" + (i + 2).ToString()].Value = buys[i].Email;
                    worksheet.Cells["E" + (i + 2).ToString()].Value = buys[i].ChequeNum;
                    worksheet.Cells["F" + (i + 2).ToString()].Value = buys[i].ChequeTime.Value.ToString("dd.MM.yyyy HH:mm:ss");
                    worksheet.Cells["G" + (i + 2).ToString()].Value = buys[i].ChequeAmount;
                    worksheet.Cells["H" + (i + 2).ToString()].Value = buys[i].CardLevel;
                    worksheet.Cells["I" + (i + 2).ToString()].Value = buys[i].OperationType;
                    worksheet.Cells["J" + (i + 2).ToString()].Value = buys[i].AddedBonus;
                    worksheet.Cells["K" + (i + 2).ToString()].Value = buys[i].SubstractBonus;
                    worksheet.Cells["L" + (i + 2).ToString()].Value = buys[i].ChangeBalance;
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class PosClient
    {
        public string Brand { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public Int64 Card { get; set; }
        public string ClientType { get; set; }
        public int QtyBuysPeriod { get; set; }
        public decimal SumAmountPeriod { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
        public int QtyRefundPeriod { get; set; }
        public decimal SumRefundPeriod { get; set; }
        public decimal Balance { get; set; }
        public string LevelCondition { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowEmail { get; set; }
        public decimal AddedBonusUnbuy { get; set; }
    }

    public class PosClientPeriodRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public string Pos { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class PosClientPeriodResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerPosClientPeriodResponse
    {
        public PosClientPeriodResponse ProcessRequest(SqlConnection cnn, PosClientPeriodRequest request)
        {
            var returnValue = new PosClientPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Reports.PosClient";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@pos", request.Pos);
            if (request.From.HasValue)
            {
                cmd.Parameters.AddWithValue("@from", request.From.Value);
            }
            if (request.To.HasValue)
            {
                cmd.Parameters.AddWithValue("@to", request.To.Value);
            }
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PosClient> posClients = new List<PosClient>();
            while (reader.Read())
            {
                PosClient posClient = new PosClient();
                if (!reader.IsDBNull(0)) posClient.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) posClient.Address = reader.GetString(1);
                if (!reader.IsDBNull(2)) posClient.Name = reader.GetString(2);
                if (!reader.IsDBNull(3)) posClient.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) posClient.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) posClient.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) posClient.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) posClient.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) posClient.QtyBuysPeriod = reader.GetInt32(8);
                if (!reader.IsDBNull(9)) posClient.SumAmountPeriod = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) posClient.AddedBonus = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) posClient.SubstractBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) posClient.QtyRefundPeriod = reader.GetInt32(12);
                if (!reader.IsDBNull(13)) posClient.SumRefundPeriod = reader.GetDecimal(13);
                if (!reader.IsDBNull(14)) posClient.Balance = reader.GetDecimal(14);
                if (!reader.IsDBNull(15)) posClient.LevelCondition = reader.GetString(15);
                if (!reader.IsDBNull(16)) posClient.BirthDate = reader.GetDateTime(16);
                if (!reader.IsDBNull(17)) posClient.AllowSms = reader.GetBoolean(17);
                if (!reader.IsDBNull(18)) posClient.AllowEmail = reader.GetBoolean(18);
                if (!reader.IsDBNull(19)) posClient.AddedBonusUnbuy = reader.GetDecimal(19);
                posClients.Add(posClient);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = string.Format("Отчёт по клиентам за период с {0} по {1}", request.From.Value.ToString("dd.MM.yyyy"), request.To.Value.ToString("dd.MM.yyyy"));
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Торговая точка Регистратор Участника";

                worksheet.Cells["C2:K2"].Merge = true;
                worksheet.Cells["C2:K2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:K2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:K2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Личные данные Участника";

                worksheet.Cells["L2:T2"].Merge = true;
                worksheet.Cells["L2:T2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["L2:T2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00FFFF"));
                worksheet.Cells["L2:T2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L2"].Value = "Активность участника в торговой точке Программы за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "Согласие на отправку смс";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "E-mail";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Согласие на отправку Email";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I3"].Value = "Номер карты";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Тип участника";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["K3"].Value = "Дата рождения";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Количество покупок";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Cумма покупок";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["N3"].Value = "Начислено бонусов за покупки";
                worksheet.Cells["N3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["O3"].Value = "Списано бонусов за покупки";
                worksheet.Cells["O3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["P3"].Value = "Начислено бонусов не за покупки";
                worksheet.Cells["P3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["Q3"].Value = "Количество возвратов";
                worksheet.Cells["Q3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["R3"].Value = "Сумма возвратов";
                worksheet.Cells["R3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["S3"].Value = "Бонусный баланс";
                worksheet.Cells["S3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["T3"].Value = "Процент начисления";
                worksheet.Cells["T3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:T2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:T2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:T3"].AutoFilter = true;
                worksheet.Cells["A3:T3"].Style.WrapText = true;
                worksheet.Cells["A3:T3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:T3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A3:T3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:T3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.View.FreezePanes(4, 1);
                for (int i = 1; i < 21; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = posClients.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = posClients[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = posClients[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = posClients[i].Name;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = posClients[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = posClients[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeSms = "Нет";
                    if (posClients[i].AllowSms)
                    {
                        agreeSms = "Да";
                    }

                    worksheet.Cells["F" + (i + 4).ToString()].Value = agreeSms;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = posClients[i].Email;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeEmail = "Нет";
                    if (posClients[i].AllowEmail)
                    {
                        agreeEmail = "Да";
                    }

                    worksheet.Cells["H" + (i + 4).ToString()].Value = agreeEmail;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = posClients[i].Card;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = posClients[i].ClientType;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    if (posClients[i].BirthDate.HasValue)
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = posClients[i].BirthDate.Value.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = "Не указана дата рождения";
                    }
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = posClients[i].QtyBuysPeriod;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = posClients[i].SumAmountPeriod;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["N" + (i + 4).ToString()].Value = posClients[i].AddedBonus;
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["O" + (i + 4).ToString()].Value = posClients[i].SubstractBonus;
                    worksheet.Cells["O" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["P" + (i + 4).ToString()].Value = posClients[i].AddedBonusUnbuy;
                    worksheet.Cells["P" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["Q" + (i + 4).ToString()].Value = posClients[i].QtyRefundPeriod;
                    worksheet.Cells["Q" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["R" + (i + 4).ToString()].Value = posClients[i].SumRefundPeriod;
                    worksheet.Cells["R" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["S" + (i + 4).ToString()].Value = posClients[i].Balance;
                    worksheet.Cells["S" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["T" + (i + 4).ToString()].Value = posClients[i].LevelCondition;
                    worksheet.Cells["T" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class PosSale
    {
        public string Brand { get; set; }
        public string Address { get; set; }
        public string ClientName { get; set; }
        public string Gender { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public Int64 Card { get; set; }
        public string ClientType { get; set; }
        public string ThisPosRegister { get; set; }
        public string OperationType { get; set; }
        public decimal Amount { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
        public DateTime ChequeTime { get; set; }
    }

    public class ServerPosSalePeriodResponse
    {
        public PosClientPeriodResponse ProcessRequest(SqlConnection cnn, PosClientPeriodRequest request)
        {
            PosClientPeriodResponse returnValue = new PosClientPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.PosSales";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@pos", request.Pos);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PosSale> posSales = new List<PosSale>();
            while (reader.Read())
            {
                PosSale posSale = new PosSale();
                if (!reader.IsDBNull(0)) posSale.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) posSale.Address = reader.GetString(1);

                if (!reader.IsDBNull(2)) posSale.ClientName = reader.GetString(2);
                if (!reader.IsDBNull(3)) posSale.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) posSale.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) posSale.Email = reader.GetString(5);

                if (!reader.IsDBNull(6)) posSale.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) posSale.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) posSale.ThisPosRegister = reader.GetString(8);
                if (!reader.IsDBNull(9)) posSale.OperationType = reader.GetString(9);
                if (!reader.IsDBNull(10)) posSale.Amount = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) posSale.AddedBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) posSale.SubstractBonus = reader.GetDecimal(12);
                if (!reader.IsDBNull(13)) posSale.ChequeTime = reader.GetDateTime(13);
                posSales.Add(posSale);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.View.FreezePanes(4, 1);
                worksheet.Cells["A1"].Value = string.Format("Отчёт по продажам за период с {0} по {1}", request.From.Value.ToString("dd.MM.yyyy"), request.To.Value.ToString("dd.MM.yyyy"));
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Партнёр";

                worksheet.Cells["C2:H2"].Merge = true;
                worksheet.Cells["C2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:H2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Участник";

                worksheet.Cells["I2:N2"].Merge = true;
                worksheet.Cells["I2:N2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["I2:N2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["I2:N2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I2"].Value = "Операции Участника за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "E-mail";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "Номер карты";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Тип участника";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["I3"].Value = "Регистрация в моей ТТ";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Дата операции";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["K3"].Value = "Операция";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Сумма операции";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Начислено бонусов";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["N3"].Value = "Списано бонусов";
                worksheet.Cells["N3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:N2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:N2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:N3"].AutoFilter = true;
                worksheet.Cells["A3:N3"].Style.WrapText = true;
                worksheet.Cells["A3:N3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:N3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                worksheet.Cells["A3:N3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:N3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                for (int i = 1; i < 15; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = posSales.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = posSales[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = posSales[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = posSales[i].ClientName;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = posSales[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = posSales[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["F" + (i + 4).ToString()].Value = posSales[i].Email;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = posSales[i].Card;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["H" + (i + 4).ToString()].Value = posSales[i].ClientType;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = posSales[i].ThisPosRegister;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = posSales[i].ChequeTime.ToString("dd.MM.yyyy HH:mm"); ;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["K" + (i + 4).ToString()].Value = posSales[i].OperationType;
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = posSales[i].Amount;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = posSales[i].AddedBonus;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["N" + (i + 4).ToString()].Value = posSales[i].SubstractBonus;
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }

            return returnValue;
        }
    }

    public class ReportOperatorClientRequest
    {
        public Int16 Operator { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class ReportOperatorClientResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ReportServerOperatorClient
    {
        public ReportOperatorClientResponse ProcessRequest(SqlConnection cnn, ReportOperatorClientRequest request)
        {
            ReportOperatorClientResponse returnValue = new ReportOperatorClientResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.OperatorClient";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PosClient> posClients = new List<PosClient>();
            while (reader.Read())
            {
                PosClient posClient = new PosClient();
                if (!reader.IsDBNull(0)) posClient.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) posClient.Address = reader.GetString(1);
                if (!reader.IsDBNull(2)) posClient.Name = reader.GetString(2);
                if (!reader.IsDBNull(3)) posClient.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) posClient.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) posClient.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) posClient.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) posClient.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) posClient.QtyBuysPeriod = reader.GetInt32(8);
                if (!reader.IsDBNull(9)) posClient.SumAmountPeriod = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) posClient.AddedBonus = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) posClient.SubstractBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) posClient.QtyRefundPeriod = reader.GetInt32(12);
                if (!reader.IsDBNull(13)) posClient.SumRefundPeriod = reader.GetDecimal(13);
                if (!reader.IsDBNull(14)) posClient.Balance = reader.GetDecimal(14);
                if (!reader.IsDBNull(15)) posClient.LevelCondition = reader.GetString(15);
                if (!reader.IsDBNull(16)) posClient.BirthDate = reader.GetDateTime(16);
                if (!reader.IsDBNull(17)) posClient.AllowSms = reader.GetBoolean(17);
                if (!reader.IsDBNull(18)) posClient.AllowEmail = reader.GetBoolean(18);
                if (!reader.IsDBNull(19)) posClient.AddedBonusUnbuy = reader.GetDecimal(19);
                posClients.Add(posClient);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = string.Format("Отчёт по клиентам за период с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));

                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Торговая точка Регистратор Участника";

                worksheet.Cells["C2:K2"].Merge = true;
                worksheet.Cells["C2:K2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:K2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:K2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Личные данные Участника";

                worksheet.Cells["L2:S2"].Merge = true;
                worksheet.Cells["L2:S2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["L2:S2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00FFFF"));
                worksheet.Cells["L2:S2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L2"].Value = "Активность участника в торговой точке Программы за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "Согласие на отправку смс";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "E-mail";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Согласие на отправку Email";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I3"].Value = "Номер карты";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Тип участника";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["K3"].Value = "Дата рождения";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Количество покупок";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Cумма покупок";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["N3"].Value = "Начислено бонусов";
                worksheet.Cells["N3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["O3"].Value = "Списано бонусов";
                worksheet.Cells["O3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["P3"].Value = "Начислено бонусов не за покупки";
                worksheet.Cells["P3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["Q3"].Value = "Количество возвратов";
                worksheet.Cells["Q3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["R3"].Value = "Сумма возвратов";
                worksheet.Cells["R3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["S3"].Value = "Бонусный баланс";
                worksheet.Cells["S3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["T3"].Value = "Процент начисления";
                worksheet.Cells["T3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:T2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:T2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:T3"].AutoFilter = true;
                worksheet.Cells["A3:T3"].Style.WrapText = true;
                worksheet.Cells["A3:T3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:T3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A3:T3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:T3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.View.FreezePanes(4, 1);
                for (int i = 1; i < 21; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = posClients.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = posClients[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = posClients[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = posClients[i].Name;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = posClients[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = posClients[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeSms = "Нет";
                    if (posClients[i].AllowSms)
                    {
                        agreeSms = "Да";
                    }

                    worksheet.Cells["F" + (i + 4).ToString()].Value = agreeSms;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = posClients[i].Email;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeEmail = "Нет";
                    if (posClients[i].AllowEmail)
                    {
                        agreeEmail = "Да";
                    }

                    worksheet.Cells["H" + (i + 4).ToString()].Value = agreeEmail;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = posClients[i].Card;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = posClients[i].ClientType;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    if (posClients[i].BirthDate.HasValue)
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = posClients[i].BirthDate.Value.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = "Не указана дата рождения";
                    }
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = posClients[i].QtyBuysPeriod;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = posClients[i].SumAmountPeriod;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["N" + (i + 4).ToString()].Value = posClients[i].AddedBonus;
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["O" + (i + 4).ToString()].Value = posClients[i].SubstractBonus;
                    worksheet.Cells["O" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["P" + (i + 4).ToString()].Value = posClients[i].AddedBonusUnbuy;
                    worksheet.Cells["P" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["Q" + (i + 4).ToString()].Value = posClients[i].QtyRefundPeriod;
                    worksheet.Cells["Q" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["R" + (i + 4).ToString()].Value = posClients[i].SumRefundPeriod;
                    worksheet.Cells["R" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["S" + (i + 4).ToString()].Value = posClients[i].Balance;
                    worksheet.Cells["S" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["T" + (i + 4).ToString()].Value = posClients[i].LevelCondition;
                    worksheet.Cells["T" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class OperatorSale
    {
        public string Brand { get; set; }
        public string Address { get; set; }
        public string ClientName { get; set; }
        public string Gender { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public Int64 Card { get; set; }
        public string ClientType { get; set; }
        public DateTime ChequeTime { get; set; }
        public string OperationType { get; set; }
        public decimal Amount { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
    }

    public class OperatorSalesRequest
    {
        public Int16 Operator { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class OperatorSalesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerOperatorSales
    {
        public OperatorSalesResponse ProcessRequest(SqlConnection cnn, OperatorSalesRequest request)
        {
            OperatorSalesResponse returnValue = new OperatorSalesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.OperatorSales";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<OperatorSale> operatorSales = new List<OperatorSale>();
            while (reader.Read())
            {
                OperatorSale operatorSale = new OperatorSale();
                if (!reader.IsDBNull(0)) operatorSale.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) operatorSale.Address = reader.GetString(1);
                if (!reader.IsDBNull(2)) operatorSale.ClientName = reader.GetString(2);
                if (!reader.IsDBNull(3)) operatorSale.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) operatorSale.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) operatorSale.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) operatorSale.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) operatorSale.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) operatorSale.ChequeTime = reader.GetDateTime(8);
                if (!reader.IsDBNull(9)) operatorSale.OperationType = reader.GetString(9);
                if (!reader.IsDBNull(10)) operatorSale.Amount = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) operatorSale.AddedBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) operatorSale.SubstractBonus = reader.GetDecimal(12);
                operatorSales.Add(operatorSale);
            }
            reader.Close();
            cnn.Close();
            string reportName = string.Format("Отчёт о продажах по программе лояльности с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.View.FreezePanes(4, 1);
                worksheet.Cells["A1"].Value = reportName;
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Партнёр";

                worksheet.Cells["C2:H2"].Merge = true;
                worksheet.Cells["C2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:H2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Личные данные Участника";

                worksheet.Cells["I2:M2"].Merge = true;
                worksheet.Cells["I2:M2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["I2:M2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["I2:M2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I2"].Value = "Операции Участников за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "E-mail";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "Номер карты";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Тип участника";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["I3"].Value = "Дата операции";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Операция";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["K3"].Value = "Сумма операции";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Начислено бонусов";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Списано бонусов";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:M2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:M2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:M3"].AutoFilter = true;
                worksheet.Cells["A3:M3"].Style.WrapText = true;
                worksheet.Cells["A3:M3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:M3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                worksheet.Cells["A3:M3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:M3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                for (int i = 1; i < 15; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = operatorSales.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = operatorSales[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = operatorSales[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = operatorSales[i].ClientName;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = operatorSales[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = operatorSales[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["F" + (i + 4).ToString()].Value = operatorSales[i].Email;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = operatorSales[i].Card;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["H" + (i + 4).ToString()].Value = operatorSales[i].ClientType;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = operatorSales[i].ChequeTime.ToString("dd.MM.yyyy HH:mm"); ;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = operatorSales[i].OperationType;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["K" + (i + 4).ToString()].Value = operatorSales[i].Amount;
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = operatorSales[i].AddedBonus;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = operatorSales[i].SubstractBonus;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class OperatorBookkeeping
    {
        public string Brand { get; set; }
        public decimal BuySum { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
        public decimal RefundSum { get; set; }
        public decimal AddedBonusRefund { get; set; }
        public decimal SubstractBonusRefund { get; set; }
        public decimal WelcomeBonus { get; set; }
        public decimal BirthdayBonus { get; set; }
        public int QtyClient { get; set; }
        //public decimal PercentComission { get; set; }
        //public decimal PaySum { get; set; }
    }

    public class OperatorBookkeepInfo
    {
        public string OperatorName { get; set; }
        public decimal WelcomeBonus { get; set; }
        public decimal BirthdayBonus { get; set; }
        public int QtyClient { get; set; }
    }

    public class OperatorBookkeepingRequest
    {
        public Int16 Operator { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class OperatorBookkeepingResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerOperatorBookkeeping
    {
        public OperatorBookkeepingResponse ProcessRequest(SqlConnection cnn, OperatorBookkeepingRequest request)
        {
            OperatorBookkeepingResponse returnValue = new OperatorBookkeepingResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.OperatorBookkeeping";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@operatorName", SqlDbType.NVarChar, 20);
            cmd.Parameters["@operatorName"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@welcomebonus", SqlDbType.Decimal);
            cmd.Parameters["@welcomebonus"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@birthdaybonus", SqlDbType.Decimal);
            cmd.Parameters["@birthdaybonus"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@qtyclient", SqlDbType.Int, 20);
            cmd.Parameters["@qtyclient"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<OperatorBookkeeping> operatorBookkeeping = new List<OperatorBookkeeping>();
            while (reader.Read())
            {
                OperatorBookkeeping operatorBookkeep = new OperatorBookkeeping();
                if (!reader.IsDBNull(0)) operatorBookkeep.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) operatorBookkeep.BuySum = reader.GetDecimal(1);
                if (!reader.IsDBNull(2)) operatorBookkeep.AddedBonus = reader.GetDecimal(2);
                if (!reader.IsDBNull(3)) operatorBookkeep.SubstractBonus = reader.GetDecimal(3);
                if (!reader.IsDBNull(4)) operatorBookkeep.RefundSum = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) operatorBookkeep.AddedBonusRefund = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) operatorBookkeep.SubstractBonusRefund = reader.GetDecimal(6);
                if (!reader.IsDBNull(7)) operatorBookkeep.QtyClient = reader.GetInt32(7);
                //operatorBookkeep.PercentComission = operatorBookkeep.AddedBonus / 100 * 1;
                //operatorBookkeep.PaySum = operatorBookkeep.AddedBonus - operatorBookkeep.SubstractBonus + operatorBookkeep.PercentComission;
                operatorBookkeeping.Add(operatorBookkeep);
            }
            reader.Close();
            OperatorBookkeepInfo operatorInfo = new OperatorBookkeepInfo();
            operatorInfo.BirthdayBonus = Convert.ToDecimal(cmd.Parameters["@birthdaybonus"].Value);
            operatorInfo.OperatorName = Convert.ToString(cmd.Parameters["@operatorName"].Value);
            operatorInfo.QtyClient = Convert.ToInt32(cmd.Parameters["@qtyclient"].Value);
            operatorInfo.WelcomeBonus = Convert.ToDecimal(cmd.Parameters["@welcomebonus"].Value);
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = string.Format("Отчёт для бухгалтерии за период с {0} - по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));

                worksheet.Cells["A2"].Value = "Бренд";
                worksheet.Cells["A2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B2"].Value = "Сумма покупок";
                worksheet.Cells["B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Начислено бонусов за покупки";
                worksheet.Cells["C2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D2"].Value = "Списано бонусов за покупки";
                worksheet.Cells["D2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E2"].Value = "Сумма возвратов";
                worksheet.Cells["E2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F2"].Value = "Начислено бонусов за возвраты";
                worksheet.Cells["F2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["G2"].Value = "Списано бонусов за возвраты";
                worksheet.Cells["G2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["H2"].Value = "Начислено welcome бонусов";
                //worksheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["I2"].Value = "Начислено birthday бонусов";
                //worksheet.Cells["I2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H2"].Value = "Зарегистрировано участников";
                worksheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["E1"].Value = "Комиссия 1%";
                //worksheet.Cells["E1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["F1"].Value = "Сумма к оплате партнёром";
                //worksheet.Cells["F1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:H2"].AutoFilter = true;
                worksheet.Cells["A2:H2"].Style.WrapText = true;
                worksheet.Cells["A2:H2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:H2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.View.FreezePanes(4, 1);
                for (int i = 1; i < 11; i++)
                {
                    worksheet.Column(i).Width = 22;
                }
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A3"].Value = operatorInfo.OperatorName;

                //worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["H3"].Value = operatorInfo.WelcomeBonus;
                //worksheet.Cells["H3"].Style.Numberformat.Format = "0.00";

                //worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["I3"].Value = operatorInfo.BirthdayBonus;
                //worksheet.Cells["I3"].Style.Numberformat.Format = "0.00";

                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = operatorInfo.QtyClient;
                worksheet.Cells["H3"].Style.Numberformat.Format = "0";

                for (int i = 0, n = operatorBookkeeping.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = operatorBookkeeping[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = operatorBookkeeping[i].BuySum;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = operatorBookkeeping[i].AddedBonus;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["D" + (i + 4).ToString()].Value = operatorBookkeeping[i].SubstractBonus;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["E" + (i + 4).ToString()].Value = operatorBookkeeping[i].RefundSum;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["F" + (i + 4).ToString()].Value = operatorBookkeeping[i].AddedBonusRefund;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = operatorBookkeeping[i].SubstractBonusRefund;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";

                    //worksheet.Cells["H" + (i + 4).ToString()].Value = operatorBookkeeping[i].WelcomeBonus;
                    //worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    //worksheet.Cells["H" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";

                    //worksheet.Cells["I" + (i + 4).ToString()].Value = operatorBookkeeping[i].BirthdayBonus;
                    //worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    //worksheet.Cells["I" + (i + 4).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["H" + (i + 4).ToString()].Value = operatorBookkeeping[i].QtyClient;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                }
                worksheet.Cells["A" + (operatorBookkeeping.Count() + 4).ToString()].Value = "Итого";
                worksheet.Cells["A" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["B" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(x => x.BuySum);
                worksheet.Cells["B" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["C" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(s => s.AddedBonus);
                worksheet.Cells["C" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["D" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(s => s.SubstractBonus);
                worksheet.Cells["D" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["E" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(s => s.RefundSum);
                worksheet.Cells["E" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["F" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(s => s.AddedBonusRefund);
                worksheet.Cells["F" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["G" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorBookkeeping.Sum(s => s.SubstractBonusRefund);
                worksheet.Cells["G" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                //worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorInfo.WelcomeBonus;
                //worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                //worksheet.Cells["I" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorInfo.BirthdayBonus;
                //worksheet.Cells["I" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["I" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0.00";

                worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Value = operatorInfo.QtyClient + operatorBookkeeping.Sum(s => s.QtyClient);
                worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H" + (operatorBookkeeping.Count() + 4).ToString()].Style.Numberformat.Format = "0";

                worksheet.Cells["A" + (operatorBookkeeping.Count() + 4).ToString() + ":J" + (operatorBookkeeping.Count() + 4).ToString()].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A" + (operatorBookkeeping.Count() + 4).ToString() + ":J" + (operatorBookkeeping.Count() + 4).ToString()].Style.Fill.BackgroundColor.SetColor(color);

                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class PartnerBookkeeping
    {
        public string Brand { get; set; }
        public decimal BuySum { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal SubstractBonus { get; set; }
        public decimal RefundSum { get; set; }
        public decimal AddedBonusRefund { get; set; }
        public decimal SubstractBonusRefund { get; set; }
        public decimal WelcomeBonus { get; set; }
        public decimal BirthdayBonus { get; set; }
        public int QtyClient { get; set; }
        //public decimal PercentComission { get; set; }
        //public decimal PaySum { get; set; }
    }

    public class PartnerBookkeepingRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class PartnerBookkeepingResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerPartnerBookkeeping
    {
        public PartnerBookkeepingResponse ProcessRequest(SqlConnection cnn, PartnerBookkeepingRequest request)
        {
            PartnerBookkeepingResponse returnValue = new PartnerBookkeepingResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.PartnerBookkeeping";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PartnerBookkeeping> partnerBookkeeping = new List<PartnerBookkeeping>();
            while (reader.Read())
            {
                PartnerBookkeeping partnerBookkeep = new PartnerBookkeeping();
                if (!reader.IsDBNull(0)) partnerBookkeep.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) partnerBookkeep.BuySum = reader.GetDecimal(1);
                if (!reader.IsDBNull(2)) partnerBookkeep.AddedBonus = reader.GetDecimal(2);
                if (!reader.IsDBNull(3)) partnerBookkeep.SubstractBonus = reader.GetDecimal(3);
                if (!reader.IsDBNull(4)) partnerBookkeep.RefundSum = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) partnerBookkeep.AddedBonusRefund = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) partnerBookkeep.SubstractBonusRefund = reader.GetDecimal(6);
                if (!reader.IsDBNull(7)) partnerBookkeep.QtyClient = reader.GetInt32(7);
                //partnerBookkeep.PercentComission = partnerBookkeep.AddedBonus / 100 * 1;
                //partnerBookkeep.PaySum = partnerBookkeep.AddedBonus - partnerBookkeep.SubstractBonus + partnerBookkeep.PercentComission;
                partnerBookkeeping.Add(partnerBookkeep);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = string.Format("Отчёт для бухгалтерии за период с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));

                worksheet.Cells["A2"].Value = "Бренд";
                worksheet.Cells["A2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B2"].Value = "Сумма покупок";
                worksheet.Cells["B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Начислено бонусов";
                worksheet.Cells["C2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D2"].Value = "Списано бонусов";
                worksheet.Cells["D2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E2"].Value = "Сумма возвратов";
                worksheet.Cells["E2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F2"].Value = "Начислено бонусов за возвраты";
                worksheet.Cells["F2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G2"].Value = "Списано бонусов за возвраты";
                worksheet.Cells["G2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["H2"].Value = "Начислено welcome бонусов";
                //worksheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["I2"].Value = "Начислено birthday бонусов";
                //worksheet.Cells["I2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H2"].Value = "Зарегистрировано участников";
                worksheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["E1"].Value = "Комиссия 1%";
                //worksheet.Cells["E1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                //worksheet.Cells["F1"].Value = "Сумма к оплате партнёром";
                //worksheet.Cells["F1"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:H2"].AutoFilter = true;
                worksheet.Cells["A2:H2"].Style.WrapText = true;
                worksheet.Cells["A2:H2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:H2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:H2"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.View.FreezePanes(4, 1);
                for (int i = 1; i < 12; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = partnerBookkeeping.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 3).ToString()].Value = partnerBookkeeping[i].Brand;
                    worksheet.Cells["A" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 3).ToString()].Value = partnerBookkeeping[i].BuySum;
                    worksheet.Cells["B" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 3).ToString()].Value = partnerBookkeeping[i].AddedBonus;
                    worksheet.Cells["C" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["C" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["D" + (i + 3).ToString()].Value = partnerBookkeeping[i].SubstractBonus;
                    worksheet.Cells["D" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["E" + (i + 3).ToString()].Value = partnerBookkeeping[i].RefundSum;
                    worksheet.Cells["E" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["E" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["F" + (i + 3).ToString()].Value = partnerBookkeeping[i].AddedBonusRefund;
                    worksheet.Cells["F" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";
                    worksheet.Cells["F" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 3).ToString()].Value = partnerBookkeeping[i].SubstractBonusRefund;
                    worksheet.Cells["G" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";

                    //worksheet.Cells["H" + (i + 3).ToString()].Value = partnerBookkeeping[i].WelcomeBonus;
                    //worksheet.Cells["H" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    //worksheet.Cells["H" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";

                    //worksheet.Cells["I" + (i + 3).ToString()].Value = partnerBookkeeping[i].BirthdayBonus;
                    //worksheet.Cells["I" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    //worksheet.Cells["I" + (i + 3).ToString()].Style.Numberformat.Format = "0.00";

                    worksheet.Cells["H" + (i + 3).ToString()].Value = partnerBookkeeping[i].QtyClient;
                    worksheet.Cells["H" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["H" + (i + 3).ToString()].Style.Numberformat.Format = "0";
                }
                returnValue.Report = package.GetAsByteArray(); ;
            }
            return returnValue;
        }
    }

    public class PartnerClientRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class PartnerClientResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerPartnerClient
    {
        public PartnerClientResponse ProcessRequest(SqlConnection cnn, PartnerClientRequest request)
        {
            PartnerClientResponse returnValue = new PartnerClientResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.PartnerClient";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PosClient> posClients = new List<PosClient>();
            while (reader.Read())
            {
                PosClient posClient = new PosClient();
                if (!reader.IsDBNull(0)) posClient.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) posClient.Address = reader.GetString(1);
                if (!reader.IsDBNull(2)) posClient.Name = reader.GetString(2);
                if (!reader.IsDBNull(3)) posClient.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) posClient.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) posClient.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) posClient.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) posClient.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) posClient.QtyBuysPeriod = reader.GetInt32(8);
                if (!reader.IsDBNull(9)) posClient.SumAmountPeriod = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) posClient.AddedBonus = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) posClient.SubstractBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) posClient.QtyRefundPeriod = reader.GetInt32(12);
                if (!reader.IsDBNull(13)) posClient.SumRefundPeriod = reader.GetDecimal(13);
                if (!reader.IsDBNull(14)) posClient.Balance = reader.GetDecimal(14);
                if (!reader.IsDBNull(15)) posClient.LevelCondition = reader.GetString(15);
                if (!reader.IsDBNull(16)) posClient.BirthDate = reader.GetDateTime(16);
                if (!reader.IsDBNull(17)) posClient.AllowSms = reader.GetBoolean(17);
                if (!reader.IsDBNull(18)) posClient.AllowEmail = reader.GetBoolean(18);
                if (!reader.IsDBNull(19)) posClient.AddedBonusUnbuy = reader.GetDecimal(19);
                posClients.Add(posClient);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].Value = string.Format("Отчёт по клиентам за период с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDDDDD"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Торговая точка Регистратор Участника";

                worksheet.Cells["C2:K2"].Merge = true;
                worksheet.Cells["C2:K2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:K2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:K2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Личные данные Участника";

                worksheet.Cells["L2:S2"].Merge = true;
                worksheet.Cells["L2:S2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["L2:S2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00FFFF"));
                worksheet.Cells["L2:S2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L2"].Value = "Активность участника в торговой точке Программы за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "Согласие на отправку смс";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "E-mail";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Согласие на отправку Email";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I3"].Value = "Номер карты";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Тип участника";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["K3"].Value = "Дата рождения";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Количество покупок";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Cумма покупок";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["N3"].Value = "Начислено бонусов";
                worksheet.Cells["N3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["O3"].Value = "Списано бонусов";
                worksheet.Cells["O3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["P3"].Value = "Начислено бонусов не за покупки";
                worksheet.Cells["P3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["Q3"].Value = "Количество возвратов";
                worksheet.Cells["Q3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["R3"].Value = "Сумма возвратов";
                worksheet.Cells["R3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["S3"].Value = "Бонусный баланс";
                worksheet.Cells["S3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["T3"].Value = "Процент начисления";
                worksheet.Cells["T3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:T2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:T2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:T3"].AutoFilter = true;
                worksheet.Cells["A3:T3"].Style.WrapText = true;
                worksheet.Cells["A3:T3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:T3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A3:T3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:T3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.View.FreezePanes(4, 1);
                for (int i = 1; i < 21; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = posClients.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = posClients[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = posClients[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = posClients[i].Name;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = posClients[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = posClients[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeSms = "Нет";
                    if (posClients[i].AllowSms)
                    {
                        agreeSms = "Да";
                    }

                    worksheet.Cells["F" + (i + 4).ToString()].Value = agreeSms;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = posClients[i].Email;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    string agreeEmail = "Нет";
                    if (posClients[i].AllowEmail)
                    {
                        agreeEmail = "Да";
                    }

                    worksheet.Cells["H" + (i + 4).ToString()].Value = agreeEmail;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = posClients[i].Card;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = posClients[i].ClientType;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    if (posClients[i].BirthDate.HasValue)
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = posClients[i].BirthDate.Value.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        worksheet.Cells["K" + (i + 4).ToString()].Value = "Не указана дата рождения";
                    }
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = posClients[i].QtyBuysPeriod;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = posClients[i].SumAmountPeriod;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["N" + (i + 4).ToString()].Value = posClients[i].AddedBonus;
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["O" + (i + 4).ToString()].Value = posClients[i].SubstractBonus;
                    worksheet.Cells["O" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["P" + (i + 4).ToString()].Value = posClients[i].AddedBonusUnbuy;
                    worksheet.Cells["P" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["Q" + (i + 4).ToString()].Value = posClients[i].QtyRefundPeriod;
                    worksheet.Cells["Q" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["R" + (i + 4).ToString()].Value = posClients[i].SumRefundPeriod;
                    worksheet.Cells["R" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["S" + (i + 4).ToString()].Value = posClients[i].Balance;
                    worksheet.Cells["S" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["T" + (i + 4).ToString()].Value = posClients[i].LevelCondition;
                    worksheet.Cells["T" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }

    public class ServerPartnerSalePeriodResponse
    {
        public PosClientPeriodResponse ProcessRequest(SqlConnection cnn, PartnerClientRequest request)
        {
            PosClientPeriodResponse returnValue = new PosClientPeriodResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.PartnerSales";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<PosSale> posSales = new List<PosSale>();
            while (reader.Read())
            {
                PosSale posSale = new PosSale();
                if (!reader.IsDBNull(0)) posSale.Brand = reader.GetString(0);
                if (!reader.IsDBNull(1)) posSale.Address = reader.GetString(1);

                if (!reader.IsDBNull(2)) posSale.ClientName = reader.GetString(2);
                if (!reader.IsDBNull(3)) posSale.Gender = reader.GetString(3);
                if (!reader.IsDBNull(4)) posSale.Phone = 80000000000 + reader.GetInt64(4);
                if (!reader.IsDBNull(5)) posSale.Email = reader.GetString(5);

                if (!reader.IsDBNull(6)) posSale.Card = reader.GetInt64(6);
                if (!reader.IsDBNull(7)) posSale.ClientType = reader.GetString(7);
                if (!reader.IsDBNull(8)) posSale.ThisPosRegister = reader.GetString(8);
                if (!reader.IsDBNull(9)) posSale.OperationType = reader.GetString(9);
                if (!reader.IsDBNull(10)) posSale.Amount = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) posSale.AddedBonus = reader.GetDecimal(11);
                if (!reader.IsDBNull(12)) posSale.SubstractBonus = reader.GetDecimal(12);
                if (!reader.IsDBNull(13)) posSale.ChequeTime = reader.GetDateTime(13);
                posSales.Add(posSale);
            }
            reader.Close();
            cnn.Close();
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.View.FreezePanes(4, 1);
                worksheet.Cells["A1"].Value = string.Format("Отчёт по продажам за период с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));
                worksheet.Cells["A2:B2"].Merge = true;
                worksheet.Cells["A2:B2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2:B2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["A2:B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2"].Value = "Партнёр";

                worksheet.Cells["C2:H2"].Merge = true;
                worksheet.Cells["C2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["C2:H2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));
                worksheet.Cells["C2:H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["C2"].Value = "Участник";

                worksheet.Cells["I2:N2"].Merge = true;
                worksheet.Cells["I2:N2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["I2:N2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#A6FA98"));
                worksheet.Cells["I2:N2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["I2"].Value = "Операции Участника за период";

                worksheet.Cells["A3"].Value = "Бренд";
                worksheet.Cells["A3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B3"].Value = "Торговая точка";
                worksheet.Cells["B3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["C3"].Value = "ФИО";
                worksheet.Cells["C3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D3"].Value = "Пол";
                worksheet.Cells["D3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E3"].Value = "Номер телефона";
                worksheet.Cells["E3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F3"].Value = "E-mail";
                worksheet.Cells["F3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G3"].Value = "Номер карты";
                worksheet.Cells["G3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H3"].Value = "Тип участника";
                worksheet.Cells["H3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["I3"].Value = "Регистрация в моей ТТ";
                worksheet.Cells["I3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J3"].Value = "Дата операции";
                worksheet.Cells["J3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["K3"].Value = "Операция";
                worksheet.Cells["K3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["L3"].Value = "Сумма операции";
                worksheet.Cells["L3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["M3"].Value = "Начислено бонусов";
                worksheet.Cells["M3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["N3"].Value = "Списано бонусов";
                worksheet.Cells["N3"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["A2:N2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:N2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A3:N3"].AutoFilter = true;
                worksheet.Cells["A3:N3"].Style.WrapText = true;
                worksheet.Cells["A3:N3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A3:N3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#F2F2F2");
                worksheet.Cells["A3:N3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A3:N3"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                for (int i = 1; i < 15; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = posSales.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 4).ToString()].Value = posSales[i].Brand;
                    worksheet.Cells["A" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 4).ToString()].Value = posSales[i].Address;
                    worksheet.Cells["B" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 4).ToString()].Value = posSales[i].ClientName;
                    worksheet.Cells["C" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["D" + (i + 4).ToString()].Value = posSales[i].Gender;
                    worksheet.Cells["D" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["E" + (i + 4).ToString()].Value = posSales[i].Phone;
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["E" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["F" + (i + 4).ToString()].Value = posSales[i].Email;
                    worksheet.Cells["F" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 4).ToString()].Value = posSales[i].Card;
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 4).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["H" + (i + 4).ToString()].Value = posSales[i].ClientType;
                    worksheet.Cells["H" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 4).ToString()].Value = posSales[i].ThisPosRegister;
                    worksheet.Cells["I" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["J" + (i + 4).ToString()].Value = posSales[i].ChequeTime.ToString("dd.MM.yyyy HH:mm"); ;
                    worksheet.Cells["J" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["K" + (i + 4).ToString()].Value = posSales[i].OperationType;
                    worksheet.Cells["K" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["L" + (i + 4).ToString()].Value = posSales[i].Amount;
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["L" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["M" + (i + 4).ToString()].Value = posSales[i].AddedBonus;
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["M" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["N" + (i + 4).ToString()].Value = posSales[i].SubstractBonus;
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Numberformat.Format = "0";
                    worksheet.Cells["N" + (i + 4).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }

            return returnValue;
        }
    }

    public class OperatorBonusSource
    {
        public DateTime BonusTime { get; set; }
        public string BonusSource { get; set; }
        public decimal BonusAdded { get; set; }
        public decimal BonusRedeemed { get; set; }
        public string ClientName { get; set; }
        public string Gender { get; set; }
        public long Phone { get; set; }
        public string Email { get; set; }
        public long Card { get; set; }
        public string ClientType { get; set; }
    }

    public class OperatorBonusSourceRequest
    {
        public Int16 Operator { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class OperatorBonusSourceResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public byte[] Report { get; set; }
    }

    public class ServerOperatorBonusSource
    {
        public OperatorBonusSourceResponse ProcessRequest(SqlConnection cnn, OperatorBonusSourceRequest request)
        {
            var returnValue = new OperatorBonusSourceResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Reports.OperatorBonus";
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@from", request.From);
            cmd.Parameters.AddWithValue("@to", request.To);
            cmd.Parameters.Add("@errormessage", System.Data.SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            List<OperatorBonusSource> bonusSource = new List<OperatorBonusSource>();
            while (reader.Read())
            {
                OperatorBonusSource bonus = new OperatorBonusSource();
                if (!reader.IsDBNull(0)) bonus.BonusTime = reader.GetDateTime(0);
                if (!reader.IsDBNull(1)) bonus.BonusSource = reader.GetString(1);
                if (!reader.IsDBNull(2)) bonus.BonusAdded = reader.GetDecimal(2);
                if (!reader.IsDBNull(3)) bonus.BonusRedeemed = reader.GetDecimal(3);
                if (!reader.IsDBNull(4)) bonus.ClientName = reader.GetString(4);
                if (!reader.IsDBNull(5)) bonus.Gender = reader.GetString(5);
                if (!reader.IsDBNull(6)) bonus.Phone = 80000000000 + reader.GetInt64(6);
                if (!reader.IsDBNull(7)) bonus.Email = reader.GetString(7);
                if (!reader.IsDBNull(8)) bonus.Card = reader.GetInt64(8);
                if (!reader.IsDBNull(9)) bonus.ClientType = reader.GetString(9);
                bonusSource.Add(bonus);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            using (ExcelPackage package = new ExcelPackage())
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.View.FreezePanes(2, 1);
                worksheet.Cells["A1"].Value = string.Format("Отчёт по бонусам не за покупки за период с {0} по {1}", request.From.ToString("dd.MM.yyyy"), request.To.ToString("dd.MM.yyyy"));
                worksheet.Cells["A2"].Value = "Дата операции";
                worksheet.Cells["A2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["B2"].Value = "Операция";
                worksheet.Cells["B2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["C2"].Value = "Начислено";
                worksheet.Cells["C2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["D2"].Value = "Списано";
                worksheet.Cells["D2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["E2"].Value = "ФИО";
                worksheet.Cells["E2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["F2"].Value = "Пол";
                worksheet.Cells["F2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["G2"].Value = "Номер телефона";
                worksheet.Cells["G2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["H2"].Value = "E-mail";
                worksheet.Cells["H2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                worksheet.Cells["I2"].Value = "Номер карты";
                worksheet.Cells["I2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["J2"].Value = "Тип участника";
                worksheet.Cells["J2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                worksheet.Cells["A2:J2"].AutoFilter = true;
                worksheet.Cells["A2:J2"].Style.WrapText = true;
                worksheet.Cells["A2:J2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["A2:J2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                var color = System.Drawing.ColorTranslator.FromHtml("#99FFCC");
                worksheet.Cells["A2:J2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A2"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2F2F2"));
                worksheet.Cells["B2:J2"].Style.Fill.BackgroundColor.SetColor(color);
                //worksheet.Cells["A2:O2"].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                for (int i = 1; i < 11; i++)
                {
                    worksheet.Column(i).Width = 20;
                }
                for (int i = 0, n = bonusSource.Count; i < n; i++)
                {
                    worksheet.Cells["A" + (i + 3).ToString()].Value = bonusSource[i].BonusTime.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells["A" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["B" + (i + 3).ToString()].Value = bonusSource[i].BonusSource;
                    worksheet.Cells["B" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["C" + (i + 3).ToString()].Value = bonusSource[i].BonusAdded;
                    worksheet.Cells["C" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["C" + (i + 3).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["D" + (i + 3).ToString()].Value = bonusSource[i].BonusRedeemed;
                    worksheet.Cells["D" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["D" + (i + 3).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["E" + (i + 3).ToString()].Value = bonusSource[i].ClientName;
                    worksheet.Cells["E" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["F" + (i + 3).ToString()].Value = bonusSource[i].Gender;
                    worksheet.Cells["F" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["G" + (i + 3).ToString()].Value = bonusSource[i].Phone;
                    worksheet.Cells["G" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["G" + (i + 3).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["H" + (i + 3).ToString()].Value = bonusSource[i].Email;
                    worksheet.Cells["H" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);

                    worksheet.Cells["I" + (i + 3).ToString()].Value = bonusSource[i].Card;
                    worksheet.Cells["I" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                    worksheet.Cells["I" + (i + 3).ToString()].Style.Numberformat.Format = "0";

                    worksheet.Cells["J" + (i + 3).ToString()].Value = bonusSource[i].ClientType;
                    worksheet.Cells["J" + (i + 3).ToString()].Style.Border.BorderAround(ExcelBorderStyle.Hair);
                }
                returnValue.Report = package.GetAsByteArray();
            }
            return returnValue;
        }
    }
}