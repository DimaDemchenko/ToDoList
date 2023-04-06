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
        public async System.Threading.Tasks.Task CreateAsync(DBmodels.Task task)
        {
            try
            {
                string query = @"INSERT INTO Tasks (category_id, title, deadline, is_completed)
                         VALUES (@CategoryId, @Title, @Deadline, @IsCompleted);
                         SELECT SCOPE_IDENTITY()";
                await _connection.ExecuteAsync(query, task);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            try
            {
                string query = "DELETE FROM Tasks WHERE Id = @id";
                await _connection.ExecuteAsync(query, new { id });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<DBmodels.Task>> GetAllAsync()
        {
            try
            {
                string query = "SELECT * FROM Tasks JOIN Categories ON Tasks.category_id = Categories.id";

                var tasks = await _connection.QueryAsync<DBmodels.Task, Category, DBmodels.Task>(query,

                    (task, category) =>
                    {
                        task.Category = category;
                        return task;
                    },
                    splitOn: "Id");
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
                string query = "SELECT * FROM Tasks JOIN Categories ON Tasks.category_id = Categories.id WHERE is_completed = @IsCompleted";

                var tasks = await _connection.QueryAsync<DBmodels.Task, Category, DBmodels.Task>(query,

                    (task, category) =>
                    {
                        task.Category = category;
                        return task;
                    },new { IsCompleted },
                    splitOn: "Id");
                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task UpdateStatusAsync(int id, bool IsCompleted)
        {
            try
            {
                string query = @"UPDATE Tasks
                         SET is_completed = @IsCompleted
                         WHERE Id = @id";
                await _connection.ExecuteAsync(query, new { id, IsCompleted });
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
