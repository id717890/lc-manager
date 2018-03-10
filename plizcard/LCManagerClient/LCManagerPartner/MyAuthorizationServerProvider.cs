﻿using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Configuration;
using System.Data.SqlClient;
using LCManagerPartner.Models;

namespace LCManagerPartner
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            //return base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //return base.GrantResourceOwnerCredentials(context);
            string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connectionString);

            ManagerLoginRequest request = new ManagerLoginRequest
            {
                Phone = Convert.ToInt64(context.UserName),
                Password = context.Password
            };
            var result = new ServerManagerLogin();
            var authentificationResult = result.ProcessRequest(cnn, request);
            if(authentificationResult.ErrorCode == 0)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, authentificationResult.RoleName));
                //identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim("user", context.UserName));
                if (authentificationResult.Operator > 0)
                {
                    identity.AddClaim(new Claim("oper", authentificationResult.Operator.ToString()));
                }
                if (authentificationResult.Partner > 0)
                {
                    identity.AddClaim(new Claim("partner", authentificationResult.Partner.ToString()));
                }
                if (authentificationResult.Pos > 0)
                {
                    identity.AddClaim(new Claim("pos", authentificationResult.Pos.ToString()));
                }
                if (!string.IsNullOrEmpty(authentificationResult.PosCode))
                {
                    identity.AddClaim(new Claim("poscode", authentificationResult.PosCode));
                }
                identity.AddClaim(new Claim("permissioncode",  authentificationResult.PermissionCode));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect", "401");
                //context.Response.StatusCode = 401;
                //context.Response.ReasonPhrase = "Provided username and password is incorrect";
                //return new System.Net.Http.HttpResponseMessage
                //{
                //    StatusCode = System.Net.HttpStatusCode.Forbidden,
                //    Content = new System.Net.Http.StringContent("You are unauthorized to access this resource")
                //}; ;
            }            
        }
    }
}