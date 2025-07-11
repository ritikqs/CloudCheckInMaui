using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCIMIGRATION.Models
{
    public class ForgotPasswordRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    public class ForgotPasswordResponse
    {

    }
    public class ResetPasswordRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }
    }
}
