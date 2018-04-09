namespace LCManagerPartner.Implementation.Services
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using Response;
    using Serilog;

    /// <summary>
    /// Класс для записи и проверки Refresh_token в БД.
    /// </summary>
    public class RefreshTokenService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Получает refresh_token пользователя. Если его нет возвращает пустую строку
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ReceiveRefreshTokenForManager(String token)
        {
            try
            {
                var returnValue = new DefaultResponse();
                _cnn.Open();
                SqlCommand cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RefreshTokenCheck";
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                if (returnValue.ErrorCode == 0) return true;
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, "LCManagerAPI");
                return false;
            }
            finally
            {
                _cnn.Close();
            }
        }

        /// <summary>
        /// Получает refresh_token клиента. Если его нет возвращает пустую строку
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ReceiveRefreshTokenForClient(String token)
        {
            try
            {
                var returnValue = new DefaultResponse();
                _cnn.Open();
                SqlCommand cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RefreshTokenCheckForClient";
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                if (returnValue.ErrorCode == 0) return true;
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, "LCManagerAPI");
                return false;
            }
            finally
            {
                _cnn.Close();
            }
        }

        /// <summary>
        /// Обновляет refresh_token у пользователя. Либо обнуляет его если отправлена пустая строка.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public bool UpdateRefreshTokenForManager(String username, String refreshToken)
        {
            try
            {
                var returnValue = new DefaultResponse();
                _cnn.Open();
                SqlCommand cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RefreshTokenUpdate";
                cmd.Parameters.AddWithValue("@login", username);
                cmd.Parameters.AddWithValue("@refresh_token", refreshToken);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                if (returnValue.ErrorCode == 0) return true;
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, "LCManagerAPI");
                return false;
            }
            finally
            {
                _cnn.Close();
            }
        }

        /// <summary>
        /// Обновляет refresh_token у клиента. Либо обнуляет его если отправлена пустая строка.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="oper"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public bool UpdateRefreshTokenForClient(String client, Int16 oper, String refreshToken)
        {
            try
            {
                var returnValue = new DefaultResponse();
                _cnn.Open();
                SqlCommand cmd = _cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "RefreshTokenUpdateForClient";
                cmd.Parameters.AddWithValue("@client", client);
                cmd.Parameters.AddWithValue("@operator", oper);
                cmd.Parameters.AddWithValue("@refresh_token", refreshToken);
                cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@result", SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
                if (returnValue.ErrorCode == 0) return true;
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, "LCManagerAPI");
                return false;
            }
            finally
            {
                _cnn.Close();
            }
        }
    }
}