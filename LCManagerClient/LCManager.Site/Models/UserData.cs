using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace LC_Manager.Models
{
    //public class UserData
    //{
    //    public static string OperatorName;

    //    public static Int16 Operator { get; set; }

    //    public static Int16 Partner { get; set; }

    //    public static string PosCode { get; set; }

    //    public static string RoleName { get; set; }

    //    public static string PermissionCode { get; set; }

    //    public static long Phone { get; set; }

    //    static UserData()
    //    {
    //        if (string.IsNullOrEmpty(OperatorName))
    //        {
    //            string lcpartner = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
    //            HttpClient httpClient = new HttpClient
    //            {
    //                BaseAddress = new Uri(lcpartner)
    //            };
    //            httpClient.DefaultRequestHeaders.Accept.Clear();
    //            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    //            var token = HttpContext.Current.Request.Cookies["lcmanageruserdata"].Value;
    //            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    //            OperatorInfoRequest operatorInfoRequest = new OperatorInfoRequest
    //            {
    //                Operator = Convert.ToInt16(HttpContext.Current.Request.Cookies["operator"].Value)
    //            };
    //            HttpResponseMessage responseMessage = httpClient.PostAsJsonAsync("api/values/OperatorInfo", operatorInfoRequest).Result;
    //            if(responseMessage.IsSuccessStatusCode)
    //            {
    //                OperatorInfoResponse operatorInfoResponse = responseMessage.Content.ReadAsAsync<OperatorInfoResponse>().Result;
    //                OperatorName = operatorInfoResponse.OperatorName;
    //            }
    //        }
    //        if (string.IsNullOrEmpty(OperatorName))
    //        {
    //            OperatorName = "";
    //        }
    //        try
    //        {
    //            if(Operator == 0)
    //            {
    //                Operator = Convert.ToInt16(HttpContext.Current.Request.Cookies["operator"].Value);
    //            }
    //            if(string.IsNullOrEmpty(RoleName))
    //            {
    //                RoleName = Convert.ToString(HttpContext.Current.Request.Cookies["roleName"].Value);
    //            }
    //            if(string.IsNullOrEmpty(PermissionCode))
    //            {
    //                PermissionCode = Convert.ToString(HttpContext.Current.Request.Cookies["permissionCode"].Value);
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        try
    //        {
    //            Partner = Convert.ToInt16(HttpContext.Current.Request.Cookies["partner"].Value);
    //        }
    //        catch
    //        {

    //        }
    //        try
    //        {
    //            PosCode = Convert.ToString(HttpContext.Current.Request.Cookies["posCode"].Value);
    //        }
    //        catch
    //        {

    //        }
    //    }
    //}

    //public class UserSession
    //{
    //    public static int user_id
    //    {
    //        get
    //        {
    //            var ret = get("trueClientID");
    //            if (ret != null) return (int)ret;
    //            return -1;
    //        }
    //        set
    //        {
    //            set("trueClientID", value);
    //        }
    //    }
        
    //    public static Client client
    //    {
    //        get
    //        {
    //            var ret = get("trueClient");
    //            if (ret != null) return (Client)ret;
    //            return null;
    //        }
    //        set
    //        {
    //            set("trueClient", value);
    //        }
    //    }
    //    //public static HttpSessionState session { get; set; }

    //    public static void set(string key, object value)
    //    {
    //        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
    //        try
    //        {
    //            if (session != null) if (session[key] != null) session[key] = value; else session.Add(key, value);
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }

    //    public static string getSessionID()
    //    {
    //        var ret = "";
    //        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
    //        try
    //        {
    //            ret = session.SessionID;
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //        return ret;
    //    }

    //    public static object get(string key)
    //    {
    //        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
    //        object returnValue = null;
    //        try
    //        {
    //            if (session != null) if (session[key] != null) returnValue = session[key];
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //        return returnValue;
    //    }

    //    public static string ReInit()
    //    {
    //        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
    //        session.Clear();
    //        session.Abandon();
    //        return session.SessionID;
    //    }

    //    public static void RegisterUserSession()
    //    {
            
    //    }

    //    public static void getUserSession()
    //    {
    //        try
    //        {
    //            user_id = (int)get("user_id");
    //            client = (Client)get("client");
    //            if (client == null)
    //            {
    //                user_id = -1;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            user_id = -1;
    //        }
    //    }
        
    //    public static void setUserSession()
    //    {
    //        //   HttpContext.Current = current;
    //    }
    //}

    //public class Client
    //{

    //}
}