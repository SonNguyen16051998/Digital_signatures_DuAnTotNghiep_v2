/*using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SignhashSample
{
    class SADValidator
    {
    }



    public class SadValidator
    {
        public static string GenerateJwtToken(Dictionary<string, object> claims, string p12Path, string p12Pass)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var privateKey = new X509Certificate2(p12Path, p12Pass).GetRSAPrivateKey();
            var signKey = new RsaSecurityKey(privateKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", "1"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signKey, SecurityAlgorithms.RsaSha256),
                Claims = claims
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateJwtToken<T>(T payload, string p12Path, string p12Pass)
        {
            var privateKey = new X509Certificate2(p12Path, p12Pass).GetRSAPrivateKey();
            var signKey = new RsaSecurityKey(privateKey);
            var credentials = new SigningCredentials(signKey, SecurityAlgorithms.RsaSha256);
            var header = new JwtHeader(credentials);

            var json2 = System.Text.Json.JsonSerializer.Serialize(payload, payload.GetType());
            var x = JwtPayload.Deserialize(json2);

            var secToken = new JwtSecurityToken(header, x);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            return handler.WriteToken(secToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="pubKey"></param>
        /// <returns></returns>
        public static (bool IsValid, string ErrMsg, JwtSecurityToken Token) VerifyJwt(string jwt, string issuer, string audience, string pubKey)
        {
            byte[] publicKeyBytes;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                var prefix = "-----BEGIN PUBLIC KEY-----";
                var suffix = "-----END PUBLIC KEY-----";
                if (pubKey.StartsWith(prefix))
                {
                    pubKey = pubKey.Replace(prefix, "");
                }
                if (pubKey.EndsWith(suffix))
                {
                    pubKey = pubKey.Replace(suffix, "");
                }
                pubKey = pubKey.Replace("\r", "").Replace("\n", "");
                publicKeyBytes = Convert.FromBase64String(pubKey);
                AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(publicKeyBytes);
                RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
                RSAParameters rsaParameters = new RSAParameters()
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                };
                rsa.ImportParameters(rsaParameters);
            }
            catch (Exception ex)
            {
                return (false, $"PublicKey invalid format {ex.Message}", null);
            }

            RsaSecurityKey key = new RsaSecurityKey(rsa);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidAudience = audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken token);

                return (true, $"Success", (JwtSecurityToken)token);
            }
            catch (Exception ex)
            {
                return (false, $"Validate token failed {ex.Message}", null);
            }
        }

        public static T VerifySAD<T>(string sad, string issuer, string kakPub, String audience, out string errMesg)
        {
            errMesg = "";
            byte[] publicKeyBytes = Convert.FromBase64String(kakPub);
            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(publicKeyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            RSAParameters rsaParameters = new RSAParameters()
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);

            RsaSecurityKey key = new RsaSecurityKey(rsa);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(sad, tokenValidationParameters, out SecurityToken token);
                if (token is null)
                {
                    errMesg = "SAD data invalid";
                    return default;
                }

                var payLoad = ((JwtSecurityToken)token).Payload["seElnSadext"].ToString();
                var y = System.Text.Json.JsonSerializer.Serialize(payLoad);
                return System.Text.Json.JsonSerializer.Deserialize<T>(payLoad);
                //var x1 = payLoad.ToString();
                //var x = JsonConvert.SerializeObject(payLoad);
                //return JsonConvert.DeserializeObject<T>(x);
            }
            catch (Exception ex)
            {
                errMesg = ex.Message;
                return default;
            }
        }

        public class KeyAuthData
        {
            [JsonPropertyName("jti")]
            public string JTI { get; set; }
            [JsonPropertyName("sub")]
            public string IdentityId { get; set; }
            [JsonPropertyName("aud")]
            public string Audience { get; set; }
            [JsonPropertyName("iss")]
            public string Issuer { get; set; }
            [JsonPropertyName("iat")]
            public string IssuerAt { get; set; }
            [JsonPropertyName("exp")]
            public string ExpireAt { get; set; }
            [JsonPropertyName("irt")]
            public string TranId { get; set; }
            [JsonPropertyName("attr")]
            public string Attribute { get; set; }
            [JsonPropertyName("loa")]
            public string LevelOfAssurance { get; set; }
            [JsonPropertyName("requestId")]
            public int SignRequestId { get; set; }
            [JsonPropertyName("docs")]
            public string Docs { get; set; }
            [JsonPropertyName("keyAuthData")]
            public string KeyAuthValue { get; set; }
            [JsonPropertyName("deviceId")]
            public string DeviceId { get; set; }
        }

        public class SAD
        {
            [JsonPropertyName("jti")]
            public string JTI { get; set; }
            [JsonPropertyName("sub")]
            public string IdentityId { get; set; }
            [JsonPropertyName("aud")]
            public string Audience { get; set; }
            [JsonPropertyName("iss")]
            public string Issuer { get; set; }
            [JsonPropertyName("iat")]
            public long IssuerAt { get; set; }
            [JsonPropertyName("exp")]
            public long ExpireAt { get; set; }
            [JsonPropertyName("seElnSadext")]
            public Exp Exp { get; set; }

        }

        public class Exp
        {
            [JsonPropertyName("irt")]
            public string Irt { get; set; }
            [JsonPropertyName("attr")]
            public string Attr { get; set; } = "urn:oid:1.2.752.29.4.13";
            [JsonPropertyName("loa")]
            public string Loa { get; set; } = "http://vnpt.vn/loa/1.0/loa3-sigmessage";
            [JsonPropertyName("requestId")]
            public int RequestId { get; set; }
            [JsonPropertyName("deviceId")]
            public string DeviceId { get; set; }
            [JsonPropertyName("keyAuthData")]
            public string KeyAuthData { get; set; }
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
            [JsonPropertyName("docs")]
            public string Docs { get; set; }
        }

        public class InitSAD
        {
            [JsonPropertyName("jti")]
            public string JTI { get; set; }
            [JsonPropertyName("sub")]
            public string IdentityId { get; set; }
            [JsonPropertyName("aud")]
            public string Audience { get; set; }
            [JsonPropertyName("iss")]
            public string Issuer { get; set; } = "VNPT SIC component";
            [JsonPropertyName("iat")]
            public long IssuerAt { get; set; }
            [JsonPropertyName("exp")]
            public long ExpireAt { get; set; }
            [JsonPropertyName("seElnSadext")]
            public KeyInitData Exp { get; set; }
        }

        public class KeyInitData
        {
            [JsonPropertyName("credentialId")]
            public string CredentialId { get; set; }
            [JsonPropertyName("keyAuthData")]
            public string KeyAuthValue { get; set; }
            [JsonPropertyName("kakPub")]
            public string KakPublicKey { get; set; }
            [JsonPropertyName("access_token")]
            public string IdentyToken { get; set; }
        }
    }
}
*/