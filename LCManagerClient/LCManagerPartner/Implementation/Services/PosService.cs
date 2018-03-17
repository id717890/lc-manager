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
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        public OperatorPosResponse GetPosByOperator(OperatorPosRequest request)
        {
            var returnValue = new OperatorPosResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
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
                if (!reader.IsDBNull(0)) pos.Id = reader.GetInt16(0);
                if (!reader.IsDBNull(1)) pos.Region = reader.GetString(1);
                if (!reader.IsDBNull(2)) pos.City = reader.GetString(2);
                if (!reader.IsDBNull(3)) pos.Address = reader.GetString(3);
                returnValue.Poses.Add(pos);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }

        public OperatorPosListResponse GetPosListByOperator(OperatorPosListCreateRequest request)
        {
            var response = new OperatorPosListResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select id, caption, operator from poslist where operator=@id";
            cmd.Parameters.AddWithValue("@id", request.Operator);

            var result = new List<OperatorPosList>();
            var buf=new List<OperatorPosList>();
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var posList = new OperatorPosList { Id = reader.GetInt16(0) };
                    if (!reader.IsDBNull(1)) posList.Caption = reader.GetString(1);
                    if (!reader.IsDBNull(2)) posList.Operator = reader.GetInt16(2);
                    buf.Add(posList);
                }
                reader.Close();

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
                cnn.Close();
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
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO poslist (caption, operator) output INSERTED.id VALUES(@Caption, @Operator)";
            cmd.Parameters.AddWithValue("@Caption", request.PosListName);
            cmd.Parameters.AddWithValue("@Operator", request.Operator);

            var transaction = cnn.BeginTransaction();
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
                cnn.Close();
            }
            return returnValue;
        }

    }
}