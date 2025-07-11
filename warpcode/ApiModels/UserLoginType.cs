using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class UserLoginType
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("loginType")]
        public int LoginType { get; set; }
    }
    public class RefreshTokenResponse
    {
        [JsonProperty("token")]
        public string AccessToken { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
