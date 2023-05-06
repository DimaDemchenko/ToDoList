using ToDoList.DBmodels;

namespace ToDoList.Repository
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllAsync();
    }
}
