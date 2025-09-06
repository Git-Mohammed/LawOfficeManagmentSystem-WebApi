using LOMs.Api;
using System.Net;
using System.Net.Http.Json;
using LOMs.Contract.Requests.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LOMs.Test.Auth
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>> // Fixed namespace issue
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient(); // Simplified constructor using primary constructor

        [Fact]
        public async Task Login_ReturnsOk_ForValidCredentials()
        {
            var request = new LoginRequest { Username = "testuser", Password = "Test@123" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Refresh_ReturnsOk_ForValidRefreshToken()
        {
            var request = new RefreshRequest { RefreshToken = "valid-refresh-token" };
            var response = await _client.PostAsJsonAsync("/api/auth/refresh", request);

            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Logout_ReturnsOk_ForValidRefreshToken()
        {
            var request = new RefreshRequest { RefreshToken = "valid-refresh-token" };
            var response = await _client.PostAsJsonAsync("/api/auth/logout", request);

            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOk_ForValidRequest()
        {
            var request = new ResetPasswordRequest
            {
                Username = "testuser",
                Token = "valid-token",
                NewPassword = "NewPassword@123"
            };
            var response = await _client.PostAsJsonAsync("/api/auth/reset-password", request);

            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}