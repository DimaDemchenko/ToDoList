using Dapper;
using System.Data;
using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _connection;

        public CategoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                string query = "SELECT * FROM categories";
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
                string query = "SELECT * FROM categories WHERE Id = @id";
                return (Category)await _connection.QueryAsync<Category>(query, new { id});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
