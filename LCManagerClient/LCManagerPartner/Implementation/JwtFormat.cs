namespace LCManagerPartner.Implementation
{
    using System;
    using System.Configuration;
    using System.IdentityModel.Tokens;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataHandler.Encoder;
    using Thinktecture.IdentityModel.Tokens;

    public class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private static readonly byte[] Secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);
        private readonly string _issuer;

        public JwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var signingKey = new HmacSigningCredentials(Secret);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;
            //var expires = issued.Value.DateTime.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_token"]));
            //var expires2 = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_token"]));

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    _issuer,
                    ConfigurationManager.AppSettings["audience"],
                    data.Identity.Claims,
                    issued.Value.UtcDateTime,
                    expires.Value.UtcDateTime,
                    signingKey
                    ));
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}