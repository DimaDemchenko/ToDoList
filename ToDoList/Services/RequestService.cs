using ToDoList.EnumData;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace ToDoList.Services
{
    public class RequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public StorageType GetStorageFromHeader()
        {
            _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("StorageType", out var storageTypeHeader);

            if (string.IsNullOrEmpty(storageTypeHeader.FirstOrDefault()))
                throw new Exception();

            bool isValidStorage = Enum.TryParse(storageTypeHeader.FirstOrDefault(), out StorageType storage);
            
            if (!isValidStorage)
                throw new Exception(); 
            
            return storage;
        }

        public StorageType GetStorageFromCookie()
        {
            var value = _httpContextAccessor.HttpContext.Request.Cookies["Storage"];

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
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Storage", value, options);
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
