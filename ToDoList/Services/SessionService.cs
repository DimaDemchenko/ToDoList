using ToDoList.EnumData;
using System;

namespace ToDoList.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public StorageType Get(string key, string storageType = null)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);

            if (value == null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(key, StorageType.SQL.ToString());
                value = _httpContextAccessor.HttpContext.Session.GetString(key);
            }

            return (StorageType)Enum.Parse(typeof(StorageType), value);
        }
    

        public void Set(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
    }
}
