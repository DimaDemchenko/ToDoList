using Dapper;
using System.Data;
using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IDbConnection _connection;

        public CategoriesRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                string query = "SELECT * FROM Tasks";
                return await _connection.QueryAsync<Category>(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                string query = "SELECT * FROM Tasks WHERE Id = @id";
                return (Category)await _connection.QueryAsync<Category>(query, new { id});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
