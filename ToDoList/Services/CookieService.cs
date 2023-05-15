using ToDoList.EnumData;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace ToDoList.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public StorageType GetStorageFromHeader()
        {
            _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("StorageType", out var storageTypeHeader);

            if (string.IsNullOrEmpty(storageTypeHeader.FirstOrDefault()))
                throw new InvalidOperationException();


            var storage=  (StorageType)Enum.Parse(typeof(StorageType), storageTypeHeader.FirstOrDefault());
            return storage;
        }

        public StorageType GetStorage(string key)
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

            return (StorageType)Enum.Parse(typeof(StorageType), value);
        }

        public void SetCookie(string key, string value)
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
