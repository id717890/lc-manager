using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace LCManagerPartner.Models
{
    public class Card
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 number { get; set; }
        /// <summary>
        /// статус
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// виртуальная?
        /// </summary>
        public bool virt { get; set; }
        /// <summary>
        /// ссылка на изображение
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// oper?
        /// </summary>
        public string oper { get; set; }
        public Card() { }
    }

    public class GetClientCardsRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class GetClientCardsResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// детализация информации по картам
        /// </summary>
        public List<Card> CardData { get; set; }
        public GetClientCardsResponse()
        {
            CardData = new List<Card>();
        }
    }

    /// <summary>
    /// Заглушка
    /// </summary>
    public class ServerGetClientCardsResponse
    {
        public GetClientCardsResponse ProcessRequest(SqlConnection cnn, GetClientCardsRequest request)
        {
            GetClientCardsResponse returnValue = new GetClientCardsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientCards";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Card card = new Card();
                card.number = reader.GetInt64(0);
                if (!reader.IsDBNull(1)) card.status = reader.GetString(1);
                if (!reader.IsDBNull(2)) card.virt = reader.GetBoolean(2);
                card.image = "http://plizcard.ru/gfx/fish/card1.png";
                returnValue.CardData.Add(card);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientPartnerSelectRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID партнера
        /// </summary>
        public int PartnerID { get; set; }
        /// <summary>
        /// Добавление \ удаление? (1 - отвязка, 0 - привязка)
        /// </summary>
        public bool Remove { get; set; }
    }

    public class ClientPartnerSelectResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public ClientPartnerSelectResponse() { }
    }

    public class ServerClientPartnerSelectResponse
    {
        public ClientPartnerSelectResponse ProcessRequest(SqlConnection cnn, ClientPartnerSelectRequest request)
        {
            ClientPartnerSelectResponse returnValue = new ClientPartnerSelectResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientPartnerChoose";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.AddWithValue("@remove", request.Remove);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientCampaignSelectRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID акции
        /// </summary>
        public int CampaignID { get; set; }
        /// <summary>
        /// Добавление \ удаление?
        /// </summary>
        public bool Remove { get; set; }
    }

    public class ClientCampaignSelectResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public ClientCampaignSelectResponse() { }
    }

    public class ServerClientCampaignSelectResponse
    {
        public ClientCampaignSelectResponse ProcessRequest(SqlConnection cnn, ClientCampaignSelectRequest request)
        {
            ClientCampaignSelectResponse returnValue = new ClientCampaignSelectResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientCampaignChoose";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@campaign", request.CampaignID);
            cmd.Parameters.AddWithValue("@remove", request.Remove);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientAddCardRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// ID партнера
        /// </summary>
        public int PartnerID { get; set; }
    }

    public class ClientAddCardResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public ClientAddCardResponse() { }
    }

    public class ServerClientAddCardResponse
    {
        public ClientAddCardResponse ProcessRequest(SqlConnection cnn, ClientAddCardRequest request)
        {
            ClientAddCardResponse returnValue = new ClientAddCardResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddCard";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class AddEmailRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// адрес электронной почты участника программы лояльности
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class AddEmailResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class ServerAddEmailResponse
    {
        public AddEmailResponse ProcessRequest(SqlConnection cnn, AddEmailRequest request)
        {
            AddEmailResponse returnValue = new AddEmailResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddEmail";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@email", request.Email);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class AddIDFBRequest
    {
        public string CodeFB { get; set; }
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
    }

    public class AddIDFBResponse
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
}

    public class FBUser
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        public Int64 IDFB { get; set; }
    }

    public class FBResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
    }

    public class FBUserResponse
    {
        public FBUserDataResponse data { get; set; }
    }

    public class FBUserDataResponse
    {
        public long user_id { get; set; }
    }

    public class ServerAddIDFBResponse
    {
        public AddIDFBResponse ProcessRequest(SqlConnection cnn, AddIDFBRequest request)
        {
            AddIDFBResponse returnValue = new AddIDFBResponse();
            var user = new FBUser();
            string socialNetworkAuth = ConfigurationManager.AppSettings["SocialNetwork"].ToString();
            string clientId = ConfigurationManager.AppSettings["FBClientid"].ToString();
            string clientSecret = ConfigurationManager.AppSettings["FBClientSecret"].ToString();
            if (request.CodeFB.StartsWith("web"))
            {
                var client = new RestClient();
                client.EndPoint = @"https://graph.facebook.com/v2.8/oauth/access_token?";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/x-www-form-urlencoded";
                client.PostData = @"code=" + request.CodeFB.Substring(3) + @"&client_id=" + clientId + "&client_secret=" + clientSecret + "&redirect_uri=" + socialNetworkAuth + "%3Ffb=1";
                string json = client.MakeRequest();

                var data = new JavaScriptSerializer().Deserialize<FBResponse>(json);

                var clientMarker = new RestClient();
                clientMarker.EndPoint = @"https://graph.facebook.com/v2.8/oauth/access_token?";
                clientMarker.Method = HttpVerb.POST;
                clientMarker.ContentType = "application/x-www-form-urlencoded";
                clientMarker.PostData = "client_id=" + clientId + "&client_secret=" + clientSecret + "&grant_type=client_credentials";
                string jsonMarker = clientMarker.MakeRequest();
                var marker = new JavaScriptSerializer().Deserialize<FBResponse>(jsonMarker);

                var clientCurrentData = new RestClient();
                clientCurrentData.EndPoint = @"https://graph.facebook.com/debug_token?input_token=" + data.access_token + "&access_token=" + marker.access_token;
                clientCurrentData.Method = HttpVerb.GET;
                clientCurrentData.ContentType = "application/x-www-form-urlencoded";

                string jsonData = clientCurrentData.MakeRequest();

                var userData = new JavaScriptSerializer().Deserialize<FBUserResponse>(jsonData);

                user.ClientID = request.ClientID;
                user.IDFB = userData.data.user_id;
            }
            if (request.CodeFB.StartsWith("mob"))
            {
                var clientMarker = new RestClient();
                clientMarker.EndPoint = @"https://graph.facebook.com/v2.8/oauth/access_token?";
                clientMarker.Method = HttpVerb.POST;
                clientMarker.ContentType = "application/x-www-form-urlencoded";
                clientMarker.PostData = "client_id=" + clientId + "&client_secret=" + clientSecret + "&grant_type=client_credentials";
                string jsonMarker = clientMarker.MakeRequest();
                var marker = new JavaScriptSerializer().Deserialize<FBResponse>(jsonMarker);

                var clientCurrentData = new RestClient();
                clientCurrentData.EndPoint = @"https://graph.facebook.com/v2.8/debug_token?input_token=" + request.CodeFB.Substring(3) + "&access_token=" + marker.access_token;
                clientCurrentData.Method = HttpVerb.GET;
                clientCurrentData.ContentType = "application/x-www-form-urlencoded";

                string jsonData = clientCurrentData.MakeRequest();

                var userData = new JavaScriptSerializer().Deserialize<FBUserResponse>(jsonData);

                user.ClientID = request.ClientID;
                user.IDFB = userData.data.user_id;
            }
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddIdFb";
            cmd.Parameters.AddWithValue("@client", user.ClientID);
            if (user.IDFB > 0)
            {
                cmd.Parameters.AddWithValue("@idfb", user.IDFB);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class AddIDOKRequest
    {
        public string CodeOK { get; set; }
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
    }

    public class OKUser
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        public Int64 IDOK { get; set; }
    }

    public class AddIDOKResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class OKResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
    }

    public class OKUserResponse
    {
        public long uid { get; set; }
    }

    public class ServerAddIDOKResponse
    {
        public AddIDOKResponse ProcessRequest(SqlConnection cnn, AddIDOKRequest request)
        {
            string clientId = ConfigurationManager.AppSettings["OKClientid"].ToString();
            string clientSecret = ConfigurationManager.AppSettings["OKClientSecret"].ToString();
            string clientPublic = ConfigurationManager.AppSettings["OKClientPublic"].ToString();
            AddIDOKResponse returnValue = new AddIDOKResponse();
            OKUser user = new OKUser();
            string socialNetworkAuth = ConfigurationManager.AppSettings["SocialNetwork"].ToString();
            if (request.CodeOK.StartsWith("web"))
            {
                var client = new RestClient();
                client.EndPoint = @"http://api.ok.ru/oauth/token.do";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/x-www-form-urlencoded";
                client.PostData = @"code=" + request.CodeOK.Replace("web", "") + @"&client_id=" + clientId + "&client_secret=" + clientSecret + "&grant_type=authorization_code&redirect_uri=" + socialNetworkAuth + "%3Fok=1";
                string json = client.MakeRequest();

                var data = new JavaScriptSerializer().Deserialize<OKResponse>(json);

                MD5 md5 = MD5.Create();
                byte[] secret_key = md5.ComputeHash(Encoding.UTF8.GetBytes(data.access_token + clientSecret));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < secret_key.Length; i++)
                {
                    sBuilder.Append(secret_key[i].ToString("x2"));
                }

                string sigBase = "application_key=CBAIDKLLEBABABABAformat=jsonmethod=users.getCurrentUser" + sBuilder.ToString();
                byte[] sigByte = md5.ComputeHash(Encoding.UTF8.GetBytes(sigBase));

                StringBuilder sig = new StringBuilder();

                for (int i = 0; i < sigByte.Length; i++)
                {
                    sig.Append(sigByte[i].ToString("x2"));
                }

                var clientCurrentUser = new RestClient();
                clientCurrentUser.EndPoint = @"https://api.ok.ru/fb.do?";
                clientCurrentUser.Method = HttpVerb.POST;
                clientCurrentUser.ContentType = "application/x-www-form-urlencoded";
                clientCurrentUser.PostData = "application_key=" + clientPublic + "&format=json&method=users.getCurrentUser&access_token=" + data.access_token + "&sig=" + sig.ToString();

                string res = clientCurrentUser.MakeRequest();

                var userData = new JavaScriptSerializer().Deserialize<OKUserResponse>(res);

                user.ClientID = request.ClientID;
                user.IDOK = userData.uid;
            }
            if (request.CodeOK.StartsWith("mob"))
            {
                //SqlCommand cmd = cnn.CreateCommand();
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "TokenInsert";
                //cmd.Parameters.AddWithValue("@token", request.CodeOK);
                //cmd.ExecuteNonQuery();
                MD5 md5 = MD5.Create();
                byte[] secret_key = md5.ComputeHash(Encoding.UTF8.GetBytes(request.CodeOK.Substring(3) + clientSecret));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < secret_key.Length; i++)
                {
                    sBuilder.Append(secret_key[i].ToString("x2"));
                }

                string sigBase = "application_key=" + clientPublic + "format=jsonmethod=users.getCurrentUser" + sBuilder.ToString();
                byte[] sigByte = md5.ComputeHash(Encoding.UTF8.GetBytes(sigBase));

                StringBuilder sig = new StringBuilder();

                for (int i = 0; i < sigByte.Length; i++)
                {
                    sig.Append(sigByte[i].ToString("x2"));
                }

                var clientCurrentUser = new RestClient();
                clientCurrentUser.EndPoint = @"https://api.ok.ru/fb.do?";
                clientCurrentUser.Method = HttpVerb.POST;
                clientCurrentUser.ContentType = "application/x-www-form-urlencoded";
                clientCurrentUser.PostData = "application_key=" + clientPublic + "&format=json&method=users.getCurrentUser&access_token=" + request.CodeOK.Substring(3) + "&sig=" + sig.ToString();

                string res = clientCurrentUser.MakeRequest();

                var userData = new JavaScriptSerializer().Deserialize<OKUserResponse>(res);

                user.ClientID = request.ClientID;
                user.IDOK = userData.uid;
            }
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddIdOk";
            cmd.Parameters.AddWithValue("@client", user.ClientID);
            if (user.IDOK > 0)
            {
                cmd.Parameters.AddWithValue("@idok", user.IDOK);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestClient
    {
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }

        public RestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = "";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
        }


        public string MakeRequest()
        {
            return MakeRequest("");
        }

        public string MakeRequest(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
            {
                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                }

                return responseValue;
            }
        }

    }

    public class AddIDVKRequest
    {
        public string CodeVK { get; set; }
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
    }

    public class VKUser
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        public Int64 IDVK { get; set; }
    }

    public class AddIDVKResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class VKResponse
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public long user_id { get; set; }
    }

    public class VKResponseMob
    {
        public List<VKReponseMobDetail> response { get; set; }
    }

    public class VKReponseMobDetail
    {
        public int uid { get; set; }
    }

    public class ServerAddIDVKResponse
    {
        public AddIDVKResponse ProcessRequest(SqlConnection cnn, AddIDVKRequest request)
        {
            AddIDVKResponse returnValue = new AddIDVKResponse();
            VKUser user = new VKUser();
            string socialNetworkAuth = ConfigurationManager.AppSettings["SocialNetwork"].ToString();
            if (request.CodeVK.StartsWith("web"))
            {
                var client = new RestClient();
                client.EndPoint = @"https://oauth.vk.com/access_token?";
                client.Method = HttpVerb.POST;
                client.ContentType = "application/x-www-form-urlencoded";
                client.PostData = @"code=" + request.CodeVK.Substring(3) + @"&client_id=6145065&client_secret=EWon7iBJ1Lvh8pxI2Cv2&redirect_uri=" + socialNetworkAuth;
                string json = "";
                try
                {
                    json = client.MakeRequest();
                }
                catch (Exception ex)
                {

                }

                var data = new JavaScriptSerializer().Deserialize<VKResponse>(json);

                user.ClientID = request.ClientID;
                user.IDVK = data.user_id;

            }
            if (request.CodeVK.StartsWith("mob"))
            {
                var client = new RestClient();
                client.EndPoint = @"https://api.vk.com/method/users.get?access_token=" + request.CodeVK.Substring(3);
                client.Method = HttpVerb.POST;
                client.ContentType = "application/x-www-form-urlencoded";
                string json = client.MakeRequest();

                var data = new JavaScriptSerializer().Deserialize<VKResponseMob>(json);

                user.ClientID = request.ClientID;
                user.IDVK = data.response.FirstOrDefault().uid;
            }
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientAddIdVk";
            cmd.Parameters.AddWithValue("@client", user.ClientID);
            if (user.IDVK > 0)
            {
                cmd.Parameters.AddWithValue("@idvk", user.IDVK);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

public class GetPersonalCampaignsRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class GetPersonalCampaignsResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// список акций
    /// </summary>
    public List<Campaign> CampaignData { get; set; }
        /// <summary>
    /// список акций
    /// </summary>
        public GetPersonalCampaignsResponse()
        {
            CampaignData = new List<Campaign>();
        }
    }


public class ServerGetPersonalCampaignsResponse
    {
        public GetPersonalCampaignsResponse ProcessRequest(SqlConnection cnn, GetPersonalCampaignsRequest request)
        {
            GetPersonalCampaignsResponse returnValue = new GetPersonalCampaignsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CampaignsPersonal";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Campaign campaign = new Campaign();
                campaign.id = reader.GetInt16(0);
                campaign.name = reader.GetString(1);
                if (reader.IsDBNull(2)) campaign.logo = null; else campaign.logo = reader.GetString(2);
                if (reader.IsDBNull(3)) campaign.description = null; else campaign.description = reader.GetString(3);
                if (reader.IsDBNull(4)) campaign.condition = null; else campaign.condition = reader.GetString(4);
                if (reader.IsDBNull(5)) campaign.tagline = null; else campaign.tagline = reader.GetString(5);
                if (reader.IsDBNull(6)) campaign.internetShop = null; else campaign.internetShop = reader.GetString(6);
                campaign.isFav = false;
                campaign.share_url = "";
                returnValue.CampaignData.Add(campaign);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class SelectPreferencesRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID категории
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Удалено?
        /// </summary>
        public bool Remove { get; set; }
    }

    public class SelectPreferencesResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Предпочтения участника
    /// </summary>
    public SelectPreferencesResponse() { }
    }

    public class ServerSelectPreferencesResponse
    {
        public SelectPreferencesResponse ProcessRequest(SqlConnection cnn, SelectPreferencesRequest request)
        {
            SelectPreferencesResponse returnValue = new SelectPreferencesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientCategoryChoose";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
            cmd.Parameters.AddWithValue("@category", request.CategoryID);
            cmd.Parameters.AddWithValue("@remove", request.Remove);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientPreferences
    {
        /// <summary>
        /// ID категории
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Наименование категории
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Включено?
        /// </summary>
        public bool SwitchOn { get; set; }
    }

    public class ClientPreferencesRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class ClientPreferencesResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    public List<ClientPreferences> ClientPreferencesData { get; set; }
        public ClientPreferencesResponse()
        {
            ClientPreferencesData = new List<ClientPreferences>();
        }
    }

    public class ServerClientPreferencesResponse
    {
        public ClientPreferencesResponse ProcessRequest(SqlConnection cnn, ClientPreferencesRequest request)
        {
            ClientPreferencesResponse returnValue = new ClientPreferencesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientCategoryGet";
            cmd.Parameters.AddWithValue("@client", request.ClientID);
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
                ClientPreferences preferences = new ClientPreferences();
                preferences.CategoryId = reader.GetInt16(0);
                if (!reader.IsDBNull(1)) preferences.SwitchOn = reader.GetBoolean(1);
                if (!reader.IsDBNull(1)) preferences.CategoryName = reader.GetString(2);
                returnValue.ClientPreferencesData.Add(preferences);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ChequeDetail
    {
        /// <summary>
        /// ID чека
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// номер чека
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// дата чека
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// тип операции
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Summ { get; set; }
        /// <summary>
        /// сумма скидки
        /// </summary>
        public decimal SummDiscount { get; set; }
        /// <summary>
        /// начисленно бонусов
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// оплачено бонусами
        /// </summary>
        public decimal PaidByBonus { get; set; }
        /// <summary>
        /// скидка
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        ///  Партнер
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// торговая точка
        /// </summary>
        public string Shop { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 CardNumber { get; set; }
        /// <summary>
        /// детализация покупок
        /// </summary>
        public List<ChequeDetailItems> items { get; set; }
        /// <summary>
        /// детализация акций
        /// </summary>
        public List<string> Campaigns { get; set; }
        public ChequeDetail()
        {
            items = new List<ChequeDetailItems>();
            Campaigns = new List<string>();
        }
    }

    public class ChequeDetailItems
    {
        /// <summary>
        /// позиция
        /// </summary>
        public int position { get; set; }
        /// <summary>
        /// код
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// цена
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// количество
        /// </summary>
        public decimal quantity { get; set; }
        /// <summary>
        /// сумма
        /// </summary>
        public decimal amount { get; set; }
    }

    public class ChequeDetailRequest
    {
        /// <summary>
        /// ID чека
        /// </summary>
        public int ChequeID { get; set; }
    }

    public class ChequeDetailResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    public ChequeDetail ChequeDetailData { get; set; }
        public ChequeDetailResponse()
        {
            ChequeDetailData = new ChequeDetail();
        }
    }

    public class ServerChequeDetailResponse
    {
        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ChequeDetailResponse ProcessRequest(SqlConnection cnn, ChequeDetailRequest request)
        {
            ChequeDetailResponse returnValue = new ChequeDetailResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "ChequeDetailGet";
            cmd.CommandText = "Cheques";
            cmd.Parameters.AddWithValue("@cheque", request.ChequeID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ChequeDetail cheque = new ChequeDetail();
                cheque.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) cheque.Number = reader.GetString(1);
                if (!reader.IsDBNull(2)) cheque.Date = reader.GetDateTime(2);
                cheque.OperationType = "Покупка";
                if (!reader.IsDBNull(3)) if (reader.GetBoolean(3) == true) cheque.OperationType = "Возврат";
                if (!reader.IsDBNull(4)) cheque.Summ = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) cheque.Discount = reader.GetDecimal(5);
                if (!reader.IsDBNull(6)) cheque.Partner = reader.GetString(6);
                if (!reader.IsDBNull(7)) cheque.Shop = reader.GetString(7);
                if (!reader.IsDBNull(8)) cheque.CardNumber = reader.GetInt64(8);
                if (!reader.IsDBNull(9)) cheque.Bonus = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) cheque.PaidByBonus = reader.GetDecimal(10);
                cheque.Campaigns.Add("Акция 1");
                cheque.Campaigns.Add("Акция 2");
                returnValue.ChequeDetailData = cheque;

                //ChequeDetailItems detail = new ChequeDetailItems();
                //if (reader.IsDBNull(0)) detail.position = 0; else detail.position = reader.GetByte(0);
                //if (reader.IsDBNull(1)) detail.code = null; else detail.code = reader.GetString(1);
                //if (reader.IsDBNull(2)) detail.price = 0; else detail.price = reader.GetDecimal(2);
                //if (reader.IsDBNull(3)) detail.quantity = 0; else detail.quantity = reader.GetDecimal(3);
                //if (reader.IsDBNull(4)) detail.amount = 0; else detail.amount = reader.GetDecimal(4);
                //returnValue.ChequeDetailData.Add(detail);
            }
            reader.Close();
            //foreach (var c in returnValue.ChequeDetailData)
            //{
            SqlCommand cmdItems = cnn.CreateCommand();
            cmdItems.CommandType = CommandType.StoredProcedure;
            cmdItems.CommandText = "ChequeDetailGet";
            cmdItems.Parameters.AddWithValue("@cheque", returnValue.ChequeDetailData.Id);
            cmdItems.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmdItems.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmdItems.Parameters.Add("@result", SqlDbType.Int);
            cmdItems.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader readerItems = cmdItems.ExecuteReader();
            while (readerItems.Read())
            {
                ChequeDetailItems detail = new ChequeDetailItems();
                if (readerItems.IsDBNull(0)) detail.position = 0; else detail.position = readerItems.GetByte(0);
                if (readerItems.IsDBNull(1)) detail.code = null; else detail.code = readerItems.GetString(1);
                if (readerItems.IsDBNull(2)) detail.price = 0; else detail.price = readerItems.GetDecimal(2);
                if (readerItems.IsDBNull(3)) detail.quantity = 0; else detail.quantity = readerItems.GetDecimal(3);
                if (readerItems.IsDBNull(4)) detail.amount = 0; else detail.amount = readerItems.GetDecimal(4);
                returnValue.ChequeDetailData.items.Add(detail);
            }
            readerItems.Close();
            //}            
            cnn.Close();
            return returnValue;
        }
    }

    public class AddDeviceRequest
    {
        /// <summary>
        /// Token устройства
        /// </summary>
        public string Device_token { get; set; }
        /// <summary>
        /// зарегестрированная операционная система
        /// </summary>
        public string OSRegistrator { get; set; }
        /// <summary>
        /// ID клиента
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class AddDeviceResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class ServerAddDeviceResponse
    {
        public AddDeviceResponse ProcessRequest(SqlConnection cnn, AddDeviceRequest request)
        {
            AddDeviceResponse returnValue = new AddDeviceResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientDevice";
            cmd.Parameters.AddWithValue("@client", request.Client);
            cmd.Parameters.AddWithValue("@appdevice", request.Device_token);
            cmd.Parameters.AddWithValue("@appregistrator", request.OSRegistrator);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class SendPushRequest
    {
        /// <summary>
        /// ID устройства
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// сообщение
        /// </summary>
        public string Message { get; set; }
    }

    public class SendPushResponse
    { }

    public class ServerSendPushResponse
    {
        public SendPushResponse ProcessRequest(SqlConnection cnn, SendPushRequest request)
        {
            SendPushResponse returnValue = new SendPushResponse();

            //var message = SendNotification(request.DeviceID, request.Message);

            var message = SendNotificationFromFirebaseCloud(request.DeviceID, request.Message);

            return returnValue;
        }

        public String SendNotificationFromFirebaseCloud(string deviceId, string message)
        {
            var result = "-1";
            var webAddr = "https://fcm.googleapis.com/fcm/send";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization:key=AIzaSyD-hw9vm584Ppf7AqnAdFT_QezawNxjJRA");
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"to\": \"" + deviceId + "\" ,\"data\":{\"body\": \"" + message + "\", \"message\": \"Hello from campaigns\"}}";
                //string json = "{\"registration_ids\":[\"AIzaSyD-hw9vm584Ppf7AqnAdFT_QezawNxjJRA\"]}";

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
    }

    public class ServerSendPushAppleResponse
    {

        public SendPushResponse ProcessRequest(SqlConnection cnn, SendPushRequest request)
        {
            SendPushResponse returnValue = new SendPushResponse();
            iphonpushnotification(request);

            return returnValue;
        }

        public void iphonpushnotification(SendPushRequest request)
        {
            string devicetocken = request.DeviceID;//  iphone device token
            int port = 2195;
            string hostname = "gateway.sandbox.push.apple.com";
            //String hostname = "gateway.push.apple.com";

            string certificatePath = @"c:\ck.p12";

            string certificatePassword = "";

            X509Certificate2 clientCertificate = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.MachineKeySet);
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(
                            client.GetStream(),
                            false,
                            new RemoteCertificateValidationCallback(ValidateServerCertificate),
                            null
            );

            try
            {
                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);
            }
            catch (AuthenticationException ex)
            {
                client.Close();
                return;
            }

            //// Encode a test message into a byte array.
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write((byte)0);  //The command
            writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
            writer.Write((byte)32); //The deviceId length (big-endian second byte)

            byte[] b0 = HexString2Bytes(devicetocken);

            writer.Write(b0);
            String payload;
            string strmsgbody = "";
            strmsgbody = request.Message;

            payload = "{\"aps\":{\"alert\":\"" + EncodeNonAsciiCharacters(strmsgbody) + "\",\"sound\":\"default\", \"content-available\":\"1\"}}";

            writer.Write((byte)0); //First byte of payload length; (big-endian first byte)
            writer.Write((byte)payload.Length);     //payload length (big-endian second byte)

            byte[] b1 = Encoding.UTF8.GetBytes(payload);
            writer.Write(b1);
            writer.Flush();

            byte[] array = memoryStream.ToArray();
            try
            {
                sslStream.Write(array);
                sslStream.Flush();
            }
            catch
            {
            }

            client.Close();
        }

        private byte[] HexString2Bytes(string hexString)
        {
            //check for null
            if (hexString == null) throw new Exception("Ошибка преобразования строки"); ;
            //get length
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            //create a byte array
            byte[] bs = new byte[len_half];
            try
            {
                //convert the hexstring to bytes
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex.Message);
            }
            //return the byte array
            return bs;
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }

    public class CardStatisticsRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
    }

    public class CardStatisticsResponse
    {
        /// <summary>
        /// уровень карты
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// состояние карты
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// баланс карты
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// полный баланс?
        /// </summary>
        public decimal FullBalance { get; set; }
        /// <summary>
        /// количество покупок
        /// </summary>
        public int Purchases { get; set; }
        /// <summary>
        /// сумма покупок
        /// </summary>
        public decimal Purchasesum { get; set; }
        /// <summary>
        /// количество возвратов
        /// </summary>
        public int Refunds { get; set; }
        /// <summary>
        /// сумма возвратов
        /// </summary>
        public decimal RefundSum { get; set; }
        /// <summary>
        /// всего потрачено
        /// </summary>
        public decimal SpentSum { get; set; }
        /// <summary>
        /// начисленно балов
        /// </summary>
        public decimal Charged { get; set; }
        /// <summary>
        /// списано баллов
        /// </summary>
        public decimal Redeemed { get; set; }
        /// <summary>
        /// начисленно за возвраты?
        /// </summary>
        public decimal ChargeRefund { get; set; }
        /// <summary>
        /// списано балов за возрат?
        /// </summary>
        public decimal RedeemRefund { get; set; }
        /// <summary>
        /// итоговая скидка
        /// </summary>
        public decimal FullDiscount { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
}

    public class ServerCardStatisticsResponse
    {
        public CardStatisticsResponse ProcessRequest(SqlConnection cnn, CardStatisticsRequest request)
        {
            CardStatisticsResponse returnValue = new CardStatisticsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardStatistics";
            cmd.Parameters.AddWithValue("@card", request.Card);

            cmd.Parameters.Add("@level", SqlDbType.NVarChar, 20);
            cmd.Parameters["@level"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@condition", SqlDbType.NVarChar, 100);
            cmd.Parameters["@condition"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@balance", SqlDbType.Decimal);
            cmd.Parameters["@balance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@fullbalance", SqlDbType.Decimal);
            cmd.Parameters["@fullbalance"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchases", SqlDbType.Int);
            cmd.Parameters["@purchases"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@purchasesum", SqlDbType.Decimal);
            cmd.Parameters["@purchasesum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refunds", SqlDbType.Int);
            cmd.Parameters["@refunds"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@refundsum", SqlDbType.Decimal);
            cmd.Parameters["@refundsum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@spentsum", SqlDbType.Decimal);
            cmd.Parameters["@spentsum"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@charged", SqlDbType.Decimal);
            cmd.Parameters["@charged"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemed", SqlDbType.Decimal);
            cmd.Parameters["@redeemed"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@chargerefund", SqlDbType.Decimal);
            cmd.Parameters["@chargerefund"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@redeemrefund", SqlDbType.Decimal);
            cmd.Parameters["@redeemrefund"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@fulldiscount", SqlDbType.Decimal);
            cmd.Parameters["@fulldiscount"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@operator", SqlDbType.Int);
            cmd.Parameters["@operator"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();
            try
            {
                returnValue.Level = Convert.ToString(cmd.Parameters["@level"].Value);
            }
            catch { }
            try
            {
                returnValue.Condition = Convert.ToString(cmd.Parameters["@condition"].Value);
            }
            catch { }
            try
            {
                returnValue.Balance = Convert.ToDecimal(cmd.Parameters["@balance"].Value);
            }
            catch { }
            try
            {
                returnValue.FullBalance = Convert.ToDecimal(cmd.Parameters["@fullbalance"].Value);
            }
            catch { }
            try
            {
                returnValue.Purchases = Convert.ToInt32(cmd.Parameters["@purchases"].Value);
            }
            catch { }
            try
            {
                returnValue.Purchasesum = Convert.ToDecimal(cmd.Parameters["@purchasesum"].Value);
            }
            catch { }
            try
            {
                returnValue.Refunds = Convert.ToInt32(cmd.Parameters["@refunds"].Value);
            }
            catch { }
            try
            {
                returnValue.RefundSum = Convert.ToInt32(cmd.Parameters["@refundsum"].Value);
            }
            catch { }
            try
            {
                returnValue.SpentSum = Convert.ToDecimal(cmd.Parameters["@spentsum"].Value);
            }
            catch { }
            try
            {
                returnValue.Charged = Convert.ToDecimal(cmd.Parameters["@charged"].Value);
            }
            catch { }
            try
            {
                returnValue.Redeemed = Convert.ToDecimal(cmd.Parameters["@redeemed"].Value);
            }
            catch { }
            try
            {
                returnValue.ChargeRefund = Convert.ToDecimal(cmd.Parameters["@chargerefund"].Value);
            }
            catch { }
            try
            {
                returnValue.RedeemRefund = Convert.ToDecimal(cmd.Parameters["@redeemrefund"].Value);
            }
            catch { }
            try
            {
                returnValue.FullDiscount = Convert.ToDecimal(cmd.Parameters["@fulldiscount"].Value);
            }
            catch { }
            try
            {
                returnValue.Operator = Convert.ToInt16(cmd.Parameters["@operator"].Value);
            }
            catch { }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class CardAggregation
    {
        /// <summary>
        /// сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// начислено баллов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано баллов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// количество чеков
        /// </summary>
        public int ChequeQty { get; set; }
        /// <summary>
        /// количество чеков без возвратов
        /// </summary>
        public int ChequeQtyWithoutRefund { get; set; }
        /// <summary>
        /// тип агрегации
        /// </summary>
        public int? MonthWeekNum { get; set; }
        /// <summary>
        /// время чека
        /// </summary>
        public DateTime ChequeDate { get; set; }
    }

    public class CardAggregationRequest
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// ID партнера
        /// </summary>
        public Int16? Partner { get; set; }
        /// <summary>
        /// Код торговой точки
        /// </summary>
        public string Pos { get; set; }
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTime? From { get; set; }
        /// <summary>
        /// Конец периода
        /// </summary>
        public DateTime? To { get; set; }
        /// <summary>
        /// тип агрегации
        /// </summary>
        public Int16 Layout { get; set; }
    }

    public class CardAggregationResponse
    {
        public List<CardAggregation> CardInfo { get; set; }
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// статистика по картам
    /// </summary>
    public CardAggregationResponse()
        {
            CardInfo = new List<CardAggregation>();
        }
    }

    public class ServerGetCardAggregation
    {
        public CardAggregationResponse ProcessRequest(SqlConnection cnn, CardAggregationRequest request)
        {
            CardAggregationResponse returnValue = new CardAggregationResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardAggregation";
            if (request.Card > 0)
            {
                cmd.Parameters.AddWithValue("@card", request.Card);
            }
            if (request.Partner > 0)
            {
                cmd.Parameters.AddWithValue("@partner", request.Partner);
            }
            if (!string.IsNullOrEmpty(request.Pos))
            {
                cmd.Parameters.AddWithValue("@pos", request.Pos);
            }
            if (request.From.HasValue)
            {
                cmd.Parameters.AddWithValue("@from", request.From.Value);
            }
            if (request.To.HasValue)
            {
                cmd.Parameters.AddWithValue("@to", request.To.Value);
            }
            cmd.Parameters.AddWithValue("@layout", request.Layout);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CardAggregation card = new CardAggregation();
                if (!reader.IsDBNull(0))
                {
                    if (request.Layout == 2)
                    {
                        card.ChequeDate = reader.GetDateTime(0);
                    }
                    else
                    {
                        card.MonthWeekNum = reader.GetInt32(0);
                    }
                }
                if (!reader.IsDBNull(1))
                {
                    card.Amount = reader.GetDecimal(1);
                }
                if (!reader.IsDBNull(2))
                {
                    card.BonusAdded = reader.GetDecimal(2);
                }
                if (!reader.IsDBNull(3))
                {
                    card.BonusRedeemed = reader.GetDecimal(3);
                }
                if (!reader.IsDBNull(4))
                {
                    card.ChequeQty = reader.GetInt32(4);
                }
                if (!reader.IsDBNull(5))
                {
                    card.ChequeQtyWithoutRefund = reader.GetInt32(5);
                }
                returnValue.CardInfo.Add(card);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientPasswordChangeRequest
    {
        /// <summary>
        /// стары пароль
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// новый пароль
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// ID клиента
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }

    public class ClientPasswordChangeResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class ServerClientPasswordChange
    {
        public ClientPasswordChangeResponse ProcessRequest(SqlConnection cnn, ClientPasswordChangeRequest request)
        {
            ClientPasswordChangeResponse returnValue = new ClientPasswordChangeResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientPasswordChange";
            cmd.Parameters.AddWithValue("@client", request.Client);
            cmd.Parameters.AddWithValue("@newPassword", request.NewPassword);
            cmd.Parameters.AddWithValue("@oldPassword", request.OldPassword);
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientUpdateCommunicationRequest
    {
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
        /// <summary>
        /// ID участника
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// Разрешение на получение SMS
        /// </summary>
        public bool? AllowSms { get; set; }
        /// <summary>
        /// Разрешение на получение писем по электронной почте
        /// </summary>
        public bool? AllowEmail { get; set; }
    }

    public class ClientUpdateCommunicationResponse
    {
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
}

    public class ServerClientUpdateCommunication
    {
        public ClientUpdateCommunicationResponse ProcessRequest(SqlConnection cnn, ClientUpdateCommunicationRequest request)
        {
            var returnValue = new ClientUpdateCommunicationResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ClientUpdateCommunication";
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@client", request.Client);
            if (request.AllowSms.HasValue)
            {
                cmd.Parameters.AddWithValue("@allowsms", request.AllowSms.Value);
            }
            if (request.AllowEmail.HasValue)
            {
                cmd.Parameters.AddWithValue("@allowemail", request.AllowEmail.Value);
            }
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class ClientBonus
    {
        public decimal Bonus { get; set; }
        public DateTime BonusTime { get; set; }
        public string BonusOperation { get; set; }
        public string BonusType { get; set; }
    }

    public class ClientBonusesRequest
    {
        public Int64 Card { get; set; }
    }

    public class ClientBonusesResponse
    {
        public List<ClientBonus> ClientBonuses { get; set; }
    /// <summary>
    /// Код ошибки
    /// </summary>
    public int ErrorCode { get; set; }
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    public ClientBonusesResponse()
        {
            ClientBonuses = new List<ClientBonus>();
        }
    }

    public class ServerClientBonuses
    {
        public ClientBonusesResponse ProcessRequest(SqlConnection cnn, ClientBonusesRequest request)
        {
            var returnValue = new ClientBonusesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CardBonusesAll";
            cmd.Parameters.AddWithValue("@card", request.Card);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientBonus bonus = new ClientBonus();
                if (!reader.IsDBNull(0)) bonus.Bonus = reader.GetDecimal(0);
                if (!reader.IsDBNull(1)) bonus.BonusTime = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) bonus.BonusOperation = reader.GetString(2);
                if (!reader.IsDBNull(3)) bonus.BonusType = reader.GetString(3);
                returnValue.ClientBonuses.Add(bonus);
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            reader.Close();

            cnn.Close();
            return returnValue;
        }
    }
}