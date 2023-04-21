using ToDoList.Enum;

namespace ToDoList.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get(string key, string defaultValue = null)
        {
            var value = _httpContextAccessor.HttpContext.Session.GetString(key);
            if (value == null)
            {
                _httpContextAccessor.HttpContext.Session.SetString(key, StorageType.SQL.ToString());
            }
            return value;
        }

        public void Set(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
    }
}
