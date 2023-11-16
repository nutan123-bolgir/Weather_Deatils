using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Weather_Deatils.Models
{
    public class Jwtcs
    {
        public string SecretKey { get; set; }
        public int TokenDuration { get; set; }
        private readonly IConfiguration config;


        public Jwtcs(IConfiguration _config)
        {
            config = _config;
            this.SecretKey = config.GetSection("JwtConfig").GetSection("Key").Value;
            this.TokenDuration = Int32.Parse(config.GetSection("JwtConfig").GetSection("Duration").Value);


        }
        public string GenerateToken( String UserName)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
            var signature = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("username",UserName),
                
            };
            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.Now.AddMinutes(TokenDuration),
                signingCredentials: signature


              );
            return jwtToken.EncodedPayload;

        }
    }
}

