﻿using ToDoList.DBmodels;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public interface ITasksRepository
    {
        Task<IEnumerable<Tasks>> GetAllAsync();

        Task<int> CreateAsync(Tasks task);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<JoinedTasksAndCategories>> GetJoinedTasksAndCategoriesAsync();
    }
}
