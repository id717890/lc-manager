namespace LCManagerPartner.Implementation.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Data;
    using Request;
    using Response;

    /// <summary>
    /// Сервис для работы ТТ оператора
    /// </summary>
    public class PosService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Получает список ТТ доступных оператору
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorPosResponse GetPosByOperator(OperatorPosRequest request)
        {
            var returnValue = new OperatorPosResponse();
            try
            {
                _cnn.Open();
                SqlCommand cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OperatorGetPos";
                if (request.Operator == 0)
                {
                    request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
                }
                cmd.Parameters.AddWithValue("@operator", request.Operator);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Pos pos = new Pos();                    
                    if (!reader.IsDBNull(0)) pos.Region = reader.GetString(0);
                    if (!reader.IsDBNull(1)) pos.City = reader.GetString(1);
                    if (!reader.IsDBNull(2)) pos.Address = reader.GetString(2);
                    if (!reader.IsDBNull(3)) pos.Id = reader.GetInt16(3);
                    returnValue.Poses.Add(pos);
                }
                reader.Close();



                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, caption, operator from poslist where operator=@id";
                cmd.Parameters.AddWithValue("@id", request.Operator);

                var result = new List<OperatorPosList>();
                var buf = new List<OperatorPosList>();

                #region Получаем списки магазинов, созданных оператором

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var posList = new OperatorPosList {Id = reader.GetInt16(0)};
                    if (!reader.IsDBNull(1)) posList.Caption = reader.GetString(1);
                    if (!reader.IsDBNull(2)) posList.Operator = reader.GetInt16(2);
                    buf.Add(posList);
                }
                reader.Close();

                #endregion

                #region Получаем содержимое списков магазинов

                cmd.CommandText =
                    @"select p.id, c.name, p.address from poslistitems pli 
                    join poslist pl on pl.id = pli.poslist and pl.id=@id 
                    join pos p on p.id = pli.pos 
                    join city c on c.id=p.city";

                foreach (var item in buf)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", item.Id);
                    var listOfPos = new List<Pos>();
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var pos = new Pos {Id = reader.GetInt16(0)};
                        if (!reader.IsDBNull(1)) pos.City = reader.GetString(1);
                        if (!reader.IsDBNull(2)) pos.Address = reader.GetString(2);
                        listOfPos.Add(pos);
                    }
                    reader.Close();
                    result.Add(new OperatorPosList
                    {
                        Id = item.Id,
                        Operator = item.Operator,
                        Caption = item.Caption,
                        Poses = listOfPos
                    });
                }
                #endregion
                returnValue.PosLists = result;
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
        /// Получает список списков магазинов оператора"
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorPosListResponse GetPosListByOperator(OperatorPosListCreateRequest request)
        {
            var response = new OperatorPosListResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id, caption, operator from poslist where operator=@id";
            cmd.Parameters.AddWithValue("@id", request.Operator);

            var result = new List<OperatorPosList>();
            var buf=new List<OperatorPosList>();
            try
            {
                #region Получаем списки магазинов, созданных оператором
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var posList = new OperatorPosList { Id = reader.GetInt16(0) };
                    if (!reader.IsDBNull(1)) posList.Caption = reader.GetString(1);
                    if (!reader.IsDBNull(2)) posList.Operator = reader.GetInt16(2);
                    buf.Add(posList);
                }
                reader.Close();
                #endregion

                #region Получаем содержимое списков магазинов
                cmd.CommandText =
                            @"select p.id, c.name, p.address from poslistitems pli 
                    join poslist pl on pl.id = pli.poslist and pl.id=@id 
                    join pos p on p.id = pli.pos 
                    join city c on c.id=p.city";

                foreach (var item in buf)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", item.Id);
                    var listOfPos = new List<Pos>();
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var pos = new Pos { Id = reader.GetInt16(0) };
                        if (!reader.IsDBNull(1)) pos.City = reader.GetString(1);
                        if (!reader.IsDBNull(2)) pos.Address = reader.GetString(2);
                        listOfPos.Add(pos);
                    }
                    reader.Close();
                    result.Add(new OperatorPosList
                    {
                        Id = item.Id,
                        Operator = item.Operator,
                        Caption = item.Caption,
                        Poses = listOfPos
                    });
                } 
                #endregion

                response.PosLists = result;
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
        /// Сохраняет список магазинов в БД
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperatorPosListResponse SaveOperatorPosList(OperatorPosListCreateRequest request)
        {
            var returnValue = new OperatorPosListResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO poslist (caption, operator) output INSERTED.id VALUES(@Caption, @Operator)";
            cmd.Parameters.AddWithValue("@Caption", request.PosListName);
            cmd.Parameters.AddWithValue("@Operator", request.Operator);

            var transaction = _cnn.BeginTransaction();
            cmd.Transaction = transaction;
            try
            {
                var id = cmd.ExecuteScalar();
                var query = string.Empty;
                foreach (var item in request.PosList)
                {
                    query += $"INSERT INTO poslistitems (poslist, pos) VALUES ({id},{item});\r\n";
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
        /// Удаляет список магазинов из БД
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DefaultResponse RemoveOperatorPosList(OperatorPosRemoveRequest request)
        {
            var returnValue = new DefaultResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from poslist where id=@id";
            cmd.Parameters.AddWithValue("@id", request.OperatorPosList);

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
    }
}