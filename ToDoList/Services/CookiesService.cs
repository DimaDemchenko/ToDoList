using ToDoList.EnumData;
using System;

namespace ToDoList.Services
{
    public class CookiesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookiesService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public StorageType Get(string key)
        {
            var value = _httpContextAccessor.HttpContext.Request.Cookies[key];

            if (value == null)
            {
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                value = StorageType.SQL.ToString();
                _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
            }
            Console.WriteLine(value);
            return (StorageType)Enum.Parse(typeof(StorageType), value);
        }

        public void Set(string key, string value)
        {
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }
    }
}
