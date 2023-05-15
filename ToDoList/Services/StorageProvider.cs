using System.Data;
using ToDoList.EnumData;
using ToDoList.Repository;

namespace ToDoList.Services
{
    public class StorageProvider
    {
        private readonly IDbConnection _connection;
        private readonly IConfiguration _config;

        public StorageProvider(IDbConnection connection, IConfiguration config)
        {
            _connection = connection;
            _config = config;
        }

        public ITaskRepository GetTaskRepository(StorageType storageType)
        { 

            if (storageType == StorageType.XML)
            {
                return new TasksXMLRepository(_config);
            }
            else
            { 
                return new TaskRepository(_connection);
            }
        }

        public ICategoryRepository GetCategoriesRepository(StorageType storageType) 
        {

            if (storageType == StorageType.XML)
            {
                return new CategoryXMLRepository(_config);
            }
            else
            {
                return new CategoryRepository(_connection);
            }

        }
    }
}
