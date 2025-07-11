using System.Text.Json.Serialization;

namespace CloudCheckInMaui.Models
{
    public class UserLoginType
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("loginType")]
        public int LoginType { get; set; }
    }

    public class RefreshTokenResponse
    {
        [JsonPropertyName("token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
} 