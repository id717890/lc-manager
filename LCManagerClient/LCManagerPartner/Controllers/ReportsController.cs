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
    [Authorize]
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiController
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        SqlConnection cnn = new SqlConnection(connectionString);

        //public Reports() { cnn.Open(); }
        //~Reports() { cnn.Close(); }

        [HttpPost]
        [Route("ClientBuys")]
        public ClientBuysResponse ClientBuys(ClientBuysRequest request)
        {
            var result = new ServerClientBuyResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("Buys")]
        public BuysResponse Buys(BuysRequest request)
        {
            var result = new ServerBuyResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PosClientPeriod")]
        public PosClientPeriodResponse PosClientPeriod(PosClientPeriodRequest request)
        {
            var result = new ServerPosClientPeriodResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PosSalePeriod")]
        public PosClientPeriodResponse PosSalePeriod(PosClientPeriodRequest request)
        {
            var result = new ServerPosSalePeriodResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorClient")]
        public ReportOperatorClientResponse OperatorClient(ReportOperatorClientRequest request)
        {
            var result = new ReportServerOperatorClient();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorSales")]
        public OperatorSalesResponse OperatorSales(OperatorSalesRequest request)
        {
            var result = new ServerOperatorSales();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorBookkeeping")]
        public OperatorBookkeepingResponse OperatorBookkeeping(OperatorBookkeepingRequest request)
        {
            var result = new ServerOperatorBookkeeping();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PartnerClient")]
        public PartnerClientResponse PartnerClient(PartnerClientRequest request)
        {
            var result = new ServerPartnerClient();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PartnerSales")]
        public PosClientPeriodResponse PartnerSales(PartnerClientRequest request)
        {
            var result = new ServerPartnerSalePeriodResponse();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("PartnerBookkeeping")]
        public PartnerBookkeepingResponse PartnerBookkeeping(PartnerBookkeepingRequest request)
        {
            var result = new ServerPartnerBookkeeping();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }

        [HttpPost]
        [Route("OperatorBonus")]
        public OperatorBonusSourceResponse OperatorBonus(OperatorBonusSourceRequest request)
        {
            var result = new ServerOperatorBonusSource();
            var returnValue = result.ProcessRequest(cnn, request);
            return returnValue;
        }
    }
}
