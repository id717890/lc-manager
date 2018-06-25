namespace LCManagerPartner.Implementation.Services
{
    using Data;
    using Request;
    using Response;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;


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
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorCreateGoodList";
            cmd.Parameters.AddWithValue("@caption", request.GoodListName);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            if (request.GoodList != null && request.GoodList.Count > 0)
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("id", typeof(int));

                    foreach (var item in request.GoodList)
                    {
                        DataRow row = table.NewRow();
                        row["id"] = item;
                        table.Rows.Add(row);
                    }
                    var items = new SqlParameter("@items", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IdItem",
                        Value = table
                    };
                    cmd.Parameters.Add(items);
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception e)
            {
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
        public OperatorGoodListResponse GetGoodListByOperator(OperatorGoodsRequest request)
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

        /// <summary>
        /// Удаляет список товаров из БД
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DefaultResponse RemoveOperatorGoodList(OperatorGoodRemoveRequest request)
        {
            var returnValue = new DefaultResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from goodlist where id=@id";
            cmd.Parameters.AddWithValue("@id", request.OperatorGoodList);

            try
            {
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = 0;
                returnValue.Message = string.Empty;
            }
            catch (Exception e)
            {
                returnValue.ErrorCode = 10;
                returnValue.Message = e.Message;
            }
            finally
            {
                _cnn.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Импортирует товары из файла
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorGoodImportResponse ImportGoodsFromExcel(OperatorGoodImportRequest request)
        {
            OperatorGoodImportResponse returnValue = new OperatorGoodImportResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OperatorImportGoods";
            cmd.Parameters.AddWithValue("@partner", request.Partner);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@insertedrows", SqlDbType.Int);
            cmd.Parameters["@insertedrows"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            if (request.Goods != null && request.Goods.Count > 0)
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("code", typeof(string));
                    table.Columns.Add("brandcode", typeof(string));
                    table.Columns.Add("goodsgroup", typeof(int));
                    table.Columns.Add("brand", typeof(int));
                    table.Columns.Add("noredeem", typeof(bool));
                    table.Columns.Add("nocharge", typeof(bool));
                    table.Columns.Add("price", typeof(decimal));
                    table.Columns.Add("minprice", typeof(decimal));
                    table.Columns.Add("name", typeof(string));
                    table.Columns.Add("catalog", typeof(int));

                    foreach (var item in request.Goods)
                    {
                        DataRow row = table.NewRow();
                        row["code"] = item.Code;
                        row["name"] = item.Name;
                        row["noredeem"] = false;
                        row["nocharge"] = false;
                        table.Rows.Add(row);
                    }
                    var items = new SqlParameter("@gooditems", SqlDbType.Structured)
                    {
                        TypeName = "dbo.GoodItem",
                        Value = table
                    };
                    cmd.Parameters.Add(items);
                }
            }

            try
            {
                cmd.ExecuteNonQuery();
                if (!DBNull.Value.Equals(cmd.Parameters["@insertedrows"].Value))
                {
                    returnValue.ImportedRows = Convert.ToInt32(cmd.Parameters["@insertedrows"].Value);
                }
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception e)
            {
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