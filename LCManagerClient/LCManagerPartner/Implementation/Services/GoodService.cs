using LCManagerPartner.Implementation.Data;
using LCManagerPartner.Implementation.Request;
using LCManagerPartner.Implementation.Response;
using LCManagerPartner.Models;
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
    /// Сервис для работы со списками товаров оператора
    /// </summary>
    public class GoodService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Сохраняет список магазинов в БД
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorGoodListResponse SaveOperatorGoodList(OperatorGoodListCreateRequest request)
        {
            var returnValue = new OperatorGoodListResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO goodlist (caption, operator) output INSERTED.id VALUES(@Caption, @Operator)";
            cmd.Parameters.AddWithValue("@Caption", request.GoodListName);
            cmd.Parameters.AddWithValue("@Operator", request.Operator);

            var transaction = _cnn.BeginTransaction();
            cmd.Transaction = transaction;
            try
            {
                var id = cmd.ExecuteScalar();
                var query = string.Empty;
                foreach (var item in request.GoodList)
                {
                    query += $"INSERT INTO goodlistitems (goodlist, good) VALUES ({id},{item});\r\n";
                }
                cmd.Parameters.Clear();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                transaction.Commit();
                returnValue.ErrorCode = 0;
                returnValue.Message = string.Empty;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                returnValue.ErrorCode = 3;
                returnValue.Message = e.Message;
            }
            finally
            {
                _cnn.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Получает список списков товаров оператора"
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorGoodListResponse GetGoodListByOperator(OperatorGoodRequest request)
        {
            var response = new OperatorGoodListResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id, caption, operator from goodlist where operator=@id";
            cmd.Parameters.AddWithValue("@id", request.Operator);

            var result = new List<OperatorGoodList>();
            var buf = new List<OperatorGoodList>();
            try
            {
                #region Получаем списки магазинов, созданных оператором
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var goodList = new OperatorGoodList { Id = reader.GetInt16(0) };
                    if (!reader.IsDBNull(1)) goodList.Name = reader.GetString(1);
                    if (!reader.IsDBNull(2)) goodList.Operator = reader.GetInt16(2);
                    buf.Add(goodList);
                }
                reader.Close();
                #endregion

                #region Получаем содержимое списков товаров
                cmd.CommandText =
                            @"select g.id, g.code, g.name from goodlistitems gli 
                    join goodlist gl on gl.id = gli.goodlist and gl.id=@id 
                    join goods g on g.id = gli.good";

                foreach (var item in buf)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", item.Id);
                    var listOfGood = new List<Good>();
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var good = new Good { Id = reader.GetInt16(0) };
                        if (!reader.IsDBNull(1)) good.Code = reader.GetString(1);
                        if (!reader.IsDBNull(2)) good.Name = reader.GetString(2);
                        listOfGood.Add(good);
                    }
                    reader.Close();
                    result.Add(new OperatorGoodList
                    {
                        Id = item.Id,
                        Operator = item.Operator,
                        Name = item.Name,
                        Goods = listOfGood
                    });
                }
                #endregion

                response.GoodLists = result;
                response.ErrorCode = 0;
                response.Message = string.Empty;
            }
            catch (Exception e)
            {
                response.ErrorCode = 3;
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