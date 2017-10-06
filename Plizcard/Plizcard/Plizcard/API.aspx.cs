using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlizCard
{
    public class Logger
    {
        public void write()
        {
            /*
            try
            {
                var t = DateTime.Now;
                var f = new StreamWriter("log.txt", true);
                f.WriteLine(t.ToString() + ": " + new JavaScriptSerializer().Serialize(UserSession.user_id));
                f.WriteLine(t.ToString() + ": " + new JavaScriptSerializer().Serialize(UserSession.getSessionID()));
                f.Close();
            }
            catch(Exception ex)
            {
            }
            */
        }
    }

    public class StandartResponse
    {
        public int ErrorCode=-1;
        public string Message="Unknown Error";
    }

    public class UserLoginResponse : StandartResponse
    {
        public string ClientID;
        public PlizCard.lcclient.Client ClientData;
    }

    public class CustomCampaign:lcsite.Campaign
    {
        public bool isFav = false;
    }
    public class CustomPartner:lcsite.Partner
    {
        public bool isFav = false;
    }

    public class MobileLoginResponse
    {
        public int ErrorCode = 0;
        public string Message = "";
    }
    public class CampaignListResponse:lcsite.GetCampaignsResponse
    {
        public List<CustomCampaign> mCampaignData;
    }

    public class CampaignResponse:lcsite.GetCampaignResponse
    {
        public CustomCampaign mCampaignData;
    }

    public class PartnerListResponse : lcsite.GetPartnersResponse
    {
        public List<CustomPartner> mPartnerData;
    }

    public class PartnerResponse: lcsite.GetPartnerResponse
    {
        public CustomPartner mPartnerData;
    }

    public class curCategory: lcsite.Category
    {
        public int cnt = 0;
    }
    public partial class API : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Information("Call page_load");
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            UserSession.getUserSession();
        }

        public static void RegisterData()
        {

        }

        public static MobileLoginResponse checkSession()
        {
            var ret = new MobileLoginResponse();
            //Mofucingbile: yes_baby
            if (HttpContext.Current.Request.Headers.AllKeys.Contains("mofucingbile"))
            {
                var v = (string)HttpContext.Current.Request.Headers["mofucingbile"];
                if (v != "yes_baby") return ret;
            } else
            {
                return ret;
            }
            try
            {
                if (UserSession.user_id > 0)
                {
                    var sid = UserSession.getSessionID();
                    string cursid = "";
                    if (System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains("plizcard"))
                    {
                        cursid = System.Web.HttpContext.Current.Request.Cookies["plizcard"].Value;
                    }
                    if (cursid != sid)
                    {
                        //var nd = UserSession.ReInit();
                        //HttpSessionStateBase ss = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
                        //ss.Abandon();

                        //SessionIDManager mm = new SessionIDManager();

                        //string nd = mm.CreateSessionID(System.Web.HttpContext.Current);
                        //bool rr = false;
                        //bool ii = false;
                        //mm.SaveSessionID(System.Web.HttpContext.Current, nd, out rr, out ii);

                        HttpCookie cc = new HttpCookie("plizcard");
                        cc.Expires = DateTime.Now.AddMonths(2);

                        cc.Value = sid;

                        System.Web.HttpContext.Current.Response.Cookies.Add(cc);
                        ret.ErrorCode = -100;
                        ret.Message = "Session Expired";
                        return ret;
                    }
                    else
                    {
                        
                        HttpCookie cookiez = new HttpCookie("plizcard");
                        cookiez.Expires = DateTime.Now.AddMonths(2);
                        cookiez.Value = sid;
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookiez);
                        return ret;
                        
                    }
                }
            }
            finally { }
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            session.Abandon();

            SessionIDManager manager = new SessionIDManager();

            string newID = manager.CreateSessionID(System.Web.HttpContext.Current);
            bool redirected = false;
            bool isAdded = false;
            manager.SaveSessionID(System.Web.HttpContext.Current, newID, out redirected, out isAdded);

            HttpCookie cookie = new HttpCookie("plizcard");
            cookie.Expires = DateTime.Now.AddMonths(2);
            cookie.Value = newID;

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

            ret.ErrorCode = -100;
            ret.Message = "Session Expired";
            return ret;
        }


        [System.Web.Services.WebMethod]
        public static string getCities()
        {
            Log.Information("getCities with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            string returnValue = "";
            var r = ApiConfig.GetCities();
            returnValue = new JavaScriptSerializer().Serialize(r);

            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string setCurrentCity(int id)
        {
            Log.Information("setCurrentCity with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"] + " and data " + id.ToString());
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            string returnValue = "{}";
            if (id >= 0)
            {

                HttpCookie cookie = new HttpCookie("CityID");
                cookie.Expires = DateTime.Now.AddMonths(2);
                cookie.Value = id.ToString();

                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

                //                HttpContext.Current.Session.Add("current_city", Array.Find(ApiConfig.GetCities().CityData, c => c.id == id));
                UserSession.set("current_city_id", id);
                UserSession.current_city = Array.Find(ApiConfig.GetCities().CityData, c => c.id == id);
                UserSession.set("current_city", UserSession.current_city);
                //           UserSession.setUserSession();
            }

            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string ClientLogin(string login, string passw, string idFB = "", string idOK = "", string idVK = "")
        {
            Log.Information("ClientLogin with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"]);
            string returnValue = new JavaScriptSerializer().Serialize(new StandartResponse());
            login = HttpUtility.UrlDecode(login, System.Text.Encoding.Default);
            passw = HttpUtility.UrlDecode(passw, System.Text.Encoding.Default);
            idOK = HttpUtility.UrlDecode(idOK, System.Text.Encoding.Default);
            long llogin = 0;            

            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            //var rrt = ApiConfig.getOnlineUsers();
            session.Clear();

            UserSession.set("s", 1);
            UserSession.RegisterUserSession();

            string newSessionId = session.SessionID;
            var c = new HttpCookie("plizcard", newSessionId);
            c.Expires = DateTime.Now.AddMonths(2);
            System.Web.HttpContext.Current.Response.Cookies.Add(c);

            try
            {
                long.TryParse(login.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""), out llogin);
                

                /*START REST*/

              
                /*END REST*/
                var r = ApiConfig.ClientLogin(llogin, passw, idFB, idOK, idVK);
                var cr = new UserLoginResponse();

                if (r.ErrorCode == 0) {
                    //HttpContext.Current.Session.Add("user_id", r.ClientID);
                  /*  var oss = rrt.FindAll(delegate(SessionStateItemCollection c)
                    {
                        try
                        {
                            return (int)c["user_id"] == r.ClientID;
                        }catch(Exception ex)
                        {
                            return false;
                        }
                    });
                    foreach(var ossk in oss)
                    {
                        ossk.Clear();
                    }*/
                    UserSession.set("user_id", r.ClientID);
                    UserSession.user_id = r.ClientID;
                    Log.Information("Client login success with ID " + r.ClientID.ToString() + " and id saved in session " + UserSession.user_id.ToString());
                    var rr = ApiConfig.GetClient(r.ClientID);
                    if (rr.ErrorCode == 0)
                    {
                        Log.Information("Get Client to id " + r.ClientID.ToString() + " with id saved in session " + UserSession.user_id.ToString());
                        //  HttpContext.Current.Session.Add("client", rr.ClientData);
                        UserSession.set("client", rr.ClientData);
                        UserSession.client = rr.ClientData;
                        cr.ClientData = rr.ClientData;
                        cr.ClientData.id = r.ClientID;

                        GetClientPartners(r.ClientID);
                        GetClientCampaigns(r.ClientID);

                    }
                    else
                    {
                        // HttpContext.Current.Session.Add("user_id", -1);
                        UserSession.set("user_id", -1);
                        UserSession.user_id = -1;
                        r.ErrorCode = -1;
                        r.Message = "Inner server error.";
                    }
                    //                    UserSession.setUserSession();
                }
                cr.ErrorCode = r.ErrorCode;
                cr.Message = r.Message;
                cr.ClientID = newSessionId;
                Log.Information("ClientLogin newSessionId " + newSessionId);

                returnValue = new JavaScriptSerializer().Serialize(cr);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string ClientLogout()
        {
            Log.Information("ClientLogout with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"]);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            string returnValue = new JavaScriptSerializer().Serialize(r);
            HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
            session.Clear();
            session.Abandon();

            SessionIDManager manager = new SessionIDManager();

            string newID = manager.CreateSessionID(System.Web.HttpContext.Current);
            bool redirected = false;
            bool isAdded = false;
            manager.SaveSessionID(System.Web.HttpContext.Current, newID, out redirected, out isAdded);
            return returnValue;
        }
        public static lcsite.City getCurrentCity()
        {
            lcsite.City returnValue = new lcsite.City();
            returnValue = (lcsite.City)UserSession.get("current_city");

            //            returnValue = UserSession.current_city;
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string CurrentCity()
        {
            Log.Information("CurrentCity with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"]);
            //            lcsite.City r = new lcsite.City();
            //            r = UserSession.current_city;
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            lcsite.City r = new lcsite.City();
            r = (lcsite.City)UserSession.get("current_city");

            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [WebMethod]
        public static string GetProfile()
        {
            Log.Information("GetProfile with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = ApiConfig.GetClient(UserSession.user_id);
            if (ret.ErrorCode == 0) ret.ClientData.id = 0;
            return new JavaScriptSerializer().Serialize(ret);
        }

        public static List<lcsite.Partner> getPartners(bool useCategory = true, int start=0, int cnt=1)
        {
            int cat_id_filter = 0;
            var s_PartnerCategoryFilter = UserSession.get("PartnerCategoryFilter");
            if (s_PartnerCategoryFilter != null) cat_id_filter = (int)s_PartnerCategoryFilter;
            if (cat_id_filter < 0) cat_id_filter = 0;

            bool iscard_issue_filter = false;
            var s_IsCardIssueFilter = UserSession.get("PartnerIsCardIssueFilter");
            if (s_IsCardIssueFilter != null) iscard_issue_filter = (bool)s_IsCardIssueFilter;

            bool isincity_filter = false;
            var s_IsInCityFilter = UserSession.get("PartnerIsInCityFilter");
            if (s_IsInCityFilter != null) isincity_filter = (bool)s_IsInCityFilter;

            bool isininternet_filter = false;
            var s_IsInInternetFilter = UserSession.get("PartnerIsInInternetFilter");
            if (s_IsInInternetFilter != null) isininternet_filter = (bool)s_IsInInternetFilter;

            var ret = ApiConfig.GetPartners(false, iscard_issue_filter, isincity_filter, isininternet_filter, 0, (useCategory) ? cat_id_filter : 0).PartnerData.ToList();
            var rcnt = ret.Count;
            if (cnt != -1)
            {
                if ((rcnt >= (start + cnt)))
                {
                    ret = ret.GetRange(start, cnt);
                }
                else if (rcnt >= start)
                {
                    ret = ret.GetRange(start, rcnt - start);
                }
                else
                {
                    ret.Clear();
                }
            }
            var q = new List<lcclient.Partner>();
            if (UserSession.user_id > 0)
            {
                q = GetClientPartners();
            }

            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);

                for (int i = 0; i < ret.Count(); i++)
                {
                    ret[i].isFav = (q.FindAll(em => em.id == ret[i].id).Count > 0);
                    ret[i].share_url = Domain + "/Partners/Details/" + ret[i].id.ToString();
                }

            return ret;
        }

        [WebMethod]
        public static string PartnersCnt(int start=0, int cnt=1)
        {
            Log.Information("PartnersCnt with cookie " + (string)HttpContext.Current.Request.Headers["Cookie"] + " and data start = " + start.ToString() + " and cnt = " + cnt.ToString());
            var p = getPartners(true, start, cnt);
            return new JavaScriptSerializer().Serialize(p);
        }

        [System.Web.Services.WebMethod]
        public static string Partners()
        {
            Log.Information("{Method} with cookie {cookie}", "Partners", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            var r = ApiConfig.GetPartners();
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        public static List<lcsite.Partner> getPartners(int cnt)
        {
            var partners = ApiConfig.GetPartners(true).PartnerData.ToList();
            if (partners.Count > cnt)
                partners.RemoveRange(cnt, partners.Count - cnt);
            return partners;
        }

        public static List<lcsite.Campaign> GetCampaigns(bool onminepage = false, bool category = true, int start=0, int cnt=1)
        {
            int cat_id_filter = 0;
            int partner_id_filter = 0;
            int segment_id_filter = 0;
            bool isnew_filter = false;
            bool ispopular_filter = false;
            var s_ActionPartnerFilter = UserSession.get("ActionPartnerFilter");
            var s_ActionCategoryFilter = UserSession.get("ActionCategoryFilter");
            var s_ActionSegmentFilter = UserSession.get("ActionSegmentFilter");
            var s_ActionIsNewFilter = UserSession.get("ActionIsNewFilter");
            var s_ActionIsPopularFilter = UserSession.get("ActionIsPopularFilter");
            if (s_ActionPartnerFilter != null) partner_id_filter = (int)s_ActionPartnerFilter;
            if (s_ActionCategoryFilter != null) cat_id_filter = (int)s_ActionCategoryFilter;
            if (s_ActionSegmentFilter != null) segment_id_filter = (int)s_ActionSegmentFilter;
            if (s_ActionIsNewFilter != null) isnew_filter = (bool)s_ActionIsNewFilter;
            if (s_ActionIsPopularFilter != null) ispopular_filter = (bool)s_ActionIsPopularFilter;
            if (partner_id_filter < 0) partner_id_filter = 0;
            if (cat_id_filter < 0) cat_id_filter = 0;
            if (segment_id_filter < 0) segment_id_filter = 0;
            bool issidebar_filter = false;

            if (!category) cat_id_filter = 0;

            if (onminepage)
            {
                var campaigns = ApiConfig.GetCampaigns(onminepage).CampaignData.ToList();
                var ret = new List<lcsite.Campaign>();
                var large = campaigns.FindAll(m => m.large == true);
                var small = campaigns.FindAll(m => m.large != true);

                if (small.Count >= 3)
                {
                    if (large.Count > 0)
                        ret.Add(large[0]);
                    ret.Add(small[0]);
                    ret.Add(small[1]);
                    ret.Add(small[2]);

                    if (large.Count > 1)
                        ret.Add(large[1]);

                    if (small.Count > 3) ret.Add(small[3]);
                    if (small.Count > 4) ret.Add(small[4]);
                    if (small.Count > 5) ret.Add(small[5]);
                }

                return ret;
            } else
            {
                var ret = ApiConfig.GetCampaigns(onminepage, partner_id_filter, issidebar_filter, segment_id_filter, cat_id_filter, isnew_filter, ispopular_filter).CampaignData.ToList();

                var rcnt = ret.Count;

                if (cnt != -1)
                {
                    if ((rcnt >= (start + cnt)))
                    {
                        ret = ret.GetRange(start, cnt);
                    }
                    else if (rcnt >= start)
                    {
                        ret = ret.GetRange(start, rcnt - start);
                    }
                    else
                    {
                        ret.Clear();
                    }
                }
                var q = new List<lcclient.Campaign>();
                if (UserSession.user_id > 0)
                {
                    q = GetClientCampaigns();
                }

                string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);

                for (int i = 0; i < ret.Count(); i++)
                {
                    ret[i].isFav = (q.FindAll(em => em.id == ret[i].id).Count > 0);
                    ret[i].share_url = Domain + "/Actions/Details/" + ret[i].id.ToString();
                }

                return ret;
            }

        }

        [WebMethod]
        public static string CampaignsCnt(int start = 0, int cnt = 1)
        {
            Log.Information("{Method} with cookie {cookie} and data start = {start} and cnt = {cnt}", "CampaignsCnt", (string)HttpContext.Current.Request.Headers["Cookie"], start, cnt);
            var p = GetCampaigns(false,true, start, cnt);
            return new JavaScriptSerializer().Serialize(p);
        }

        [System.Web.Services.WebMethod]
        public static string Campaigns(bool onminepage = false)
        {
            Log.Information("{Method} with cookie {cookie}", "Campaigns", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            int cat_id_filter = 0;
            int partner_id_filter = 0;
            bool issidebar_filter = false;
            int segment_id = 0;
            var r = ApiConfig.GetCampaigns(onminepage);
            Log.Information("{Method} returns qty = {count} of campaigns ", "Campaigns", r.CampaignData.Count());
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        public static List<lcsite.Campaign> GetRandomCampaigns(int cnt = 1, bool onminepage = false, int PartnerID = 0, bool IsSideBar = false)
        {
            var rand = new Random();
            var campaigns = ApiConfig.GetCampaigns(onminepage, PartnerID, IsSideBar).CampaignData.ToList();
            if (campaigns.Count <= cnt) return campaigns;
            List<lcsite.Campaign> ret = new List<lcsite.Campaign>();
            while (ret.Count < cnt)
            {
                int i = rand.Next(campaigns.Count);
                if (!ret.Contains(campaigns[i]))
                {
                    ret.Add(campaigns[i]);
                }
            }
            return ret;
        }

        public static List<lcsite.Segment> getSegments()
        {
            return ApiConfig.GetSegments().SegmentData.ToList();
        }

        [System.Web.Services.WebMethod]
        public static string Segments()
        {
            Log.Information("{Method} with cookie {cookie}", "Segments", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetSegments();
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }
        public static List<lcsite.Category> getCategories()
        {
            return ApiConfig.GetCategories().CategoryData.ToList();
        }

        public static List<curCategory> getCategoriesPartners()
        {
            var ret = new List<curCategory>();
            var catList = ApiConfig.GetCategories().CategoryData.ToList();
            var parList = API.getPartners(false,0,-1);

            var rt = new curCategory();
            rt.id = -1;
            rt.name = "All";
            rt.partners = -1;
            rt.campaigns = -1;
            rt.cnt = parList.Count;
            ret.Add(rt);

            foreach (var cat in catList)
            {
                var r = new curCategory();
                r.id = cat.id;
                r.name = cat.name;
                r.partners = cat.partners;
                r.campaigns = cat.campaigns;
                r.cnt = parList.FindAll(em => em.categoryId.Contains(cat.id)).Count;
                ret.Add(r);
            }
            return ret;
        }

        public static List<curCategory> getCategoriesCampaigns()
        {
            var partner_filter = UserSession.get("ActionPartnerFilter");
            int partner_filter_id = (partner_filter == null) ? 0 : ((int)partner_filter);

            var isnew_filter = UserSession.get("ActionIsNewFilter");
            bool isnew = (isnew_filter == null) ? false : ((bool)isnew_filter);

            var ispopular_filter = UserSession.get("ActionIsPopularFilter");
            bool ispopular = (ispopular_filter == null) ? false : ((bool)ispopular_filter);


            var ret = new List<curCategory>();
            var catList = ApiConfig.GetCategories().CategoryData.ToList();
            var camList = API.GetCampaigns(false, false,0,-1);

            var rr = new curCategory();
            rr.id = -1;
            rr.name = "All";
            rr.partners = -1;
            rr.campaigns = -1;
            rr.cnt = camList.Count;
            ret.Add(rr);

            foreach (var cat in catList)
            {
                var r = new curCategory();
                r.id = cat.id;
                r.name = cat.name;
                r.partners = cat.partners;
                r.campaigns = cat.campaigns;
                r.cnt = camList.FindAll(em => em.categoryId.Contains(cat.id)).Count;
                ret.Add(r);
            }
            return ret;
        }


        [System.Web.Services.WebMethod]
        public static string Categories()
        {
            Log.Information("{Method} with cookie {cookie}", "Segments", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetCategories();
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }


        public static lcsite.Partner GetPartner(int id)
        {
            return ApiConfig.GetPartner(id).PartnerData;
        }

        [System.Web.Services.WebMethod]
        public static string Partner(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "Segments", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetPartner(id);

            var q = new List<lcclient.Partner>();
            if (UserSession.user_id > 0)
            {
                q = GetClientPartners();
            }
            if (r.ErrorCode == 0)
            {
                string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
                r.PartnerData.isFav = (q.FindAll(em => em.id == id).Count > 0);
                r.PartnerData.share_url = Domain + "/Partners/Details/" + id.ToString();
            }
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        public static List<lcsite.Pos> GetPoses(int PartnerID)
        {
            return ApiConfig.GetPoses(PartnerID).PosData.ToList();
        }

        [System.Web.Services.WebMethod]
        public static string Poses(int PartnerID)
        {
            Log.Information("{Method} with cookie {cookie} and PartnerID = {PartnerID}", "Poses", (string)HttpContext.Current.Request.Headers["Cookie"], PartnerID);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetPoses(PartnerID);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }


        public static List<lcsite.PartnerInfo> GetPartnerInfo(int id)
        {
            return ApiConfig.GetPartnerInfo(id).PartnerInfoData.ToList();
        }

        [System.Web.Services.WebMethod]
        public static string PartnerInfo(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "PartnerInfo", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetPartnerInfo(id);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetActionPartnerFilter(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "SetActionPartnerFilter", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("ActionPartnerFilter", id);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetActionSegmentFilter(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "SetActionSegmentFilter", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("ActionSegmentFilter", id);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetActionCategoryFilter(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "SetActionCategoryFilter", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("ActionCategoryFilter", id);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetActionIsNewFilter(bool value)
        {
            Log.Information("{Method} with cookie {cookie} and value = {value}", "SetActionIsNewFilter", (string)HttpContext.Current.Request.Headers["Cookie"], value);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("ActionIsNewFilter", value);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetActionIsPopularFilter(bool value)
        {
            Log.Information("{Method} with cookie {cookie} and value = {value}", "SetActionIsPopularFilter", (string)HttpContext.Current.Request.Headers["Cookie"], value);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("ActionIsPopularFilter", value);
            return returnValue;
        }


        [System.Web.Services.WebMethod]
        public static string SetPartnerCategoryFilter(int id)
        {
            Log.Information("{Method} with cookie {cookie} and id = {id}", "SetPartnerCategoryFilter", (string)HttpContext.Current.Request.Headers["Cookie"], id);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("PartnerCategoryFilter", id);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetPartnerIsCardIssueFilter(bool value)
        {
            Log.Information("{Method} with cookie {cookie} and value = {value}", "SetPartnerIsCardIssueFilter", (string)HttpContext.Current.Request.Headers["Cookie"], value);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("PartnerIsCardIssueFilter", value);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetPartnerIsInCityFilter(bool value)
        {
            Log.Information("{Method} with cookie {cookie} and value = {value}", "SetPartnerIsInCityFilter", (string)HttpContext.Current.Request.Headers["Cookie"], value);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("PartnerIsInCityFilter", value);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetPartnerIsInInternetFilter(bool value)
        {
            Log.Information("{Method} with cookie {cookie} and value = {value}", "SetPartnerIsInInternetFilter", (string)HttpContext.Current.Request.Headers["Cookie"], value);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = new StandartResponse();
            r.ErrorCode = 0;
            r.Message = "";
            var returnValue = new JavaScriptSerializer().Serialize(r);
            UserSession.set("PartnerIsInInternetFilter", value);
            return returnValue;
        }



        [System.Web.Services.WebMethod]
        public static string GetVerificationCode(string Phone)
        {
            Log.Information("{Method} with cookie {cookie} and Phone = {Phone}", "GetVerificationCode", (string)HttpContext.Current.Request.Headers["Cookie"], Phone);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length<13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            var r = ApiConfig.GetVerificationCode(llogin);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetConfirmCode(string Phone, string Code)
        {
            Log.Information("{Method} with cookie {cookie} and Phone = {Phone} and Code = {Code}", "GetConfirmCode", (string)HttpContext.Current.Request.Headers["Cookie"], Phone, Code);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            var r = ApiConfig.GetConfirmCode(llogin, Code);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetRegistrationUser(lcclient.Client client)
        {
            Log.Information("{Method} with cookie {cookie} and Phone = {Phone} and id = {id}", "GetRegistrationUser", (string)HttpContext.Current.Request.Headers["Cookie"], client.phone, client.id);
            return GetRegistrationUser(client, 0);
        }

        [System.Web.Services.WebMethod]
        public static string GetRegistrationUser(lcclient.Client client, long card)
        {
            Log.Information("{Method} with cookie {cookie} and Phone = {Phone} and id = {id} and card = {card}", "GetRegistrationUser", (string)HttpContext.Current.Request.Headers["Cookie"], client.phone, client.id, card);
            var rt = new UserLoginResponse();
            string returnValue = new JavaScriptSerializer().Serialize(rt);
            var rr = ApiConfig.GetRegistrationUser(client.phone, card);
            if (rr.ErrorCode != 0) {
                rt.ErrorCode = rr.ErrorCode;
                rt.Message = rr.Message;
                return new JavaScriptSerializer().Serialize(rt);
            }

            client.id = rr.Client;

            var r = ApiConfig.ChangeClient(client);

            if (r.ErrorCode != 0) {
                rt.ErrorCode = r.ErrorCode;
                rt.Message = r.Message;
                return new JavaScriptSerializer().Serialize(r);
            }

            var cl = ApiConfig.GetClient(rr.Client);
            rt.ErrorCode = cl.ErrorCode;
            rt.Message = cl.Message;

            cl.ClientData.id = rr.Client;

            if (cl.ErrorCode == 0)
            {
                UserSession.set("user_id", rr.Client);
                UserSession.user_id = rr.Client;

                UserSession.set("client", null);
                UserSession.client = null;
                rt.ClientID = UserSession.getSessionID();
                rt.ClientData = cl.ClientData;
                returnValue = new JavaScriptSerializer().Serialize(rt);
            }


            return new JavaScriptSerializer().Serialize(rt);
        }

        public static List<lcclient.Cheque> GetCheque()
        {
            int client_id = -1;
            var clid = UserSession.get("user_id");
            if (client_id == -1 && clid != null) client_id = (int)clid;
            var returnValue = ApiConfig.GetCheques(client_id).ChequeData.ToList();
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string Cheque()
        {
            Log.Information("{Method} with cookie {cookie}", "Cheque", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var clid = UserSession.get("user_id");
            int client_id = (int)clid;
            var returnValue = ApiConfig.GetCheques(client_id);
            return new JavaScriptSerializer().Serialize(returnValue);
        }

        public static List<lcclient.Card> GetClientCards()
        {
            var clid = UserSession.get("user_id");
            int client_id = (int)clid;
            var returnValue = ApiConfig.GetClientCards(client_id).CardData.ToList();
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string ClientCards()
        {
            Log.Information("{Method} with cookie {cookie}", "ClientCards", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var clid = UserSession.get("user_id");
            int client_id = (int)clid;
            var returnValue = ApiConfig.GetClientCards(client_id);
            return new JavaScriptSerializer().Serialize(returnValue);
        }


        /*
         Special for mobile
         */
        [System.Web.Services.WebMethod]
        public static string CampaignsMobile(bool onminepage = false, int city_id = 0, int partner_id = 0, bool issidebar = false, int segment_id = 0, int category_id = 0, bool isNew = false, bool isPopular = false)
        {
            Log.Information("{Method} with cookie {cookie}", "CampaignsMobile", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetCampaignsMobile(onminepage, city_id, partner_id, issidebar, segment_id, category_id, isNew, isPopular);

            var q = new List<lcclient.Campaign>();
            if (UserSession.user_id > 0)
            {
                q = GetClientCampaigns();
            }


            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            if(r.ErrorCode==0)
            for (int i =0;i<r.CampaignData.Count();i++)
            {
                r.CampaignData[i].isFav = (q.FindAll(em => em.id == r.CampaignData[i].id).Count > 0);
                r.CampaignData[i].share_url = Domain + "/Actions/Details/" + r.CampaignData[i].id.ToString();
            }
            Log.Information("{Method} with cookie {cookie} and campaign qty = {qty}", "CampaignsMobile", (string)HttpContext.Current.Request.Headers["Cookie"], r.CampaignData.Count());
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string PartnersMobile(bool onminepage = false, bool isCardIssue = false, bool isInCity = false, bool isInInternet = false, int city_id = 0, int segment_id = 0, int category_id = 0)
        {
            Log.Information("{Method} with cookie {cookie}", "PartnersMobile", (string)HttpContext.Current.Request.Headers["Cookie"]);
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetPartnersMobile(onminepage, isCardIssue, isInCity, isInInternet, city_id, segment_id, category_id);


            var q = new List<lcclient.Partner>();
            if (UserSession.user_id > 0)
            {
                q = GetClientPartners();
            }

            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);

            if(r.ErrorCode==0)
            for (int i=0;i<r.PartnerData.Count();i++)
            {
                r.PartnerData[i].isFav = (q.FindAll(em => em.id == r.PartnerData[i].id).Count > 0);
                r.PartnerData[i].share_url = Domain + "/Partners/Details/" + r.PartnerData[i].id.ToString();
            }
            Log.Information("{Method} with cookie {cookie} and partners qty = {qty}", "PartnersMobile", (string)HttpContext.Current.Request.Headers["Cookie"], r.PartnerData.Count());
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SetClientPassword(string Phone, string Code, string Password)
        {
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            var passw = HttpUtility.UrlDecode(Password, System.Text.Encoding.Default);
            var r = ApiConfig.SetClientPassword(llogin, Code, passw);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }


        [System.Web.Services.WebMethod]
        public static string SaveProfile1(string Lastname, string Firstname, string Middlename, int Gender, bool Haschildren, DateTime Birthday, string Description)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var desc = HttpUtility.UrlDecode(Description, System.Text.Encoding.Default);

            var client = UserSession.client;
            client.id = UserSession.user_id;
            client.lastname = Lastname;
            client.firstname = Firstname;
            client.middlename = Middlename;
            client.gender = Gender;
            client.haschildren = Haschildren;
            client.birthdate = Birthday;
            client.description = desc;
            UserSession.set("client", client);
            UserSession.client = client;

            var r = ApiConfig.ChangeClient(client);

            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string SaveProfilePhone(string Phone, string Code)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            var client = UserSession.client;
            client.id = UserSession.user_id;

            var returnValue = "{}";

            var request_confirm_code = ApiConfig.GetConfirmCode(llogin, Code);
            if (request_confirm_code.ErrorCode != 0)
            {
                returnValue = new JavaScriptSerializer().Serialize(request_confirm_code);
            } else
            {
                UserSession.set("client", client);
                UserSession.client = client;

                var r = ApiConfig.ChangeClient(client);
                returnValue = new JavaScriptSerializer().Serialize(r);
            }
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetCampaign(int CampaignID)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetCampaign(CampaignID);
            List<lcclient.Campaign> q = new List<lcclient.Campaign>();
            if (UserSession.user_id > 0)
            {
                q = GetClientCampaigns();
            }



            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            if (r.ErrorCode == 0)
                    r.CampaignData.isFav = (q.FindAll(em => em.id == r.CampaignData.id).Count > 0);
                    r.CampaignData.share_url = Domain + "/Actions/Details/" + r.CampaignData.id.ToString();


            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string GetCampaignInfo(int CampaignID)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.GetCampaignInfo(CampaignID);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        public static List<lcsite.CampaignInfo> GetCampaign_Info(int CampaignID)
        {
            var r = ApiConfig.GetCampaignInfo(CampaignID);
            return r.CampaignInfoData.ToList();
        }

        [System.Web.Services.WebMethod]
        public static string ClientPartners()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var r = ApiConfig.ClientGetPartners();

            var q = new List<lcclient.Partner>();
            if (UserSession.user_id > 0)
            {
                q = GetClientPartners();
            }

            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);

            if (r.ErrorCode == 0)
                for (int i = 0; i < r.PartnerData.Count(); i++)
                {
                    r.PartnerData[i].isFav = (q.FindAll(em => em.id == r.PartnerData[i].id).Count > 0);
                    r.PartnerData[i].share_url = Domain + "/Partners/Details/" + r.PartnerData[i].id.ToString();
                }

            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }

        [System.Web.Services.WebMethod]
        public static string ClientCampaigns()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            var r = ApiConfig.ClientGetCampaigns();

            var q = new List<lcclient.Campaign>();
            if (UserSession.user_id > 0)
            {
                q = GetClientCampaigns();
            }


            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            if (r.ErrorCode == 0)
                for (int i = 0; i < r.CampaignData.Count(); i++)
                {
                    r.CampaignData[i].isFav = (q.FindAll(em => em.id == r.CampaignData[i].id).Count > 0);
                    r.CampaignData[i].share_url = Domain + "/Actions/Details/" + r.CampaignData[i].id.ToString();
                }

            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;
        }


        public static List<lcclient.Partner> GetClientPartners(int ClientID = -1)
        {
            var r = ApiConfig.ClientGetPartners(ClientID);
            UserSession.set("personalPartners", r.PartnerData.ToList());
            return r.PartnerData.ToList();
        }

        public static List<lcclient.Campaign> GetClientCampaigns(int ClientID = -1)
        {
            var r = ApiConfig.ClientGetCampaigns(ClientID);
            UserSession.set("personalCampaigns", r.CampaignData.ToList());
            return r.CampaignData.ToList();
        }

        [System.Web.Services.WebMethod]
        public static string GetSetClientCampaign(int CampaignID, bool Remove = false)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var personalCampaigns = (List<lcclient.Campaign>)UserSession.get("personalCampaigns");
            if (personalCampaigns == null)
            {
                personalCampaigns = ApiConfig.ClientGetCampaigns().CampaignData.ToList();
            }
            if (personalCampaigns == null)
            {
                personalCampaigns = new List<lcclient.Campaign>();
            }
            var srch = personalCampaigns.Find(em => em.id == CampaignID);
            if (srch != null)
            {
                Remove = true;
                personalCampaigns.Remove(srch);
                UserSession.set("personalCampaigns", personalCampaigns);
            } else
            {
                Remove = false;
                var pr = new lcclient.Campaign();
                pr.id = CampaignID;
                personalCampaigns.Add(pr);
                UserSession.set("personalCampaigns", personalCampaigns);
            }
            var r = ApiConfig.ClientGetSetCampaignsFav(CampaignID, Remove);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;

        }

        [System.Web.Services.WebMethod]
        public static string GetSetClientPartner(int PartnerID, bool Remove = false)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var personalPartners = (List<lcclient.Partner>)UserSession.get("personalPartners");
            if (personalPartners == null)
                personalPartners = ApiConfig.ClientGetPartners().PartnerData.ToList();
            if (personalPartners == null)
                personalPartners = new List<lcclient.Partner>();
            var srch = personalPartners.Find(em => em.id == PartnerID);
            if (srch != null)
            {
                Remove = true;
                personalPartners.Remove(srch);
                UserSession.set("personalPartners", personalPartners);
            } else
            {
                Remove = false;
                var pr = new lcclient.Partner();
                pr.id = PartnerID;
                personalPartners.Add(pr);
                UserSession.set("personalPartners", personalPartners);
            }
            var r = ApiConfig.ClientGetSetPartnersFav(PartnerID, Remove);
            var returnValue = new JavaScriptSerializer().Serialize(r);
            return returnValue;

        }

        public static bool isFavPartner(int PartnerID)
        {
            bool returnValue = false;
            var r = (List<lcclient.Partner>)UserSession.get("personalPartners");
            if (r != null)
            {
                var r2 = r.Find(em => em.id == PartnerID);
                if (r2 != null) returnValue = true;
            }
            return returnValue;
        }

        public static bool isFavCampaing(int CampaingID)
        {
            bool returnValue = false;
            var r = (List<lcclient.Campaign>)UserSession.get("personalCampaigns");
            if (r != null)
            {
                var r2 = r.Find(em => em.id == CampaingID);
                if (r2 != null) returnValue = true;
            }
            return returnValue;
        }


        /*
         NEW Methods.
         */

        [WebMethod]
        public static string getSessionID()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            return UserSession.getSessionID();
        }

        [WebMethod]
        public static string AddEmail(string Email)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.AddEmail(Email));
        }

        [WebMethod]
        public static string AddIDFB(string idFB)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.AddIDFB(idFB));
        }

        [WebMethod]
        public static string AddIDVK(string idVK)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.AddIDVK(idVK));
        }

        [WebMethod]
        public static string AddIDOK(string idOK)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.AddIDOK(idOK));
        }

        [WebMethod]
        public static string AddPhone(string Phone)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            return new JavaScriptSerializer().Serialize(ApiConfig.AddPhone(llogin));
        }


        [WebMethod]
        public static string GetBalance(string Phone)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            return new JavaScriptSerializer().Serialize(ApiConfig.GetBalance(llogin));
        }

        [WebMethod]
        public static string ClientAddCard(string Card)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            long llogin = 0;
            Card = HttpUtility.UrlDecode(Card, System.Text.Encoding.Default);

            Regex digitsOnly = new Regex(@"[^\d]");
            Card = digitsOnly.Replace(Card, "");
            llogin = long.Parse(Card);
            return new JavaScriptSerializer().Serialize(ApiConfig.ClientAddCard(llogin));
        }

        [WebMethod]
        public static string DeletePhone(string Phone)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            long llogin = 0;
            Phone = HttpUtility.UrlDecode(Phone, System.Text.Encoding.Default);
            Phone = Phone.Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            if (Phone.Length > 10 && Phone.Length < 13) Phone = Phone.Remove(0, Phone.Length - 10);
            if (Phone.Length > 13) Phone = Phone.Remove(0, Phone.Length - 13);
            llogin = long.Parse(Phone);

            return new JavaScriptSerializer().Serialize(ApiConfig.DeletePhone(llogin));
        }

        [WebMethod]
        public static string LeaveMessage(string Subject, string Text)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.LeaveMessage(Subject, Text));
        }

        [WebMethod]
        public static string SelectPreferences(int CategoryID, bool Remove)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.SelectPreferences(CategoryID, Remove));
        }

        [WebMethod]
        public static string SendEmailCode(string Email)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.SendEmailCode(Email));
        }

        [WebMethod]
        public static string ValidateEmail(string Email, string Code, int Client = 0)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.ValidateEmail(Email, Code, Client));
        }

        [WebMethod]
        public static string ClientAddEmail(string Email, string Code)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = new StandartResponse();
            var r = ApiConfig.ValidateEmail(Email, Code);
            if (r.ErrorCode == 0)
            {
                var rr = ApiConfig.AddEmail(Email);
                if (rr.ErrorCode == 0)
                {
                    UserSession.client.email = Email;
                }
                return new JavaScriptSerializer().Serialize(rr);
            }
            else
            {
                ret.ErrorCode = r.ErrorCode;
                ret.Message = r.Message;
            }
            return new JavaScriptSerializer().Serialize(ret);
        }
        public static List<lcclient.Campaign> GetPersonalCampaigns()
        {
            return ApiConfig.GetPersonalCampaigns().CampaignData.ToList();
        }

        [WebMethod]
        public static string PersonalCampaigns()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);

            var r = ApiConfig.GetPersonalCampaigns();
            List<lcclient.Campaign> q = new List<lcclient.Campaign>();
            if (UserSession.user_id > 0)
            {
                q = GetClientCampaigns();
            }

            string Domain = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
            if (r.ErrorCode == 0)
                for (int i = 0; i < r.CampaignData.Count(); i++)
                {
                    r.CampaignData[i].isFav = (q.FindAll(em => em.id == r.CampaignData[i].id).Count > 0);
                    r.CampaignData[i].share_url = Domain + "/Actions/Details/" + r.CampaignData[i].id.ToString();
                }

            return new JavaScriptSerializer().Serialize(r);
        }

        [WebMethod]
        public static string ChangeClientPassword(string Password)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var response = ApiConfig.ChangeClientPassword(Password);
            return new JavaScriptSerializer().Serialize(response);
        }

        [WebMethod]
        public static string Faq()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.GetFaq());
        }

        [WebMethod]
        public static string ClientPreferences()
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            return new JavaScriptSerializer().Serialize(ApiConfig.GetClientPreferences());
        }

        [WebMethod]
        public static string AllowSMS(bool allow)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            UserSession.client.id = UserSession.user_id;
            UserSession.client.allowsms = allow;
            var r = ApiConfig.ChangeClient(UserSession.client);
            UserSession.client.id = 0;
            return new JavaScriptSerializer().Serialize(r);
        }

        [WebMethod]
        public static string AllowEMAIL(bool allow)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            UserSession.client.id = UserSession.user_id;
            UserSession.client.allowemail = allow;
            var r = ApiConfig.ChangeClient(UserSession.client);
            UserSession.client.id = 0;
            return new JavaScriptSerializer().Serialize(r);
        }

        [WebMethod]
        public static string AllowPUSH(bool allow)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            UserSession.client.id = UserSession.user_id;
            UserSession.client.allowpush = allow;
            var r = ApiConfig.ChangeClient(UserSession.client);
            UserSession.client.id = 0;
            return new JavaScriptSerializer().Serialize(r);
        }

        [WebMethod]
        public static string ChangeClientProfile(lcclient.Client ClientData)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            ClientData.id = UserSession.user_id;
            var r = ApiConfig.ChangeClient(ClientData);
            return new JavaScriptSerializer().Serialize(r);
        }


        [WebMethod]
        public static string LeaveMessageUnregister(string Email, string Subject, string Text, string Capcha)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var str = (string)UserSession.get("CaptchaImageText");
            var ret = new StandartResponse();
            if (str.ToLower() == Capcha.ToLower())
            {
                var rr = ApiConfig.LeaveMessage(Email, Subject, Text);
                return new JavaScriptSerializer().Serialize(rr);
            }
            ret.ErrorCode = 123;
            ret.Message = "Неправильный текст на картинке";
            return new JavaScriptSerializer().Serialize(ret);
        }

        [WebMethod]
        public static string GetCampaignDetail(int CampaignID)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = ApiConfig.GetCampaignDetail(CampaignID);
            return new JavaScriptSerializer().Serialize(ret);
        }


        [WebMethod]
        public static string SetPreferences(int CategoryID, bool Set)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = ApiConfig.SelectPreferences(CategoryID, !Set);
            return new JavaScriptSerializer().Serialize(ret);
        }

        [WebMethod]
        public static string ChequeDetails(int ChequeID)
        {
            var cs = checkSession();
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = ApiConfig.GetChequeDetails(ChequeID);
            return new JavaScriptSerializer().Serialize(ret);
        }

        public static lcclient.ChequeDetail GetChequeDetails(int ChequeID)
        {
            var ret = ApiConfig.GetChequeDetails(ChequeID);
            return ret.ChequeDetailData;
        }

        [WebMethod]
        public static string SetClientDevice(string Token, string OSRegistrator)
        {
            Log.Information("SetClientDevice with cookie" + (string)HttpContext.Current.Request.Headers["Cookie"]);
            Log.Information("SetClientDevice saved user id " + UserSession.user_id.ToString() + " in session before checksession");
            var cs = checkSession();
            Log.Information("SetClientDevice saved user id " + UserSession.user_id.ToString() + " in session after checksession");
            if (cs.ErrorCode != 0) return new JavaScriptSerializer().Serialize(cs);
            var ret = ApiConfig.SetClientDevice(Token, OSRegistrator);  
            return new JavaScriptSerializer().Serialize(ret);

        }

        [WebMethod]
        public static string BecomePartner(string City, string Site, string GoodsSell, int PosQty, string CashSoftware, string Name, long Phone, string Email)
        {
            var ret = ApiConfig.BecomePartner(City, Site, GoodsSell, PosQty, CashSoftware, Name, Phone, Email);            
            return new JavaScriptSerializer().Serialize(ret);
        }
    }
}