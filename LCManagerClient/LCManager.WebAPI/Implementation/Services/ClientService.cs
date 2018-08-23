using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;

namespace LCManagerPartner.Implementation.Services
{
    public class ClientService
    {
        readonly SqlConnection _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);

        /// <summary>
        /// Проверяет принадлежит ли email какому-либо участнику ПЛ
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ClientEmailIsFreeResponse CheckEmailForFree(ClientEmailIsFreeRequest request)
        {
            ClientEmailIsFreeResponse returnValue = new ClientEmailIsFreeResponse();
            _cnn.Open();
            SqlCommand cmd = _cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientEmailIsFree";
            cmd.Parameters.AddWithValue("@email", request.Email);
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            try
            {
                cmd.ExecuteNonQuery();
                returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
                returnValue.IsFree = Convert.ToInt32(cmd.Parameters["@result"].Value) == 1;
                returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            }
            catch (Exception ex)
            {
                returnValue.Message = ex.Message;
            }
            _cnn.Close();
            return returnValue;
        }
    }
}