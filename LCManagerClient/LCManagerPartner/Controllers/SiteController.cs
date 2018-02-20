using LCManagerPartner.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCManagerPartner.Controllers
{
    [RoutePrefix("api/site")]
    public class SiteController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //Дублирующиеся методы: GetCampaigns, GetPoses, GetClient

        [HttpPost]
        public GetSegmentsResponse GetSegments(GetSegmentsRequest request)
        {
            var result = new ServerGetSegmentsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCategoriesResponse GetCategories(GetCategoriesRequest request)
        {
            var result = new ServerGetCategoriesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCitiesResponse GetCities(GetCitiesRequest request)
        {
            var result = new ServerGetCitiesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPartnersResponse GetPartners(GetPartnersRequest request)
        {
            var result = new ServerGetPartnersResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPartnerResponse GetPartner(GetPartnerRequest request)
        {
            var result = new ServerGetPartnerResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCampaignsResponse GetCampaigns(GetCampaignsRequest request)
        {
            var result = new ServerGetCampaignsResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCampaignResponse GetCampaign(GetCampaignRequest request)
        {
            var result = new ServerGetCampaignResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPosesResponse GetPoses(GetPosesRequest request)
        {
            var result = new ServerGetPosesResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetPartnerInfoResponse GetPartnerInfo(GetPartnerInfoRequest request)
        {
            var result = new ServerGetPartnerInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetCampaignInfoResponse GetCampaignInfo(GetCampaignInfoRequest request)
        {
            var result = new ServerGetCampaignInfoResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetClientResponse GetClient(GetClientRequest request)
        {
            var result = new ServerGetClientResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public LeaveMessageResponse LeaveMessage(LeaveMessageRequest request)
        {
            var result = new ServerLeaveMessageResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public GetFaqResponse GetFaq(GetFaqRequest request)
        {
            var result = new ServerFaqResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public CampaignDetailResponse GetCampaignDetail(CampaignDetailRequest request)
        {
            var result = new ServerCampaignDetailResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        public BecomePartnerResponse BecomePartner(BecomePartnerRequest request)
        {
            var result = new ServerBecomePartner();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
