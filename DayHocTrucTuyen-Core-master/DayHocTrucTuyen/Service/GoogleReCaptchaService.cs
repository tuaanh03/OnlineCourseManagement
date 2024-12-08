namespace DayHocTrucTuyen.Service
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class GoogleReCaptchaService
    {
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public GoogleReCaptchaService(IConfiguration configuration, HttpClient httpClient)
        {
            _secretKey = configuration["GoogleReCaptcha:SecretKey"];
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyCaptchaAsync(string token)
        {
            var response = await _httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("response", token)
                })
            );

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReCaptchaResponse>(json);

            return result?.Success ?? false;
        }

        private class ReCaptchaResponse
        {
            public bool Success { get; set; }
            public string ChallengeTs { get; set; }
            public string Hostname { get; set; }
        }
    }
}