using System.Data;
using ToDoList.EnumData;
using ToDoList.Repository;

namespace ToDoList.Services
{
    public class TaskProvider
    {
        private readonly CookieService _cookieService;
        private readonly IDbConnection _connection;

        public TaskProvider(CookieService cookieService, IDbConnection connection)
        {
            _cookieService = cookieService;
            _connection = connection;
        }

        public ITasksRepository GetTaskRepository()
        { 
            var storageType = _cookieService.Get("Storage");

            if (storageType == StorageType.XML)
            {
                return null;
            }
            else
            { 
                return new TasksRepository(_connection);
            }
        }
    }
}
