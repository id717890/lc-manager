using LCManagerPartner.Implementation.Request;
using LCManagerPartner.Implementation.Response;
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
        public OperatorGoodListResponse SaveOperatorPosList(OperatorGoodListCreateRequest request)
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
    }
}