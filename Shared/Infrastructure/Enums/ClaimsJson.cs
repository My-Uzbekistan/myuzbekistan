using Newtonsoft.Json;
using System.Security.Claims;

namespace myuzbekistan.Shared
{
    public class ClaimsJson
    {
        [JsonProperty(ClaimTypes.Surname)]
        public string? Surname { get; set; }
        [JsonProperty(ClaimTypes.GivenName)]
        public string? GivenName { get; set; }
        [JsonProperty(ClaimTypes.NameIdentifier)]
        public string? NameIdentifier { get; set; }
        [JsonProperty(ClaimTypes.Email)]
        public string? Email { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("email_verified")]
        public string? EmailVerified { get; set; }
        [JsonProperty(ClaimTypes.Role)]
        public string? Role { get; set; }
        [JsonProperty("preferred_username")]
        public string? Username { get; set; }
        [JsonProperty("auth_time")]
        public string? AuthTime { get; set; }
    }
}
