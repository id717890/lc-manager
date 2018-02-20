using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Models
{
    public class Segment
    {
        public int id { get; set; }
        public string name { get; set; }
        public Segment() { }
        public Segment(int Id, string Name)
        {
            id = Id;
            name = Name;
        }
    }

    public class GetSegmentsRequest
    {
        public Int16 Operator { get; set; }
    }

    public class GetSegmentsResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Segment> SegmentData { get; set; }
        public GetSegmentsResponse()
        {
            SegmentData = new List<Segment>();
        }
    }

    public class ServerGetSegmentsResponse
    {
        public GetSegmentsResponse ProcessRequest(SqlConnection cnn, GetSegmentsRequest request)
        {
            GetSegmentsResponse returnValue = new GetSegmentsResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Segments";
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
                Segment segment = new Segment();
                segment.id = reader.GetInt16(0);
                segment.name = reader.GetString(1);
                returnValue.SegmentData.Add(segment);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public int partners { get; set; }
        public int campaigns { get; set; }
        public Category() { }
        public Category(int Id, string Name, int Partners, int Campaigns)
        {
            id = Id;
            name = Name;
            partners = Partners;
            campaigns = Campaigns;
        }
    }

    public class GetCategoriesRequest
    {
        public Int16 Operator { get; set; }
    }

    public class GetCategoriesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Category> CategoryData { get; set; }
        public GetCategoriesResponse()
        {
            CategoryData = new List<Category>();
        }
    }

    public class ServerGetCategoriesResponse
    {
        public GetCategoriesResponse ProcessRequest(SqlConnection cnn, GetCategoriesRequest request)
        {
            GetCategoriesResponse returnValue = new GetCategoriesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Categories";
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
                Category category = new Category();
                category.id = reader.GetInt16(0);
                category.name = reader.GetString(1);
                if (!reader.IsDBNull(2)) category.partners = reader.GetInt32(2);
                if (!reader.IsDBNull(3)) category.campaigns = reader.GetInt32(3);
                returnValue.CategoryData.Add(category);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public City() { }
        public City(int Id, string Name, string Region)
        {
            id = Id;
            name = Name;
            region = Region;
        }
    }

    public class GetCitiesRequest
    {
        public Int16 Operator { get; set; }
    }

    public class GetCitiesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<City> CityData { get; set; }
        public GetCitiesResponse()
        {
            CityData = new List<City>();
        }
    }

    public class ServerGetCitiesResponse
    {
        public GetCitiesResponse ProcessRequest(SqlConnection cnn, GetCitiesRequest request)
        {
            GetCitiesResponse returnValue = new GetCitiesResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Cities";
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
                City city = new City();
                city.id = reader.GetInt16(0);
                city.name = reader.GetString(1);
                city.region = reader.GetString(2);
                returnValue.CityData.Add(city);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class Partner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public string description { get; set; }
        public string condition { get; set; }
        public string tagline { get; set; }
        public string internetshop { get; set; }
        public bool isCardIssue { get; set; }
        public bool isInCity { get; set; }
        public bool isInInternet { get; set; }
        public List<int> categoryId { get; set; }

        public bool isFav { get; set; }
        public string share_url { get; set; }

        public string biglogo { get; set; }

        public Partner() { }
        public Partner(int Id, string Name, string Logo, string Description, string Condition, string Tagline, string InternetShop, bool IsCardIssue, bool IsInCity, bool IsInInternet, string BigLogo)
        {
            id = Id;
            name = Name;
            logo = Logo;
            description = Description;
            condition = Condition;
            tagline = Tagline;
            internetshop = InternetShop;
            isCardIssue = IsCardIssue;
            isInCity = IsInCity;
            isInInternet = IsInInternet;
            categoryId = new List<int>();
            biglogo = BigLogo;
        }
    }

    public class GetPartnersRequest
    {
        public bool IsMainPage { get; set; }
        public int CityID { get; set; }
        public int SegmentID { get; set; }
        public int CategoryID { get; set; }
        public bool IsCardIssue { get; set; }
        public bool IsInCity { get; set; }
        public bool IsInInternet { get; set; }

        public Int16 Operator { get; set; }
    }

    public class GetPartnersResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Partner> PartnerData { get; set; }
        public GetPartnersResponse()
        {
            PartnerData = new List<Partner>();
        }
    }

    public class ServerGetPartnersResponse
    {
        public GetPartnersResponse ProcessRequest(SqlConnection cnn, GetPartnersRequest request)
        {
            GetPartnersResponse returnValue = new GetPartnersResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Partners";
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@mainpage", request.IsMainPage);
            cmd.Parameters.AddWithValue("@city", request.CityID);
            cmd.Parameters.AddWithValue("@segment", request.SegmentID);
            cmd.Parameters.AddWithValue("@category", request.CategoryID);

            cmd.Parameters.AddWithValue("@isCardIssue", request.IsCardIssue);
            cmd.Parameters.AddWithValue("@isInCity", request.IsInCity);
            cmd.Parameters.AddWithValue("@isInInternet", request.IsInInternet);

            cmd.Parameters.AddWithValue("@operator", request.Operator);

            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Partner partner = new Partner();
                partner.id = reader.GetInt16(0);
                partner.name = reader.GetString(1);
                if (reader.IsDBNull(2)) partner.logo = null; else partner.logo = reader.GetString(2);
                if (reader.IsDBNull(3)) partner.condition = null; else partner.condition = reader.GetString(3);
                if (reader.IsDBNull(4)) partner.tagline = null; else partner.tagline = reader.GetString(4);
                if (reader.IsDBNull(5)) partner.internetshop = null; else partner.internetshop = reader.GetString(5);
                if (reader.IsDBNull(6)) partner.isCardIssue = false; else partner.isCardIssue = reader.GetBoolean(6);
                if (reader.IsDBNull(7)) partner.isInCity = false; else partner.isInCity = reader.GetBoolean(7);
                if (reader.IsDBNull(8)) partner.isInInternet = false; else partner.isInInternet = reader.GetBoolean(8);
                partner.isFav = false;
                partner.share_url = "";
                returnValue.PartnerData.Add(partner);
            }
            reader.Close();
            foreach (var p in returnValue.PartnerData)
            {
                p.categoryId = new List<int>();
                SqlCommand cmdCategories = cnn.CreateCommand();
                cmdCategories.CommandType = CommandType.StoredProcedure;
                cmdCategories.CommandText = "PartnerCategoryGet";
                cmdCategories.Parameters.AddWithValue("@partner", p.id);

                cmdCategories.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
                cmdCategories.Parameters["@errormessage"].Direction = ParameterDirection.Output;
                cmdCategories.Parameters.Add("@result", SqlDbType.Int);
                cmdCategories.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

                System.Data.SqlClient.SqlDataReader readerCategories = cmdCategories.ExecuteReader();
                while (readerCategories.Read())
                {
                    p.categoryId.Add(readerCategories.GetInt16(0));
                }
                readerCategories.Close();
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class GetPartnerRequest
    {
        public int PartnerID { get; set; }
    }

    public class GetPartnerResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Partner PartnerData { get; set; }
        public GetPartnerResponse()
        {
            PartnerData = new Partner();
        }
    }

    public class ServerGetPartnerResponse
    {
        public GetPartnerResponse ProcessRequest(SqlConnection cnn, GetPartnerRequest request)
        {
            GetPartnerResponse returnValue = new GetPartnerResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PartnerGet";
            cmd.Parameters.AddWithValue("@id", request.PartnerID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.PartnerData = null;
            if (returnValue.ErrorCode == 0)
            {
                System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Partner partner = new Partner();
                    partner.name = reader.GetString(0);
                    if (!reader.IsDBNull(1)) partner.logo = reader.GetString(1);
                    if (!reader.IsDBNull(2)) partner.description = reader.GetString(2);
                    if (!reader.IsDBNull(3)) partner.internetshop = reader.GetString(3);
                    if (!reader.IsDBNull(4)) partner.tagline = reader.GetString(4);
                    if (!reader.IsDBNull(5)) partner.condition = reader.GetString(5);
                    if (!reader.IsDBNull(6)) partner.id = reader.GetInt16(6);
                    if (!reader.IsDBNull(7)) partner.biglogo = reader.GetString(7);
                    partner.isFav = false;
                    partner.share_url = "";
                    returnValue.PartnerData = partner;
                }
                reader.Close();
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    //public class Campaign
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string logo { get; set; }
    //    public bool large { get; set; }
    //    public string partnerlogo { get; set; }
    //    public string description { get; set; }
    //    public string condition { get; set; }
    //    public string tagline { get; set; }

    //    public bool isPopular { get; set; }
    //    public bool isNew { get; set; }
    //    public List<int> categoryId { get; set; }
    //    public int partnerId { get; set; }
    //    public string internetShop { get; set; }

    //    public bool isFav { get; set; }
    //    public string share_url { get; set; }

    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }

    //    public Campaign() { }
    //    public Campaign(int Id, string Name, string Logo, bool Large, string PartnerLogo, string Description, string Condition, string Tagline, bool IsPopular, bool IsNew, int PartnerId, string InternetShop)
    //    {
    //        id = Id;
    //        name = Name;
    //        logo = Logo;
    //        large = Large;
    //        partnerlogo = PartnerLogo;
    //        description = Description;
    //        condition = Condition;
    //        tagline = Tagline;
    //        isPopular = IsPopular;
    //        isNew = IsNew;
    //        partnerId = PartnerId;
    //        internetShop = InternetShop;
    //    }
    //}

    //public class GetCampaignsRequest
    //{
    //    public bool IsMainPage { get; set; }
    //    public bool IsSideBar { get; set; }
    //    public int PartnerID { get; set; }
    //    public bool IsPopular { get; set; }
    //    public bool IsNew { get; set; }
    //    public int CityID { get; set; }
    //    public int SegmentID { get; set; }

    //    public int CategoryID { get; set; }

    //    public Int16 Operator { get; set; }
    //}

    //public class GetCampaignsResponse
    //{
    //    public int ErrorCode { get; set; }
    //    public string Message { get; set; }
    //    public List<Campaign> CampaignData { get; set; }
    //    public GetCampaignsResponse()
    //    {
    //        CampaignData = new List<Campaign>();
    //    }
    //}

    //public class ServerGetCampaignsResponse
    //{
    //    public GetCampaignsResponse ProcessRequest(SqlConnection cnn, GetCampaignsRequest request)
    //    {
    //        GetCampaignsResponse returnValue = new GetCampaignsResponse();
    //        cnn.Open();
    //        SqlCommand cmd = cnn.CreateCommand();
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandText = "Campaigns";
    //        if (request.Operator == 0)
    //        {
    //            request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
    //        }
    //        cmd.Parameters.AddWithValue("@mainpage", request.IsMainPage);
    //        cmd.Parameters.AddWithValue("@sidebar", request.IsSideBar);
    //        cmd.Parameters.AddWithValue("@partner", request.PartnerID);
    //        cmd.Parameters.AddWithValue("@recommended", request.IsPopular);
    //        cmd.Parameters.AddWithValue("@new", request.IsNew);
    //        cmd.Parameters.AddWithValue("@city", request.CityID);
    //        cmd.Parameters.AddWithValue("@segment", request.SegmentID);
    //        cmd.Parameters.AddWithValue("@category", request.CategoryID);
    //        cmd.Parameters.AddWithValue("@operator", request.Operator);
    //        cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
    //        cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
    //        cmd.Parameters.Add("@result", SqlDbType.Int);
    //        cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
    //        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            Campaign campaign = new Campaign();
    //            campaign.id = reader.GetInt16(0);
    //            if (reader.IsDBNull(1)) campaign.logo = null; else campaign.logo = reader.GetString(1);
    //            if (reader.IsDBNull(2)) campaign.large = false; else campaign.large = reader.GetBoolean(2);
    //            if (reader.IsDBNull(3)) campaign.partnerlogo = null; else campaign.partnerlogo = reader.GetString(3);
    //            if (reader.IsDBNull(4)) campaign.condition = null; else campaign.condition = reader.GetString(4);
    //            if (reader.IsDBNull(5)) campaign.tagline = null; else campaign.tagline = reader.GetString(5);
    //            if (reader.IsDBNull(6)) campaign.isNew = false; else campaign.isNew = reader.GetBoolean(6);
    //            if (reader.IsDBNull(7)) campaign.isPopular = false; else campaign.isPopular = reader.GetBoolean(7);
    //            if (!reader.IsDBNull(8)) campaign.partnerId = reader.GetInt16(8);

    //            campaign.isFav = false;
    //            campaign.share_url = "";
    //            returnValue.CampaignData.Add(campaign);
    //        }
    //        reader.Close();
    //        foreach (var c in returnValue.CampaignData)
    //        {
    //            c.categoryId = new List<int>();
    //            SqlCommand cmdCategory = cnn.CreateCommand();
    //            cmdCategory.CommandType = CommandType.StoredProcedure;
    //            cmdCategory.CommandText = "PartnerCategoryGet";
    //            cmdCategory.Parameters.AddWithValue("@partner", c.partnerId);
    //            cmdCategory.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
    //            cmdCategory.Parameters["@errormessage"].Direction = ParameterDirection.Output;
    //            cmdCategory.Parameters.Add("@result", SqlDbType.Int);
    //            cmdCategory.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
    //            SqlDataReader readerCategory = cmdCategory.ExecuteReader();
    //            while (readerCategory.Read())
    //            {
    //                if (!readerCategory.IsDBNull(0)) c.categoryId.Add(readerCategory.GetInt16(0));
    //            }
    //            readerCategory.Close();
    //        }
    //        returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
    //        returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
    //        cnn.Close();
    //        return returnValue;
    //    }
    //}

    public class GetCampaignRequest
    {
        public int CampaignID { get; set; }
    }

    public class GetCampaignResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Campaign CampaignData { get; set; }
        public GetCampaignResponse()
        {
            CampaignData = new Campaign();
        }
    }

    public class ServerGetCampaignResponse
    {
        public GetCampaignResponse ProcessRequest(SqlConnection cnn, GetCampaignRequest request)
        {
            GetCampaignResponse returnValue = new GetCampaignResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CampaignGet";
            cmd.Parameters.AddWithValue("@id", request.CampaignID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            returnValue.CampaignData = null;
            if (returnValue.ErrorCode == 0)
            {
                System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Campaign campaign = new Campaign();
                    campaign.name = reader.GetString(0);
                    if (!reader.IsDBNull(1)) campaign.logo = reader.GetString(1);
                    if (!reader.IsDBNull(2)) campaign.partnerlogo = reader.GetString(2);
                    if (!reader.IsDBNull(3)) campaign.description = reader.GetString(3);
                    if (!reader.IsDBNull(4)) campaign.condition = reader.GetString(4);
                    if (!reader.IsDBNull(5)) campaign.tagline = reader.GetString(5);
                    if (!reader.IsDBNull(6)) campaign.id = reader.GetInt16(6);
                    if (!reader.IsDBNull(7)) campaign.partnerId = reader.GetInt16(7);
                    if (!reader.IsDBNull(8)) campaign.internetShop = reader.GetString(8);
                    if (!reader.IsDBNull(9)) campaign.startDate = reader.GetDateTime(9);
                    if (!reader.IsDBNull(10)) campaign.endDate = reader.GetDateTime(10);
                    campaign.isFav = false;
                    campaign.share_url = "";
                    returnValue.CampaignData = campaign;
                }
                reader.Close();
            }
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    //public class Pos
    //{
    //    public int id { get; set; }
    //    public string partner { get; set; }
    //    public string code { get; set; }
    //    public string region { get; set; }
    //    public string city { get; set; }
    //    public string address { get; set; }
    //    public string phone { get; set; }
    //    public string mapposition { get; set; }
    //    public bool isCardIssue { get; set; }
    //    public Pos() { }
    //    public Pos(int Id, string Partner, string Code, string Region, string City, string Address, string Phone, string MapPosition, bool IsCardIssue)
    //    {
    //        id = Id;
    //        partner = Partner;
    //        code = Code;
    //        region = Region;
    //        city = City;
    //        address = Address;
    //        phone = Phone;
    //        mapposition = MapPosition;
    //        isCardIssue = IsCardIssue;
    //    }
    //}

    //public class GetPosesRequest
    //{
    //    public int PartnerID { get; set; }
    //    public Int16 Operator { get; set; }
    //}

    //public class GetPosesResponse
    //{
    //    public int ErrorCode { get; set; }
    //    public string Message { get; set; }
    //    public List<Pos> PosData { get; set; }
    //    public GetPosesResponse()
    //    {
    //        PosData = new List<Pos>();
    //    }
    //}

    //public class ServerGetPosesResponse
    //{
    //    public GetPosesResponse ProcessRequest(SqlConnection cnn, GetPosesRequest request)
    //    {
    //        GetPosesResponse returnValue = new GetPosesResponse();
    //        cnn.Open();
    //        SqlCommand cmd = cnn.CreateCommand();
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandText = "Poses";
    //        cmd.Parameters.AddWithValue("@partner", request.PartnerID);
    //        if (request.Operator == 0)
    //        {
    //            request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
    //        }
    //        cmd.Parameters.AddWithValue("@operator", request.Operator);

    //        cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
    //        cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
    //        cmd.Parameters.Add("@result", SqlDbType.Int);
    //        cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
    //        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            Pos pos = new Pos();
    //            pos.id = reader.GetInt16(0);
    //            pos.partner = reader.GetString(1);
    //            pos.code = reader.GetString(2);
    //            if (reader.IsDBNull(3)) pos.region = null; else pos.region = reader.GetString(3);
    //            if (reader.IsDBNull(4)) pos.city = null; else pos.city = reader.GetString(4);
    //            if (reader.IsDBNull(5)) pos.address = null; else pos.address = reader.GetString(5);
    //            if (reader.IsDBNull(6)) pos.phone = null; else pos.phone = reader.GetString(6);
    //            if (reader.IsDBNull(7)) pos.mapposition = null; else pos.mapposition = reader.GetString(7);
    //            if (reader.IsDBNull(8)) pos.isCardIssue = false; else pos.isCardIssue = reader.GetBoolean(8);
    //            returnValue.PosData.Add(pos);
    //        }
    //        reader.Close();
    //        returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
    //        returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
    //        cnn.Close();
    //        return returnValue;
    //    }
    //}

    public class PartnerInfo
    {
        public string name { get; set; }
        public string description { get; set; }
        public PartnerInfo() { }
        public PartnerInfo(string Name, string Description)
        {
            name = Name;
            description = Description;
        }
    }

    public class GetPartnerInfoRequest
    {
        public int PartnerID { get; set; }
    }

    public class GetPartnerInfoResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<PartnerInfo> PartnerInfoData { get; set; }
        public GetPartnerInfoResponse()
        {
            PartnerInfoData = new List<PartnerInfo>();
        }
    }

    public class ServerGetPartnerInfoResponse
    {
        public GetPartnerInfoResponse ProcessRequest(SqlConnection cnn, GetPartnerInfoRequest request)
        {
            GetPartnerInfoResponse returnValue = new GetPartnerInfoResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PartnerInfoGet";
            cmd.Parameters.AddWithValue("@partner", request.PartnerID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PartnerInfo partnerinfo = new PartnerInfo();
                partnerinfo.name = reader.GetString(0);
                if (reader.IsDBNull(1)) partnerinfo.description = null; else partnerinfo.description = reader.GetString(1);
                returnValue.PartnerInfoData.Add(partnerinfo);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class CampaignInfo
    {
        public string name { get; set; }
        public string description { get; set; }
        public CampaignInfo() { }
        public CampaignInfo(string Name, string Description)
        {
            name = Name;
            description = Description;
        }
    }

    public class GetCampaignInfoRequest
    {
        public int CampaignID { get; set; }
    }

    public class GetCampaignInfoResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<CampaignInfo> CampaignInfoData { get; set; }
        public GetCampaignInfoResponse()
        {
            CampaignInfoData = new List<CampaignInfo>();
        }
    }

    public class ServerGetCampaignInfoResponse
    {
        public GetCampaignInfoResponse ProcessRequest(SqlConnection cnn, GetCampaignInfoRequest request)
        {
            GetCampaignInfoResponse returnValue = new GetCampaignInfoResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CampaignInfoGet";
            cmd.Parameters.AddWithValue("@campaign", request.CampaignID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CampaignInfo campaigninfo = new CampaignInfo();
                campaigninfo.name = reader.GetString(0);
                if (reader.IsDBNull(1)) campaigninfo.description = null; else campaigninfo.description = reader.GetString(1);
                returnValue.CampaignInfoData.Add(campaigninfo);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    //public class Client
    //{
    //    public int id { get; set; }
    //    public string firstname { get; set; }
    //    public string middlename { get; set; }
    //    public string lastname { get; set; }
    //    public int gender { get; set; }
    //    public DateTime birthdate { get; set; }
    //    public bool haschildren { get; set; }
    //    public string description { get; set; }
    //    public Int64 phone { get; set; }
    //    public string email { get; set; }
    //    public bool allowsms { get; set; }
    //    public bool allowemail { get; set; }
    //    public decimal balance { get; set; }
    //    public Client() { }
    //    public Client(int Id, string FirstName, string MiddleName, string LastName, int Gender, DateTime Birthdate, bool HasChildren, string Description, int Phone, string Email, bool AllowSMS, bool AllowEmail, decimal Balance)
    //    {
    //        id = Id;
    //        firstname = FirstName;
    //        middlename = MiddleName;
    //        lastname = LastName;
    //        gender = Gender;
    //        birthdate = Birthdate;
    //        haschildren = HasChildren;
    //        description = Description;
    //        phone = Phone;
    //        email = Email;
    //        allowsms = AllowSMS;
    //        allowemail = AllowEmail;
    //        balance = Balance;
    //    }
    //}

    //public class GetClientRequest
    //{
    //    public int ClientID { get; set; }
    //}

    //public class GetClientResponse
    //{
    //    public int ErrorCode { get; set; }
    //    public string Message { get; set; }
    //    public Client ClientData { get; set; }
    //    public GetClientResponse()
    //    {
    //        ClientData = new Client();
    //    }
    //}

    //public class ServerGetClientResponse
    //{
    //    public GetClientResponse ProcessRequest(SqlConnection cnn, GetClientRequest request)
    //    {
    //        GetClientResponse returnValue = new GetClientResponse();
    //        cnn.Open();
    //        SqlCommand cmd = cnn.CreateCommand();
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.CommandText = "ClientGet";
    //        cmd.Parameters.AddWithValue("@id", request.ClientID);
    //        cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
    //        cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
    //        cmd.Parameters.Add("@result", SqlDbType.Int);
    //        cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
    //        returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
    //        returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
    //        returnValue.ClientData = null;
    //        if (returnValue.ErrorCode == 0)
    //        {
    //            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                Client client = new Client();
    //                if (!reader.IsDBNull(0)) client.firstname = reader.GetString(0);
    //                if (!reader.IsDBNull(1)) client.middlename = reader.GetString(1);
    //                if (!reader.IsDBNull(2)) client.lastname = reader.GetString(2);
    //                if (!reader.IsDBNull(3)) if (reader.GetBoolean(3) == false) client.gender = -1; else client.gender = 1;
    //                if (!reader.IsDBNull(4)) client.birthdate = reader.GetDateTime(4);
    //                if (!reader.IsDBNull(5)) client.haschildren = reader.GetBoolean(5);
    //                if (!reader.IsDBNull(6)) client.description = reader.GetString(6);
    //                if (!reader.IsDBNull(7)) client.phone = reader.GetInt64(7);
    //                if (!reader.IsDBNull(8)) client.email = reader.GetString(8);
    //                if (!reader.IsDBNull(9)) client.allowsms = reader.GetBoolean(9);
    //                if (!reader.IsDBNull(10)) client.allowemail = reader.GetBoolean(10);
    //                if (!reader.IsDBNull(11)) client.balance = reader.GetDecimal(11);
    //                returnValue.ClientData = client;
    //            }
    //            reader.Close();
    //        }
    //        cnn.Close();
    //        return returnValue;
    //    }
    //}

    public class LeaveMessageRequest
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public Int16 Operator { get; set; }
    }

    public class LeaveMessageResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ServerLeaveMessageResponse
    {
        public LeaveMessageResponse ProcessRequest(SqlConnection cnn, LeaveMessageRequest request)
        {
            LeaveMessageResponse returnValue = new LeaveMessageResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "MessageAdd";
            cmd.Parameters.AddWithValue("@text", request.Text);

            cmd.Parameters.AddWithValue("@subject", request.Subject);
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


    public class GetFaq
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class GetFaqRequest
    {
        public Int16 Operator { get; set; }
    }

    public class GetFaqResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<GetFaq> FaqData { get; set; }
        public GetFaqResponse()
        {
            FaqData = new List<GetFaq>();
        }
    }

    public class ServerFaqResponse
    {
        public GetFaqResponse ProcessRequest(SqlConnection cnn, GetFaqRequest request)
        {
            GetFaqResponse returnValue = new GetFaqResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "FaqGet";
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
                GetFaq faq = new GetFaq();
                if (!reader.IsDBNull(0)) faq.Question = reader.GetString(0);
                if (!reader.IsDBNull(1)) faq.Answer = reader.GetString(1);
                returnValue.FaqData.Add(faq);
            }
            reader.Close();
            returnValue.ErrorCode = Convert.ToInt32(cmd.Parameters["@result"].Value);
            returnValue.Message = Convert.ToString(cmd.Parameters["@errormessage"].Value);
            cnn.Close();
            return returnValue;
        }
    }

    public class CampaignDetail
    {
        public int id { get; set; }
        public string campaign { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string mapposition { get; set; }
        public bool isCardIssue { get; set; }
        public CampaignDetail()
        { }
        public CampaignDetail(int ID, string Campaign, string Region, string City, string Address, string Mapposition, bool IsCardIssue, string Phone)
        {
            id = ID;
            campaign = Campaign;
            region = Region;
            city = City;
            address = Address;
            phone = Phone;
            mapposition = Mapposition;
            isCardIssue = IsCardIssue;
        }
    }

    public class CampaignDetailRequest
    {
        public int CampaignID { get; set; }
    }

    public class CampaignDetailResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<CampaignDetail> CampaignDetailData { get; set; }
        public CampaignDetailResponse()
        {
            CampaignDetailData = new List<CampaignDetail>();
        }
    }

    public class ServerCampaignDetailResponse
    {
        public CampaignDetailResponse ProcessRequest(SqlConnection cnn, CampaignDetailRequest request)
        {
            CampaignDetailResponse returnValue = new CampaignDetailResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Poses";
            cmd.Parameters.AddWithValue("@Campaign", request.CampaignID);
            cmd.Parameters.Add("@errormessage", SqlDbType.NVarChar, 100);
            cmd.Parameters["@errormessage"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@result", SqlDbType.Int);
            cmd.Parameters["@result"].Direction = ParameterDirection.ReturnValue;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CampaignDetail campaign = new CampaignDetail();
                campaign.id = reader.GetInt16(0);
                campaign.campaign = reader.GetString(1);
                if (reader.IsDBNull(2)) campaign.city = null; else campaign.city = reader.GetString(2);
                if (reader.IsDBNull(3)) campaign.region = null; else campaign.region = reader.GetString(3);
                if (reader.IsDBNull(4)) campaign.address = null; else campaign.address = reader.GetString(4);
                if (reader.IsDBNull(5)) campaign.phone = null; else campaign.phone = reader.GetString(5);
                if (reader.IsDBNull(6)) campaign.mapposition = null; else campaign.mapposition = reader.GetString(6);
                if (reader.IsDBNull(7)) campaign.isCardIssue = false; else campaign.isCardIssue = reader.GetBoolean(7);
                returnValue.CampaignDetailData.Add(campaign);
            }
            reader.Close();
            cnn.Close();
            return returnValue;
        }
    }

    public class BecomePartnerRequest
    {
        public string City { get; set; }
        public string Site { get; set; }
        public string GoodsSell { get; set; }
        public int? PosQty { get; set; }
        public string CashSoftware { get; set; }
        public string Name { get; set; }
        public Int64 Phone { get; set; }
        public string Email { get; set; }
        public Int16 Operator { get; set; }
    }

    public class BecomePartnerResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class ServerBecomePartner
    {
        public BecomePartnerResponse ProcessRequest(SqlConnection cnn, BecomePartnerRequest request)
        {
            BecomePartnerResponse returnValue = new BecomePartnerResponse();
            cnn.Open();
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "MessageAdd";
            string text = "Город: " + request.City ?? "Не указан";
            text += "\r\nСайт: " + request.Site ?? "Не указан";
            text += "\r\nЧто продают: " + request.GoodsSell ?? "Не указан";
            text += "\r\nКоличество ТТ: " + request.PosQty ?? "Не указан";
            text += "\r\nНаименование кассового ПО: " + request.CashSoftware ?? "Не указан";
            text += "\r\nИмя: " + request.Name ?? "Не указано";
            text += "\r\nНомер телефона: " + request.Phone.ToString();
            text += "\r\nАдрес электронной почты: " + request.Email ?? "Не указан";
            string subject = "Стать партнёром";
            if (request.Operator == 0)
            {
                request.Operator = Convert.ToInt16(ConfigurationManager.AppSettings["Operator"]);
            }
            cmd.Parameters.AddWithValue("@operator", request.Operator);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@subject", subject);
            cmd.Parameters.AddWithValue("@email", request.Email);
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
}