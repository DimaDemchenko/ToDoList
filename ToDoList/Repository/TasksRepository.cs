using Dapper;
using System.Data;
using System.Data.Common;
using ToDoList.DBmodels;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly IDbConnection _connection;

        public TasksRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> CreateAsync(Tasks task)
        {
            try
            {
                string query = @"INSERT INTO Tasks (category_id, title, deadline, is_completed)
                         VALUES (@CategoryId, @Title, @Deadline, @IsCompleted);
                         SELECT SCOPE_IDENTITY()";
                return await _connection.ExecuteScalarAsync<int>(query, task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                string query = "DELETE FROM Tasks WHERE Id = @id";
                int rowsAffected = await _connection.ExecuteAsync(query, new { id });
                return rowsAffected > 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync()
        {
            try
            {
                string query = "SELECT * FROM Tasks";
                return await _connection.QueryAsync<Tasks>(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<JoinedTasksAndCategories>> GetJoinedTasksAndCategoriesAsync()
        {
            try
            {
                string query = "SELECT Tasks.id, title, deadline, is_completed, name FROM Categories join Tasks on Categories.id = Tasks.category_id";
                return await _connection.QueryAsync<JoinedTasksAndCategories>(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
