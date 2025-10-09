namespace LOMs.Api.Extensions
{
    public static class CookieExtensions
    {
        public static HttpContext SetInsideCookie(this HttpContext httpContext, string cookieName, string cookieValue)
        {
            var options = CreateCookieOptions(cookieName);

            httpContext.Response.Cookies.Append(cookieName, cookieValue, options);

            return httpContext;
        }

        private static CookieOptions CreateCookieOptions(string cookieName)
        {
            // Default Options
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // HTTPS only
                IsEssential = true,
                SameSite = SameSiteMode.None,
                // Customize by cookie name
                Expires = cookieName.ToLower() switch
                {
                    "access-token" => DateTime.UtcNow.AddSeconds(15),
                    "refresh-token" => DateTime.UtcNow.AddDays(30),
                    _ => DateTime.UtcNow.AddMinutes(10)
                }
            };

            return options;
        }
    }
}