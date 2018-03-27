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
        public bool ReceiveRefreshToken(String token)
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
                return true;
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
        public bool UpdateRefreshToken(String username, String refreshToken)
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
                return true;
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