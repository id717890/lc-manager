using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PlizCard
{

    public class SocialMeta
    {
        public string title= "Пожалуйста - просто скачай, просто используй!";
        public string description = "ПОЖАЛУЙСТА!";
        public string site = "http://plizcard.ru";
        public string image = ("favicon.ico");

        public SocialMeta()
        { 
}

        public SocialMeta(string t, string d, string s, string i)
        {
            title = t;
            description = d;
            site = s;
            image = i;
        }

        public SocialMeta(string a, int i)
        {
            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            switch (a)
            {
                case "actions":
                    var objA = ApiConfig.GetCampaign(i);
                    if (objA.ErrorCode == 0)
                    {
                        title = objA.CampaignData.name;
                        description = objA.CampaignData.description;
                        site = Domain + "/Actions/Details/" + i.ToString();
                        image = objA.CampaignData.logo;
                    }
                    break;
                case "partners":
                    var objP = ApiConfig.GetPartner(i);
                    if (objP.ErrorCode == 0)
                    {
                        title = objP.PartnerData.name;
                        description = objP.PartnerData.description;
                        site = Domain + "/Partners/Details/" + i.ToString();
                        image = objP.PartnerData.logo;
                    }
                    break;
            }
        }
    }
    public class Partner
    {
        public lcsite.Partner partnerData;
        public List<lcsite.Pos> posesData;
        
    }

    public class ActionsFilter
    {
        public int CategoryID;
        public int CityID;
        public int PartnerID;
        public bool IsMainPage;
        public bool IsSideBar;
        public int SegmentID;

        public ActionsFilter()
        {
            
        }
    }
    public class UserSession
    {        
        public static int user_id { get {
               var ret = get("trueClientID");
                if (ret != null) return (int)ret;
                return -1;
            }
            set {
                set("trueClientID", value);
            }
        }
        public static lcsite.City current_city {
            get {
                var ret = get("trueCurrentCity");
                if (ret != null) return (lcsite.City)ret;
                return null;
            } set {
                set("trueCurrentCity", value);
            } }
        public static lcclient.Client client {
            get {
                var ret = get("trueClient");
                if (ret != null) return (lcclient.Client)ret;
                return null;
            }
            set {
                set("trueClient", value);
            }
        }
        //public static HttpSessionState session { get; set; }

        public static void set(string key, object value)
        {
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            try
            {
                if (session != null) if (session[key] != null) session[key] = value; else session.Add(key, value);
            }catch(Exception ex)
            {

            }
        }

        public static string getSessionID()
        {
            var ret = "";
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            try
            {
                ret = session.SessionID;
            }
            catch (Exception ex)
            {

            }
            return ret;
        }

        public static object get(string key)
        {
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            object returnValue = null;
            try
            {
                if (session != null) if (session[key] != null) returnValue = session[key];
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string ReInit()
        {
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            session.Clear();
            session.Abandon();
            return session.SessionID;
        }

        public static void RegisterUserSession()
        {
            /*
            getUserSession();
            actionsFilter = new ActionsFilter();
            if (current.Items.Contains("CurrentCity"))
            {
                current_city = (lcsite.City)current.Items["CurrentCity"];
            }else
            {
                var cities = ApiConfig.GetCities();
                current_city = cities.CityData.First();
            }

            if (current.Items.Contains("user_id"))
            {
                user_id = (int)current.Items["user_id"];
            }else
            {
                user_id = -1;
            }

            if (current.Items.Contains("client"))
            {
                client = (lcsite.Client)current.Items["client"];
            }else
            {
                if (user_id > 0) {
                    var resp = ApiConfig.GetClient(user_id);
                    if (resp.ErrorCode == 0)
                    {
                        client = resp.ClientData;
                    }else
                    {
                        user_id = -1;
                    }
                }
            }

            if (current.Items.Contains("Actionsfilter"))
            {
                actionsFilter = (ActionsFilter)current.Items["ActionsFilter"];
            }
            */
        }

        public static void getUserSession()
        {
//            current_city = (lcsite.City)get("current_city");
//            if (current_city == null)
//           {
//                var cities = ApiConfig.GetCities();
//                current_city = cities.CityData.First();
//            }
            try
            {
                user_id = (int)get("user_id");
                client = (lcclient.Client)get("client");
                if (client == null)
                {
                    user_id = -1;
                }
            }catch(Exception ex)
            {
                user_id = -1;
            }
        }

        public static void PreloadAll()
        {
            //allPartners = ApiConfig.GetPartners().PartnerData.ToList();
            //allCampaigns = ApiConfig.GetCampaigns().CampaignData.ToList();
           /*
            foreach(var p in allPartners)
            {
                var pp = ApiConfig.GetPoses(p.id).PosData.ToList();
                foreach(var pd in pp)
                {
                    allPoses.Add(pd);
                }
            }
            */
        }

        public static void setUserSession()
        {
         //   HttpContext.Current = current;
        }



        /*
            New Methods.
         */
    }
}