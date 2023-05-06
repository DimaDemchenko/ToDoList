using Dapper;
using System.Data;
using System.Data.Common;
using ToDoList.DBmodels;
using ToDoList.Models;

namespace ToDoList.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnection _connection;

        public TaskRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async System.Threading.Tasks.Task<int> CreateAsync(DBmodels.Task task)
        {
            try
            {
                string query = @"INSERT INTO Tasks (category_id, title, deadline, is_completed)
                         VALUES (@CategoryId, @Title, @Deadline, @IsCompleted);
                         SELECT SCOPE_IDENTITY()";
                var id = await _connection.ExecuteScalarAsync<int>(query, task);

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> DeleteAsync(int id)
        {
            try
            {
                string query = "DELETE FROM Tasks WHERE Id = @id";
                var affectedRows = await _connection.ExecuteAsync(query, new { id });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllAsync()
        {
            try
            {
                string query = "SELECT Id, category_id AS CategoryId, title, deadline, is_completed as IsCompleted  FROM Tasks ";
                var tasks = await _connection.QueryAsync<DBmodels.Task>(query);

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllByStatusAsync(bool IsCompleted)
        {
            try
            {
                string query = "SELECT Id, category_id AS CategoryId, title, deadline, is_completed as IsCompleted FROM Tasks  WHERE is_completed = @IsCompleted";

                var tasks = await _connection.QueryAsync<DBmodels.Task>(query, new { IsCompleted });

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> UpdateStatusAsync(int id, bool IsCompleted)
        {
            try
            {
                string query = @"UPDATE Tasks
                         SET is_completed = @IsCompleted
                         WHERE Id = @id";
                var affectedRows = await _connection.ExecuteAsync(query, new { id, IsCompleted });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
