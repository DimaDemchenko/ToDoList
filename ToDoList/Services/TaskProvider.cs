﻿using System.Data;
using ToDoList.EnumData;
using ToDoList.Repository;

namespace ToDoList.Services
{
    public class TaskProvider
    {
        private readonly CookieService _cookieService;
        private readonly IDbConnection _connection;
        private readonly IConfiguration _config;

        public TaskProvider(CookieService cookieService, IDbConnection connection, IConfiguration config)
        {
            _cookieService = cookieService;
            _connection = connection;
            _config = config;
        }

        public ITaskRepository GetTaskRepository()
        { 
            var storageType = _cookieService.Get("Storage");

            if (storageType == StorageType.XML)
            {
                return new TasksXMLRepository(_config);
            }
            else
            { 
                return new TaskRepository(_connection);
            }
        }

        public ICategoryRepository GetCategoriesRepository() 
        {
            var storageType = _cookieService.Get("Storage");

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
